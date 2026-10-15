const http = require('http');
const fs = require('fs');

const file = fs.createWriteStream('newBpm.bmp');

const options = {
    host: 'localhost',
    path: '/bmp/MyFile.bmp',
    port: 3000,
    method: 'GET'
};

const req = http.request(options, (res) => {
    if (res.statusCode === 200) {

        res.pipe(file);

        file.on('close', () => {
            console.log('File downloaded successfully.');
        });

    } else {
        console.log(`Failed: ${res.statusCode}`);
    }
});

req.on('error', (e) => {
    console.log('Error:', e.message);
});

req.end();