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
            // Place all cooled containers on the first row
            int col = 0;
            for (int i = 0; i < containers.Length; i++)
            {
                if (containers[i].Temperature == ContainerTemperature.Cold)
                {
                    bool isPlaced = false;
                    for (int x = 0; x < width; x++)
                    {
                        if (layout[0, x] == null)
                        {
                            layout[0, x] = containers[i];
                            isPlaced = true;
                            break;
                        }
                    }
                    if (!isPlaced)
                    {
                        Console.WriteLine("Unable to place container: " + containers[i].ToString());
                    }
                }
            }

            // Place the remaining containers in the rest of the ship
            int row = 0;
            col = 0;
            for (int i = 0; i < containers.Length; i++)
            {
                if (containers[i].Temperature != ContainerTemperature.Cold)
                {
                    bool isPlaced = false;
                    while (row < length)
                    {
                        if (layout[row, col] == null)
                        {
                            layout[row, col] = containers[i];
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
                        Console.WriteLine("Unable to place container: " + containers[i].ToString());
                    }
                }
            }
        }

        public void PrintLayout()
        {
            Console.WriteLine("Layout with containers:");
            for (int x = 0; x < length; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    if (layout[x, y] != null)
                    {
                        switch (layout[x, y]?.Type)
                        {
                            case ContainerType.Normal:
                                switch (layout[x, y]?.Temperature)
                                {
                                    case ContainerTemperature.Normal:
                                        Console.Write("N ");
                                        break;
                                    case ContainerTemperature.Cold:
                                        Console.Write("C ");
                                        break;
                                }
                                break;
                            case ContainerType.Valuable:
                                switch (layout[x, y]?.Temperature)
                                {
                                    case ContainerTemperature.Normal:
                                        Console.Write("V ");
                                        break;
                                    case ContainerTemperature.Cold:
                                        Console.Write("B ");
                                        break;
                                }
                                break;
                        }
                    }
                    else
                    {
                        Console.Write(". ");
                    }
                }
                Console.WriteLine();
            }
        }

    }
}

