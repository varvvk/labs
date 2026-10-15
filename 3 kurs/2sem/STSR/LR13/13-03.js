const net = require('net');

  let sum = 0;
  
const server = net.createServer((socket) => {

  console.log('Client connected');

  const interval = setInterval(() => {
    if (!socket.destroyed) {
      socket.write('SUM: ' + sum + '\n');
    }
  }, 5000);

  socket.on('data', (data) => {
    const num = parseInt(data.toString().trim());
    if (!isNaN(num)) sum += num;
  });

  socket.on('close', () => {
    console.log('Client disconnected');
    clearInterval(interval);
  });

  socket.on('error', (err) => {
    console.log('Socket error:', err.message);
    clearInterval(interval);
  });
});

server.listen(6000, () => {
  console.log('Server on 6000');
});