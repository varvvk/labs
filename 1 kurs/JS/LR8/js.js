var span1 = document.getElementById("span1");
span1.onmouseover = function() {
    span1.style.color="red";
}

var span2 = document.getElementById("span2");
span2.onclick = function() {
    span2.style.fontSize="20px";
}

var pic = document.getElementById("pic");
pic.onclick = function() {
    pic.src ="images/pic2.jpg";
}

var div1 = document.getElementById("div1");
div1.onclick = function() {
    div1.innerHTML = div1.innerHTML.replace(/([^\d]*)/, '<img src="images/pic3.jpg">');
    //([^\d]*) — это регулярное выражение, соответствующее любой строке, не содержащей цифры
}

var pic2 = document.getElementById("pic2");
pic2.onmouseover = function() {
    pic2.style.width="460px";
}
pic2.onmouseout = function() {
    pic2.style.width="300px";
}

var div2 = document.getElementById("div2");
div2.ondblclick = function() {
    div2.style.border="solid blue 3px";
}