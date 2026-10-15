const http = require('http');
const fs = require('fs');

const port = 5000;

http.createServer((request, response) => {

    if (request.url === '/jquery') {

        fs.readFile('jquery.html', 'utf8', (error, data) => {
            if (error) {
                response.writeHead(500, {'Content-Type': 'text/plain; charset=utf-8'});
                response.end('Ошибка чтения файла');
            } else {
                response.writeHead(200, {'Content-Type': 'text/html; charset=utf-8'});
                response.end(data);
            }
        });

    }

    else if (request.url === '/api/name') {

        response.writeHead(200, {'Content-Type': 'text/plain; charset=utf-8'});
        response.end('Казакова Варвара Вадимовна');

    }
    
    else {
        response.writeHead(404, {'Content-Type': 'text/plain; charset=utf-8'});
        response.end('Страница не найдена');
    }

}).listen(port, () => {
    console.log(`Server running at http://localhost:${port}/jquery`);
});