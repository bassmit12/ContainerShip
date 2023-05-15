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
        private int height;
        private Container?[,,] layout;

        public Ship(int width, int length)
        {
            this.width = width;
            this.length = length;
            this.height = 1; // Initialize height to 1
            layout = new Container[length, width, height];
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

            // Check if a new layer needs to be added
            if (IsLayerFull(height - 1))
            {
                IncreaseHeight();
            }
        }

        private void IncreaseHeight()
        {
            int newHeight = height + 1;

            // Create a new temporary array with increased height
            Container?[,,] newLayout = new Container[length, width, newHeight];

            // Copy existing data to the new array
            for (int x = 0; x < length; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    for (int z = 0; z < height; z++)
                    {
                        newLayout[x, y, z] = layout[x, y, z];
                    }
                }
            }

            // Assign the new array to the layout variable
            layout = newLayout;

            // Update the height
            height = newHeight;
        }

        private bool IsLayerFull(int layer)
        {
            for (int x = 0; x < length; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    if (layout[x, y, layer] == null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void PlaceCooledContainers(Container[] containers)
        {
            int col = 0;
            int layer = 0; // Track the current layer
            foreach (var container in containers)
            {
                if (container.Temperature == ContainerTemperature.Cold)
                {
                    bool isPlaced = false;
                    while (col < width && !isPlaced) // Iterate through the first row until a suitable position is found
                    {
                        if (layout[0, col, layer] == null)
                        {
                            layout[0, col, layer] = container; // Place in the specified layer
                            isPlaced = true;
                        }
                        col++;
                    }
                    if (!isPlaced)
                    {
                        // If the first row is full in the current layer, move to the next layer and try again
                        layer++;
                        col = 0; // Reset the column position
                        if (layer >= height)
                        {
                            // If all existing layers are full, add a new layer
                            IncreaseHeight();
                        }
                        // Retry placing the container in the new layer
                        layout[0, col, layer] = container;
                    }
                }
            }
        }

        private void PlaceRemainingContainers(Container[] containers)
        {
            int row = 0;
            int col = 0;
            int layer = 0;
            foreach (var container in containers)
            {
                if (container.Temperature != ContainerTemperature.Cold)
                {
                    bool isPlaced = false;
                    while (layer < height)
                    {
                        if (layout[row, col, layer] == null)
                        {
                            layout[row, col, layer] = container;
                            isPlaced = true;
                            break;
                        }
                        col++;
                        if (col >= width)
                        {
                            row++;
                            col = 0;
                            if (row >= length)
                            {
                                row = 0;
                                col = 0;
                                layer++; // Move to the next layer if the current layer is full
                            }
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
                    for (int z = 0; z < height; z++)
                    {
                        if (layout[x, y, z] == container)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void PrintLayout()
        {
            Console.WriteLine("");
            Console.WriteLine("Layout with containers:");

            for (int z = 0; z < height; z++)
            {
                Console.WriteLine($"Layer {z + 1}:");
                for (int x = 0; x < length; x++)
                {
                    for (int y = 0; y < width; y++)
                    {
                        if (layout[x, y, z] != null)
                        {
                            Console.Write(GetContainerSymbol(layout[x, y, z]) + " ");
                        }
                        else
                        {
                            Console.Write(". ");
                        }
                    }
                    Console.WriteLine();
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

