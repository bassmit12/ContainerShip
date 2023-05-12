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
        public ContainerType Type { get; set; }
    }
}
