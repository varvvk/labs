const net = require('net');

const client = net.createConnection({ port: 6000 });

let counter = 0;

const interval = setInterval(() => {
  counter++;
  client.write(counter.toString());
}, 1000);

client.on('data', (data) => {
  console.log(data.toString());
});

setTimeout(() => {
  clearInterval(interval);
  client.end();
}, 20000);