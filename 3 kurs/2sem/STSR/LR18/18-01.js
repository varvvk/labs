import http from 'http';
import fs from 'fs';
import path from 'path';
import url from 'url';
import { Sequelize, DataTypes } from 'sequelize';


// Подключение к базе
const sequelize = new Sequelize('KVV', 'student', 'Pa$$w0rd', {
  host: '80.94.224.209',
  dialect: 'mssql',
  logging: false,
  pool: { max: 10, min: 0, acquire: 30000, idle: 30000 },
  dialectOptions: { options: { encrypt: false, trustServerCertificate: true } }
});


// Модели
const Faculty = sequelize.define('Faculty', {
  FACULTY: { type: DataTypes.STRING(10), primaryKey: true },
  FACULTY_NAME: DataTypes.STRING(50)
}, { tableName: 'FACULTY', timestamps: false });

const Pulpit = sequelize.define('Pulpit', {
  PULPIT: { type: DataTypes.STRING(10), primaryKey: true },
  PULPIT_NAME: DataTypes.STRING(50),
  FACULTY: DataTypes.STRING(10)
}, { tableName: 'PULPIT', timestamps: false });

const Subject = sequelize.define('Subject', {
  SUBJECT: { type: DataTypes.STRING(10), primaryKey: true },
  SUBJECT_NAME: DataTypes.STRING(50),
  PULPIT: DataTypes.STRING(10)
}, { tableName: 'SUBJECT', timestamps: false });

const AuditoriumType = sequelize.define('AuditoriumType', {
  AUDITORIUM_TYPE: { type: DataTypes.STRING(10), primaryKey: true },
  AUDITORIUM_TYPENAME: DataTypes.STRING(50)
}, { tableName: 'AUDITORIUM_TYPE', timestamps: false });

const Auditorium = sequelize.define('Auditorium', {
  AUDITORIUM: { type: DataTypes.STRING(10), primaryKey: true },
  AUDITORIUM_TYPE: DataTypes.STRING(10),
  AUDITORIUM_CAPACITY: DataTypes.INTEGER,
  AUDITORIUM_NAME: DataTypes.STRING(50)
}, { tableName: 'AUDITORIUM', timestamps: false });


// Вспомогательные функции
function getBody(req) {
  return new Promise((resolve, reject) => {
    let body = '';
    req.on('data', chunk => body += chunk);
    req.on('end', () => resolve(JSON.parse(body)));
    req.on('error', reject);
  });
}

function send(res, status, data) {
  res.writeHead(status, { 'Content-Type': 'application/json' });
  res.end(JSON.stringify(data));
}

// Обрезка пробелов у ключевых полей
function trimKeys(obj, keys) {
  keys.forEach(k => { if(obj[k] && typeof obj[k] === 'string') obj[k] = obj[k].trim(); });
  return obj;
}

// Универсальные функции для PUT и DELETE
async function updateAndReturn(model, keyField, body, updateFields, res) {
  body = trimKeys(body, [keyField,...updateFields]);
  const record = await model.findOne({ where: { [keyField]: body[keyField] } });
  if (!record) return send(res, 404, { error: `${model.name} not found` });
  const dataToUpdate = {};
  updateFields.forEach(f => { dataToUpdate[f] = body[f]; });
  await record.update(dataToUpdate);
  return send(res, 200, record);
}

async function deleteAndReturn(model, keyField, id, res) {
  id = id.trim();
  const record = await model.findOne({ where: { [keyField]: id } });
  if (!record) return send(res, 404, { error: `${model.name} not found` });
  await record.destroy();
  return send(res, 200, record);
}

// HTTP сервер
const PORT = 3000;
const server = http.createServer(async (req, res) => {
  const parsedUrl = url.parse(req.url,true);
  const pathname = parsedUrl.pathname;

  try {

    // GET таблицы
    if(req.method==='GET'){
      if(pathname==='/api/faculties') return send(res,200,await Faculty.findAll());
      if(pathname==='/api/pulpits') return send(res,200,await Pulpit.findAll());
      if(pathname==='/api/subjects') return send(res,200,await Subject.findAll());
      if(pathname==='/api/auditoriumstypes') return send(res,200,await AuditoriumType.findAll());
      if(pathname==='/api/auditoriums') return send(res,200,await Auditorium.findAll());
    }

    // POST таблицы
    if(req.method==='POST'){
      let body = await getBody(req);
      body = trimKeys(body,['FACULTY','FACULTY_NAME','PULPIT','PULPIT_NAME','SUBJECT','SUBJECT_NAME','AUDITORIUM_TYPE','AUDITORIUM_TYPENAME','AUDITORIUM','AUDITORIUM_NAME']);
      if(pathname==='/api/faculties') return send(res,200,await Faculty.create(body));
      if(pathname==='/api/pulpits') return send(res,200,await Pulpit.create(body));
      if(pathname==='/api/subjects') return send(res,200,await Subject.create(body));
      if(pathname==='/api/auditoriumstypes') return send(res,200,await AuditoriumType.create(body));
      if(pathname==='/api/auditoriums') return send(res,200,await Auditorium.create(body));
    }

    // PUT таблицы
    if(req.method==='PUT'){
      let body = await getBody(req);
      if(pathname==='/api/faculties') return updateAndReturn(Faculty,'FACULTY',body,['FACULTY_NAME'],res);
      if(pathname==='/api/pulpits') return updateAndReturn(Pulpit,'PULPIT',body,['PULPIT_NAME','FACULTY'],res);
      if(pathname==='/api/subjects') return updateAndReturn(Subject,'SUBJECT',body,['SUBJECT_NAME','PULPIT'],res);
      if(pathname==='/api/auditoriumstypes') return updateAndReturn(AuditoriumType,'AUDITORIUM_TYPE',body,['AUDITORIUM_TYPENAME'],res);
      if(pathname==='/api/auditoriums') return updateAndReturn(Auditorium,'AUDITORIUM',body,['AUDITORIUM_NAME','AUDITORIUM_TYPE','AUDITORIUM_CAPACITY'],res);
    }

    // DELETE таблицы
    if(req.method==='DELETE'){
      if(pathname.startsWith('/api/faculties/')) return deleteAndReturn(Faculty,'FACULTY',decodeURIComponent(pathname.split('/')[3]),res);
      if(pathname.startsWith('/api/pulpits/')) return deleteAndReturn(Pulpit,'PULPIT',decodeURIComponent(pathname.split('/')[3]),res);
      if(pathname.startsWith('/api/subjects/')) return deleteAndReturn(Subject,'SUBJECT',decodeURIComponent(pathname.split('/')[3]),res);
      if(pathname.startsWith('/api/auditoriumstypes/')) return deleteAndReturn(AuditoriumType,'AUDITORIUM_TYPE',decodeURIComponent(pathname.split('/')[3]),res);
      if(pathname.startsWith('/api/auditoriums/')) return deleteAndReturn(Auditorium,'AUDITORIUM',decodeURIComponent(pathname.split('/')[3]),res);
    }

    send(res,404,{error:'Not found'});

  } catch(err){
    send(res,500,{error:err.message});
  }
});

// запуск сервера
sequelize.authenticate()
  .then(()=>{console.log('Connected to DB via Sequelize'); server.listen(PORT,()=>console.log(`Server running on port ${PORT}`));})
  .catch(err=>console.error('DB connection error:',err));