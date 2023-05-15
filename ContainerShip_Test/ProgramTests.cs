using Microsoft.VisualStudio.CodeCoverage;
using System.ComponentModel;
using ContainerShip;
using System.Text;

namespace ContainerShip_Test
{
    [TestFixture]
    public class ProgramTests
    {
        [Test]
        public void PlaceContainers_SingleContainer_SuccessfulPlacement()
        {
            // Arrange
            Ship ship = new Ship(4, 3);
            ContainerShip.Container[] containers = new ContainerShip.Container[]
            {
                new ContainerShip.Container { Type = ContainerType.Normal, Temperature = ContainerTemperature.Normal }
            };

            // Act
            ship.PlaceContainers(containers);

            // Assert
            // Ensure the container is placed correctly
            Assert.IsTrue(ship.IsContainerPlaced(containers[0]));
        }

        [Test]
        public void PlaceContainers_MultipleContainers_SuccessfulPlacement()
        {
            // Arrange
            Ship ship = new Ship(4, 3);
            ContainerShip.Container[] containers = new ContainerShip.Container[]
            {
                new ContainerShip.Container { Type = ContainerType.Normal, Temperature = ContainerTemperature.Normal },
                new ContainerShip.Container { Type = ContainerType.Normal, Temperature = ContainerTemperature.Cold },
                new ContainerShip.Container { Type = ContainerType.Normal, Temperature = ContainerTemperature.Cold }
            };

            // Act
            ship.PlaceContainers(containers);

            // Assert
            // Ensure all containers are placed correctly
            foreach (var container in containers)
            {
                Assert.IsTrue(ship.IsContainerPlaced(container));
            }
        }
    }
}