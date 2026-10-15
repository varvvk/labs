const nodemailer = require("nodemailer");
const gmail = 'kazakovavarvara660@gmail.com';
const password = 'nefo oeaf ohcq tfwo';

const transporter = nodemailer.createTransport({
    host: 'smtp.gmail.com',
    port: 587,
    secure: false,
    auth: {
        user: gmail,
        pass: password
    }
});

function send(email) {
    transporter.sendMail({
        from: 'kazakovavarvara660@gmail.com',
        to: gmail,
        subject: 'СТСР',
        text: email
    }, (smtpErr, info) => {
        if (smtpErr) {
            console.error('Ошибка отправки через nodemailer:', smtpErr);
        } else {
            console.log('Сообщение успешно отправлено!');
        }
    });
    return email.toString();
}

exports.send = send;