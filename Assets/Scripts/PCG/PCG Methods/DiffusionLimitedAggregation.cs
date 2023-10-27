using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiffusionLimitedAggregation : AbstractGenerator
{
    [SerializeField]
    private int maxRadius, roomCount, offset;

    [SerializeField]
    [Range(0.1f, 1f)]
    private float coveragePercentage;

    [SerializeField]
    private Vector2Int minRoomSize, maxRoomSize;


    public override void Generate()
    {
        // 1. Inicialize map
        List<Vector2Int> deadEnds = new List<Vector2Int>();
        HashSet<Vector2Int> map = new HashSet<Vector2Int>();

        // 2. Find start positions for walkers
        List<Vector2Int> startPositions = FindRadiusPoints(map);

        // 3. Run walkers
        RunWalker(map, startPositions);

        // 4. Generate rooms at dead ends
        SimpleRandomWalk.FindDeadEnds(map, deadEnds);
        GenerateRooms(deadEnds, map, roomCount);

        // 5. Create walls

        // 6. Draw the map
        tilemapGen.ClearAll();
        tilemapGen.GenerateTilemap(map);
    }

    // TODO - do i really have to ensure walkers start on radius around center point ?
    private List<Vector2Int> FindRadiusPoints(HashSet<Vector2Int> mapArray)
    {
        Vector2Int centerPoint = new Vector2Int(maxRadius / 2, maxRadius / 2);
        mapArray.Add(centerPoint);
        List<Vector2Int> radiusPoints = new List<Vector2Int>();

        for (int x = 0; x < maxRadius + 1; x++)
        {
            for (int y = 0; y < maxRadius + 1; y++)
            {
                if ((int)Vector2.Distance(centerPoint, new Vector2(x, y)) == maxRadius / 2)
                {
                    radiusPoints.Add(new Vector2Int(x, y));
                }
            }
        }

        return radiusPoints;
    }

    private List<Vector2Int> DeterminStartPositions(int[,] mapArray)
    {
        List<Vector2Int> startPositions = new List<Vector2Int>();

        for (int x = 0; x < mapArray.GetUpperBound(0) + 1; x++)
        {
            for (int y = 0; y < mapArray.GetUpperBound(1) + 1; y++)
            {
                if (x == 0 || x == mapArray.GetUpperBound(0) ||
                    y == 0 || y == mapArray.GetUpperBound(1))
                    startPositions.Add(new Vector2Int(x, y));
            }
        }

        return startPositions;
    }

    private void RunWalker(HashSet<Vector2Int> mapArray, List<Vector2Int> startPositions)
    {
        Vector2Int centerPoint = new Vector2Int(maxRadius / 2, maxRadius / 2);
        int tilesAmount = Mathf.RoundToInt(Mathf.Pow(maxRadius, 2) * coveragePercentage);

        while (tilesAmount > 0)
        {
            Vector2Int pos = startPositions[rand.Next(0, startPositions.Count)];
            for (int i = 0; i < maxRadius;)
            {
                Vector2Int dir = Directions2D.GetRandomCardinalDir(rand);

                if (rand.Next(0, 2) > 0.5f)
                {
                    if (pos.x > centerPoint.x)
                        dir = Vector2Int.left;
                    else if (pos.x < centerPoint.x)
                        dir = Vector2Int.right;
                    else if (pos.y > centerPoint.y)
                        dir = Vector2Int.down;
                    else if (pos.y < centerPoint.y)
                        dir = Vector2Int.up;
                }
                else
                {
                    if (pos.y > centerPoint.y)
                        dir = Vector2Int.down;
                    else if (pos.y < centerPoint.y)
                        dir = Vector2Int.up;
                    else if (pos.x > centerPoint.x)
                        dir = Vector2Int.left;
                    else if (pos.x < centerPoint.x)
                        dir = Vector2Int.right;
                }

                Vector2Int newPos = pos + dir;

                if (Vector2Int.Distance(newPos, centerPoint) > maxRadius)
                    break;


                if (mapArray.Contains(newPos))
                {
                    mapArray.Add(pos);
                    tilesAmount--;
                    break;
                }
                else
                    pos = newPos;

                i++;
            }
        }
    }

    private HashSet<Vector2Int> ConvertToMap(int[,] mapArray)
    {
        HashSet<Vector2Int> map = new HashSet<Vector2Int>();

        for (int x = 0; x < mapArray.GetUpperBound(0); x++)
        {
            for (int y = 0; y < mapArray.GetUpperBound(1); y++)
            {
                if (mapArray[x, y] == 1)
                    map.Add(new Vector2Int(x, y));
            }
        }

        return map;
    }

    private int[,] GenerateArray()
    {
        int[,] mapArray = new int[maxRadius + 2, maxRadius + 2];

        for (int x = 0; x < mapArray.GetUpperBound(0); x++)
        {
            for (int y = 0; y < mapArray.GetUpperBound(1); y++)
            {
                mapArray[x, y] = 0;
            }
        }

        // Set center to 1 for DLA to work
        mapArray[mapArray.GetUpperBound(0) / 2, mapArray.GetUpperBound(1) / 2] = 1;

        return mapArray;
    }

    private void GenerateRooms(List<Vector2Int> deadEnds, HashSet<Vector2Int> map, int roomCount)
    {
        HashSet<Vector2Int> rooms = new HashSet<Vector2Int>();

        int roomToCreate = roomCount > deadEnds.Count ? deadEnds.Count : roomCount;

        for (int i = 0; i < roomToCreate;)
        {
            HashSet<Vector2Int> room = new HashSet<Vector2Int>();
            Vector2Int roomSize = new Vector2Int(rand.Next(minRoomSize.x, maxRoomSize.x), rand.Next(minRoomSize.y, maxRoomSize.y));
            Vector2Int roomStart = deadEnds[rand.Next(0, deadEnds.Count)];
            roomStart -= new Vector2Int(roomSize.x / 2, roomSize.y / 2);

            if (CheckRoomOverlap(rooms, roomSize, roomStart) == false)
            {
                deadEnds.Remove(roomStart);

                for (int x = 0; x < roomSize.x; x++)
                {
                    for (int y = 0; y < roomSize.y; y++)
                    {
                        Vector2Int newPos = roomStart + new Vector2Int(x, y);
                        room.Add(newPos);
                    }
                }

                rooms.UnionWith(room);
                i++;
            }
        }

        map.UnionWith(rooms);
    }

    private bool CheckRoomOverlap(HashSet<Vector2Int> rooms, Vector2Int roomSize, Vector2Int roomStart)
    {
        bool contains = false;
        HashSet<Vector2Int> border = new HashSet<Vector2Int>();

        for (int x = -offset; x < roomSize.x + offset; x++)
        {
            border.Add(new Vector2Int(roomStart.x + x, roomStart.y - offset));
            border.Add(new Vector2Int(roomStart.x + x, roomStart.y + roomSize.y + offset));
        }

        for (int y = 0; y < roomSize.y; y++)
        {
            border.Add(new Vector2Int(roomStart.x - offset, roomStart.y + y));
            border.Add(new Vector2Int(roomStart.x + roomSize.x + offset, roomStart.y + y));
        }

        foreach (var pos in border)
        {
            if (rooms.Contains(pos))
            {
                contains = true;
                break;
            }
        }

        return contains;
    }
}
