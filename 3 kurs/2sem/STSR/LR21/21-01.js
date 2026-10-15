const express = require('express')
const passport = require('passport')
const BasicStrategy = require('passport-http').BasicStrategy
const Users = require('./users.json')
const session = require('express-session')

const getCredential = (user) => {
    return Users.find((e) => e.user.toUpperCase() === user.toUpperCase())
}

const app = express()

app.use(session({
    resave: false,
    saveUninitialized: false,
    secret: '1111'
}))
app.use(passport.initialize())

passport.use(new BasicStrategy((user, password, done) => {
    console.log('Попытка входа:', user)
    const cr = getCredential(user)
    
    if (!cr) {
        return done(null, false, { message: 'Неверное имя пользователя' })
    }
    if (cr.password !== password) {
        return done(null, false, { message: 'Неверный пароль' })
    }
    return done(null, cr)
}))

app.get('/login',
    (req, res, next) => {
        if (req.session && req.session.logout) {
            req.session.logout = false
            delete req.headers['authorization']
        }
        next()
    },
    passport.authenticate('basic', { session: false }),
    (req, res) => {
        res.redirect('/resource')
    }
)

app.get('/logout', (req, res) => {
    req.session.logout = true
    res.redirect('/login')
})

app.get('/resource',
    passport.authenticate('basic', { session: false }),
    (req, res) => {
        res.send('RESOURCE')
    }
)

app.use((req, res) => {
    res.status(404).send('404 - Страница не найдена')
})

app.listen(3000, () => console.log('Сервер запущен: http://localhost:3000/resource'))