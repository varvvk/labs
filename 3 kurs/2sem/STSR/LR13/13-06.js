const net = require('net');

const X = parseInt(process.argv[2]) || 1;

const client = net.createConnection({ port: 7000 });

setInterval(() => {
  client.write(X.toString());
}, 1000);

client.on('data', (data) => {
  console.log(data.toString());
});