let openWind;
function openButton() {
    openWind = window.open("about:blank", "hello", "width=200,height=200");
    openWind.document.write('<html><head><title>Новое окно</title></head></html>');
}
function closeButton() {
    openWind.close();
}
function text() {
    openWind.document.write("Название окна: " + openWind.name);
}