let img = document.getElementById('img');
let position = 0;
let speed = 2;

function moveImage() {
position += speed;
img.style.left = position + 'px';
if (position >= 500) {
    clearInterval(animation);
  }
}
let animation = setInterval(moveImage, 20);