using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR_2
{
    internal partial class House
    {
        public int FlatNumber
        {
            get { return flatNumber; }
            set { flatNumber = value; }
        }

        public int Area
        {
            get { return area; }
            set { area = value; }
        }

        public int Floor
        {
            get { return floor; }
            set { floor = value; }
        }

        public int Rooms
        {
            get { return rooms; }
            set { rooms = value; }
        }

        public string Street
        {
            get { return street; }
            set { street = value; }
        }

        public string BuildingType
        {
            get { return buildingType; }
            set { buildingType = value; }
        }

        public int ExploitationTime
        {
            get { return exploitationTime; }
            set { exploitationTime = value; }
        }

        public int Id
        {
            get { return id; }
            private set { GetHashCode(); }
        }
    }
}
