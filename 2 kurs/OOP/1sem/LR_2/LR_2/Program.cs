using System;
using System.IO;

namespace LR_2
{

    internal class Program
    {
        static void Main(string[] args)
        {
            House house1 = new House();
            House house2 = new House(45, 80, 2, 3, "Green Street", "Brick", 15);
            House house3 = new House("Red Street", "Panel");

            Console.WriteLine(house1.ToString());
            Console.WriteLine(house2.ToString());
            Console.WriteLine(house3.ToString());

            house1.FlatNumber = 101;
            house1.Floor = 5;
            house1.Rooms = 3;
            house1.Area = 200;
            house1.Street = "Blue Street";
            house1.BuildingType = "Monolithic";
            house1.ExploitationTime = 12;
            

            Console.WriteLine($"House 1 updated: {house1}");

            house2.CheckForMajorRepairs();

            bool areEqual = house1.Equals(house2);
            Console.WriteLine($"House 1 и House 2 равны? {areEqual}");

            if (house1 is House)
            {
                Console.WriteLine("house1 является объектом класса House.");
            }

            House.DisplayClassInfo();

            int newFloor = 7;
            string buildingType;
            house1.UpdateFloorAndGetBuildingType(ref newFloor, out buildingType);
            Console.WriteLine($"House 1: Новый этаж = {newFloor}, Текущий тип здания = {buildingType}");

            house1.CallPrivateConstructor();


            House[] houses = new House[]
            {
                new House(101, 50, 3, 2, "Blue Street", "Brick", 10),
                new House(102, 65, 5, 3, "Green Street", "Panel", 12),
                new House(103, 75, 8, 3, "Red Street", "Monolithic", 15),
                new House(104, 40, 2, 1, "Yellow Street", "Brick", 8),
                new House(105, 90, 10, 4, "Purple Street", "Panel", 20)
            };

            for (int i = 0; i < houses.Length; i++)
            {
                Console.WriteLine(houses[i].ToString());
            }

            Console.Write("Введите количество комнат: ");
            int roomCount = int.Parse(Console.ReadLine());

            Console.WriteLine($"\nСписок квартир с {roomCount} комнатами:");
            foreach (var house in houses)
            {
                if (house.Rooms == roomCount)
                {
                    Console.WriteLine(house);
                }
            }

            Console.Write("\nВведите минимальный этаж: ");
            int minFloor = int.Parse(Console.ReadLine());

            Console.Write("Введите максимальный этаж: ");
            int maxFloor = int.Parse(Console.ReadLine());

            Console.WriteLine($"\nСписок квартир с {roomCount} комнатами и расположенных на этажах от {minFloor} до {maxFloor}:");
            foreach (var house in houses)
            {
                if (house.Rooms == roomCount && house.Floor >= minFloor && house.Floor <= maxFloor)
                {
                    Console.WriteLine(house);
                }
            }

            var anonymHouse = new { flatNumber = 1000, area = 1000, floor = 1000, rooms = 1000, street = "aaa", buildingType = "aaa", exploitationTime = 1000 };
            Console.WriteLine($"Квартира: {anonymHouse.flatNumber}, Площадь: {anonymHouse.area}, Этаж: {anonymHouse.floor}, Комнаты: {anonymHouse.rooms}, Улица: {anonymHouse.street}, Тип здания: {anonymHouse.buildingType}, Время эксплуатации: {anonymHouse.exploitationTime} лет");
        }   
    }
}
