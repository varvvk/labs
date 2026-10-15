const http = require('http');

const port = 5000;

http.createServer((request, response) => {

    if (request.url === '/api/name') {
        response.writeHead(200, {'Content-Type': 'text/plain; charset=utf-8'});
        response.end('Казакова Варвара Вадимовна');
    }

    else {
        response.writeHead(404, {'Content-Type': 'text/plain; charset=utf-8'});
        response.end('Страница не найдена');
    }

}).listen(port, () => {
    console.log(`Server running at http://localhost:${port}/api/name`);
});