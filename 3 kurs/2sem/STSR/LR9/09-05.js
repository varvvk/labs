const http = require('http');
const xmlbuilder = require('xmlbuilder');

let xmlData = xmlbuilder.create('request')
    .att('id', '28')
    .ele('x').att('value', '1').up()
    .ele('x').att('value', '2').up()
    .ele('m').att('value', 'a').up()
    .ele('m').att('value', 'b').up()
    .ele('m').att('value', 'c').up()
    .end({ pretty: true });

let options = {
    host: 'localhost',
    port: 3000,
    path: '/xml',
    method: 'POST',
    headers: {
        'Content-Type': 'text/xml',
        'Accept': 'text/xml',
        'Content-Length': Buffer.byteLength(xmlData)
    }
};

let req = http.request(options, (res) => {
    console.log('Status Code:', res.statusCode);

    let data = '';
    res.on('data', chunk => data += chunk);

    res.on('end', () => {
        console.log('Response XML:\n', data);
    });
});

req.on('error', err => console.log('Error:', err.message));

req.write(xmlData);
req.end();