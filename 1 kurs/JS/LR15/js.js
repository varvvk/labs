//1
const regex = /о.м/s;
const str1 = 'строка с переносо\nм';
console.log(regex.test(str1));

/*const regex = /о.м/s;
const str = 'строка с переносо\nм';
console.log(str.match(regex));*/

//2
var str2 = "Текст с [u]подчеркнутой[/u] частью.";
var newStr2 = str2.replace(/\[u\]([\s\S]*?)\[\/u\]/g, '<u>$1</u>');
console.log(newStr2); 

//3
var links = document.getElementsByTagName('a');
for(var i=0; i<links.length; i++){
    links[i].outerHTML = links[i].outerHTML.replace(/<a>/g, '<span>');
}

//4
let str4 = "firstName, lastName";
let modifiedString = str4.replace(/(\w+), (\w+)/, "$2, $1");
console.log(modifiedString);

//5
let pers = {
    firstName: 'John',
    lastName: 'Doe',
    age: 30,
    email: 'johndoe@example.com'
};
let {firstName, age, ...resttt} = pers;
console.log(resttt);

//6
const person = {amount:'$500', age:18};
const account = {sname:'Ban', fname:'Kate'};
const all = {...person, ...account};
console.log(all);
const {...rest} = person;
console.log(rest);
const new6 = {
    ...rest,
    fname: 'varvara',
    sname: 'kazakova'
};
console.log(new6);