using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerShip
{
    enum ContainerType
    {
        Normal,
        Cooled,
        Valuable
    }

    class Container
    {
        private static readonly Random random = new Random();

        public ContainerType Type { get; set; }
        public int Weight { get; set; }
        public int MaxLoad { get; set; } = 120000;

        public Container()
        {
            Weight = random.Next(4000, 30001);
        }
    }
}
