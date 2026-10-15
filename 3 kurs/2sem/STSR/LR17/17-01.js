const redis = require('redis');

const REDIS_URL = 'redis://default:jBXVF1lHiMG8HPLqFmcIqHnCT05iw2EH@redis-16691.c273.us-east-1-2.ec2.cloud.redislabs.com:16691';

const client = redis.createClient({
    url: REDIS_URL
});

client.on('connect', () => console.log('Соединение установлено'));
client.on('ready', () => console.log('Клиент готов к работе с Redis'));
client.on('error', (err) => console.error('Ошибка Redis:', err));
client.on('end', () => console.log('Клиент отключился'));

client.connect()
    .then(() => client.ping())
    .then((pong) => {
        console.log('PING ответ:', pong);
        client.quit();
    })
    .catch((err) => {
        console.error('Ошибка при работе с Redis:', err);
        client.quit();
    });