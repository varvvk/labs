console.log("dialog.js loaded");

var knowledge = [
["моделирование","является","основным инструментом исследования динамических объектов"],
["моделирование","применяется","в инженерии"],
["моделирование","применяется","в экономике"],
["моделирование","применяется","в образовании"],
["моделирование","применяется","в управлении"],
["моделирование","применяется","в технических системах"],

["методы моделирования","ориентированы","на решение практических задач"],
["модель","заменяет","реальный объект"],
["модель","является","упрощенным представлением системы"],
["модель","позволяет","исследовать свойства объекта"],

["моделирование","важно","при исследовании сложных систем"],
["эксперименты","могут быть","опасными"],
["эксперименты","могут быть","дорогими"],
["эксперименты","могут быть","невозможными"],

["формализация","является","этапом моделирования"],
["построение модели","является","этапом моделирования"],
["решение модели","является","этапом моделирования"],
["проверка модели","является","этапом моделирования"],
["реализация решения","является","этапом моделирования"],

["имитационное моделирование","является","методом исследования"],
["система","заменяется","моделью"],
["модель","описывает","систему"],
["эксперименты","проводятся","с моделью"],

["имитационная модель","не содержит","аналитическую формулу"],
["имитатор","является","программой поведения системы"],
["имитационное моделирование","используется","при отсутствии формул"],
["имитационное моделирование","используется","для анализа времени"],
["имитационное моделирование","используется","для обучения персонала"],

["система","разбивается","на элементы"],
["элементы","образуют","структуру системы"],
["элементы","связаны","между собой"],
["модель","начинается","с входного элемента"],
["процесс","проходит","через элементы"],
["процесс","заканчивается","на выходе"],

["цель моделирования","состоит","в воспроизведении поведения системы"],
["симулятор","позволяет","проводить эксперименты"],
["симулятор","исключает","влияние на реальную систему"],

["симулятор","анализирует","сценарии работы"],
["симулятор","оценивает","эффективность решений"],
["симулятор","выявляет","ошибки системы"],
["симулятор","прогнозирует","развитие процессов"],

["время","можно","ускорять"],
["время","можно","замедлять"],
["модель","отображает","поведение системы во времени"],

["пользователь","изменяет","параметры системы"],
["пользователь","взаимодействует","с моделью"],
["графика","отображает","поведение системы"],

["имитационное моделирование","снижает","затраты"],
["имитационное моделирование","повышает","безопасность"],
["имитационное моделирование","обеспечивает","наглядность"],

["математическая модель","описывает","систему глобально"],
["имитационная модель","описывает","систему по элементам"],

["зависимости","выражаются","таблицами"],
["зависимости","выражаются","логикой"],
["правила","имеют вид","если-то"],

["имитационная модель","используется","для расчетов"],
["имитационная модель","используется","для анализа вариантов"],

/* КАССОВЫЙ АППАРАТ */

["кассовый аппарат","является","устройством учета продаж"],
["кассовый аппарат","регистрирует","денежные операции"],
["кассовый аппарат","контролирует","продажи"],
["кассовый аппарат","печатает","чеки"],
["кассовый аппарат","автоматизирует","выдачу чеков"],

["кассовый аппарат","интегрируется","с POS-системой"],
["кассовый аппарат","интегрируется","с базой данных"],
["кассовый аппарат","интегрируется","с аналитикой"],

["монитор","отображает","информацию"],
["принтер чеков","печатает","чеки"],
["сканер","считывает","штрих-код"],
["терминал","принимает","оплату картой"],
["денежный ящик","хранит","деньги"],

["кассовый аппарат","изобрел","Джеймс Ритти"],
["Джеймс Ритти","создал","первый кассовый аппарат"],
["кассовый аппарат","появился","в 1879 году"],

["кассовый аппарат","предотвращает","хищения"],
["кассовый аппарат","фиксирует","продажи"],
["устройство","представляет","механическую коробку"],

["кассы","развивались","от механических к электронным"],
["электронные кассы","имеют","дисплей"],
["электронные кассы","имеют","память"],

["кассы","интегрируются","с компьютерами"],
["кассы","автоматизируют","отчетность"],
["кассы","автоматизируют","учет товаров"],

["NCR","разрабатывает","кассовые системы"],
["Epson","производит","кассовое оборудование"],

["разработчики","улучшают","удобство работы"],
["разработчики","повышают","скорость обслуживания"],

["кассовый аппарат","используется","в магазинах"],
["кассовый аппарат","используется","в кафе"],
["кассовый аппарат","используется","в ресторанах"],
["кассовый аппарат","используется","в аптеках"],

["кассовый аппарат","собирает","статистику"],
["кассовый аппарат","анализирует","продажи"],
["кассовый аппарат","учитывает","товары"],

["POS-система","объединяет","оборудование и ПО"],
["сканирование","ускоряет","обработку товаров"],
["автоматический расчет","выполняет","итоги покупки"],

["кассовый аппарат","отсканирует","товар по штрих-коду"],
["кассовый аппарат","покажет","стоимость покупки"],
["кассовый аппарат","примет","выбор способа оплаты"],
["кассовый аппарат","примет","оплату наличными или картой"],
["кассовый аппарат","выдаст","сдачу при необходимости"],
["кассовый аппарат","напечатает","чек покупателю"],
["кассовый аппарат","откроет","денежный ящик"],
["кассовый аппарат","запишет","данные о продаже в память"],

["кассовый аппарат","поддерживает","скидки"],
["кассовый аппарат","поддерживает","акции"],
["кассовый аппарат","поддерживает","бонусы"],

["кассовый аппарат","является","ключевым устройством торговли"],
["кассовый аппарат","обеспечивает","учет продаж"],
["кассовый аппарат","обеспечивает","контроль персонала"],

["современная система","объединяет","технологии и безопасность"],

["кассовый аппарат","является создателем","Джеймс Ритти"],
["Джеймс Ритти","является","создателем кассового аппарата"],
["кассовый аппарат","создал","Джеймс Ритти"],
["кассовый аппарат","работает","по принципу регистрации и обработки продаж"],
["кассовый аппарат","работает","через учет денежных операций и печать чеков"],
["кассовый аппарат","обрабатывает","данные о товарах и оплате"],
["кассовый аппарат","работает по принципу","регистрации продаж и учета операций"],
["кассовый аппарат","основан","на автоматизации учета продаж"],
["кассовый аппарат","функционирует","на основе POS-системы"],
["симулятор","разработала","Казкова Варвара"],
["симулятор","создала","Казакова Варвара"],
["автор симулятора","является","Казакова Варвара"],
["симулятор кассового аппарата","разработал","Казакова Варвара"],
["кассовый аппарат","что такое","устройство учета продаж и обслуживания клиентов"],
["кассовый аппарат","для чего используется","для регистрации продаж и печати чеков"],

["кассовый аппарат","выглядит","на рисунок: <br><img src='images/pos-system_main.png'>"],

["Джеймс Ритти","изображен","на рисунок: <br><img src='images/James_Jacob_Ritty.jpg'>"],

["первый кассовый аппарат","показан","на рисунок: <br><img src='images/pervye-kassovye-apparaty-istoriya-vozniknoveniya.jpg'>"],

["монитор","показан","на рисунок: <br><img src='images/display.jpg'>"],

["принтер чеков","показан","на рисунок: <br><img src='images/print_receipt.jpg'>"],

["сканер штрихкодов","показан","на рисунок: <br><img src='images/Barcode_Scanner.jpg'>"],

["терминал","показан","на рисунок: <br><img src='images/pos-terminal.png'>"],

["денежный ящик","показан","на рисунок: <br><img src='images/cash-drawer.jpg'>"],

["компания NCR","изображена","на рисунок: <br><img src='images/ncr.jpg'>"],

["компания Epson","изображена","на рисунок: <br><img src='images/epson.jpg'>"]

];

