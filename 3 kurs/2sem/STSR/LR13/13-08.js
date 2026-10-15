const net = require('net');

const port = parseInt(process.argv[2]);

const client = net.createConnection({ port });

setInterval(() => {
  client.write('123');
}, 1000);

client.on('data', (data) => {
  console.log(data.toString());
});