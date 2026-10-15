// 17-03.js
const redis = require('redis');

const REDIS_URL = 'redis://default:jBXVF1lHiMG8HPLqFmcIqHnCT05iw2EH@redis-16691.c273.us-east-1-2.ec2.cloud.redislabs.com:16691';
const client = redis.createClient({ url: REDIS_URL });

client.on('error', (err) => console.error('Redis error:', err));

async function benchmark() {
    await client.connect();

    const N = 10;

    // Инициализация ключа
    await client.set('incr', 0);

    // INCR
    let start = Date.now();
    for (let i = 0; i < N; i++) {
        await client.incr('incr');
    }
    const incrTime = Date.now() - start;
    console.log(`INCR 100 ops: ${incrTime} ms`);

    // DECR
    start = Date.now();
    for (let i = 0; i < N; i++) {
        await client.decr('incr');
    }
    const decrTime = Date.now() - start;
    console.log(`DECR 100 ops: ${decrTime} ms`);

    await client.disconnect();

    // Таблица
    console.log('\nТаблица выполнения операций:');
    console.log('№\tОперация\tВыполнение 100 операций (мс)');
    console.log(`1\tincr('incr')\t${incrTime}`);
    console.log(`2\tdecr('incr')\t${decrTime}`);
}

benchmark().catch(console.error);