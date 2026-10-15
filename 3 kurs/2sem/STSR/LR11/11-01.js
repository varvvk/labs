const WebSocket = require("ws");

const fs = require("fs");

let k = 0;

const wss = new WebSocket.Server({ port: 4000 }, () => {
  console.log('Сервер запущен на ws://localhost:4000');
});

wss.on("connection", (socket) => {
  console.log("Клиент подключился");

  const duplex = WebSocket.createWebSocketStream(socket);

  const wfile = fs.createWriteStream(`./upload/file${++k}.txt`);
  duplex.pipe(wfile);


  wfile.on("finish", () => {
    console.log(`Файл сохранён: ./upload/file${k}.txt`);

    socket.close();
  });

  duplex.on("error", (err) => {
    console.error("Ошибка duplex:", err.message);
  });
});

wss.on("error", (e) => {
  console.error("Ошибка сервера:", e.message);
});
