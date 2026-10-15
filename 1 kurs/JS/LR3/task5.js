let fio = "Казакова Варвара Вадимовна";
document.write( '<br />' + '<br />' + "Длина строки: " + fio.length);
let fioUp = fio.toUpperCase();
let fioLow = fio.toLowerCase();
let fioUpLow = fioUp.concat(fioLow);
let rev = fio.replace('Казакова Варвара Вадимовна', 'К. В. В.');
document.write('<br />' + fio);
document.write('<br />' + rev);