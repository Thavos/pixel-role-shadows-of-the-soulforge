using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRoomPlacement : AbstractGenerator
{
    [SerializeField]
    private int roomCount, offset, corridorWidth;

    [SerializeField]
    private Vector2Int minRoomSize, maxRoomSize, mapSize;

    public override void Generate()
    {
        // 1. Inicialize map
        HashSet<Vector2Int> map = new HashSet<Vector2Int>();
        List<Vector2Int> roomCenters = new List<Vector2Int>();

        // 2. Generate rooms
        GenerateRooms(roomCenters, map);

        // 3. Create corridors
        GenerateCorridors(roomCenters, map, corridorWidth);

        // 4. Create walls

        // 5. Draw the map
        tilemapGen.ClearAll();
        tilemapGen.GenerateTilemap(map);
    }

    private void GenerateRooms(List<Vector2Int> roomCenters, HashSet<Vector2Int> map)
    {
        Vector2Int roomCenter, roomSize;

        for (int i = 0; i < roomCount;)
        {
            roomCenter = new Vector2Int(rand.Next(0, mapSize.x), rand.Next(0, mapSize.y));
            roomSize = new Vector2Int(rand.Next(minRoomSize.x, maxRoomSize.x), rand.Next(minRoomSize.y, maxRoomSize.y));

            // Check if map contains future room
            Vector2Int halfRoomSize = roomSize / 2;
            if (CheckIfMapContainsRoom(roomCenter, roomSize, map))
                continue;

            HashSet<Vector2Int> room = GenerateRoom(roomCenter, roomSize, map);

            roomCenters.Add(roomCenter);
            map.UnionWith(room);

            i++;
        }
    }

    private bool CheckIfMapContainsRoom(Vector2Int roomCenter, Vector2Int roomSize, HashSet<Vector2Int> map)
    {
        bool contains = false;
        Vector2Int startPoint = roomCenter - (roomSize / 2);

        for (int x = -offset; x < roomSize.x + offset; x++)
        {
            for (int y = -offset; y < roomSize.y + offset; y++)
            {
                contains = map.Contains(startPoint + new Vector2Int(x, y));
                if (contains)
                    return true;
            }
        }

        return false;
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

    public static void GenerateCorridors(List<Vector2Int> roomCenters, HashSet<Vector2Int> map, int corridorWidth)
    {
        Vector2Int curPos = roomCenters[0];
        int roomCount = roomCenters.Count;

        for (int i = 1; i < roomCount; i++)
        {
            Vector2Int nextRoom = FindClosestCenter(curPos, roomCenters);
            HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();

            Vector2Int dir = Vector2Int.right;

            while (curPos.x != nextRoom.x)
            {
                if (curPos.x < nextRoom.x)
                {
                    curPos += Vector2Int.right;
                    dir = Vector2Int.left;
                }
                else
                {
                    curPos += Vector2Int.left;
                    dir = Vector2Int.right;
                }

                corridor.Add(curPos);
                if (corridorWidth > 1)
                    for (int width = 0; width < corridorWidth; width++)
                    {
                        corridor.Add(curPos + Vector2Int.up * width);
                    }
            }

            while (curPos.y != nextRoom.y)
            {
                if (curPos.y < nextRoom.y)
                {
                    curPos += Vector2Int.up;
                }
                else
                {
                    curPos += Vector2Int.down;
                }

                corridor.Add(curPos);
                if (corridorWidth > 1)
                    for (int width = 0; width < corridorWidth; width++)
                    {
                        corridor.Add(curPos + dir * width);
                    }
            }

            map.UnionWith(corridor);
        }
    }

    public static Vector2Int FindClosestCenter(Vector2Int currentPos, List<Vector2Int> roomCenters)
    {
        int closestId = 0;
        float distance = 0;

        roomCenters.Remove(currentPos);

        for (int i = 0; i < roomCenters.Count; i++)
        {
            float newDistance = Vector2.Distance(currentPos, roomCenters[i]);

            if (newDistance < distance && newDistance > 0)
            {
                distance = newDistance;
                closestId = i;
            }

            if (i == 0)
                distance = newDistance;
        }

        return roomCenters[closestId];
    }
}
