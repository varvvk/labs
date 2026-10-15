const net = require('net');

function createServer(port) {
  const server = net.createServer((socket) => {

    console.log(`Client connected on port ${port}`);

    socket.on('data', (data) => {
      const msg = data.toString().trim();
      socket.write('ECHO: ' + msg);
    });

    socket.on('close', () => {
      console.log('Client disconnected');
    });

    socket.on('error', (err) => {
      console.log('Socket error:', err.message);
    });

  });

  server.listen(port, () => {
    console.log('Server on port', port);
  });
}

createServer(4000);
createServer(5000);