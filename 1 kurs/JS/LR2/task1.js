function messageWrite1()
{
    alert ('Вас приветствует учебный центр');
    var person=prompt ('Введите имя');
    alert (`Добро пожаловать на наши курсы, ` + person);
    if(confirm ('Хотите стать Web-дизайнером?')) 
        alert('Учите стили CSS и JavaScript!');
    else
        alert ('Упускаете время!');
}

var messageWrite2=() =>
{
var string1 = '10';
var string2 = '5';
var string3 = '22';
console.log(10 + 5);
console.log(Number(string1) + Number(string2));
document.write('<p style="color: red">'+(22 + Number(string2)) +'</p>');
document.write('<br />')
document.write('<p style="color: blue">'+(Number(string3) + 5) +'</p>');
alert('Результатом сложения строки и числа всегда будет строка');
}

function messageWrite3()
{
    var x = 2;
    var y = 200;
    var result = ( 35 * y - 25 * x ) / 5 + 232;
    var result2 = 8 * y / x + 5 * x / y - 43 * 6;
    var result3 = result % result2;
    document.write('<p style="color: brown"> Результат:' + result + '</p>');
    document.write('<br />');
    document.write('<p style="color: red"> Результат:' + result2 + '</p>');
    document.write('<br />');
    document.write('<p style="color: green"> Результат:' + result3 + '</p>');
    alert(result3);
}

function messageWrite4()
{
    var chislo = prompt ('Введите число');
    if (chislo != 15 && chislo < 20 || chislo > 40 && chislo % 5 == 0)
    {
        alert('Правильное значение');
    }
    else {alert('Неправильное значение');} 
}

function messageWrite5()
{
    var chislo = prompt ('Введите первое число');
    var chislo2 = prompt ('Введите второе число');
    if (chislo > chislo2)
    {
        alert ('Первое число больше второго');
    }
    else if (chislo < chislo2)
    {
        alert('Второе число больше первого');}
    else {
        alert ('Числа равны');
    }
    var kyrs = prompt ('На каком ты курсе?');
    var message = (kyrs == 1) ? 'Привет' : 'Здравствуй';
    alert (message); 
}

function messageWrite6()
{
var now = new Date();
var day = now.getDay()
switch (day)
{   case 0:
    alert('Воскресенье');
    break;
    case 1:
    alert('Понедельник');
    break;
    case 2:
    alert('Вторник');
    break;
    case 3:
    alert('Среда');
    break;
    case 4:
    alert('Четверг');
    break;
    case 5:
    alert('Пятница');
    break;
    case 6:
    alert('Суббота');
    break;}
}

function messageWrite7()
{
var x = 0x456cdf;
var y = 0b10010;
var z = -0o5673;
document.write(x);
document.write('<br />');
document.write(y);
document.write('<br />');
document.write(z);
}