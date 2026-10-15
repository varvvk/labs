var util = require('util');
var ee = require('events');

var db_data = [
    {id: 1, name: 'Ivanov I.I.', bday: '2001-01-01'},
    {id: 2, name: 'Petrov P.P.', bday: '2001-01-02'},
    {id: 3, name: 'Sidorov S.S.', bday: '2001-01-03'}
];

function DB() {
    this.get = ()=> {
        return db_data
    }

    this.post = row => {
        db_data.push(row)
    }

    this.put = row => {
        const index = db_data.findIndex(item => item.id == row.id)
        if (index === -1) {
            return false
        }
        db_data[index] = row
        return true
    }

    this.delete = id => {
        const index = db_data.findIndex(item => item.id == id)
        if (index === -1) {
            return false
        }
        const deletedRow = db_data.splice(index, 1)[0]
        return deletedRow
    }

    this.commit = () => {
        console.log('committing database');
        return true;
    }
}

util.inherits(DB, ee.EventEmitter)
exports.DB = DB