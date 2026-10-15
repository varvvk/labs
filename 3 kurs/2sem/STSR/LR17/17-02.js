const redis = require('redis');

const REDIS_URL = 'redis://default:jBXVF1lHiMG8HPLqFmcIqHnCT05iw2EH@redis-16691.c273.us-east-1-2.ec2.cloud.redislabs.com:16691';
const client = redis.createClient({ url: REDIS_URL });

client.on('error', (err) => console.error('Redis error:', err));

async function benchmark() {
    await client.connect();

    const N = 10;

    // 1. SET
    let start = Date.now();
    for (let i = 1; i <= N; i++) {
        await client.set(`key${i}`, `set${i}`);
    }
    let setTime = Date.now() - start;
    console.log(`SET 100 ops: ${setTime} ms`);

    // 2. GET
    start = Date.now();
    for (let i = 1; i <= N; i++) {
        await client.get(`key${i}`);
    }
    let getTime = Date.now() - start;
    console.log(`GET 100 ops: ${getTime} ms`);

    // 3. DEL
    start = Date.now();
    for (let i = 1; i <= N; i++) {
        await client.del(`key${i}`);
    }
    let delTime = Date.now() - start;
    console.log(`DEL 100 ops: ${delTime} ms`);

    await client.disconnect();

    return { setTime, getTime, delTime };
}

// Запуск
benchmark().then((result) => {
    console.log('\nТаблица выполнения операций:');
    console.log('№\tОперация\tВыполнение 100 операций (мс)');
    console.log(`1\tset(n, 'setn')\t${result.setTime}`);
    console.log(`2\tget(n)\t\t${result.getTime}`);
    console.log(`3\tdel(n)\t\t${result.delTime}`);
}).catch(err => console.error(err));