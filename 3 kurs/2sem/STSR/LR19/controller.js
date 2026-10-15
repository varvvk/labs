let users = [
    {id: 1, name: 'Lisa'},
    {id: 2, name: 'Kate'},
    {id: 3, name: 'Peter'}
]

const controller = {
    getAll: (req, res) => {
        res.json({users})
    },

    getOne: (req, res) => {
        const id = parseInt(req.params.id)
        const user = users.find(u=>u.id === id)
        if(!user) {
            return res.status(404).json({error: 'user not found'})
        }
        res.json({user})
    },

    create: (req, res) => {
        const {name} = req.body
        if(!name) {
            return res.status(400).json({error: 'name is required'})
        }
        const newId = users.length ? Math.max(...users.map(u=>u.id)) + 1 : 1
        const newUser = {id: newId, name}
        users.push(newUser)
        res.status(201).json({user: newUser})
    },

    update: (req, res) => {
        const id = parseInt(req.params.id)
        const {name} = req.body
        const user = users.find(u=>u.id === id)
        if(!user) {
            return res.status(404).json({error: 'user not found'})
        }
        if(name) {user.name = name}
        res.json({user})
    },

    delete: (req, res) => {
        const id = parseInt(req.params.id)
        const user = users.find(u=>u.id === id)
        const index = users.findIndex(u=>u.id === id)
        if(index === -1) {
            return res.status(404).json({error: 'user not found'})
        }
        const deletedUser = users.splice(index, 1)[0]
        res.json({ user: deletedUser })
    }
}

module.exports = controller