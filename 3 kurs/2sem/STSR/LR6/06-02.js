const http = require('http');
const fs = require('fs');
const url = require('url');
const { parse } = require('querystring');
const nodemailer = require('nodemailer');

const transporter = nodemailer.createTransport({
    host: 'smtp.gmail.com',
    port: 587,
    secure: false,
    auth: {
        user: 'kazakovavarvara660@gmail.com',
        pass: 'nefo oeaf ohcq tfwo'
    }
});

http.createServer((req, resp) => {
    resp.writeHead(200, { 'Content-Type': 'text/html; charset=utf-8' });
    const pathname = url.parse(req.url).pathname;
    if (pathname === '/' && req.method === 'GET') {
        fs.readFile('./index.html', (err, data) => {
            if (err) {
                console.error('Форма не загрузилась:', err);
            } else {
                resp.end(data);
            }
        });
    } else if (pathname === '/' && req.method === 'POST') {
        let body = '';
        req.on('data', chunk => {
            body += chunk.toString();
        });
        req.on('end', () => {
            const {
                from,
                to,
                message
            } = parse(body);

            if (!from || !to || !message) {
                resp.end('<h1>Все поля должны быть заполнены!</h1>');
                return;
            }

            transporter.sendMail({
                from: 'kazakovavarvara660@gmail.com',
                to: to,
                subject: 'СТСР',
                text: message
            }, (smtpErr, info) => {
                if (smtpErr) {
                    console.error('Ошибка отправки через nodemailer:', smtpErr);
                } else {
                    resp.end('<h1>Сообщение успешно отправлено!</h1>');
                }
            });
        });
    } else {
        resp.end('<h1>Not supported</h1>');
    }
}).listen(5001, () => console.log('Server running at http://localhost:5001/'));