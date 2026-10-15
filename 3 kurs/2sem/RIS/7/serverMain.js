const udp = require('dgram')
const server = udp.createSocket('udp4')
const port = 5555
const host = '0.0.0.0'
const config = require('./config.json')
const Commands = require('./commands')

let coordinator = null
let awaitingClients = []

server.on('message', (msg, info) => {
    let obj
    try {
        obj = JSON.parse(msg.toString())
    } catch (e) {
        console.log("Invalid JSON")
        return;
    }

    if (obj.command === Commands.get) {
        console.log(`получен запрос времени от клиента ${info.address}:${info.port}`)
        
        if (coordinator) {
            const params = JSON.stringify({ command: Commands.get })
            server.send(params, 5555, coordinator, (error) => {
                if (error) {
                    console.log(`ошибка в отправке запроса координатору ${coordinator}: ${error}`)
                } else {
                    console.log(`переадресованный запрос от ${info.address}:${info.port} к координатору ${coordinator}`)
                }
            })
            awaitingClients.push({ port: info.port, address: info.address })
        } else {
            console.log('координатор не инициализирован. Не удается обработать запрос.')
        }
    } 
    else if (obj.command === Commands.connect) {
        coordinator = obj.ip
        console.log(`[PROTOCOL] Координатор обновлен: ${coordinator}`)
    } 
    else if (obj.command === Commands.time) {
        console.log(`Получено время от координатора: ${obj.time}`)
        if (awaitingClients.length === 0) {
            console.log('Нет ожидающих клиентов.')
            return;
        }
        
        const clientsToServe = [...awaitingClients]
        awaitingClients = []
        
        for (const client of clientsToServe) {
            server.send(obj.time, client.port, client.address, (error) => {
                if (error) {
                    console.log(`[PROTOCOL] Error sending time to client ${client.address}:${client.port}: ${error}`)
                } else {
                    console.log(`[PROTOCOL] Время отправлено клиенту ${client.address}:${client.port}`)
                }
            });
        }
    }
});

server.on('listening', () => {
    console.log(`[PROTOCOL] Mediator server is listening on ${config.mainServer}:${port}`)
})

server.on('close', () => console.log('Mediator socket is closed'))
server.on('error', (error) => console.log('Mediator error: ' + error))

server.bind(port, config.mainServer)