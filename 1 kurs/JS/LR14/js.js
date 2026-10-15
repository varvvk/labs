//1
const info = {  country:"Belarus", region:"Vitebsk region", city: "Glubokoye"};
const infoV = Object.keys(info).map(key => info [key] );
console.log ("С помощью keys: " + infoV);
const values = Object.values(info);
console.log("С помощью values: " + values);

//2
const list=[0, 1, 12, 123, 1234, 12345];

console.log("Список до выравнивания: " + list);
const formatted = list.map(num=>
num.toString().padStart(9, '1' ).padEnd(15,'2'));
console.log("Список после выравнивания:" + formatted);

//3
let namee = 'Varvara';
let surname = 'Kazakova';
let paddedName = namee.padStart(11, '*'); 
let paddedSurname = surname.padEnd(10, '18'); 
console.log(paddedName + ' ' + paddedSurname);

//4
let charArray = ['a', 'b', 'c', 'd', 'e', 'f', 'g'];

let modifiedArray = ['1', '1', '1', ...charArray, '0'.padStart(4, '    ')];
console.log(modifiedArray);

//5
let inf = {
    name: 'Tom',
    age: '20',
    city: 'Paris'
};
const ob = Object.getOwnPropertyDescriptors(inf);
console.log(ob.city.value);

//6
let person = {};
Object.defineProperty(person, 'name', {
  value: 'Sam',
  writable: false,
  enumerable: true,
  configurable: true
});

Object.defineProperty(person, 'age', {
  value: 20,
  writable: true,
  enumerable: true,
  configurable: true
});
console.log(person.name+ ' ' +person.age);

person.name='Tom';
delete person.age;
person.age='25';
console.log(person.name+ ' ' +person.age);