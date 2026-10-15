//1
for(var i = 0; i<document.getElementsByTagName('*').length; i++) {
    console.log('nomer tega ' +(i+1) + ': ' + document.getElementsByTagName('*')[i].tagName);
}

//2
console.log(document.body.childNodes);
//console.log(document.body.children);

//3
console.log(document.getElementsByTagName('span')[0]);
console.log(document.body.querySelector('span'));
console.log(document.getElementById('firstSpan'));

//4
let table = document.getElementsByTagName('table')[1];
let math = table.getElementsByTagName('td');
let sum= 0;
for (let i = 0; i < math.length; i++) {
    sum += Number(math[i].textContent);
}
document.querySelector('.secondSpan').innerHTML += ' Средний балл: ' + sum/3;