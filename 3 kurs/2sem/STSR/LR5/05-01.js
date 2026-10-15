var http = require('http');
var url = require('url');
var fs = require('fs');
var data = require('./m05-01');

const PATH_DB = '/api/db';
const PATH_STATS = '/api/ss';

var db = new data.DB();

let statsActive = false;
let stats = { start: null, finish: null, requests: 0, commits: 0 };

let stopTimeout = null;
let commitInterval = null;
let statsTimer = null;

db.on('COMMIT', async () => {
    console.log('DB.COMMIT');
    db.commit(); 
    if (statsActive) {
        stats.commits++;
        console.log('Commits:', stats.commits);
    }
})

db.on('GET', (request, response) => {
    console.log('DB.GET');
    if (statsActive) {
        stats.requests++;
        console.log('Requests (GET):', stats.requests);
    }
    response.writeHead(200, { 'content-type': 'application/json;charset=utf-8' });
    response.end(JSON.stringify(db.get()))
});

db.on('POST', (request, response) => {
    console.log('DB.POST');
    if (statsActive) {
        stats.requests++;  
        console.log('Requests (POST):', stats.requests);
    }
    let body = ''
    request.on('data', chunk => {
            body += chunk
    })
    request.on('end', () => {
        const row = JSON.parse(body);
        const maxId = db.get().reduce((max, item) => item.id > max ? item.id : max, 0);
        row.id = maxId + 1;
        db.post(row);
        response.writeHead(200, { 'content-type': 'application/json;charset=utf-8' });
        response.end(JSON.stringify(db.get()))
    })
});

db.on('PUT', (request, response) => {
    console.log('DB.PUT');
    if (statsActive) {
        stats.requests++;
        console.log('Requests (PUT):', stats.requests);
    }
    let body = ''
    request.on('data', chunk => {
            body += chunk
    })
    request.on('end', () => {
        const parsedURL = url.parse(request.url, true);
        const id = parsedURL.query.id;
        
        const row = JSON.parse(body);
        row.id = parseInt(id);
        
        const result = db.put(row);
        if (result) {
            response.writeHead(200, { 'content-type': 'application/json;charset=utf-8' });
            response.end(JSON.stringify(db.get()));
        } else {
            response.writeHead(404, { 'content-type': 'application/json;charset=utf-8' });
            response.end(JSON.stringify({ error: 'Record not found' }));
        }
    })
});

db.on('DELETE', (request, response) => {
    console.log('DB.DELETE');
    if (statsActive) {
        stats.requests++;
        console.log('Requests (DELETE):', stats.requests);
    }
    let parsedURL = url.parse(request.url, true);
    const id = parsedURL.query.id;

    if (!id) {
        response.writeHead(400, { 'content-type': 'application/json;charset=utf-8' });
        response.end(JSON.stringify({ error: 'ID is required' }));
        return;
    }

    const deletedRow = db.delete(parseInt(id));
    if (deletedRow) {
        response.writeHead(200, { 'content-type': 'application/json;charset=utf-8' });
        response.end(JSON.stringify(deletedRow));
    } else {
        response.writeHead(404, { 'content-type': 'application/json;charset=utf-8' });
        response.end(JSON.stringify({ error: 'Record not found' }));
    }
});

const server = http.createServer(function (request, response) {
        const path = url.parse(request.url).pathname

        if (path === PATH_DB) {
            db.emit(request.method, request, response)
        } else if (path === PATH_STATS && request.method === 'GET') {
            console.log('GET /api/ss');
            console.log('Current stats:', stats);
            response.writeHead(200, {
                'content-type': 'application/json;charset=utf-8',
            })
            response.end(JSON.stringify(stats))
        } else if (request.url === '/') {
            let html = fs.readFileSync('./05-01.html')
            response.writeHead(200, { 'content-type': 'text/html;charset=utf-8' })
            response.end(html)
        } else {
            response.writeHead(404, { 'content-type': 'text/plain; charset=utf-8' })
            response.end('Not found')
        }
    }).listen(5000)

console.log(`Server running at http://localhost:5000`);

process.stdin.setEncoding("utf-8");
process.stdin.on("readable", () => {
    let chunk;
    while ((chunk = process.stdin.read()) !== null) {
        const input = chunk.trim();
        if (!input) continue;

        const [command, parameter] = input.split(' ');

        switch (command) {
            case 'sd': {
                if (stopTimeout) {
                    clearTimeout(stopTimeout);
                    stopTimeout = null;
                }

                if (parameter) {
                    const seconds = parseInt(parameter);
                    console.log(`server will be stopped in ${seconds} seconds`)
                    stopTimeout = setTimeout(() => {
                        server.close(() => console.log(`Server closed`));
                        process.exit(0);
                    }, seconds * 1000);
                    stopTimeout.unref();
                } else {
                    console.log('Server stop cancelled');
                }
                break;
            }

            case 'sc': {
                if (commitInterval) {
                    clearInterval(commitInterval);
                    commitInterval = null;
                    console.log('Interval commits stopped');
                }

                if (parameter) {
                    const seconds = parseInt(parameter);
                    commitInterval = setInterval(() => {
                        db.emit("COMMIT");
                    }, seconds * 1000);
                    commitInterval.unref();
                    console.log(`Interval commit started every ${seconds} seconds`);
                }
                break;
            }

            case 'ss': {
                if (statsActive) {
                    statsActive = false;
                    stats.finish = new Date().toISOString();
                    if (statsTimer) clearTimeout(statsTimer);
                    console.log('Statistics collection stopped');
                    break;
                }

                if (parameter) {
                    statsActive = true;
                    stats.start = new Date().toISOString();
                    stats.finish = null;
                    stats.requests = 0;
                    stats.commits = 0;
                    const seconds = parseInt(parameter);
                    console.log(`Statistics collection started for ${seconds} seconds.`);
                    statsTimer = setTimeout(() => {
                        statsActive = false;
                        stats.finish = new Date().toISOString();
                        console.log('Statistics collection stopped');
                        console.log('Final stats:', stats);
                    }, seconds * 1000);
                    statsTimer.unref();
                }
                else {
                    console.log('No parameter for ss');
                }
                break;
            }

            default: {
                console.log('Invalid command.\n Valid options: sd [x], sc [x], ss [x] (x - seconds)');
                break;
            }
        }
    }
})