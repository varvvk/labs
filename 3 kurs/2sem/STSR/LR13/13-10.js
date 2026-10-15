const dgram = require('dgram');

const client = dgram.createSocket('udp4');

setInterval(() => {
  const msg = Buffer.from('Hello UDP');

  client.send(msg, 8000, '127.0.0.1');
}, 1000);

client.on('message', (msg) => {
  console.log(msg.toString());
});