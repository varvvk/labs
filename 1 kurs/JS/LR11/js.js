$("div").css("margin", "10px");
$("#span1").mouseover(function(){$("#span1").css("color","red")});
$("#span2").click(function(){$("#span2").css("fontSize", "20px")});
$("#pic").click(function(){$("#pic").attr("src", "images/pic2.jpg")});
$("#div1").click(function(){$("#div1").replaceWith("<img src='images/pic3.jpg'>");});
$("#pic2").mouseover(function(){$("#pic2").css("width", "460px")});
$("#pic2").mouseout(function(){$("#pic2").css("width", "300px")});
$("#div2").dblclick(function(){$("#div2").css("border", "solid blue 3px")});