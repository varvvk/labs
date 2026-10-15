const express = require('express');
const fs = require('fs');
const path = require('path');
const WebSocket = require('ws');

const app = express();
app.use(express.json());

const FILE = path.join(__dirname, 'StudentList.json');
const BACKUP_DIR = path.join(__dirname, 'backups');

// WebSocket сервер
const wss = new WebSocket.Server({ port: 5000 });
let clients = [];

wss.on('connection', (ws) => {
  clients.push(ws);
  ws.on('close', () => {
    clients = clients.filter(c => c !== ws);
  });
});

function notify(msg) {
  clients.forEach(c => c.send(JSON.stringify(msg)));
}

// --- Вспомогательные функции ---
function readData() {
  try {
    return JSON.parse(fs.readFileSync(FILE));
  } catch {
    return null;
  }
}

function writeData(data) {
  fs.writeFileSync(FILE, JSON.stringify(data, null, 2));
}

// --- 1. GET / ---
app.get('/', (req, res) => {
  const data = readData();
  if (!data) {
    return res.json({ error: 1, message: 'Ошибка чтения файла' });
  }
  res.json(data);
});

// --- 7. GET /backup ---
app.get('/backup', (req, res) => {
  const files = fs.readdirSync(BACKUP_DIR);
  res.json(files);
});

// --- 2. GET /:id ---
app.get('/:id', (req, res) => {
  const data = readData();
  const student = data.find(s => s.id == req.params.id);

  if (!student) {
    return res.json({ error: 2, message: 'Студент не найден' });
  }

  res.json(student);
});

// --- 3. POST / ---
app.post('/', (req, res) => {
  const data = readData();
  const exists = data.find(s => s.id == req.body.id);

  if (exists) {
    return res.json({ error: 3, message: 'Студент уже существует' });
  }

  data.push(req.body);
  writeData(data);

  notify({ event: 'added', student: req.body });

  res.json(req.body);
});

// --- 4. PUT / ---
app.put('/', (req, res) => {
  let data = readData();
  let index = data.findIndex(s => s.id == req.body.id);

  if (index === -1) {
    return res.json({ error: 2, message: 'Студент не найден' });
  }

  data[index] = req.body;
  writeData(data);

  notify({ event: 'updated', student: req.body });

  res.json(req.body);
});

// --- 5. DELETE /:id ---
app.delete('/:id', (req, res) => {
  let data = readData();
  let index = data.findIndex(s => s.id == req.params.id);

  if (index === -1) {
    return res.json({ error: 2, message: 'Студент не найден' });
  }

  const removed = data.splice(index, 1)[0];
  writeData(data);

  notify({ event: 'deleted', student: removed });

  res.json(removed);
});

// --- 6. POST /backup ---
app.post('/backup', (req, res) => {
  setTimeout(() => {
    const now = new Date();
    const name = now.toISOString().replace(/[-:T]/g, '').slice(0, 12);
    const fileName = `${name}_StudentList.json`;

    fs.copyFileSync(FILE, path.join(BACKUP_DIR, fileName));

    notify({ event: 'backup_created', file: fileName });
  }, 2000);

  res.json({ message: 'Backup started (2 sec delay)' });
});

// --- 8. DELETE /backup/:date ---
app.delete('/backup/:date', (req, res) => {
  const dateLimit = req.params.date;

  const files = fs.readdirSync(BACKUP_DIR);

  let deleted = [];

  files.forEach(file => {
    const fileDate = file.split('_')[0];

    if (fileDate < dateLimit) {
      fs.unlinkSync(path.join(BACKUP_DIR, file));
      deleted.push(file);
    }
  });

  notify({ event: 'backup_deleted', files: deleted });

  res.json(deleted);
});

// --- запуск ---
app.listen(3000, () => {
  console.log('HTTP server: http://localhost:3000');
  console.log('WS server: ws://localhost:5000');
});