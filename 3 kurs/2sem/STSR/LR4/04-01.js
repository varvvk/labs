const http = require('http');
const url = require('url');
const fs = require('fs');
const data = require('./DB');

var db = new data.DB();

db.on('GET', (req, res) => {
    res.end(JSON.stringify(db.select()));
});
db.on('POST', (req, res) => {
    req.on('data', data => {
        let par = JSON.parse(data);
        db.insert(par);
        res.end(JSON.stringify(par));
    })
});

db.on('PUT', (req, res) => {
    req.on('data', data => {
        let par = JSON.parse(data);
        db.update(par);
        res.end(JSON.stringify(par));
    })
});

db.on('DELETE', (req, res) => {
    const queryParams = new URLSearchParams(url.parse(req.url).query);
    const param = parseInt(queryParams.get('id'));
    const deletedItem = db.delete(param);
    if (deletedItem) {
        res.writeHead(200);
        res.end(JSON.stringify(deletedItem)); 
    }
});

http.createServer((req, res) => {
    if (url.parse(req.url).pathname === '/api/db') {
        db.emit(req.method, req, res);
    }
}).listen(5000);
