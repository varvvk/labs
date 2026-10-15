const http = require('http');

const server = http.createServer((req, res) => {

    if (req.method === 'POST' && req.url === '/mypath') {

        let body = '';

        req.on('data', chunk => {
            body += chunk.toString();
        });

        req.on('end', () => {
            let data = JSON.parse(body);

            let response = {
                "__comment": "Ответ.Лабораторная работа 9",
                "x_plus_y": data.x + data.y,
                "Concatenation_s_o": `${data.s}: ${data.o.surname}, ${data.o.name}`,
                "Length_m": data.m.length
            };

            res.writeHead(200, { 'Content-Type': 'application/json' });
            res.end(JSON.stringify(response));
        });

    } else {
        res.writeHead(404, { 'Content-Type': 'application/json' });
        res.end(JSON.stringify({ error: 'Not found' }));
    }
});

server.listen(3000, () => {
    console.log('Server running on http://localhost:3000');
});