let http = require('http');

let requestData = {
    "__comment": "Запрос.Лабораторная работа 9",
    "x": 1,
    "y": 2,
    "s": "Сообщение",
    "m": ["a", "b", "c", "d"],
    "o": {
        "surname": "Иванов",
        "name": "Иван"
    }
};

let JSONData = JSON.stringify(requestData);

let options = {
    host: 'localhost',
    port: 3000,
    path: '/mypath',
    method: 'POST',
    headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'Content-Length': Buffer.byteLength(JSONData)
    }
};

let req = http.request(options, (res) => {
    console.log('Status Code:', res.statusCode);

    let data = '';

    res.on('data', (chunk) => {
        data += chunk;
    });

    res.on('end', () => {
        console.log('Response body:', data);
        console.log('Parsed:', JSON.parse(data));
    });
});

req.on('error', (err) => {
    console.log('Error:', err.message);
});

req.write(JSONData);
req.end();