// 17-05.js
const redis = require('redis');

const REDIS_URL = 'redis://default:jBXVF1lHiMG8HPLqFmcIqHnCT05iw2EH@redis-16691.c273.us-east-1-2.ec2.cloud.redislabs.com:16691';

// Создаем клиента для подписки
const subscriber = redis.createClient({ url: REDIS_URL });
const publisher = redis.createClient({ url: REDIS_URL });

// Обработка ошибок
subscriber.on('error', (err) => console.error('Subscriber error:', err));
publisher.on('error', (err) => console.error('Publisher error:', err));

(async () => {
    // Подключаем клиентов
    await subscriber.connect();
    await publisher.connect();

    const CHANNEL = 'demo_channel';

    // Подписка на канал
    await subscriber.subscribe(CHANNEL, (message) => {
        console.log(`Получено сообщение на канале "${CHANNEL}": ${message}`);
    });

    console.log(`Подписчик запущен на канале "${CHANNEL}"`);

    // Отправляем несколько сообщений с задержкой
    let count = 1;
    const interval = setInterval(async () => {
        const msg = `Сообщение #${count}`;
        await publisher.publish(CHANNEL, msg);
        console.log(`Отправлено: ${msg}`);
        count++;
        if (count > 5) { // после 5 сообщений завершаем
            clearInterval(interval);
            await subscriber.disconnect();
            await publisher.disconnect();
            console.log('Pub/Sub демонстрация завершена');
        }
    }, 1000); // каждые 1 секунду
})();