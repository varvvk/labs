//задание 1
function Gruppa(speciality, group, amount){
    this.speciality = speciality;
    this.group = group;
    this.amount = amount;
    this.add_stud = function add_stud(k) {
        this.amount += k;
        console.log('В группу № ' + this.group + ' добавили ' + k + ' студентов. Стало: ' + this.amount);
    }
    this.sub_stud = function sub_stud(k){
        this.amount -= k;
        console.log('Из группы № ' + this.group + ' отчислили ' + k + ' студентов.Стало: ' + this.amount);
    }
}
let isit1 = new Gruppa('ИСиТ', 1, 28);
isit1.add_stud(1);
isit1.sub_stud(4);
let pi6 = new Gruppa('ПИ', 6, 30);
pi6.add_stud(1);
pi6.sub_stud(3);

//задание 2
function Student(name, surname, physics, math, inform,){
    this.name=name;
    this.surname=surname;
    this.physics=physics;
    this.math=math;
    this.inform=inform;
    this.introduce = function introduce(){
        console.log('Фамилия и имя студента: ' + this.surname + ' ' +this.name);
    }
    this.average = function average(){
        console.log('Средний балл: ' + ((this.physics + this.math + this.inform) / 3).toFixed(2));
    }
    this.info = function info(){
		console.log('Иформация о студенте:' + '\nФамилия: ' + this.surname + '\tИмя: ' + this.name + '\tОценка по физике: ' + this.physics + '\tОценка по математике: ' + this.math + '\tОценка по информатике: ' + this.inform);
	}
}
let stud1 = new Student('Варвара', 'Казакова', 8, 9, 10);
let stud2 = new Student('Ульяна', 'Дужик', 10, 10, 10);
let stud3 = new Student('Ева', 'Старовойтова', 9, 10, 10);
stud1.info();
stud1.introduce();
stud1.average();
stud2.info();
stud2.introduce();
stud2.average();
stud3.info();
stud3.introduce();
stud3.average();

//Задание 3
let arr = [1, 8, 9, 4, 5, 6];
console.log("Исходный массив: " + arr);
delete arr[3];
console.log("С удаленным элементом: " + arr);
console.log('Оператор in');
console.log(2 in arr);
console.log('amount' in pi6);
console.log('info' in stud2);
console.log('Оператор instanceof');
console.log(arr instanceof Array);
console.log(arr instanceof String);
console.log(Student instanceof Object);
function return5() {
    return 5
};
console.log('Оператор typeof');
console.log(typeof Gruppa);
console.log(typeof isit1);
console.log(typeof Student);
console.log(typeof stud1);
console.log(typeof arr);
console.log(typeof return5);