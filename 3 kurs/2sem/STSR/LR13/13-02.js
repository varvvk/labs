const net = require('net');
const readline = require('readline');

const client = net.createConnection({ port: 5000 }, () => {
  console.log('Connected to server');
  process.stdout.write('> ');
});

const rl = readline.createInterface({
  input: process.stdin,
  output: process.stdout
});

rl.on('line', (input) => {
  client.write(input);
});

client.on('data', (data) => {
  process.stdout.clearLine();
  process.stdout.cursorTo(0);

  console.log('Server:', data.toString().trim());
  process.stdout.write('> ');
});