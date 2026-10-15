function createButtonHandler() {
    var count = 0;
    return function() {
        count++;
        this.innerText = 'Нажатий: ' + count;
    }
}
var buttons = document.getElementsByTagName('button');
// Привязываем обработчик к каждой кнопке
for (var i = 0; i < buttons.length; i++) {
    buttons[i].onclick = createButtonHandler();
}