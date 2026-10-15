const http = require('http')
const url = require('url')
const { buildSchema, graphql } = require('graphql')
const sql = require('mssql')

const dbConfig = {
    user: 'student',
    password: 'Pa$$w0rd',
    server: '80.94.224.209',
    port: 1433,
    database: 'KVV',
    options: {
        encrypt: false,
        trustServerCertificate: true
    }
}

//GraphQL схема
const schema = buildSchema(`
    type FACULTY {
        FACULTY: String!
        FACULTY_NAME: String
    }

    type PULPIT {
        PULPIT: String!
        PULPIT_NAME: String
        FACULTY: String!
    }

    type TEACHER {
        TEACHER: String!
        TEACHER_NAME: String
        PULPIT: String!
    }

    type SUBJECT {
        SUBJECT: String!
        SUBJECT_NAME: String
        PULPIT: String!
    }

    type TEACHERS_BY_FACULTY_RESULT {
        FACULTY: String!
        TEACHERS: [TEACHER]
    }

    type SUBJECTS_BY_FACULTY_RESULT {
        PULPIT: String!
        PULPIT_NAME: String!
        FACULTY: String!
        SUBJECTS: [SUBJECT]
    }

    type Query {
        getFaculties(faculty: String): [FACULTY]
        getPulpits(pulpit: String): [PULPIT]
        getTeachers(teacher: String): [TEACHER]
        getSubjects(subject: String): [SUBJECT]
        getTeachersByFaculty(faculty: String!): [TEACHERS_BY_FACULTY_RESULT]
        getSubjectsByFaculties(faculty: String!): [SUBJECTS_BY_FACULTY_RESULT]
    }

    type Mutation {
        setFaculty(faculty: String!, faculty_name: String!): FACULTY
        setPulpit(pulpit: String!, pulpit_name: String!, faculty: String!): PULPIT
        setTeacher(teacher: String!, teacher_name: String!, pulpit: String!): TEACHER
        setSubject(subject: String!, subject_name: String!, pulpit: String!): SUBJECT
        delFaculty(faculty: String!): Boolean
        delPulpit(pulpit: String!): Boolean
        delTeacher(teacher: String!): Boolean
        delSubject(subject: String!): Boolean
    }
`)

const pool = new sql.ConnectionPool(dbConfig)
const poolConnect = pool.connect()

