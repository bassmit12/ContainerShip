using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerShip
{
    public class Ship
    {
        private int width;
        private int length;
        private int height;
        private List<Container>[,] layout;

        // Weight tracking variables
        private int LeftSideWeight { get; set; }
        private int MiddleSideWeight { get; set; }
        private int RightSideWeight { get; set; }

        public Ship(int width, int length)
        {
            this.width = width;
            this.length = length;
            this.height = 1; // Initialize height to 1
            layout = new List<Container>[length, width];

            // Initialize weight tracking variables
            LeftSideWeight = 0;
            MiddleSideWeight = 0;
            RightSideWeight = 0;
        }

        public void PlaceContainers(List<Container> containers)
        {
            // Separate the containers into cold and normal lists
            List<Container> coldContainers = containers
                .Where(container => container.Temperature == ContainerTemperature.Cold)
                .OrderByDescending(container => container.Weight)
                .ToList();

            List<Container> normalContainers = containers
                .Where(container => container.Temperature != ContainerTemperature.Cold)
                .OrderByDescending(container => container.Weight)
                .ToList();

            // Place the cold containers first
            PlaceCooledContainers(coldContainers);

            // Place the normal containers next
            PlaceRemainingContainers(normalContainers);

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
            List<Container>[,] newLayout = new List<Container>[length, width];

            // Copy existing data to the new array
            for (int x = 0; x < length; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    newLayout[x, y] = layout[x, y];
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
                    if (layout[x, y] == null || layout[x, y].Count == layer)
                    {
                        return false;
                    }
                }
            }
            return true;
        }


        private void PlaceCooledContainers(List<Container> containers)
        {
            int col = 0;
            int layer = 0;
            foreach (var container in containers)
            {
                if (container.Temperature == ContainerTemperature.Cold)
                {
                    bool isPlaced = false;
                    while (layer < height && !isPlaced) // Iterate through the layers until a suitable position is found
                    {
                        col = 0; // Reset the column position
                        while (col < width && !isPlaced) // Iterate through the columns until a suitable position is found
                        {
                            if (layout[0, col]?.Count <= layer && CheckWeightLimit(container, col, layer))
                            {
                                if (layout[0, col] == null)
                                {
                                    layout[0, col] = new List<Container>();
                                }
                                layout[0, col].Add(container); // Place in the specified layer
                                isPlaced = true;
                                UpdateWeight(container, col);
                            }
                            col++;
                        }

                        if (!isPlaced)
                        {
                            layer++;
                            if (layer >= height)
                            {
                                IncreaseHeight();
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

        private void PlaceRemainingContainers(List<Container> containers)
        {
            int row = 0;
            int col = 0;
            int layer = 0;
            foreach (var container in containers)
            {
                if (container.Temperature != ContainerTemperature.Cold)
                {
                    bool isPlaced = false;
                    while (layer < height && !isPlaced) // Iterate through the layers until a suitable position is found
                    {
                        while (row < length && !isPlaced) // Iterate through the rows until a suitable position is found
                        {
                            col = 0; // Reset the column position
                            while (col < width && !isPlaced) // Iterate through the columns until a suitable position is found
                            {
                                if (layout[row, col]?.Count <= layer && CheckWeightLimit(container, col, layer))
                                {
                                    if (layout[row, col] == null)
                                    {
                                        layout[row, col] = new List<Container>();
                                    }
                                    layout[row, col].Add(container);
                                    isPlaced = true;
                                    UpdateWeight(container, col);
                                }
                                col++;
                            }
                            if (!isPlaced)
                            {
                                row++;
                                col = 0; // Reset the column position
                                if (row >= length && layer == height - 1)
                                {
                                    // If all layers are full and there is no remaining layer, increase the height
                                    IncreaseHeight();
                                    row = 0; // Reset the row position
                                }
                            }
                        }
                        if (!isPlaced)
                        {
                            layer++;
                            row = 0; // Reset the row position
                        }
                    }

                    if (!isPlaced)
                    {
                        Console.WriteLine("Unable to place container: " + container.ToString());
                    }
                }
            }
        }

        private bool CheckWeightLimit(Container container, int col, int layer)
        {
            int positionWeight = 0;
            for (int z = 0; z <= layer; z++) // Include the current layer in weight calculation
            {
                if (layout[0, col]?.Count > z)
                {
                    positionWeight += layout[0, col][z].Weight;
                }
            }
            return positionWeight + container.Weight <= container.MaxLoad;
        }



        private void UpdateWeight(Container container, int col)
        {
            int shipWidth = width;

            // Calculate the weight based on the container's weight
            int containerWeight = container.Weight;

            // Determine the placement side
            int middleLaneStartCol = shipWidth / 2;

            if (shipWidth % 2 == 1) // Odd width, middle lane exists
            {
                if (col < middleLaneStartCol) // Left side
                {
                    LeftSideWeight += containerWeight;
                }
                else if (col > middleLaneStartCol) // Right side
                {
                    RightSideWeight += containerWeight;
                }
                else // Middle lane
                {
                    MiddleSideWeight += containerWeight;
                }
            }
            else // Even width, no middle lane
            {
                if (col < middleLaneStartCol) // Left side
                {
                    LeftSideWeight += containerWeight;
                }
                else // Right side
                {
                    RightSideWeight += containerWeight;
                }
            }
        }


        public bool IsContainerPlaced(Container container)
        {
            for (int x = 0; x < length; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    if (layout[x, y] != null && layout[x, y].Contains(container))
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

            for (int z = 0; z < height; z++)
            {
                Console.WriteLine($"Layer {z + 1}:");
                for (int x = 0; x < length; x++)
                {
                    for (int y = 0; y < width; y++)
                    {
                        if (layout[x, y] != null && layout[x, y].Count > z)
                        {
                            Console.Write(GetContainerSymbol(layout[x, y][z]) + " ");
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
            Console.WriteLine($"Left Side Weight: {LeftSideWeight}");
            Console.WriteLine($"Middle Side Weight: {MiddleSideWeight}");
            Console.WriteLine($"Right Side Weight: {RightSideWeight}");
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

        public void CheckWeightAboveContainersOnFirstLayer()
        {
            Console.WriteLine("Weight above containers on the first layer:");

            for (int col = 0; col < width; col++)
            {
                for (int row = 0; row < length; row++)
                {
                    int weightAboveContainer = 0;

                    if (layout[row, col] != null && layout[row, col].Count > 0)
                    {
                        for (int z = 1; z < height; z++)
                        {
                            if (layout[row, col].Count > z)
                            {
                                weightAboveContainer += layout[row, col][z].Weight;
                            }
                        }
                    }

                    Console.WriteLine($"Container at column {col + 1}, row {row + 1}: {weightAboveContainer} kg");
                }
            }
        }





    }
}

