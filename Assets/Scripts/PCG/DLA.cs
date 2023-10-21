using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DLA : AbstractGenerator
{
    [SerializeField]
    int maxRadius, walkers, steps, roomCount;

    [SerializeField]
    Vector2Int minRoomSize, maxRoomSize;

    public override void Generate()
    {
        int[,] mapArray;
        List<Vector2Int> radiusPoints = new List<Vector2Int>();
        HashSet<Vector2Int> map = new HashSet<Vector2Int>();
        List<Vector2Int> deadEnds = new List<Vector2Int>();

        while (deadEnds.Count < roomCount + 2)
        {
            map.Clear();
            deadEnds.Clear();
            radiusPoints.Clear();

            mapArray = GenerateArray();
            mapArray[mapArray.GetUpperBound(0) / 2, mapArray.GetUpperBound(1) / 2] = 1;
            radiusPoints = FindRadiusPoints(mapArray);

            for (int i = 0; i < walkers; i++)
            {
                RunWalker(mapArray, radiusPoints);
            }

            map = ConvertToMap(mapArray);

            RSW.FindDeadEnds(map, deadEnds);
        }

        GenerateRooms(deadEnds, map, roomCount, minRoomSize, maxRoomSize);

        HashSet<Vector2Int> walls = WallsGenerator.GenerateWalls(map);

        tilemapGen.ClearAll();
        tilemapGen.GenerateTilemap(map);
        tilemapGen.GenerateWalls(walls);
    }

    private List<Vector2Int> FindRadiusPoints(int[,] mapArray)
    {
        Vector2Int centerPoint = new Vector2Int(mapArray.GetUpperBound(0) / 2, mapArray.GetUpperBound(1) / 2);
        List<Vector2Int> radiusPoints = new List<Vector2Int>();

        for (int x = 0; x < mapArray.GetUpperBound(0); x++)
        {
            for (int y = 0; y < mapArray.GetUpperBound(1); y++)
            {
                if ((int)Vector2.Distance(centerPoint, new Vector2(x, y)) == maxRadius / 2)
                {
                    radiusPoints.Add(new Vector2Int(x, y));
                }
            }
        }

        return radiusPoints;
    }

    private void RunWalker(int[,] mapArray, List<Vector2Int> radiusPoints)
    {
        Vector2Int centerPoint = new Vector2Int(mapArray.GetUpperBound(0) / 2, mapArray.GetUpperBound(1) / 2);
        Vector2Int pos = radiusPoints[rand.Next(0, radiusPoints.Count)];

        for (int i = 0; i < steps; i++)
        {
            Vector2Int dir = Directions2D.GetRandomCardinalDir(rand);
            Vector2Int newPos = pos + dir;

            if (newPos.x < 0 || newPos.y < 0 || newPos.x > mapArray.GetUpperBound(0) || newPos.y > mapArray.GetUpperBound(1))
            {
                i--;
                continue;
            }

            if (mapArray[newPos.x, newPos.y] == 1)
            {
                mapArray[pos.x, pos.y] = 1;
                break;
            }
            else
                pos = newPos;
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
        int[,] box = new int[maxRadius + 2, maxRadius + 2];

        for (int x = 0; x < box.GetUpperBound(0); x++)
        {
            for (int y = 0; y < box.GetUpperBound(1); y++)
            {
                box[x, y] = 0;
            }
        }

        return box;
    }

    public static void GenerateRooms(List<Vector2Int> deadEnds, HashSet<Vector2Int> map, int roomCount, Vector2Int minRoomSize, Vector2Int maxRoomSize)
    {
        HashSet<Vector2Int> rooms = new HashSet<Vector2Int>();

        for (int i = 0; i < roomCount; i++)
        {
            Vector2Int roomSize = new Vector2Int(rand.Next(minRoomSize.x, maxRoomSize.x), rand.Next(minRoomSize.y, maxRoomSize.y));
            Vector2Int roomCenter = deadEnds[rand.Next(0, deadEnds.Count)];

            while (rooms.Contains(roomCenter))
            {
                roomCenter = deadEnds[rand.Next(0, deadEnds.Count)];
            }

            roomCenter -= new Vector2Int(roomSize.x / 2, roomSize.y / 2);

            HashSet<Vector2Int> room = new HashSet<Vector2Int>();

            bool contains = false;
            for (int x = 0; x < roomSize.x; x++)
            {
                for (int y = 0; y < roomSize.y; y++)
                {
                    Vector2Int newPos = new Vector2Int(roomCenter.x + x, roomCenter.y + y);

                    if (rooms.Contains(newPos) || rooms.Contains(newPos + Vector2Int.down) || rooms.Contains(newPos + Vector2Int.left) || rooms.Contains(newPos + Vector2Int.up) || rooms.Contains(newPos + Vector2Int.right))
                    {
                        contains = true;
                        break;
                    }

                    room.Add(new Vector2Int(newPos.x, newPos.y));
                }

                if (contains)
                    break;
            }

            if (contains)
                i--;
            else
                rooms.UnionWith(room);
        }

        map.UnionWith(rooms);
    }
}
