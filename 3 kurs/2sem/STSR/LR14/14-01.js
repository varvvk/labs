const http = require('http');
const fs = require('fs');
const path = require('path');
const url = require('url');
const { sql, pool, poolConnect } = require('./db');

const PORT = 3000;

// подключение к БД
poolConnect
    .then(() => console.log('DB connected'))
    .catch(err => console.log('DB error:', err));

// чтение body
function getBody(req) {
    return new Promise((resolve, reject) => {
        let body = '';
        req.on('data', chunk => body += chunk);
        req.on('end', () => resolve(JSON.parse(body)));
        req.on('error', reject);
    });
}

// универсальный ответ
function send(res, status, data) {
    res.writeHead(status, { 'Content-Type': 'application/json' });
    res.end(JSON.stringify(data));
}

// проверка "не найдено"
function notFound(res, result, entity) {
    if (result.rowsAffected[0] === 0) {
        send(res, 404, { error: `${entity} not found` });
        return true;
    }
    return false;
}

const server = http.createServer(async (req, res) => {

    const parsedUrl = url.parse(req.url, true);
    const pathname = parsedUrl.pathname;

    try {
        await poolConnect;

        // ===== GET / =====
        if (req.method === 'GET' && pathname === '/') {
            const html = fs.readFileSync(path.join(__dirname, 'index.html'));
            res.writeHead(200, { 'Content-Type': 'text/html' });
            return res.end(html);
        }

        // ===== GET =====
        if (req.method === 'GET') {

            if (pathname === '/api/faculties') {
                const r = await pool.request().query('SELECT * FROM FACULTY');
                return send(res, 200, r.recordset);
            }

            if (pathname === '/api/pulpits') {
                const r = await pool.request().query('SELECT * FROM PULPIT');
                return send(res, 200, r.recordset);
            }

            if (pathname === '/api/subjects') {
                const r = await pool.request().query('SELECT * FROM SUBJECT');
                return send(res, 200, r.recordset);
            }

            if (pathname === '/api/auditoriumstypes') {
                const r = await pool.request().query('SELECT * FROM AUDITORIUM_TYPE');
                return send(res, 200, r.recordset);
            }

            if (pathname === '/api/auditoriums') {
                const r = await pool.request().query('SELECT * FROM AUDITORIUM');
                return send(res, 200, r.recordset);
            }
        }

        // ===== POST =====
        if (req.method === 'POST') {
            const body = await getBody(req);

            if (pathname === '/api/faculties') {
                await pool.request()
                    .input('FACULTY', sql.VarChar, body.FACULTY)
                    .input('FACULTY_NAME', sql.VarChar, body.FACULTY_NAME)
                    .query('INSERT INTO FACULTY VALUES (@FACULTY, @FACULTY_NAME)');
                return send(res, 200, body);
            }

            if (pathname === '/api/pulpits') {
                await pool.request()
                    .input('PULPIT', sql.VarChar, body.PULPIT)
                    .input('PULPIT_NAME', sql.VarChar, body.PULPIT_NAME)
                    .input('FACULTY', sql.VarChar, body.FACULTY)
                    .query('INSERT INTO PULPIT VALUES (@PULPIT, @PULPIT_NAME, @FACULTY)');
                return send(res, 200, body);
            }

            if (pathname === '/api/subjects') {
                await pool.request()
                    .input('SUBJECT', sql.VarChar, body.SUBJECT)
                    .input('SUBJECT_NAME', sql.VarChar, body.SUBJECT_NAME)
                    .input('PULPIT', sql.VarChar, body.PULPIT)
                    .query('INSERT INTO SUBJECT VALUES (@SUBJECT, @SUBJECT_NAME, @PULPIT)');
                return send(res, 200, body);
            }

            if (pathname === '/api/auditoriumstypes') {
                await pool.request()
                    .input('AUDITORIUM_TYPE', sql.VarChar, body.AUDITORIUM_TYPE)
                    .input('AUDITORIUM_TYPENAME', sql.VarChar, body.AUDITORIUM_TYPENAME)
                    .query('INSERT INTO AUDITORIUM_TYPE VALUES (@AUDITORIUM_TYPE, @AUDITORIUM_TYPENAME)');
                return send(res, 200, body);
            }

            if (pathname === '/api/auditoriums') {
                await pool.request()
                    .input('AUDITORIUM', sql.VarChar, body.AUDITORIUM)
                    .input('AUDITORIUM_TYPE', sql.VarChar, body.AUDITORIUM_TYPE)
                    .input('AUDITORIUM_CAPACITY', sql.Int, body.AUDITORIUM_CAPACITY)
                    .input('AUDITORIUM_NAME', sql.VarChar, body.AUDITORIUM_NAME)
                    .query('INSERT INTO AUDITORIUM VALUES (@AUDITORIUM, @AUDITORIUM_TYPE, @AUDITORIUM_CAPACITY, @AUDITORIUM_NAME)');
                return send(res, 200, body);
            }
        }

        // ===== PUT =====
        if (req.method === 'PUT') {
            const body = await getBody(req);

            if (pathname === '/api/faculties') {
                const r = await pool.request()
                    .input('FACULTY', sql.VarChar, body.FACULTY)
                    .input('FACULTY_NAME', sql.VarChar, body.FACULTY_NAME)
                    .query('UPDATE FACULTY SET FACULTY_NAME=@FACULTY_NAME WHERE RTRIM(FACULTY)=@FACULTY');

                if (notFound(res, r, 'Faculty')) return;
                return send(res, 200, body);
            }

            if (pathname === '/api/pulpits') {
                const r = await pool.request()
                    .input('PULPIT', sql.VarChar, body.PULPIT)
                    .input('PULPIT_NAME', sql.VarChar, body.PULPIT_NAME)
                    .input('FACULTY', sql.VarChar, body.FACULTY)
                    .query('UPDATE PULPIT SET PULPIT_NAME=@PULPIT_NAME, FACULTY=@FACULTY WHERE RTRIM(PULPIT)=@PULPIT');

                if (notFound(res, r, 'Pulpit')) return;
                return send(res, 200, body);
            }

            if (pathname === '/api/subjects') {
                const r = await pool.request()
                    .input('SUBJECT', sql.VarChar, body.SUBJECT)
                    .input('SUBJECT_NAME', sql.VarChar, body.SUBJECT_NAME)
                    .input('PULPIT', sql.VarChar, body.PULPIT)
                    .query('UPDATE SUBJECT SET SUBJECT_NAME=@SUBJECT_NAME, PULPIT=@PULPIT WHERE RTRIM(SUBJECT)=@SUBJECT');

                if (notFound(res, r, 'Subject')) return;
                return send(res, 200, body);
            }

            if (pathname === '/api/auditoriumstypes') {
                const r = await pool.request()
                    .input('AUDITORIUM_TYPE', sql.VarChar, body.AUDITORIUM_TYPE)
                    .input('AUDITORIUM_TYPENAME', sql.VarChar, body.AUDITORIUM_TYPENAME)
                    .query('UPDATE AUDITORIUM_TYPE SET AUDITORIUM_TYPENAME=@AUDITORIUM_TYPENAME WHERE RTRIM(AUDITORIUM_TYPE)=@AUDITORIUM_TYPE');

                if (notFound(res, r, 'AuditoriumType')) return;
                return send(res, 200, body);
            }

            if (pathname === '/api/auditoriums') {
                const r = await pool.request()
                    .input('AUDITORIUM', sql.VarChar, body.AUDITORIUM)
                    .input('AUDITORIUM_NAME', sql.VarChar, body.AUDITORIUM_NAME)
                    .input('AUDITORIUM_CAPACITY', sql.Int, body.AUDITORIUM_CAPACITY)
                    .input('AUDITORIUM_TYPE', sql.VarChar, body.AUDITORIUM_TYPE)
                    .query(`UPDATE AUDITORIUM 
                            SET AUDITORIUM_NAME=@AUDITORIUM_NAME,
                                AUDITORIUM_CAPACITY=@AUDITORIUM_CAPACITY,
                                AUDITORIUM_TYPE=@AUDITORIUM_TYPE
                            WHERE RTRIM(AUDITORIUM)=@AUDITORIUM`);

                if (notFound(res, r, 'Auditorium')) return;
                return send(res, 200, body);
            }
        }

        // ===== DELETE =====
        if (req.method === 'DELETE') {

            if (pathname.startsWith('/api/faculties/')) {
                const id = decodeURIComponent(pathname.split('/')[3]);

                const r = await pool.request()
                    .input('FACULTY', sql.VarChar, id)
                    .query('DELETE FROM FACULTY WHERE RTRIM(FACULTY)=@FACULTY');

                if (notFound(res, r, 'Faculty')) return;
                return send(res, 200, { deleted: id });
            }

            if (pathname.startsWith('/api/pulpits/')) {
                const id = decodeURIComponent(pathname.split('/')[3]);

                const r = await pool.request()
                    .input('PULPIT', sql.VarChar, id)
                    .query('DELETE FROM PULPIT WHERE RTRIM(PULPIT)=@PULPIT');

                if (notFound(res, r, 'Pulpit')) return;
                return send(res, 200, { deleted: id });
            }

            if (pathname.startsWith('/api/subjects/')) {
                const id = decodeURIComponent(pathname.split('/')[3]);

                const r = await pool.request()
                    .input('SUBJECT', sql.VarChar, id)
                    .query('DELETE FROM SUBJECT WHERE RTRIM(SUBJECT)=@SUBJECT');

                if (notFound(res, r, 'Subject')) return;
                return send(res, 200, { deleted: id });
            }

            if (pathname.startsWith('/api/auditoriumstypes/')) {
                const id = decodeURIComponent(pathname.split('/')[3]);

                const r = await pool.request()
                    .input('AUDITORIUM_TYPE', sql.VarChar, id)
                    .query('DELETE FROM AUDITORIUM_TYPE WHERE RTRIM(AUDITORIUM_TYPE)=@AUDITORIUM_TYPE');

                if (notFound(res, r, 'AuditoriumType')) return;
                return send(res, 200, { deleted: id });
            }

            if (pathname.startsWith('/api/auditoriums/')) {
                const id = decodeURIComponent(pathname.split('/')[3]);

                const r = await pool.request()
                    .input('AUDITORIUM', sql.VarChar, id)
                    .query('DELETE FROM AUDITORIUM WHERE RTRIM(AUDITORIUM)=@AUDITORIUM');

                if (notFound(res, r, 'Auditorium')) return;
                return send(res, 200, { deleted: id });
            }
        }

        send(res, 404, { error: 'Not found' });

    } catch (err) {
        send(res, 500, { error: err.message });
    }
});

server.listen(PORT, () => {
    console.log(`Server running on http://localhost:${PORT}`);
});