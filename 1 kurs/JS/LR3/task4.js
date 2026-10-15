arr = ['pow', 'pop', 'push', 'shift', 'round', 'floor', 'sline', 'sort'];
let MathMethods = [];
let ArrayMethods = [];
for (let i = 0; i < arr.length; i++) {
	if (Math.hasOwnProperty(arr[i])) 
		MathMethods.push(arr[i]);
	else if (Array.prototype.hasOwnProperty(arr[i]))
		ArrayMethods.push(arr[i]);
}
ArrayMethods.push('reverse');
MathMethods.push('PI');
document.write('<br />' + '<br />' + "Исходный массив: " + arr);
document.write('<br />' + "Массив с методами массива: " + ArrayMethods);
document.write('<br />' + "Длина массива с методами массива: " + ArrayMethods.length);
document.write('<br />' + "Массив с методами объекта Math: " + MathMethods);
document.write('<br />' + "Длина массива с методами объекта Math: " + ArrayMethods.length);