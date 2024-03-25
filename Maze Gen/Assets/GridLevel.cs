using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLevel : Level
{
    protected Vector3[] NEIGHBORS = new Vector3[4] 
    {
        new Vector3(1, 0, 0),
        new Vector3(0, 1, 1),
        new Vector3(0, -1, 2),
        new Vector3(-1, 0, 3)
    };

    protected int mWidth;
    protected int mHeight;
    public Connections[,] cells;

    public GridLevel(int width, int height)
    {
        mWidth = width;
        mHeight = height;
        cells = new Connections[width, height];
        for (int i=0; i < mWidth; i++)
        {
            for (int j=0; j < mHeight; j++)
            {
                cells[i, j] = new Connections();
            }
        }
    }

    public override void startAt(Location location)
    {
        cells[location.x, location.y].inMaze = true;
    }

    bool canPlaceCorridor(int x, int y, int dirn)
    {
        return (x >= 0 && x < mWidth) && (y >= 0 && y < mHeight) && !cells[x,y].inMaze;
    }

    void shuffleNeighbors()
    {
        int n = NEIGHBORS.Length;
        while (n > 1)
        {
            n--;
            int k = (int)Random.Range(0, n);
            Vector3 v = NEIGHBORS[k];
            NEIGHBORS[k] = NEIGHBORS[n];
            NEIGHBORS[n] = v;
        }
    }

    public override Location makeConnection(Location location)
    {
        shuffleNeighbors();

        int x = location.x;
        int y = location.y;
        foreach (Vector3 v in NEIGHBORS)
        {
            int dx = (int)v.x;
            int dy = (int)v.y;
            int dirn = (int)v.z;

            // check if that location is valid
            int nx = x + dx;
            int ny = y + dy;
            int fromDirn = 3 - dirn;
            if (canPlaceCorridor(nx, ny, fromDirn))
            {
                // perform the connection
                cells[x, y].directions[dirn] = true;
                cells[nx, ny].inMaze = true;
                cells[nx, ny].directions[fromDirn] = true;
                return new Location(nx, ny);
            }
        }

        // none of the locations were valid
        return null;
    }
}
