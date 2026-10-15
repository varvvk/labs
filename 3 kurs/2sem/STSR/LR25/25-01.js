const express = require('express')
const bodyParser = require('body-parser')
const rpcServer = require('./rpc')

const app = express()
app.use(bodyParser.json())

app.post('/json-rpc', (req, res) => {
    const JSONRPCRequest = req.body
    rpcServer.receive(JSONRPCRequest)
    .then(jsonRPCResponse => {
        if(jsonRPCResponse) {
            res.json(jsonRPCResponse)
        } else {
            res.sendStatus(204)
        }
    })
    .catch(error => {
        res.status(500).json({
            jsonrpc: '2.0',
            error: {
                code: -32603,
                message: 'internal error',
                data: error.message
            },
            id: JSONRPCRequest.id || null
        })
    })
})

app.listen(3000, () => {
    console.log(`server is running at http://localhost:3000`)
})