const root = {
    getFaculties: async ({ faculty }) => {
        await poolConnect
        const request = pool.request()
        let query = 'SELECT * FROM FACULTY'
        if (faculty) {
            request.input('faculty', sql.NVarChar, faculty)
            query += ' WHERE LTRIM(RTRIM(FACULTY)) = @faculty'
        }
        const result = await request.query(query)
        return result.recordset
    },

    getPulpits: async ({ pulpit }) => {
        await poolConnect
        const request = pool.request()
        let query = 'SELECT * FROM PULPIT'
        if (pulpit) {
            request.input('pulpit', sql.NVarChar, pulpit)
            query += ' WHERE LTRIM(RTRIM(PULPIT)) = @pulpit'
        }
        const result = await request.query(query)
        return result.recordset
    },

    getTeachers: async ({ teacher }) => {
        await poolConnect
        const request = pool.request()
        let query = 'SELECT * FROM TEACHER'
        if (teacher) {
            request.input('teacher', sql.NVarChar, teacher)
            query += ' WHERE LTRIM(RTRIM(TEACHER)) = @teacher'
        }
        const result = await request.query(query)
        return result.recordset
    },

    getSubjects: async ({ subject }) => {
        await poolConnect
        const request = pool.request()
        let query = 'SELECT * FROM SUBJECT'
        if (subject) {
            request.input('subject', sql.NVarChar, subject)
            query += ' WHERE LTRIM(RTRIM(SUBJECT)) = @subject'
        }
        const result = await request.query(query)
        return result.recordset
    },

    getTeachersByFaculty: async ({ faculty }) => {
        await poolConnect
        const request = pool.request()
        const result = await request
            .input('faculty', sql.NVarChar, faculty)
            .query(`
                SELECT t.TEACHER, t.TEACHER_NAME, t.PULPIT 
                FROM TEACHER t
                INNER JOIN PULPIT p ON t.PULPIT = p.PULPIT
                WHERE LTRIM(RTRIM(p.FACULTY)) = @faculty
            `)
        return [{ FACULTY: faculty, TEACHERS: result.recordset }]
    },

    getSubjectsByFaculties: async ({ faculty }) => {
        await poolConnect
        const request = pool.request()
        const result = await request
            .input('faculty', sql.NVarChar, faculty)
            .query(`
                SELECT p.PULPIT, p.PULPIT_NAME, p.FACULTY, s.SUBJECT, s.SUBJECT_NAME
                FROM PULPIT p
                LEFT JOIN SUBJECT s ON s.PULPIT = p.PULPIT
                WHERE LTRIM(RTRIM(p.FACULTY)) = @faculty
            `)
        const pulpitsMap = new Map()
        result.recordset.forEach(row => {
            if (!pulpitsMap.has(row.PULPIT)) {
                pulpitsMap.set(row.PULPIT, {
                    PULPIT: row.PULPIT,
                    PULPIT_NAME: row.PULPIT_NAME,
                    FACULTY: row.FACULTY,
                    SUBJECTS: []
                })
            }
            if (row.SUBJECT) {
                pulpitsMap.get(row.PULPIT).SUBJECTS.push({
                    SUBJECT: row.SUBJECT,
                    SUBJECT_NAME: row.SUBJECT_NAME,
                    PULPIT: row.PULPIT
                })
            }
        })
        return Array.from(pulpitsMap.values())
    },

    // === Mutation резолверы ===
    setFaculty: async ({ faculty, faculty_name }) => {
        await poolConnect
        const request = pool.request()
        const result = await request
            .input('faculty', sql.NVarChar, faculty)
            .input('faculty_name', sql.NVarChar, faculty_name)
            .query(`
                MERGE FACULTY AS target
                USING (SELECT @faculty AS FACULTY, @faculty_name AS FACULTY_NAME) AS source
                ON LTRIM(RTRIM(target.FACULTY)) = @faculty
                WHEN MATCHED THEN UPDATE SET FACULTY_NAME = source.FACULTY_NAME
                WHEN NOT MATCHED THEN INSERT (FACULTY, FACULTY_NAME) VALUES (source.FACULTY, source.FACULTY_NAME);
                SELECT * FROM FACULTY WHERE LTRIM(RTRIM(FACULTY)) = @faculty;
            `)
        return result.recordset[0]
    },

    setPulpit: async ({ pulpit, pulpit_name, faculty }) => {
        await poolConnect
        const request = pool.request()
        const result = await request
            .input('pulpit', sql.NVarChar, pulpit)
            .input('pulpit_name', sql.NVarChar, pulpit_name)
            .input('faculty', sql.NVarChar, faculty)
            .query(`
                MERGE PULPIT AS target
                USING (SELECT @pulpit AS PULPIT, @pulpit_name AS PULPIT_NAME, @faculty AS FACULTY) AS source
                ON LTRIM(RTRIM(target.PULPIT)) = @pulpit
                WHEN MATCHED THEN UPDATE SET PULPIT_NAME = source.PULPIT_NAME, FACULTY = source.FACULTY
                WHEN NOT MATCHED THEN INSERT (PULPIT, PULPIT_NAME, FACULTY) VALUES (source.PULPIT, source.PULPIT_NAME, source.FACULTY);
                SELECT * FROM PULPIT WHERE LTRIM(RTRIM(PULPIT)) = @pulpit;
            `)
        return result.recordset[0]
    },

    setTeacher: async ({ teacher, teacher_name, pulpit }) => {
        await poolConnect
        const request = pool.request()
        const result = await request
            .input('teacher', sql.NVarChar, teacher)
            .input('teacher_name', sql.NVarChar, teacher_name)
            .input('pulpit', sql.NVarChar, pulpit)
            .query(`
                MERGE TEACHER AS target
                USING (SELECT @teacher AS TEACHER, @teacher_name AS TEACHER_NAME, @pulpit AS PULPIT) AS source
                ON LTRIM(RTRIM(target.TEACHER)) = @teacher
                WHEN MATCHED THEN UPDATE SET TEACHER_NAME = source.TEACHER_NAME, PULPIT = source.PULPIT
                WHEN NOT MATCHED THEN INSERT (TEACHER, TEACHER_NAME, PULPIT) VALUES (source.TEACHER, source.TEACHER_NAME, source.PULPIT);
                SELECT * FROM TEACHER WHERE LTRIM(RTRIM(TEACHER)) = @teacher;
            `)
        return result.recordset[0]
    },

    setSubject: async ({ subject, subject_name, pulpit }) => {
        await poolConnect
        const request = pool.request()
        const result = await request
            .input('subject', sql.NVarChar, subject)
            .input('subject_name', sql.NVarChar, subject_name)
            .input('pulpit', sql.NVarChar, pulpit)
            .query(`
                MERGE SUBJECT AS target
                USING (SELECT @subject AS SUBJECT, @subject_name AS SUBJECT_NAME, @pulpit AS PULPIT) AS source
                ON LTRIM(RTRIM(target.SUBJECT)) = @subject
                WHEN MATCHED THEN UPDATE SET SUBJECT_NAME = source.SUBJECT_NAME, PULPIT = source.PULPIT
                WHEN NOT MATCHED THEN INSERT (SUBJECT, SUBJECT_NAME, PULPIT) VALUES (source.SUBJECT, source.SUBJECT_NAME, source.PULPIT);
                SELECT * FROM SUBJECT WHERE LTRIM(RTRIM(SUBJECT)) = @subject;
            `)
        return result.recordset[0]
    },

    // === DELETE с обработкой FK ===
    delFaculty: async ({ faculty }) => {
        await poolConnect;
        const request = pool.request()
        try {
            const result = await request
                .input('faculty', sql.NVarChar, faculty)
                .query('DELETE FROM FACULTY WHERE LTRIM(RTRIM(FACULTY)) = @faculty')
            return result.rowsAffected[0] > 0
        } catch (err) {
            if (err.number === 547) return false // FK violation
            throw err
        }
    },

    delPulpit: async ({ pulpit }) => {
        await poolConnect
        const request = pool.request()
        try {
            const result = await request
                .input('pulpit', sql.NVarChar, pulpit)
                .query('DELETE FROM PULPIT WHERE LTRIM(RTRIM(PULPIT)) = @pulpit')
            return result.rowsAffected[0] > 0
        } catch (err) {
            if (err.number === 547) return false
            throw err
        }
    },

    delTeacher: async ({ teacher }) => {
        await poolConnect
        const request = pool.request()
        try {
            const result = await request
                .input('teacher', sql.NVarChar, teacher)
                .query('DELETE FROM TEACHER WHERE LTRIM(RTRIM(TEACHER)) = @teacher')
            return result.rowsAffected[0] > 0
        } catch (err) {
            if (err.number === 547) return false
            throw err
        }
    },

    delSubject: async ({ subject }) => {
        await poolConnect
        const request = pool.request()
        try {
            const result = await request
                .input('subject', sql.NVarChar, subject)
                .query('DELETE FROM SUBJECT WHERE LTRIM(RTRIM(SUBJECT)) = @subject')
            return result.rowsAffected[0] > 0
        } catch (err) {
            if (err.number === 547) return false
            throw err
        }
    }
}

