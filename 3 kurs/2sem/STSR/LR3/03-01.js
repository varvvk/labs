// norm, stop, test, idle

const http = require('http');
let currentState = 'norm';

http.createServer((req, res) => {
    res.writeHead(200, { 'Content-Type': 'text/html; charset=utf-8' });
    res.end(`
        <html>
            <head>
            </head>
            <body>
                <h1>состояние: ${currentState}</h1>
            </body>
        </html>
    `);
}).listen(5000);

process.stdin.on('readable', () => {

    let chunk = null;

    while ((chunk = process.stdin.read()) !== null) {

        const input = chunk.toString().trim();

        if (input === 'exit') {
            console.log('Приложение завершено');
            process.exit();
        }
        else if (['norm', 'stop', 'test', 'idle'].includes(input)) {
            console.log(`${currentState}->${input}`);
            currentState = input;
        } 
        else {
            console.log(`"${input}"`);
        }
    }
});