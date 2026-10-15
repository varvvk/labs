//1
let arr = [];
for (let i = 0; i < 10; i++) {
    arr.push(Math.floor(Math.random() * 100));
}
console.log("Массив случайных чисел:", arr);

let num = parseInt(prompt("Введите число для проверки:"));
    
if (arr.includes(num)) {
    console.log(`Число ${num} существует в массиве.`);
} else {
    console.log(`Числа ${num} нет в массиве.`);
}

//2
let array = [];
for (let i = 0; i < 20; i++) {
    arr.push(Math.floor(Math.random() * 100));
}
array.push(NaN);

if (array.indexOf(NaN)!==-1 ) {
    console.log("Элемент NaN есть (с использованием indexOf) <br>");
} else {
    console.log("Элемента NaN нет (с использованием indexOf) <br>");
}

if (array.includes(NaN)) {
    console.log("Элемент NaN есть (с использованием Array.prototype.includes) <br>"); 
} else {
    console.log("Элемента NaN нет (с использованием Array.prototype.includes) <br>");  
}

//3
let arr1 = [];
for (let i = 0; i < 10; i++) {
    arr1.push(Math.floor(Math.random() * 50));
}
console.log("Массив случайных чисел:", arr1);
let arr2 = [];
for (let i = 0; i < 10; i++) {
    arr2.push(Math.floor(Math.random() * 50));
}
console.log("Массив случайных чисел:", arr2);
let arr3 = [];
for (let i = 0; i < 10; i++) {
    arr3.push(Math.floor(Math.random() * 50));
}
console.log("Массив случайных чисел:", arr3);
let num1 = parseInt(prompt("Введите  второе число для проверки:"));
if (arr1.includes(num1) || arr2.includes(num1) || arr3.includes(num1)) {
    console.log(`Число ${num1} существует в массиве(-ах).`);
} else {
    console.log(`Числа ${num1} нет в массивах.`);
}

//4
let result1=Math.round(1234**2*23/5 +234);
let result2=Math.round(28*765**2/5* 43**2);
let result3=Math.round(3734**6-(436**6+1024**4)/5);
let result4=Math.round(6543**35-965732);
let result5=Math.round((1000**10+100000**10)/10000);
console.log(result1);
console.log(result2);
console.log(result3);
console.log(result4);
console.log(result5);