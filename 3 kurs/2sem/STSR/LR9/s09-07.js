const http = require('http');
const fs = require('fs');

const server = http.createServer((req, res) => {
  if (req.method === 'POST' && req.url === '/mypath') {

    let chunks = [];

    req.on('data', c => chunks.push(c));

    req.on('end', () => {
      const buffer = Buffer.concat(chunks);

      const contentType = req.headers['content-type'];
      const boundary = Buffer.from('--' + contentType.split('boundary=')[1]);

      // ищем начало файла
      let start = buffer.indexOf(boundary);

      start = buffer.indexOf('\r\n\r\n', start) + 4;

      // следующий boundary
      let end = buffer.indexOf(boundary, start) - 2;

      const fileBuffer = buffer.slice(start, end);

      fs.writeFileSync('newPng.png', fileBuffer);

      res.end('OK');
    });

  } else {
    res.statusCode = 404;
    res.end('Not found');
  }
});

server.listen(3000, () => {
    console.log('Server running on http://localhost:3000');
});