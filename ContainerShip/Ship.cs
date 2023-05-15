using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerShip
{
    class Ship
    {
        private int width;
        private int length;
        private Container?[,] layout;

        public Ship(int width, int length)
        {
            this.width = width;
            this.length = length;
            layout = new Container[length, width];
        }

        public void PlaceContainers(Container[] containers)
        {
            PlaceCooledContainers(containers);
            PlaceRemainingContainers(containers);

            // Check if there are containers that couldn't be placed
            if (containers.Any(container => container.Temperature == ContainerTemperature.Cold && !IsContainerPlaced(container)))
            {
                Console.WriteLine("Not enough space in the first row to place all cooled containers.");
            }
            else if (containers.Any(container => container.Temperature != ContainerTemperature.Cold && !IsContainerPlaced(container)))
            {
                Console.WriteLine("Not enough space in the ship to place all containers.");
            }
        }

        private void PlaceCooledContainers(Container[] containers)
        {
            int col = 0;
            foreach (var container in containers)
            {
                if (container.Temperature == ContainerTemperature.Cold)
                {
                    bool isPlaced = false;
                    for (int x = 0; x < width; x++)
                    {
                        if (layout[0, x] == null)
                        {
                            layout[0, x] = container;
                            isPlaced = true;
                            break;
                        }
                    }
                    if (!isPlaced)
                    {
                        Console.WriteLine("Unable to place container: " + container.ToString());
                    }
                }
            }
        }

        private void PlaceRemainingContainers(Container[] containers)
        {
            int row = 0;
            int col = 0;
            foreach (var container in containers)
            {
                if (container.Temperature != ContainerTemperature.Cold)
                {
                    bool isPlaced = false;
                    while (row < length)
                    {
                        if (layout[row, col] == null)
                        {
                            layout[row, col] = container;
                            isPlaced = true;
                            break;
                        }
                        col++;
                        if (col >= width)
                        {
                            row++;
                            col = 0;
                        }
                    }
                    if (!isPlaced)
                    {
                        Console.WriteLine("Unable to place container: " + container.ToString());
                    }
                }
            }
        }

        private bool IsContainerPlaced(Container container)
        {
            for (int x = 0; x < length; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    if (layout[x, y] == container)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void PrintLayout()
        {
            Console.WriteLine("");
            Console.WriteLine("Layout with containers:");
            for (int x = 0; x < length; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    if (layout[x, y] != null)
                    {
                        Console.Write(GetContainerSymbol(layout[x, y]) + " ");
                    }
                    else
                    {
                        Console.Write(". ");
                    }
                }
                Console.WriteLine();
            }
        }

        private char GetContainerSymbol(Container container)
        {
            switch (container.Type)
            {
                case ContainerType.Normal:
                    switch (container.Temperature)
                    {
                        case ContainerTemperature.Normal:
                            return 'N';
                        case ContainerTemperature.Cold:
                            return 'C';
                    }
                    break;
                case ContainerType.Valuable:
                    switch (container.Temperature)
                    {
                        case ContainerTemperature.Normal:
                            return 'V';
                        case ContainerTemperature.Cold:
                            return 'B';
                    }
                    break;
            }

            return ' ';
        }

    }
}

