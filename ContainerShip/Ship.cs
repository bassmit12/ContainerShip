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
                if (containers[i].Type == ContainerType.Cooled)
                {
                    layout[0, col] = containers[i];
                    col++;
                    if (col >= width)
                    {
                        break;
                    }
                }
            }

            // Place the remaining containers on the first row if the space is not already taken by a container
            col = 0;
            for (int i = 0; i < containers.Length; i++)
            {
                if (containers[i].Type != ContainerType.Cooled)
                {
                    if (layout[0, col] == null)
                    {
                        layout[0, col] = containers[i];
                        col++;
                        if (col >= width)
                        {
                            break;
                        }
                    }
                    else
                    {
                        // Move to the next column if the space is already taken
                        col++;
                        if (col >= width)
                        {
                            break;
                        }
                    }
                }
            }

            // Place the remaining containers in the rest of the ship
            int row = 1;
            col = 0;
            for (int i = 0; i < containers.Length; i++)
            {
                if (containers[i].Type != ContainerType.Cooled)
                {
                    if (layout[row, col] == null)
                    {
                        layout[row, col] = containers[i];

                        col++;
                        if (col >= width)
                        {
                            row++;
                            col = 0;
                        }

                        if (row >= length)
                        {
                            break;
                        }
                    }
                    else
                    {
                        // Move to the next column if the space is already taken
                        col++;
                        if (col >= width)
                        {
                            row++;
                            col = 0;
                        }

                        if (row >= length)
                        {
                            break;
                        }
                        // Place the container in the next available space
                        if (layout[row, col] == null)
                        {
                            layout[row, col] = containers[i];

                            col++;
                            if (col >= width)
                            {
                                row++;
                                col = 0;
                            }

                            if (row >= length)
                            {
                                break;
                            }
                        }
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
                                Console.Write("N ");
                                break;
                            case ContainerType.Cooled:
                                Console.Write("C ");
                                break;
                            case ContainerType.Valuable:
                                Console.Write("V ");
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
