let map = new Map();
map.set('1', '+375(29)456-67-78')
    .set('2', '+375(33)908-67-78')
    .set('3', '+375(29)843-98-78')
    .set('4', '+375(29)156-67-43')
    .set('5', '+375(33)456-40-78')
function findPhoneNumberByKey() {
    let key = prompt("Введите ключ (1-5):");
    if (map.has(key)) {
        alert(`Номер телефона: ${map.get(key)}`);
    } else {
        alert("Такого ключа не существует в телефонной книге");
    }
}
findPhoneNumberByKey();


let set = new Set();
set.add(1).add(3).add(5);
console.log(set.size);
console.log(set.has(1));
console.log(set.has(3));
console.log(set.has(5));


let proto = {
    name: 'Varvara',
    surname: 'Kazakova',
    patronymic: 'Vadzimovna',
    info: function() 
    {
        document.writeln(this.name + " " + this.surname + " " + this.patronymic);
    }     
}
let proto2 = Object.create(proto);
proto2.name='Natasha';
proto.info();
proto2.info();

function greet(name = "Аноним") {
    alert("Привет, " + name);
}
const getname = prompt("введите ваше имя");
if (getname===''){
    greet()
}else {
    greet(getname)
}