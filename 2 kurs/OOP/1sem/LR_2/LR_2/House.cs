using System;

namespace LR_2
{
    internal partial class House
    {
        private readonly int id;

        private int flatNumber;
        private int area;
        private int floor;
        private int rooms;
        private string street;
        private string buildingType;
        private int exploitationTime;

        private static int objectCount;

        public House()
        {
            flatNumber = 0;
            area = 0;
            floor = 0;
            rooms = 0;
            street = "none";
            buildingType = "none";
            exploitationTime = 0;

            id = GetHashCode();
            objectCount++;
        }
        public House(int flatNumber, int area, int floor, int rooms, string street, string buildingType, int exploitationTime)
        {
            this.flatNumber = flatNumber;
            this.area = area;
            this.floor = floor;
            this.rooms = rooms;
            this.street = street;
            this.buildingType = buildingType;
            this.exploitationTime = exploitationTime;

            id = GetHashCode();
            objectCount++;
        }
        public House(string street, string buildingType) : this(1, 1, 1, 1, street, buildingType, 1) 
        {
            id = GetHashCode();
        }
        static House()
        {
            Console.WriteLine("Класс House инициализирован.");
        }
        private House(string country)
        {
            Console.WriteLine($"Все дома находятся в стране {country}. \n");
        }

        public object CallPrivateConstructor()
        {
            return new House("Belarus");    

        }

        public void CheckForMajorRepairs()
        {
            if (ExploitationTime > 50)
            {
                Console.WriteLine($"Квартира {flatNumber} на улице {street}: нееобходим капитальный ремонт");
            }
            else
            {
                Console.WriteLine($"Квартира {flatNumber} на улице {street}: капитальный ремонт не требуется");
            }
        }

        public static void DisplayClassInfo()
        {
            Console.WriteLine($"Объектов создано: {objectCount}");
        }

        public void UpdateFloorAndGetBuildingType(ref int newFloor, out string buildingType)
        {
            this.floor = newFloor;

            buildingType = this.buildingType;
        }

        public override string ToString()
        {
            return $"ID: {id}, Квартира: {flatNumber}, Площадь: {area}, Этаж: {floor}, Комнаты: {rooms}, Улица: {street}, Тип здания: {buildingType}, Время эксплуатации: {exploitationTime} лет \n";
        }

        public override bool Equals(object obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
                return false;

            House other = (House)obj;
            return this.flatNumber == other.flatNumber &&
                   this.area == other.area &&
                   this.floor == other.floor &&
                   this.rooms == other.rooms &&
                   this.street == other.street &&
                   this.buildingType == other.buildingType &&
                   this.exploitationTime == other.exploitationTime;
        }

        public override int GetHashCode()
        {
            int hash = 269;
            hash = hash * 47 + flatNumber.GetHashCode();
            hash = hash * 47 + area.GetHashCode();
            hash = hash * 47 + floor.GetHashCode();
            hash = hash * 47 + rooms.GetHashCode();
            hash = hash * 47 + (street != null ? street.GetHashCode() : 0);
            hash = hash * 47 + (buildingType != null ? buildingType.GetHashCode() : 0);
            hash = hash * 47 + exploitationTime.GetHashCode();
            return hash;
        }
    }
}