var endings = [
  ["ет", "(ет|ут|ют)"], ["ут", "(ет|ут|ют)"], ["ют", "(ет|ут|ют)"],
  ["ит", "(ит|ат|ят)"], ["ат", "(ит|ат|ят)"], ["ят", "(ит|ат|ят)"],
  ["ется", "(ется|утся|ются)"], ["утся", "(ется|утся|ются)"], ["ются", "(ется|утся|ются)"],
  ["ится", "(ится|атся|ятся)"], ["атся", "(ится|атся|ятся)"], ["ятся", "(ится|атся|ятся)"],
  ["ен", "(ен|ена|ено|ены)"], ["ена", "(ен|ена|ено|ены)"], ["ено", "(ен|ена|ено|ены)"], ["ены", "(ен|ена|ено|ены)"],
  ["ан", "(ан|ана|ано|аны)"], ["ана", "(ан|ана|ано|аны)"], ["ано", "(ан|ана|ано|аны)"], ["аны", "(ан|ана|ано|аны)"],
  ["жен", "(жен|жна|жно|жны)"], ["жна", "(жен|жна|жно|жны)"], ["жно", "(жен|жна|жно|жны)"], ["жны", "(жен|жна|жно|жны)"],
  ["ует", "(ует|уют)"], ["уют", "(ует|уют)"],
  ["зан", "(зан|зана|зано|заны)"], ["зана", "(зан|зана|зано|заны)"],
  ["ть", "(ть|ет|ит|ат|ят)"]
];

