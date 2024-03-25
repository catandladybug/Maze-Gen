using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Level
{
    public abstract void startAt(Location location);
    public abstract Location makeConnection(Location location);
}

public class Location
{
    public int x;
    public int y;

    public Location(int xCoord, int yCoord)
    {
        x = xCoord;
        y = yCoord;
    }
}

public class Connections
{
    public bool inMaze;
    public bool[] directions = new bool[4] {false, false, false, false};

    public override string ToString()
    {
        string s = "InMaze = " + inMaze + "; Connections: ";
        foreach (bool b in directions)
        {
            s += (b + " ");
        }
        return s;
    }
}
