using System;
using System.Collections.Generic;
using System.ComponentModel;

// Gedaan:
// Een volle container weegt maximaal 30 ton. Een lege container weegt 4000 kg.
// Alle containers die gekoeld moeten blijven moeten in de eerste rij worden geplaatst vanwege de stroomvoorziening die aan de voorkant van elk schip zit.
// De afmeting van een schip moet instelbaar zijn in de applicatie, waarbij de hoogte en breedte in containers aangegeven kan worden.

// To Do:
// Het maximum gewicht bovenop een container is 120 ton.
// Er mag niets bovenop een container met waardevolle lading worden gestapeld; wel mogen deze containers zelf op andere containers geplaatst worden.
// Een container met waardevolle lading moet altijd via de voor- of achterkant te benaderen zijn. Je mag er vanuit gaan dat ook gestapelde containers te benaderen zijn.
// Om kapseizen te voorkomen moet ten minste 50% van het maximum gewicht van een schip zijn benut.
// Het schip moet in evenwicht zijn: het volledige gewicht van de containers voor iedere helft mag niet meer dan 20% van de totale lading verschillen.



namespace ContainerShip
{
    public class Program
    {
        static void Main(string[] args)
        {
            Ship ship = new Ship(5, 3);

            // Create an array of containers
            Container[] containers = new Container[30];

            int index = 0;
            for (int i = 0; i < 15; i++)
            {
                containers[index] = new Container { Type = ContainerType.Normal, Temperature = ContainerTemperature.Normal };
                index++;
            }

            for (int i = 0; i < 7; i++)
            {
                containers[index] = new Container { Type = ContainerType.Normal, Temperature = ContainerTemperature.Cold };
                index++;
            }

            for (int i = 0; i < 7; i++)
            {
                containers[index] = new Container { Type = ContainerType.Valuable, Temperature = ContainerTemperature.Cold };
                index++;
            }

            for (int i = 0; i < 1; i++)
            {
                containers[index] = new Container { Type = ContainerType.Valuable, Temperature = ContainerTemperature.Normal };
                index++;
            }

            // Place the containers on the ship
            ship.PlaceContainers(containers);

            // Print the ship layout
            ship.PrintLayout();
            ship.CheckWeightAboveContainersOnFirstLayer();
        }
    }
}