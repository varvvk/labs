function pechat() {
   var faculty = document.querySelector("textarea").value;
   var lastName = document.forms.forma.lastName.value;
   var specialty = document.forms.forma.specialty.value;
   var kurs = document.forms.forma.kurs.value;
   let predmet = {};
	let counter = 0;
	for (let i = 0; i < document.forms.forma.predmet.length; i++) {
		if(document.forms.forma.predmet[i].checked){
			predmet[counter] = (document.forms.forma.predmet[i].value);
			counter++;
		}
	}
   document.write("<p>" + faculty + "</p>" + "Студент " + lastName + " специальность " + specialty + " курс " + kurs + " должен сдавать следующие предметы:<br>");
   let ul = document.createElement("ul");
	for (var i = 0; i < counter; i++){
		ul.appendChild(document.createElement("li")).innerHTML = predmet[i];
	}
	document.body.appendChild(ul);


let secondForm = document.createElement("form");
let select =  secondForm.appendChild(document.createElement("select"));
for (let i = 0; i < counter; i++) {
   select.appendChild(document.createElement("option")).innerHTML = predmet[i];
}
document.body.appendChild(secondForm);
}