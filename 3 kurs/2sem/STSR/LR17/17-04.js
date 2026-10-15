// 17-04.js
const redis = require('redis');

const REDIS_URL = 'redis://default:jBXVF1lHiMG8HPLqFmcIqHnCT05iw2EH@redis-16691.c273.us-east-1-2.ec2.cloud.redislabs.com:16691';
const client = redis.createClient({ url: REDIS_URL });

client.on('error', (err) => console.error('Redis error:', err));

async function benchmark() {
    await client.connect();

    const N = 10;

    // HSET
    let start = Date.now();
    for (let i = 1; i <= N; i++) {
        await client.hSet('hash', `n${i}`, JSON.stringify({ id: i, val: `val-${i}` }));
    }
    const hsetTime = Date.now() - start;
    console.log(`HSET 100 ops: ${hsetTime} ms`);

    // HGET
    start = Date.now();
    for (let i = 1; i <= N; i++) {
        await client.hGet('hash', `n${i}`);
    }
    const hgetTime = Date.now() - start;
    console.log(`HGET 100 ops: ${hgetTime} ms`);

    await client.disconnect();

    // Таблица
    console.log('\nТаблица выполнения операций:');
    console.log('№\tОперация\tВыполнение 100 операций (мс)');
    console.log(`1\thset(n, '{id:n,val:"val-n"}')\t${hsetTime}`);
    console.log(`2\thget(n)\t\t${hgetTime}`);
}

benchmark().catch(console.error);