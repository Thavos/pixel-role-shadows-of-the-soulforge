using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellularAutomata : AbstractGenerator
{
    [SerializeField]
    [Range(0.1f, 0.9f)]
    private float coveragePercentage = 0.1f;

    [SerializeField]
    private int iterations;

    [SerializeField]
    private Vector2Int mapSize;

    public override void Generate()
    {
        // 1. Inicialize map
        HashSet<Vector2Int> map = new HashSet<Vector2Int>();
        int[,] mapArray = GenerateNoiseMap();

        // 2. Generate cellular automata iterations
        for (int i = 0; i < iterations; i++)
        {
            mapArray = RunCLA(mapArray);
        }

        // 3. Convert automat output to map
        ConvertToMap(mapArray, map);

        // 4. Generate start and finish positions, check if theres path between them
        List<Vector2Int> mapList = new List<Vector2Int>(map);

        Vector2Int start = Vector2Int.zero,
                   finish = Vector2Int.zero;

        float minimalDistance = Mathf.Sqrt(Mathf.Pow(mapSize.x, 2) + Mathf.Pow(mapSize.y, 2)) * 0.6f; // 60% of diagonal length
        while (Vector2Int.Distance(start, finish) < minimalDistance)
        {
            start = mapList[rand.Next(0, mapList.Count)];
            finish = mapList[rand.Next(0, mapList.Count)];
        }

        List<Vector2Int> AStarPath = PathfindingAStar.AStar(start, finish, map);
        if (AStarPath.Count == 0)
            return; // TODO reset on no path found

        // 5. Flood-fill map hashset to get rid of unreachable parts of map
        map = FloodFill.CheckReachableTiles(start, map);

        // 6. Update player and finish position
        player.position = new Vector3(start.x + 0.5f, start.y + 0.5f, 0);
        portal.position = new Vector3(finish.x + 0.5f, finish.y + 0.5f, 0);

        // 7. Draw the map and generate walls
        tilemapGen.ClearAll();
        tilemapGen.GenerateTilemap(map);
        //tilemapGen.ShowPath(AStarPath); // Show A* path
    }

    private void ConvertToMap(int[,] mapArray, HashSet<Vector2Int> map)
    {
        for (int x = 0; x < mapArray.GetUpperBound(0); x++)
        {
            for (int y = 0; y < mapArray.GetUpperBound(1); y++)
            {
                if (mapArray[x, y] == 1)
                    map.Add(new Vector2Int(x, y));
            }
        }
    }

    private int[,] RunCLA(int[,] mapArray)
    {
        int[,] newMapArray = new int[mapSize.x, mapSize.y];

        // We check every single tile in a map
        for (int x = 0; x < mapArray.GetUpperBound(0); x++)
        {
            for (int y = 0; y < mapArray.GetUpperBound(1); y++)
            {
                // Each tile is checked for its neighbours
                //   we change current tile if theres more or exactly four neighbouring tiles
                //   with value of 1 (meaning floor tile) to another floor tile
                //   on the other saide if there are four 0 values (meaning wall tile)
                //   we change current tile to wall aswell
                int neighbourCount = 0;
                foreach (var dir in Directions2D.EightDir)
                {
                    int xPos = x + dir.x;
                    int yPos = y + dir.y;

                    if (xPos < 0 || xPos > mapArray.GetUpperBound(0) || yPos < 0 || yPos > mapArray.GetUpperBound(1))
                        continue;

                    else if (mapArray[xPos, yPos] == 1)
                        neighbourCount++;
                }

                if (neighbourCount >= 4)
                    newMapArray[x, y] = 1;
                else
                    newMapArray[x, y] = 0;
            }
        }

        return newMapArray;
    }

    public int[,] GenerateNoiseMap()
    {
        // We generate our own simple noise based on percent coverage
        int[,] map = new int[mapSize.x, mapSize.y];
        int cellsCount = Mathf.RoundToInt(mapSize.x * mapSize.y * coveragePercentage);

        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                map[x, y] = 0;
            }
        }

        for (int i = 0; i < cellsCount; i++)
        {
            int x = rand.Next(0, map.GetUpperBound(0));
            int y = rand.Next(0, map.GetUpperBound(1));

            if (map[x, y] == 1)
                i--;
            else
                map[x, y] = 1;
        }

        return map;
    }
}
