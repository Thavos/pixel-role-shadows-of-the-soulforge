using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SRP : AbstractGenerator
{
    [SerializeField]
    int roomCount, offset;

    [SerializeField]
    Vector2Int roomSizeMin, roomSizeMax, mapSize;

    [SerializeField]
    bool randomWalkRooms = false;
    [SerializeField]
    RandomStepWalkSO roomParamRSW;

    public override void Generate()
    {
        HashSet<Vector2Int> map = new HashSet<Vector2Int>();

        HashSet<Vector2Int> rooms = new HashSet<Vector2Int>();
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();

        List<Vector2Int> roomCenters = new List<Vector2Int>();

        GenerateRooms(roomCenters, rooms);

        if (randomWalkRooms == true)
        {
            ApplyRandomWalkV2I(roomCenters, rooms);
        }

        GenerateCorridors(roomCenters, corridors);

        map.UnionWith(rooms);
        map.UnionWith(corridors);
        HashSet<Vector2Int> walls = WallsGenerator.GenerateWalls(map);

        tilemapGen.ClearAll();
        tilemapGen.GenerateTilemap(map);
        tilemapGen.GenerateWalls(walls);
    }

    public void GenerateRooms(List<Vector2Int> roomCenters, HashSet<Vector2Int> rooms)
    {
        for (int i = 0; i < roomCount; i++)
        {
            Vector2Int roomCenter = new Vector2Int(rand.Next(0, mapSize.x), rand.Next(0, mapSize.y));
            if (roomCenter.x > mapSize.x || roomCenter.y > mapSize.y)
            {
                i--;
                continue;
            }

            Vector2Int roomSize = new Vector2Int(rand.Next(roomSizeMin.x, roomSizeMax.x), rand.Next(roomSizeMin.y, roomSizeMax.y));
            if (roomCenter.x + roomSize.x > mapSize.x || roomCenter.y + roomSize.y > mapSize.y)
            {
                i--;
                continue;
            }

            HashSet<Vector2Int> room = new HashSet<Vector2Int>();
            bool contains = false;
            for (int row = 0; row < roomSize.x; row++)
            {
                for (int col = 0; col < roomSize.y; col++)
                {
                    Vector2Int pos = roomCenter + new Vector2Int(row, col);

                    if (rooms.Contains(pos) ||
                        rooms.Contains(pos + Vector2Int.up * offset) ||
                        rooms.Contains(pos + Vector2Int.right * offset) ||
                        rooms.Contains(pos + Vector2Int.down * offset) ||
                        rooms.Contains(pos + Vector2Int.left * offset))
                    {
                        contains = true;
                        break;
                    }

                    room.Add(pos);
                }
                if (contains)
                    break;
            }

            if (contains)
                i--;
            else
            {
                roomCenter = new Vector2Int(roomCenter.x + roomSize.x / 2, roomCenter.y + roomSize.y / 2);
                roomCenters.Add(roomCenter);
                rooms.UnionWith(room);
            }
        }
    }

    public static void GenerateCorridors(List<Vector2Int> roomCenters, HashSet<Vector2Int> corridors)
    {
        Vector2Int curPos = roomCenters[0];
        int roomCount = roomCenters.Count;

        for (int i = 1; i < roomCount; i++)
        {
            Vector2Int nextRoom = FindClosesestCenter(curPos, roomCenters);

            while (curPos.x != nextRoom.x)
            {
                if (curPos.x < nextRoom.x)
                {
                    curPos += Vector2Int.right;
                }
                else
                {
                    curPos += Vector2Int.left;
                }

                corridors.Add(curPos);
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

                corridors.Add(curPos);
            }
        }
    }

    public static Vector2Int FindClosesestCenter(Vector2Int currentCenter, List<Vector2Int> roomCenters)
    {
        int closestId = 0;
        float distance = 0;

        roomCenters.Remove(currentCenter);

        for (int i = 0; i < roomCenters.Count; i++)
        {
            float newDistance = Vector2.Distance(currentCenter, roomCenters[i]);

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

    private void ApplyRandomWalkV2I(List<Vector2Int> roomCenters, HashSet<Vector2Int> rooms)
    {
        RandomStepWalkSO roomParam = roomParamRSW;
        for (int i = 0; i < roomCenters.Count; i++)
        {
            if (roomParamRSW == null)
            {
                roomParam = new RandomStepWalkSO(); //TODO fix this with addition of new function overload or smth (same problem in BSP.cs)
                roomParam.rIterrations = 40;
                roomParam.rWalkLength = Mathf.Max(roomSizeMax.x, roomSizeMax.y) + 5;
                roomParam.randomiseItteration = false;
            }

            RSW.GenerateRoom(rooms, roomCenters[i], roomParam);
        }
    }
}
