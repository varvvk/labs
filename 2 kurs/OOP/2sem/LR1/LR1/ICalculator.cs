using System;

namespace LR1
{
    public interface ICalculator
    {
        // Метод для расчета индекса массы тела
        string BodymassIndexCalc();

        // Метод для интерпретации результата индекса массы тела
        string InterpretBodyMassIndex();

        // Метод для расчета нормы калорий по формуле
        double CalorieNormCalc();

        // Метод для расчета дефицита калорий для достижения цели
        double CalorieCalc();
    }
}
