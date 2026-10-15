// server.js
const net = require('net');

const server = net.createServer((socket) => {
  console.log('Client connected');

  socket.on('data', (data) => {
    const msg = data.toString().trim();
    console.log('Received:', msg);

    socket.write('ECHO: ' + msg);
  });

  socket.on('end', () => {
    console.log('Client disconnected');
  });
});

server.listen(5000, () => {
  console.log('TCP server running on port 5000');
});