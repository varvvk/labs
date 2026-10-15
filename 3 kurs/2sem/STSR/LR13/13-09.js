const dgram = require('dgram');

const server = dgram.createSocket('udp4');

server.on('message', (msg, rinfo) => {
  const response = Buffer.from('ECHO: ' + msg.toString());

  server.send(response, rinfo.port, rinfo.address);
});

server.bind(8000);