// === HTTP сервер и GraphQL интерфейс ===
const graphqlHTML = `
<!DOCTYPE html>
<html>
<head>
    <title>GraphQL</title>
    <style>
        body { font-family: Arial; margin: 20px; }
        textarea { width: 100%; height: 150px; margin: 10px 0; }
        button { padding: 10px 20px; background: #807a7e; color: white; border: none; cursor: pointer; }
        pre { background: #f4f4f4; padding: 10px; border-radius: 5px; }
    </style>
</head>
<body>
    <h1>GraphQL</h1>
    <textarea id="query" placeholder="Введите GraphQL запрос...">
{
  getFaculties {
    FACULTY
    FACULTY_NAME
  }
}</textarea><br>
    <button onclick="executeQuery()">Выполнить запрос</button>
    <h3>Результат:</h3>
    <pre id="result"></pre>

    <script>
        async function executeQuery() {
            const query = document.getElementById('query').value;
            try {
                const response = await fetch('/graphql', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ query })
                });
                const data = await response.json();
                document.getElementById('result').textContent = JSON.stringify(data, null, 2);
            } catch (error) {
                document.getElementById('result').textContent = 'Ошибка: ' + error.message;
            }
        }
    </script>
</body>
</html>
`;

const server = http.createServer(async (req, res) => {
    res.setHeader('Access-Control-Allow-Origin', '*')
    res.setHeader('Access-Control-Allow-Methods', 'GET, POST, OPTIONS')
    res.setHeader('Access-Control-Allow-Headers', 'Content-Type')

    if (req.method === 'OPTIONS') {
        res.writeHead(200)
        res.end()
        return
    }

    const parsedUrl = url.parse(req.url, true)
    const pathname = parsedUrl.pathname

    if (req.method === 'GET' && (pathname === '/' || pathname === '/graphql')) {
        res.writeHead(200, { 'Content-Type': 'text/html; charset=utf-8' })
        res.end(graphqlHTML)
        return
    }

    if (req.method === 'POST' && (pathname === '/' || pathname === '/graphql')) {
        let body = ''
        req.on('data', chunk => { body += chunk.toString() })
        req.on('end', async () => {
            try {
                const { query, variables, operationName } = JSON.parse(body)
                const result = await graphql({ schema, source: query, rootValue: root, variableValues: variables, operationName })
                res.writeHead(200, { 'Content-Type': 'application/json; charset=utf-8' })
                res.end(JSON.stringify(result))
            } catch (error) {
                res.writeHead(400, { 'Content-Type': 'application/json' })
                res.end(JSON.stringify({ errors: [{ message: 'Ошибка обработки запроса: ' + error.message }] }))
            }
        })
        return
    }

    res.writeHead(404, { 'Content-Type': 'application/json' })
    res.end(JSON.stringify({ error: 'Маршрут не найден' }))
})

const PORT = 3000
server.listen(PORT, () => {
    console.log(`GraphQL сервер запущен на http://localhost:${PORT}/graphql`)
})