function smallFirst(str) {
  if (!str) return "";
  return str.charAt(0).toLowerCase() + str.slice(1);
}

function bigFirst(str) {
  if (!str) return "";
  return str.charAt(0).toUpperCase() + str.slice(1);
}

function normalizeText(text) {
  return String(text || "")
    .toLowerCase()
    .replace(/ё/g, "е")
    .replace(/([.,!?;:()])/g, " $1 ")
    .replace(/\s+/g, " ")
    .trim();
}

function getEndingIndex(word) {
  if (!word || word.length < 2) return -1;
  for (var i = 0; i < endings.length; i++) {
    if (word.slice(-endings[i][0].length) === endings[i][0]) return i;
  }
  return -1;
}

function formatTriadAsAnswer(index) {
  if (index < 0 || index >= knowledge.length) return "Ответ не найден.";
  var obj = knowledge[index][2];
  var text = bigFirst(knowledge[index][0]) + " " + knowledge[index][1] + " " + obj;
  if (!/<[^>]+>/.test(obj)) text += ".";
  return text;
}

function handleWhyQuestion(question) {
  var whyPatterns = [
    /^(зачем|для чего)\s+нужен\s+(.+)$/i,
    /^(зачем|для чего)\s+нужна\s+(.+)$/i,
    /^(зачем|для чего)\s+нужно\s+(.+)$/i,
    /^(зачем|для чего)\s+нужны\s+(.+)$/i,
    /^(зачем|для чего)\s+(.+)$/i
  ];
  
  var target = null;
  for (var p = 0; p < whyPatterns.length; p++) {
    var match = question.match(whyPatterns[p]);
    if (match) {
      target = match[match.length - 1].toLowerCase();
      break;
    }
  }
  
  if (!target) return null;
  
  for (var i = 0; i < knowledge.length; i++) {
    var subject = knowledge[i][0].toLowerCase();
    var predicate = knowledge[i][1].toLowerCase();
    
    if ((subject.indexOf(target) >= 0 || target.indexOf(subject) >= 0) &&
        (predicate.indexOf("предназначен") >= 0 || 
         predicate.indexOf("использует") >= 0 ||
         predicate.indexOf("нужен") >= 0 ||
         predicate.indexOf("служит") >= 0 ||
         predicate.indexOf("применяет") >= 0)) {
      return formatTriadAsAnswer(i);
    }
  }
  
  for (var i = 0; i < knowledge.length; i++) {
    var subject = knowledge[i][0].toLowerCase();
    if (subject.indexOf(target) >= 0 || target.indexOf(subject) >= 0) {
      return formatTriadAsAnswer(i);
    }
  }
  
  return null;
}

function handleWhatQuestion(question) {
  var whatMatch = question.match(/^(что такое|кто такой|что значит)\s+(.+)$/i);
  if (whatMatch) {
    var target = whatMatch[2].toLowerCase();
    
    for (var i = 0; i < knowledge.length; i++) {
      var subject = knowledge[i][0].toLowerCase();
      var predicate = knowledge[i][1].toLowerCase();
      
      if ((subject.indexOf(target) >= 0 || target.indexOf(subject) >= 0) &&
          (predicate === "это" || predicate === "является" || predicate === "предназначена")) {
        return formatTriadAsAnswer(i);
      }
    }
  }
  
  var inventorMatch = question.match(/^(кто придумал|кто изобрёл|кто создал|кем была изобретена)\s+(.+)$/i);
  if (inventorMatch) {
    for (var i = 0; i < knowledge.length; i++) {
      var predicate = knowledge[i][1].toLowerCase();
      if (predicate.indexOf("изобрёл") >= 0 || predicate.indexOf("изобрел") >= 0 ||
          predicate.indexOf("создал") >= 0 || predicate.indexOf("считается") >= 0) {
        return formatTriadAsAnswer(i);
      }
    }
  }
  
  return null;
}

