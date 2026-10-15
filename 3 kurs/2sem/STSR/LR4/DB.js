var util = require('util');
var ee = require('events');

var db_data = [
    {
        id: 1,
        name: 'Varvara',
        bday: '2005-10-19'
    },
    {
        id: 2,
        name: 'Lena',
        bday: '2004-10-20'
    },
    {
        id: 3,
        name: 'Nastya',
        bday: '2004-10-26'
    },
    {
        id: 4,
        name: 'Dasha',
        bday: '2004-10-06'
    }
];

function DB() {
    this.select = () => { return db_data; };
    this.insert = (par) => { db_data.push(par); };
    this.update = (par) => {
        let index = db_data.findIndex(x => x.id === par.id);
        if (index!== -1) 
            db_data[index] = par;
    };
    this.delete = (par) => {
        let index = db_data.findIndex(x => x.id === par);
        if (index!== -1) 
           return db_data.splice(index, 1);
        return null;
    } 
}

util.inherits(DB, ee.EventEmitter);

exports.DB = DB;