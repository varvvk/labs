const http = require('http')

const PORT = 8080
let data = [{ id: 1, name: 'Казакова' }]

http.createServer((req, res) => {
    console.log(`${req.method} ${req.url}`)

    if (req.url === '/api/item' && req.method === 'GET') {
        res.writeHead(200, { 'content-type': 'application/json' })
        res.end(JSON.stringify(data))
        return
    }

    if (req.url === '/api/item' && req.method === 'POST') {
        let body = ''

        req.on('data', chunk => body += chunk)

        req.on('end', () => {
            const newItem = JSON.parse(body)

            data.push(newItem)

            res.writeHead(201, { 'content-type': 'application/json' })
            res.end(JSON.stringify(newItem))
        })

        return
    }

    if (req.url === '/api/item' && req.method === 'PUT') {
        let body = ''

        req.on('data', chunk => body += chunk)

        req.on('end', () => {
            const updated = JSON.parse(body)

            const index = data.findIndex(i => i.id === updated.id)

            if (index === -1) {
                res.writeHead(404, { 'content-type': 'application/json' })
                res.end(JSON.stringify({ error: 'Item not found' }))
                return
            }

            data[index] = { ...data[index], ...updated }

            res.writeHead(200, { 'content-type': 'application/json' })
            res.end(JSON.stringify(data[index]))
        })

        return
    }

    if (req.url.startsWith('/api/item/') && req.method === 'DELETE') {
        const id = parseInt(req.url.split('/')[3])

        const index = data.findIndex(i => i.id === id)

        if (index === -1) {
            res.writeHead(404, { 'content-type': 'application/json' })
            res.end(JSON.stringify({ error: 'Item not found' }))
            return
        }

        const deleted = data.splice(index, 1)[0]

        res.writeHead(200, { 'content-type': 'application/json' })
        res.end(JSON.stringify(deleted))
        return
    }

    res.writeHead(404, { 'content-type': 'text/plain; charset=utf-8' })
    res.end('Resource not found')
}).listen(PORT, () => console.log(`Server is running on http://localhost:${PORT}/`))
