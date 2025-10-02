using System;

class Room
{
    public string Name;
    public string Description;
    public string FirstVisitDescription;
    public string AsciiText;
    public int EnergyUsed = 0;

    public bool isVisited = false;
    public bool isTradeable;

    public Room NorthDoor;
    public Room SouthDoor;
    public Room EastDoor;
    public Room WestDoor;

    public Room(string name, string description, string firstVisit, string asciiText, bool istradable)
    {
        Name = name;
        Description = description;
        FirstVisitDescription = firstVisit;
        AsciiText = asciiText;
        isTradeable = istradable;
        
    }

    public void Visit()
    {
        if (!isVisited)
        {
            Console.WriteLine($"\nYou enter the {Name} for the first time.\n{Description}\n");
            Console.WriteLine(FirstVisitDescription);
            isVisited = true;
        }
        else
        {
            Console.WriteLine($"You are in the {Name}");
            Console.WriteLine("Nothing new to see here");
        }
    }


}