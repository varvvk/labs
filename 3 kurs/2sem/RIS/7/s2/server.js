const udp = require("dgram")
const server = udp.createSocket("udp4")
const port = 5555
const config = require("./config.json")
const Commands = require("./commands")

let addressLocal = config.localServer
let lastOctet = parseInt(addressLocal.split(".")[3])
let allServers = config.servers
let coordinator = null
let timerWaitRejects = null
let timerWaitPong = null
let pingInterval = null
let missedPings = 0
let highestServer = null

function getFormattedTime() {
    const now = new Date()
    const dd = String(now.getDate()).padStart(2, '0')
    const mm = String(now.getMonth() + 1).padStart(2, '0')
    const yyyy = now.getFullYear()
    const hours = String(now.getHours()).padStart(2, '0')
    const minutes = String(now.getMinutes()).padStart(2, '0')
    const seconds = String(now.getSeconds()).padStart(2, '0')
    return `${dd}${mm}${yyyy}:${hours}:${minutes}:${seconds}`
}

function startPingCoordinator() {
    console.log("PING TIMER WORKS")
    clearInterval(pingInterval)
    if (coordinator && coordinator !== addressLocal) {
        missedPings = 0
        pingInterval = setInterval(() => pingCoordinator(), 5000)
        console.log(`Started pinging coordinator ${coordinator}`)
    }
}

server.on("message", (msg, info) => {//получение сообщений от других серверов
    if (info.address === addressLocal) return

    console.log(`Data received from ${info.address}:${info.port} : ${msg.toString()}`)
    
    let obj
    try {
        obj = JSON.parse(msg.toString())
    } catch (e) {
        console.log("Invalid JSON received")
        return
    }

    switch(obj.command) {
        case Commands.voting:
            if (parseInt(info.address.split(".")[3]) < lastOctet) {
                const okMsg = JSON.stringify({ command: Commands.ok })
                server.send(okMsg, 5555, info.address, (err) => {
                    if (!err) console.log(`OK sent to ${info.address}`)
                })
            }
            break

        case Commands.ok:
            clearTimeout(timerWaitRejects)
            console.log(`Received OK, cancelling own voting`)
            break

        case Commands.won:
            console.log(`Received WON from ${info.address}`)
            setCoordinator(info.address)
            startPingCoordinator()
            break

        case Commands.ping:
            const pongMsg = JSON.stringify({ command: Commands.pong })
            server.send(pongMsg, 5555, info.address, (err) => {
                if (!err) console.log(`Pong sent to ${info.address}`)
            })
            break

        case Commands.pong:
            missedPings = 0
            clearTimeout(timerWaitPong)
            console.log(`Pong received from ${info.address}`)
            break

        case Commands.get:
            const timeMsg = JSON.stringify({
                command: Commands.time,
                time: getFormattedTime()
            })
            server.send(timeMsg, 5555, config.mainServer, (err) => {
                if (!err) console.log(`Time sent to main server`)
            })
            break
    }
})

function setCoordinator(ip) {
    coordinator = ip
    highestServer = ip
    console.log(`Coordinator set to ${ip}`)
    
    const connectMsg = JSON.stringify({ command: Commands.connect, ip: ip })
    server.send(connectMsg, 5555, config.mainServer, (err) => {
        if (!err) console.log(`Sent connect to main server`)
    })
}

function pingCoordinator() {
    if (!coordinator) return
    console.log(`Pinging coordinator ${coordinator}`)
    const pingMsg = JSON.stringify({ command: Commands.ping })
    
    server.send(pingMsg, 5555, coordinator, (err) => {
        if (err) {
            console.log(`Error sending ping: ${err}`)
            handlePingFailure()
        } else {
            clearTimeout(timerWaitPong);
            timerWaitPong = setTimeout(() => handlePingFailure(), 2000)
        }
    })
}

function handlePingFailure() {
    missedPings++
    console.log(`Missed pings: ${missedPings}`)
    if (missedPings >= 3) {
        console.log(`Coordinator ${coordinator} failed after 3 missed pings. Starting voting.`)
        clearInterval(pingInterval)
        coordinator = null
        missedPings = 0
        voting()
    }
}

function voting() {
    console.log(`Initiating voting (my IP: ${addressLocal}, last octet: ${lastOctet})`)
    const votingMsg = JSON.stringify({ command: Commands.voting })
    
    const higherServers = allServers.filter(addr => {
        const octet = parseInt(addr.split(".")[3])
        return octet > lastOctet
    })

    if (higherServers.length === 0) {
        console.log(`No higher servers. I am the winner.`)
        setCoordinator(addressLocal)
        const wonMsg = JSON.stringify({ command: Commands.won })
        allServers.forEach(addr => {
            if (addr !== addressLocal) {
                server.send(wonMsg, 5555, addr, (err) => {
                    if (!err) console.log(`Won message sent to ${addr}`)
                })
            }
        })
        return
    }

    higherServers.forEach(addr => {
        server.send(votingMsg, 5555, addr, (err) => {
            if (!err) console.log(`Vote sent to ${addr}`)
        })
    })

    clearTimeout(timerWaitRejects)
    timerWaitRejects = setTimeout(() => {
        console.log(`No OK received. I am the winner.`)
        setCoordinator(addressLocal)
        const wonMsg = JSON.stringify({ command: Commands.won })
        allServers.forEach(addr => {
            if (addr !== addressLocal) {
                server.send(wonMsg, 5555, addr, (err) => {
                    if (!err) console.log(`Won message sent to ${addr}`)
                })
            }
        })
    }, 5000)
}

server.on("listening", () => {
    console.log(`Server SVV is listening on ${addressLocal}:${port}`)
    
    const maxOctet = Math.max(...allServers.map(ip => parseInt(ip.split(".")[3])))
    highestServer = allServers.find(ip => parseInt(ip.split(".")[3]) === maxOctet)
    
    if (lastOctet === maxOctet) {
        console.log(`I have the highest number. I am the initial coordinator.`)
        setCoordinator(addressLocal)
    } else {
        console.log(`I'm not the highest. Highest is ${maxOctet}. Waiting for coordinator announcement.`)
        setCoordinator(highestServer)
        startPingCoordinator()
    }
})

server.on("close", () => console.log("Socket is closed !"))
server.on("error", (error) => console.log("Error: " + error))

server.bind(port, addressLocal)