using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoronoiDiagram : AbstractGenerator
{
    [SerializeField]
    Vector2Int mapSize, minRoomSize, maxRoomSize;

    [SerializeField]
    int voronoiSeeds, roomCount;

    public override void Generate()
    {
        // 1. Inicialize map
        int[,] mapArray;
        HashSet<Vector2Int> map = new HashSet<Vector2Int>();
        HashSet<Vector2Int> walls = new HashSet<Vector2Int>();
        List<Vector2Int> roomCenters = new List<Vector2Int>();

        // 2. Generate map
        while (roomCenters.Count < roomCount + 2)
        {
            map.Clear();
            roomCenters.Clear();

            mapArray = GenerateArray();
            map.UnionWith(ConvertVoronoiEdges(mapArray));

            SimpleRandomWalk.FindDeadEnds(map, roomCenters);
        }

        // 3. Generate rooms
        GenerateRooms(roomCenters, map);

        // 4. Create walls

        // 5. Draw the map
        tilemapGen.ClearAll();
        tilemapGen.GenerateTilemap(map);
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
        for (int i = 0; i < voronoiSeeds; i++)
        {
            voronois.Add(new HashSet<Vector2Int>());
            Vector2Int seedPos = new Vector2Int(rand.Next(0, mapSize.x), rand.Next(0, mapSize.y));

            voronois[i].Add(seedPos);
            mapArray[seedPos.x, seedPos.y] = i + 1;
        }

        for (int i = 0; i < mapArray.Length / voronoiSeeds; i++)
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

    private void GenerateRooms(List<Vector2Int> roomCenters, HashSet<Vector2Int> map)
    {
        Vector2Int roomCenter, roomSize;

        for (int i = 0; i < roomCount;)
        {
            roomCenter = new Vector2Int(rand.Next(0, mapSize.x), rand.Next(0, mapSize.y));
            roomSize = new Vector2Int(rand.Next(minRoomSize.x, maxRoomSize.x), rand.Next(minRoomSize.y, maxRoomSize.y));

            HashSet<Vector2Int> room = GenerateRoom(roomCenter, roomSize, map);

            roomCenters.Add(roomCenter);
            map.UnionWith(room);

            i++;
        }
    }

    private HashSet<Vector2Int> GenerateRoom(Vector2Int roomCenter, Vector2Int roomSize, HashSet<Vector2Int> map)
    {
        HashSet<Vector2Int> room = new HashSet<Vector2Int>();
        Vector2Int startPoint = roomCenter - (roomSize / 2);

        for (int x = 0; x < roomSize.x; x++)
        {
            for (int y = 0; y < roomSize.y; y++)
            {
                room.Add(startPoint + new Vector2Int(x, y));
            }
        }

        return room;
    }
}
