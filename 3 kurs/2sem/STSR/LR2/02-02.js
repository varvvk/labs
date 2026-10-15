const http = require('http');
const fs = require('fs');

const port = 5000;

http.createServer((request, response) => {

    if (request.url === '/png') {

        fs.readFile('pic.jpg', (error, data) => {

            if (error) {
                response.writeHead(500, {'Content-Type': 'text/plain; charset=utf-8'});
                response.end('Ошибка чтения файла');
            } else {
                response.writeHead(200, {'Content-Type': 'image/jpeg'});
                response.end(data);
            }

        });

    } else {
        response.writeHead(404, {'Content-Type': 'text/plain; charset=utf-8'});
        response.end('Страница не найдена');
    }

}).listen(port, () => {
    console.log(`Server running at http://localhost:${port}/png`);
});