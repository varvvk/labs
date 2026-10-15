function moveInCircle() {
    var img = document.getElementById('pic');
    var radius = 200;
    var speed = 0.05;
    var angle = 0;
    var moveInterval = setInterval(function() {
    var x = radius * Math.cos(angle) + 150;
    var y = radius * Math.sin(angle) + 150;
    img.style.left = x + 'px';
    img.style.top = y + 'px';
    angle += speed;
    if (angle >= (4 * Math.PI)) {
    clearInterval(moveInterval); 
    img.style.left = '150px'; 
    img.style.top = '50px';
    }
    }, 10);
}
moveInCircle();