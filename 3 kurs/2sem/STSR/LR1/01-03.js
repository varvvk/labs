const http = require('http');

http.createServer((request, response) => {

    let body = '';

    request.on('data', chunk => {
        body += chunk;
    });

    response.writeHead(200, {'Content-Type': 'text/html; charset=utf-8'});

    request.on('end', () => {
        response.end(`
            <h1>Данные запроса</h1>
            <p>Метод: ${request.method}</p>
            <p>URI: ${request.url}</p>

            <p>Заголовки:</p>
            <table>
                <tr>
                    <th>Название</th>
                    <th>Значение</th>
                </tr>
                ${Object.entries(request.headers)
                    .map(([key, value]) => `
                        <tr>
                            <td>${key}</td>
                            <td>${value}</td>
                        </tr>
                    `).join('')}
            </table>

            <p>Тело запроса:</p>
            <p>${body}</p>
        `);
    });

}).listen(3000, () => {
    console.log('Server running at http://localhost:3000/');
});