function handleHowWorkQuestion(question) {
  var workMatch = question.match(/^(как работает|принцип работы)\s+(.+)$/i);
  if (workMatch) {
    var target = workMatch[2].toLowerCase();
    
    // Ищем триады где subject совпадает с target
    var answers = [];
    for (var i = 0; i < knowledge.length; i++) {
      var subject = knowledge[i][0].toLowerCase();
      var predicate = knowledge[i][1].toLowerCase();
      
      // Ищем триады с subject = target и полезными предикатами
      if ((subject === target || subject.indexOf(target) >= 0 || target.indexOf(subject) >= 0) &&
          (predicate.indexOf("работает") >= 0 || 
           predicate.indexOf("регистрирует") >= 0 ||
           predicate.indexOf("обрабатывает") >= 0 ||
           predicate.indexOf("печатает") >= 0 ||
           predicate.indexOf("контролирует") >= 0 ||
           predicate.indexOf("функционирует") >= 0 ||
           predicate.indexOf("отсканирует") >= 0 ||
           predicate.indexOf("покажет") >= 0 ||
           predicate.indexOf("примет") >= 0 ||
           predicate.indexOf("выдаст") >= 0 ||
           predicate.indexOf("напечатает") >= 0 ||
           predicate.indexOf("откроет") >= 0 ||
           predicate.indexOf("запишет") >= 0 ||
           predicate.indexOf("включает") >= 0)) {
        answers.push(formatTriadAsAnswer(i));
      }
    }
    
    if (answers.length > 0) {
      return answers.slice(0, 6).join(" ");
    }
    
    // Если не нашли - ищем любые триады с этим subject
    for (var i = 0; i < knowledge.length; i++) {
      var subject = knowledge[i][0].toLowerCase();
      if (subject === target || subject.indexOf(target) >= 0 || target.indexOf(subject) >= 0) {
        answers.push(formatTriadAsAnswer(i));
      }
    }
    
    if (answers.length > 0) {
      return "Принцип работы: " + answers.slice(0, 3).join(" ");
    }
  }
  return null;
}

function handleLooksLikeQuestion(question) {
  var patterns = [
    /как выглядит\s+(.+)$/i,
    /как выгляд\s+(.+)$/i,
    /какой вид имеет\s+(.+)$/i,
    /как выглядит на рисунке\s+(.+)$/i
  ];
  
  var targetObj = null;
  for (var p = 0; p < patterns.length; p++) {
    var match = question.match(patterns[p]);
    if (match) {
      targetObj = match[1].toLowerCase();
      break;
    }
  }
  if (!targetObj) return null;
  
  targetObj = targetObj.replace(/^на рисунке\s+|^пожалуйста\s+|^это\s+/i, '');
  
  var bestMatch = null;
  var bestScore = 0;
  
  for (var i = 0; i < knowledge.length; i++) {
    var subject = knowledge[i][0].toLowerCase();
    var object = knowledge[i][2];
    var isVisual = /<img|<video/i.test(object);
    
    if (!isVisual) continue;
    
    var score = 0;
    if (subject === targetObj) score = 10;
    else if (subject.indexOf(targetObj) >= 0) score = 5;
    else if (targetObj.indexOf(subject) >= 0) score = 3;
    
    if (score > bestScore) {
      bestScore = score;
      bestMatch = i;
    }
  }
  if (bestMatch !== null && bestScore >= 3) {
    return formatTriadAsAnswer(bestMatch);
  }
  return null;
}

function handleShowCommand(question) {
  var patterns = [
    /^(покажи|показать|покажите|показ)\s+(.+)$/i,
    /^(где находится|где показан|где показана|где показано)\s+(.+)$/i
  ];
  
  var target = null;
  for (var p = 0; p < patterns.length; p++) {
    var match = question.match(patterns[p]);
    if (match) {
      target = match[match.length - 1].toLowerCase();
      break;
    }
  }
  if (!target) return null;
  
  target = target.replace(/^на рисунке\s+|^пожалуйста\s+/i, '');
  var bestMatch = null;
  var bestScore = 0;
  
  for (var i = 0; i < knowledge.length; i++) {
    var subject = knowledge[i][0].toLowerCase();
    var object = knowledge[i][2];
    var isVisual = /<img|<video/i.test(object);
    
    if (!isVisual) continue;
    
    var score = 0;
    if (subject === target) score = 10;
    else if (subject.indexOf(target) >= 0) score = 5;
    else if (target.indexOf(subject) >= 0) score = 3;
    
    if (score > bestScore) {
      bestScore = score;
      bestMatch = i;
    }
  }
  if (bestMatch !== null && bestScore >= 3) {
    return formatTriadAsAnswer(bestMatch);
  }
  return null;
}

