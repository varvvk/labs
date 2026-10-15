const express = require('express')
const passport = require('passport')
const DigestStrategy = require('passport-http').DigestStrategy
const Users = require('./users.json')

const app = express()

passport.use(new DigestStrategy({ qop: 'auth' },
    (user, done) => {
        const foundUser = Users.find(u => u.user === user)
        if (!foundUser) {
            return done(null, false)
        }
        return done(null, foundUser.user, foundUser.password)
    },
    (params, done) => {
        done(null, true)
    }
))

app.use(passport.initialize())

app.get('/login', (req, res) => {
    res.send(`
        <h2>DIGEST-аутентификация</h2>
        <p>Нажмите "Войти" для доступа к ресурсу</p>
        <button onclick="window.location.href='/resource'">Войти</button>
    `)
})

app.get('/resource', 
    passport.authenticate('digest', { session: false }),
    (req, res) => {
        res.send('RESOURCE')
    }
)

app.get('/logout', (req, res) => {
    res.redirect('/login')
})

app.use((req, res) => {
    res.status(404).send('404 - Страница не найдена')
})

app.listen(3000, () => console.log('http://localhost:3000/resource'))