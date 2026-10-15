document.getElementById('nav').onmouseover = function(event) {
    let target = event.target;
    if (target.className === 'menu-item') {
        let s = target.getElementsByClassName('submenu');
        closeMenu();
        s[0].style.display = 'block';
    }
}
document.onmouseover = function(event) {
    let target = event.target;
    if (target.className != 'menu-item' && target.className != 'submenu') {
        closeMenu();
    }
}
function closeMenu() {
    let menu = document.getElementById('nav');
    let subm = document.getElementsByClassName('submenu');
    for (let i = 0; i < subm.length; i++) {
        subm[i].style.display = "none";
    }
}

let img = document.getElementById('img');
let position = 0;
let speed = 2;

function moveImage() {
position += speed;
img.style.left = position + 'px';
if (position >= 1000) {
    clearInterval(animation);
  }
}
let animation = setInterval(moveImage, 20);