using System;

namespace LR1
{
    public class Calculator : ICalculator
    {
        private int _weight;
        private double _height;
        private int _age;
        private double body_index;
        private double norm;
        int _days;
        int _goalWeight;

        private bool _isMale;

        public Calculator(int weight, double height, int age, bool isMale, int days, int goalWeight) 
        {
            _weight = weight;
            _height = height;
            _age = age;
            _isMale = isMale;
            _days = days;
            _goalWeight = goalWeight;
            BodymassIndexCalc();
        }

        public string BodymassIndexCalc()
        {
            body_index = Math.Round(_weight / Math.Pow(_height / 100, 2), 1);
            return $"your body index {body_index}";
        }

        public string InterpretBodyMassIndex()
        {
            if (body_index == 0)
            {
                BodymassIndexCalc();
            }

            if (body_index < 18.5)
                return "Недостаточный вес: рекомендуется набрать массу.";
            else if (body_index < 25.0)
                return "Нормальный вес: у вас здоровый вес.";
            else if (body_index < 30.0)
                return "Избыточный вес: рекомендуется контролировать рацион и заниматься спортом.";
            else if (body_index < 35.0)
                return "Ожирение I степени: повышенный риск заболеваний. Желательно проконсультироваться с врачом.";
            else if (body_index < 40.0)
                return "Ожирение II степени: высокий риск для здоровья. Требуется консультация специалиста.";
            else
                return "Ожирение III степени: очень высокий риск. Необходимо срочно обратиться к врачу.";
        }
        public double CalorieNormCalc()
        {
            if (_isMale)
            {
                norm = 10 * _weight + 6.25 * _height + 5 * _age + 5;
            }
            else
            {
                norm = 10 * _weight + 6.25 * _height + 5 * _age - 161;
            }

            return norm;
        }

        public double CalorieCalc()
        {
            if (_days == 0) return 0;
            double totalDeficit = (7700 * _goalWeight) / _days;
            return totalDeficit;
        }

    }
}
