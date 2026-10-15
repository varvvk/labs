document.getElementById('nav').onmouseover = function(event) {
    let target = event.target;
    //отслеживаем клик
    if (target.className === 'menu-item') {
        let s = target.getElementsByClassName('submenu');
        closeMenu();
        s[0].style.display = 'block';
        //массив, содержащий ложное меню
    }
}
// Проверяем, не находится ли мышь над элементом с классом 'menu-item' или 'submenu'
document.onmouseover = function(event) {
    let target = event.target;
    if (target.className != 'menu-item' && target.className != 'submenu') {
        closeMenu();
    }
}
function closeMenu() {
    let menu = document.getElementById('nav');
    let subm = document.getElementsByClassName('submenu');
    for (let i = 0; i < subm.length; i++) {  // Проходим по каждому элементу подменю и устанавливаем его
        subm[i].style.display = "none";
    }
}