using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VD : AbstractGenerator
{
    [SerializeField]
    Vector2Int mapSize, minRoomSize, maxRoomSize;

    [SerializeField]
    int seedCount, roomCount;

    public override void Generate()
    {
        int[,] mapArray;
        HashSet<Vector2Int> map = new HashSet<Vector2Int>();
        HashSet<Vector2Int> walls = new HashSet<Vector2Int>();
        List<Vector2Int> deadEnds = new List<Vector2Int>();

        while (deadEnds.Count < roomCount + 2)
        {
            map.Clear();
            deadEnds.Clear();

            mapArray = GenerateArray();
            map.UnionWith(ConvertVoronoiEdges(mapArray));

            RSW.FindDeadEnds(map, deadEnds);
        }

        DLA.GenerateRooms(deadEnds, map, roomCount, minRoomSize, maxRoomSize);

        walls = WallsGenerator.GenerateWalls(map);

        tilemapGen.ClearAll();
        tilemapGen.GenerateTilemap(map);
        tilemapGen.GenerateWalls(walls);
    }

    private int[,] GenerateArray()
    {
        int[,] mapArray = new int[mapSize.x, mapSize.y];

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                mapArray[x, y] = 0;
            }
        }

        List<HashSet<Vector2Int>> voronois = new List<HashSet<Vector2Int>>();
        for (int i = 0; i < seedCount; i++)
        {
            voronois.Add(new HashSet<Vector2Int>());
            Vector2Int seedPos = new Vector2Int(rand.Next(0, mapSize.x), rand.Next(0, mapSize.y));

            voronois[i].Add(seedPos);
            mapArray[seedPos.x, seedPos.y] = i + 1;
        }

        for (int i = 0; i < mapArray.Length / seedCount; i++)
        {
            for (int j = 0; j < roomCount; j++)
            {
                foreach (var pos in voronois[j])
                {
                    bool added = false;
                    foreach (var dir in Directions2D.CardinalDir)
                    {
                        Vector2Int newPos = pos + dir;

                        if (newPos.x < 0 || newPos.x > mapArray.GetUpperBound(0) || newPos.y < 0 || newPos.y > mapArray.GetUpperBound(1))
                            continue;

                        if (mapArray[newPos.x, newPos.y] == 0)
                        {
                            added = true;
                            voronois[j].Add(newPos);
                            mapArray[newPos.x, newPos.y] = j + 1;
                            break;
                        }
                    }

                    if (added)
                        break;
                }
            }
        }

        return mapArray;
    }

    private HashSet<Vector2Int> ConvertVoronoiEdges(int[,] mapArray)
    {
        HashSet<Vector2Int> edges = new HashSet<Vector2Int>();
        List<Vector2Int> directions = new List<Vector2Int>();
        directions.Add(Vector2Int.right);
        directions.Add(Vector2Int.up);
        directions.Add(Vector2Int.right + Vector2Int.up);

        for (int x = 0; x < mapArray.GetUpperBound(0); x++)
        {
            for (int y = 0; y < mapArray.GetUpperBound(1); y++)
            {
                foreach (var dir in directions)
                {
                    Vector2Int newPos = new Vector2Int(x, y) + dir;

                    if (newPos.x < 0 || newPos.x > mapArray.GetUpperBound(0) || newPos.y < 0 || newPos.y > mapArray.GetUpperBound(1))
                        continue;

                    if (mapArray[x, y] != mapArray[newPos.x, newPos.y] && edges.Contains(newPos) == false)
                    {
                        edges.Add(newPos);
                    }
                }
            }
        }

        return edges;
    }

    private HashSet<Vector2Int> ConvertVoronoiToMap(int[,] mapArray, int id)
    {
        HashSet<Vector2Int> map = new HashSet<Vector2Int>();

        for (int x = 0; x < mapArray.GetUpperBound(0); x++)
        {
            for (int y = 0; y < mapArray.GetUpperBound(1); y++)
            {
                if (mapArray[x, y] == id)
                    map.Add(new Vector2Int(x, y));
            }
        }

        return map;
    }
}
