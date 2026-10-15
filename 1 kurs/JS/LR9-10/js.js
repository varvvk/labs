pic.onmousedown = function(event) {
    // разместить поверх остального содержимого и в абсолютных координатах
    pic.style.position = 'absolute';
    pic.style.zIndex = 1000;
    // переместим в body, чтобы картинка была точно не внутри position:relative
    document.body.append(pic);
    // и установим абсолютно спозиционированную картинку под курсор
    moveAt(event.pageX, event.pageY);
    // передвинуть картинку под координаты курсора
    // и сдвинуть на половину ширины/высоты для центрирования
    function moveAt(pageX, pageY) {
      pic.style.left = pageX - pic.offsetWidth / 2 + 'px';
      pic.style.top = pageY - pic.offsetHeight / 2 + 'px';
    }
    function onMouseMove(event) {
      moveAt(event.pageX, event.pageY);
    }
    // (3) перемещать по экрану
    document.addEventListener('mousemove', onMouseMove);
    // (4) положить картинку, удалить более ненужные обработчики событий
    pic.onmouseup = function() {
      document.removeEventListener('mousemove', onMouseMove);
      pic.onmouseup = null;
    };
  };
  pic.ondragstart = function() {
    return false;
  };
text.onmousedown = function(event) {
    text.style.position = 'absolute';
    text.style.zIndex = 1000;
    document.body.append(text);
    moveAt(event.pageX, event.pageY);
    function moveAt(pageX, pageY) {
      text.style.left = pageX - text.offsetWidth / 2 + 'px';
      text.style.top = pageY - text.offsetHeight / 2 + 'px';
    }
    function onMouseMove(event) {
      moveAt(event.pageX, event.pageY);
    }
    document.addEventListener('mousemove', onMouseMove);
  text.onmouseup = function() {
      document.removeEventListener('mousemove', onMouseMove);
      text.onmouseup = null;
    };
  };
  text.ondragstart = function() {
    return false;
  };