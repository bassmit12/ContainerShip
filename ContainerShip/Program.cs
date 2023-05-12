using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ContainerShip
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a ship with dimensions 4x3
            Ship ship = new Ship(4, 3);

            // Create an array of containers
            Container[] containers = new Container[]
            {
                new Container { Type = ContainerType.Normal },
                new Container { Type = ContainerType.Normal },
                new Container { Type = ContainerType.Normal },
                new Container { Type = ContainerType.Cooled },
                new Container { Type = ContainerType.Cooled },
                new Container { Type = ContainerType.Cooled },
                new Container { Type = ContainerType.Cooled },
                new Container { Type = ContainerType.Valuable },
            };

            // Place the containers on the ship
            ship.PlaceContainers(containers);

            // Print the ship layout
            ship.PrintLayout();
        }
    }
}