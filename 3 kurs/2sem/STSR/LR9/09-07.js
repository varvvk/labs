const http = require('http');
const fs = require('fs');

const boundary = '----NodeBoundary' + Date.now();
const fileContent = fs.readFileSync('MyFile.png');

const CRLF = '\r\n';

const body =
  Buffer.concat([
    Buffer.from(`--${boundary}${CRLF}`),
    Buffer.from(
      `Content-Disposition: form-data; name="file"; filename="MyFile.png"${CRLF}`
    ),
    Buffer.from(`Content-Type: image/png${CRLF}${CRLF}`),
    fileContent,
    Buffer.from(CRLF + `--${boundary}--${CRLF}`)
  ]);

const options = {
  host: 'localhost',
  port: 3000,
  path: '/mypath',
  method: 'POST',
  headers: {
    'Content-Type': `multipart/form-data; boundary=${boundary}`,
    'Content-Length': body.length
  }
};

const req = http.request(options, res => {
  let data = '';
  res.on('data', c => data += c);
  res.on('end', () => console.log(data));
});

req.write(body);
req.end();