const http = require('http');
const xml2js = require('xml2js');
const xmlbuilder = require('xmlbuilder');

const server = http.createServer((req, res) => {

    if (req.method === 'POST' && req.url === '/xml') {

        let body = '';

        req.on('data', chunk => {
            body += chunk;
        });

        req.on('end', () => {

            xml2js.parseString(body, (err, result) => {

                if (err) {
                    res.writeHead(400, { 'Content-Type': 'text/xml' });
                    return res.end('<error>Invalid XML</error>');
                }

                let request = result.request;

                let requestId = request.$.id;

                // массив x
                let xValues = request.x.map(el => parseInt(el.$.value));

                // массив m
                let mValues = request.m.map(el => el.$.value);

                let sumX = xValues.reduce((a, b) => a + b, 0);
                let concatM = mValues.join('');

                let responseXml = xmlbuilder.create('response')
                    .att('id', '33')
                    .att('request', requestId)
                    .ele('sum')
                        .att('element', 'x')
                        .att('result', sumX)
                    .up()
                    .ele('concat')
                        .att('element', 'm')
                        .att('result', concatM)
                    .end({ pretty: true });

                res.writeHead(200, { 'Content-Type': 'text/xml' });
                res.end(responseXml);
            });
        });

    } else {
        res.writeHead(404);
        res.end('Not found');
    }
});

server.listen(3000, () => {
    console.log('Server running on http://localhost:3000');
});