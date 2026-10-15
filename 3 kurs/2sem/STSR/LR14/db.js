const sql = require('mssql');

const config = {
    user: 'student',
    password: 'Pa$$w0rd',
    server: '80.94.224.209',
    database: 'KVV',
    options: {
        encrypt: false,
        trustServerCertificate: true
    },
    pool: {
        max: 10,
        min: 0,
        idleTimeoutMillis: 30000
    }
};

const pool = new sql.ConnectionPool(config);
const poolConnect = pool.connect();

module.exports = {
    sql, pool, poolConnect
};