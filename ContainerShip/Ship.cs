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
        private ContainerType?[,] layout;

        public Ship(int width, int length)
        {
            this.width = width;
            this.length = length;
            layout = new ContainerType?[length, width];
        }

        public void PlaceContainers(Container[] containers)
        {
            int row = 0;
            int col = 0;
            for (int i = 0; i < containers.Length; i++)
            {
                layout[row, col] = containers[i].Type;

                col++;
                if (col >= width)
                {
                    row++;
                    col = 0;
                }

                if (row >= length || col >= width)
                {
                    break;
                }
            }
        }

        public void PrintLayout()
        {
            Console.WriteLine("Layout with containers:");
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    var container = layout[i, j];
                    Console.Write(container switch
                    {
                        ContainerType.Normal => "N",
                        ContainerType.Cooled => "C",
                        ContainerType.Valuable => "V",
                        _ => "."
                    } + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
