const udp = require('dgram')
const client = udp.createSocket('udp4')
const Commands = require('./commands')

client.bind(9999, '127.0.0.5')

client.on('message', (msg, info) => {
    console.log('Данные получены от сервера: ' + msg.toString())
})

setInterval(() => {
    const params = JSON.stringify({command: Commands.get})
    client.send(params, 5555, '127.0.0.1', (error) => {
        if(error) {
            console.log('Error sending request')
        } else {
            console.log('Requesting time...')
        }
    })
}, 5000)