function handleConsistQuestion(question) {
  var patterns = [
    /из чего состоит\s+(.+)$/i,
    /из чего состоит$/i,
    /что входит в состав\s+(.+)$/i,
    /компоненты\s+(.+)$/i
  ];
  
  var target = null;
  for (var p = 0; p < patterns.length; p++) {
    var match = question.match(patterns[p]);
    if (match) {
      target = match[1] ? match[1].toLowerCase() : "кассовый аппарат";
      break;
    }
  }
  
  if (!target) return null;
  
  // Компоненты кассового аппарата
  var components = [];
  for (var i = 0; i < knowledge.length; i++) {
    var subject = knowledge[i][0].toLowerCase();
    var predicate = knowledge[i][1].toLowerCase();
    var object = knowledge[i][2].toLowerCase();
    
    // Ищем компоненты через предикаты
    if ((subject.indexOf("монитор") >= 0 || subject.indexOf("принтер") >= 0 ||
         subject.indexOf("сканер") >= 0 || subject.indexOf("терминал") >= 0 ||
         subject.indexOf("ящик") >= 0) &&
        target.indexOf("кассов") >= 0) {
      if (components.indexOf(knowledge[i][0]) < 0) {
        components.push(knowledge[i][0]);
      }
    }
  }
  
  if (components.length > 0 && target.indexOf("кассов") >= 0) {
    return "Кассовый аппарат состоит из следующих компонентов: " + components.join(", ") + ".";
  }
  
  return null;
}

function searchByKeywords(question) {
  var stopWords = {
    "что":1,"как":1,"где":1,"когда":1,"почему":1,"чем":1,"кто":1,
    "для":1,"при":1,"если":1,"или":1,"и":1,"в":1,"на":1,"по":1,"к":1,"с":1,
    "от":1,"до":1,"из":1,"у":1,"не":1,"это":1,"такое":1,"может":1,"можно":1,
    "покажи":1,"показать":1,"показ":1,"сменить":1,"намотать":1,"настроить":1,
    "выглядит":1,"выгляд":1,"зачем":1,"для чего":1
  };
  var words = question.split(" ");
  var keywords = [];
  for (var i = 0; i < words.length; i++) {
    if (words[i].length >= 3 && !stopWords[words[i]]) {
      keywords.push(words[i]);
    }
  }
  if (keywords.length === 0) return -1;
  
  var bestIdx = -1;
  var bestScore = 0;
  for (var i = 0; i < knowledge.length; i++) {
    var row = normalizeText(knowledge[i].join(" "));
    var score = 0;
    for (var k = 0; k < keywords.length; k++) {
      if (row.indexOf(keywords[k]) >= 0) score++;
    }
    if (score > bestScore && score >= 1) {
      bestScore = score;
      bestIdx = i;
    }
  }
  return bestIdx;
}

