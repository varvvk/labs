let arr = [6-Math.pow(Math.PI,2)+3*Math.exp(8), 2*Math.cos(4)+Math.cos(12)+8-Math.exp(3), 3*Math.sin(9)+Math.log(5)+2, 2*Math.tan(5)+6+Math.sqrt(12) ];
document.write("Максимальный элемент массива: " + Math.max(...arr) + "\tИндекс: " + arr.indexOf(Math.max(...arr)));
document.write('<br />');
document.write("Минимальный элемент массива: " + Math.min(...arr) + "\tИндекс: " + arr.indexOf(Math.min(...arr)));

