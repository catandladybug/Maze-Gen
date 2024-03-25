using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeMaker : MonoBehaviour
{
    public int mazeWidth;
    public int mazeHeight;
    public Location mazeStart = new Location(0,0);
    bool hasExit = false;

    GridLevelWithRooms levelOne;
    GameObject wallPrefab;
    GameObject blockerPrefab;

    void Start()
    {
        wallPrefab = Resources.Load<GameObject>("Wall");
        blockerPrefab = Resources.Load<GameObject>("Blocker");

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
            foreach(GameObject wall in walls)
            {
                Destroy(wall);
            }

            mazeWidth = (int)Random.Range(10f, 20f);
            mazeHeight = (int)Random.Range(10f, 20f);
            mazeStart = new Location((int)Random.Range(0f, mazeWidth - 1), 0);
            levelOne = new GridLevelWithRooms(mazeWidth, mazeHeight);

            generateMaze(levelOne, mazeStart);
            MakeDoorway(mazeStart);

            if (hasExit)
            {
                int exitX = mazeWidth - 1 - mazeStart.x;
                int exitY = mazeHeight - 1 - mazeStart.y;
                Location exit = new Location(exitX, exitY);
                MakeDoorway(exit);
            }
            BuildMaze();
        }

        if (levelOne != null)
        {
            for (int x = 0; x < mazeWidth; x++)
            {
                for (int y = 0; y < mazeHeight; y++)
                {
                    Connections currentCell = levelOne.cells[x, y];
                    if (currentCell.inMaze)
                    {
                        Vector3 cellPos = new Vector3(x, 0, y);
                        float lineLength = 1f;
                        if (currentCell.directions[0])
                        {
                            Vector3 neighborPos = new Vector3(x + lineLength, 0, y);
                            Debug.DrawLine(cellPos, neighborPos, Color.cyan);
                        }
                        if (currentCell.directions[1])
                        {
                            Vector3 neighborPos = new Vector3(x, 0, y + lineLength);
                            Debug.DrawLine(cellPos, neighborPos, Color.cyan);
                        }
                        if (currentCell.directions[2])
                        {
                            Vector3 neighborPos = new Vector3(x, 0, y - lineLength);
                            Debug.DrawLine(cellPos, neighborPos, Color.cyan);
                        }
                        if (currentCell.directions[3])
                        {
                            Vector3 neighborPos = new Vector3(x - lineLength, 0, y);
                            Debug.DrawLine(cellPos, neighborPos, Color.cyan);
                        }
                    }
                }
            }
        }
    }

    void BuildMaze()
    {
        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                Connections currentCell = levelOne.cells[x, y];
                if (levelOne.cells[x, y].inMaze)
                {
                    Vector3 cellPos = new Vector3(x, 0, y);
                    float lineLength = 1f;
                    if (!currentCell.directions[0])
                    {
                        Vector3 wallPos = new Vector3(x + lineLength / 2, 0, y);
                        GameObject wall = Instantiate(wallPrefab, wallPos, Quaternion.identity) as GameObject;
                    }
                    if (!currentCell.directions[1])
                    {
                        Vector3 wallPos = new Vector3(x, 0, y + lineLength / 2);
                        GameObject wall = Instantiate(wallPrefab, wallPos, Quaternion.Euler(0f, 90f, 0f)) as GameObject;
                    }
                    if (y == 0 && !currentCell.directions[2])
                    {
                        Vector3 wallPos = new Vector3(x, 0, y - lineLength / 2);
                        GameObject wall = Instantiate(wallPrefab, wallPos, Quaternion.Euler(0f, 90f, 0f)) as GameObject;
                    }
                    if (x == 0 && !currentCell.directions[3])
                    {
                        Vector3 wallPos = new Vector3(x - lineLength / 2, 0, y);
                        GameObject wall = Instantiate(wallPrefab, wallPos, Quaternion.identity) as GameObject;
                    }
                }
                if (!currentCell.directions[0] && !currentCell.directions[1] && !currentCell.directions[2] && !currentCell.directions[3])
                {
                    GameObject blocker = Instantiate(blockerPrefab, new Vector3(x, 0, y), Quaternion.identity) as GameObject;
                }
            }
        }
    }

    void MakeDoorway(Location location)
    {
        Connections cell = levelOne.cells[location.x, location.y];

        if (location.x == 0)
        {
            cell.directions[3] = true;
        }
        else if (location.x == mazeWidth - 1)
        {
            cell.directions[0] = true;
        }
        else if (location.y == 0)
        {
            cell.directions[2] = true;
        }
        else if (location.y == mazeHeight - 1)
        {
            cell.directions[1] = true;
        }
    }

    void generateMaze(Level level, Location start)
    {
        Stack<Location> locations = new Stack<Location>();
        locations.Push(start);
        level.startAt(start);

        while (locations.Count > 0)
        {
            Location current = locations.Peek();
            Location next = level.makeConnection(current);

            if (next != null)
            {
                locations.Push(next);
            }
            else
            {
                locations.Pop();
            }
        }
    }
}