function getAnswer(question) {
  var originalQuestion = question;
  var q = smallFirst(normalizeText(question));
  
  var whyResult = handleWhyQuestion(q);
  if (whyResult) return whyResult;
  
  var whatResult = handleWhatQuestion(q);
  if (whatResult) return whatResult;
  
  var workResult = handleHowWorkQuestion(q);
  if (workResult) return workResult;
  
  var looksResult = handleLooksLikeQuestion(q);
  if (looksResult) return looksResult;
  
  var showResult = handleShowCommand(originalQuestion);
  if (showResult) return showResult;
  
  var consistResult = handleConsistQuestion(q);
  if (consistResult) return consistResult;
  
  var words = q.split(" ");
  
  for (var i = 0; i < words.length; i++) {
    var endingIdx = getEndingIndex(words[i]);
    if (endingIdx < 0) continue;
    
    var base = words[i].slice(0, words[i].length - endings[endingIdx][0].length);
    var predicatePattern = new RegExp("^" + base + endings[endingIdx][1] + "$");
    
    var subjectParts = [];
    for (var j = i + 1; j < words.length; j++) {
      if (!/[.,!?;:]/.test(words[j])) subjectParts.push(words[j]);
    }
    
    var subjectPattern = null;
    if (subjectParts.length > 0) {
      var subjExpr = ".*" + subjectParts.join(".*") + ".*";
      subjectPattern = new RegExp(subjExpr);
    }
    
    for (var t = 0; t < knowledge.length; t++) {
      var subjNorm = normalizeText(knowledge[t][0]);
      var predNorm = normalizeText(knowledge[t][1]);
      var objNorm = normalizeText(knowledge[t][2]);
      
      var predMatch = predicatePattern.test(predNorm);
      var subjMatch = !subjectPattern || subjectPattern.test(subjNorm) || subjectPattern.test(objNorm);
      
      if (predMatch && subjMatch) {
        return formatTriadAsAnswer(t);
      }
    }
    
    if (subjectPattern) {
      for (var t = 0; t < knowledge.length; t++) {
        var subjNorm2 = normalizeText(knowledge[t][0]);
        var objNorm2 = normalizeText(knowledge[t][2]);
        if (subjectPattern.test(subjNorm2) || subjectPattern.test(objNorm2)) {
          return formatTriadAsAnswer(t);
        }
      }
    }
  }
  
  var keywordIdx = searchByKeywords(q);
  if (keywordIdx >= 0) return formatTriadAsAnswer(keywordIdx);
  
  return "Ответ не найден. Пожалуйста, переформулируйте вопрос.";
}

let dialogActive = false;

function dialog_window() {
  console.log("dialog_window старт");

  let dialog = document.createElement("div");
  dialog.id = "dialogWindow";
  dialog.className = "dialog-window";

  let header = document.createElement("div");
  header.className = "dialog-header";
  header.innerHTML = "Диалог с базой знаний";

  let chat = document.createElement("div");
  chat.id = "chatBox";
  chat.className = "dialog-chat";

  let input = document.createElement("input");
  input.id = "userInput";
  input.placeholder = "Введите вопрос...";

  let sendBtn = document.createElement("button");
  sendBtn.innerHTML = "Отправить";
  sendBtn.onclick = ask;

  let voiceBtn = document.createElement("button");
  voiceBtn.innerHTML = "🎤";
  voiceBtn.title = "Голосовой ввод";
  voiceBtn.onclick = startVoiceInput;

  let openBtn = document.createElement("button");
  openBtn.innerHTML = "Открыть чат";
  openBtn.className = "open-dialog-btn";
  openBtn.onclick = openDialog;

  let inputContainer = document.createElement("div");
  inputContainer.className = "dialog-input-container";
  inputContainer.appendChild(input);
  inputContainer.appendChild(voiceBtn);
  inputContainer.appendChild(sendBtn);

  document.body.appendChild(openBtn);
  dialog.appendChild(header);
  dialog.appendChild(chat);
  dialog.appendChild(inputContainer);

  document.body.appendChild(dialog);
}

function openDialog() {
  let dialog = document.getElementById("dialogWindow");
  dialog.classList.toggle("open");
}

function ask() {
  let input = document.getElementById("userInput");
  let chat = document.getElementById("chatBox");

  let question = input.value.trim();
  if (!question) return;

  let qDiv = document.createElement("div");
  qDiv.className = "question";
  qDiv.innerHTML = question;
  chat.appendChild(qDiv);

  let answerText = getAnswer(question);

  let aDiv = document.createElement("div");
  aDiv.className = "answer";
  aDiv.innerHTML = answerText;
  chat.appendChild(aDiv);

  chat.scrollTop = chat.scrollHeight;
  input.value = "";
}

function startVoiceInput() {
  let input = document.getElementById("userInput");
  if (!input) return;
  
  let SpeechRecognition = window.SpeechRecognition || window.webkitSpeechRecognition;
  if (SpeechRecognition) {
    let recognition = new SpeechRecognition();
    recognition.lang = "ru-RU";
    recognition.interimResults = false;
    recognition.maxAlternatives = 1;
    
    recognition.onresult = function(event) {
      let text = event.results[0][0].transcript;
      input.value = text;
    };
    
    recognition.onerror = function(event) {
      console.log("Ошибка микрофона: " + event.error);
    };
    
    recognition.start();
  } else {
    alert("Ваш браузер не поддерживает голосовой ввод");
  }
}
