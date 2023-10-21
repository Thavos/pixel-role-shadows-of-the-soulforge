using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSP : AbstractGenerator
{
    [SerializeField]
    BoundsInt mapSize;

    [SerializeField]
    Vector2Int roomSizeMin;

    [SerializeField]
    int roomCount, offset;

    [SerializeField]
    bool randomWalkRooms = false;
    [SerializeField]
    RandomStepWalkSO roomParamRSW;

    //TODO refactor into smaller functions
    public override void Generate()
    {
        Queue<BoundsInt> roomQueue = new Queue<BoundsInt>();
        List<BoundsInt> roomList = new List<BoundsInt>();

        while (roomList.Count < roomCount)
        {
            roomList.Clear();
            roomQueue.Clear();

            roomQueue.Enqueue(mapSize);
            while (roomQueue.Count > 0)
            {
                var room = roomQueue.Dequeue();

                if (room.size.x >= roomSizeMin.x && room.size.y >= roomSizeMin.y)
                {
                    if (Random.Range(0f, 1f) < 0.5f)
                    {
                        if (room.size.y >= roomSizeMin.y * 2)
                        {
                            HorizontalSplit(roomQueue, room);
                        }
                        else if (room.size.x >= roomSizeMin.x * 2)
                        {
                            VerticalSplit(roomQueue, room);
                        }
                        else if (room.size.x >= roomSizeMin.x && room.size.y >= roomSizeMin.y)
                        {
                            roomList.Add(room);
                        }
                    }
                    else
                    {
                        if (room.size.x >= roomSizeMin.x * 2)
                        {
                            VerticalSplit(roomQueue, room);
                        }
                        else if (room.size.y >= roomSizeMin.y * 2)
                        {
                            HorizontalSplit(roomQueue, room);
                        }
                        else if (room.size.x >= roomSizeMin.x && room.size.y >= roomSizeMin.y)
                        {
                            roomList.Add(room);
                        }
                    }
                }
            }
        }

        HashSet<Vector2Int> map = new HashSet<Vector2Int>();

        HashSet<Vector2Int> rooms = BoundToHashRoom(roomList);
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();

        List<Vector2Int> roomCenters = new List<Vector2Int>();
        foreach (var roomBound in roomList)
        {
            roomCenters.Add(new Vector2Int((int)Mathf.Floor(roomBound.center.x), (int)Mathf.Floor(roomBound.center.y)));
        }

        if (randomWalkRooms == true)
        {
            RandomStepWalkSO roomParam = roomParamRSW;

            for (int i = 0; i < roomCenters.Count; i++)
            {
                if (roomParamRSW == null)
                {
                    roomParam = new RandomStepWalkSO(); //TODO fix this with addition of new function overload or smth (same problem in SRP.cs)
                    roomParam.rIterrations = 40;
                    roomParam.rWalkLength = Mathf.Max(roomList[i].size.x, roomList[i].size.y) + 5;
                    roomParam.randomiseItteration = false;
                }

                RSW.GenerateRoom(rooms, roomCenters[i], roomParam);
            }
        }

        for (int i = 0; i < roomCenters.Count; i++)
        {
            SRP.GenerateCorridors(roomCenters, corridors);
        }

        map.UnionWith(rooms);
        map.UnionWith(corridors);
        HashSet<Vector2Int> walls = WallsGenerator.GenerateWalls(map);

        tilemapGen.ClearAll();
        tilemapGen.GenerateTilemap(map);
        tilemapGen.GenerateWalls(walls);
    }

    private void HorizontalSplit(Queue<BoundsInt> roomQueue, BoundsInt curRoom)
    {
        var ySplit = rand.Next(1, curRoom.size.y);

        BoundsInt nRoom1 = new BoundsInt(curRoom.min, new Vector3Int(curRoom.size.x, ySplit, curRoom.size.z));
        BoundsInt nRoom2 = new BoundsInt(new Vector3Int(curRoom.min.x, curRoom.min.y + ySplit, curRoom.min.z),
                                         new Vector3Int(curRoom.size.x, curRoom.size.y - ySplit, curRoom.size.z));

        roomQueue.Enqueue(nRoom1);
        roomQueue.Enqueue(nRoom2);
    }

    private static void VerticalSplit(Queue<BoundsInt> roomQueue, BoundsInt curRoom)
    {
        var xSplit = Random.Range(1, curRoom.size.x);

        BoundsInt nRoom1 = new BoundsInt(curRoom.min, new Vector3Int(xSplit, curRoom.size.y, curRoom.size.z));
        BoundsInt nRoom2 = new BoundsInt(new Vector3Int(curRoom.min.x + xSplit, curRoom.min.y, curRoom.min.z),
                                         new Vector3Int(curRoom.size.x - xSplit, curRoom.size.y, curRoom.size.z));

        roomQueue.Enqueue(nRoom1);
        roomQueue.Enqueue(nRoom2);
    }

    private HashSet<Vector2Int> BoundToHashRoom(List<BoundsInt> roomList)
    {
        HashSet<Vector2Int> rooms = new HashSet<Vector2Int>();

        foreach (var room in roomList)
        {
            for (int row = offset; row < room.size.x - offset; row++)
            {
                for (int col = offset; col < room.size.y - offset; col++)
                {
                    Vector2Int newPos = (Vector2Int)room.min + new Vector2Int(row, col);
                    rooms.Add(newPos);
                }
            }
        }

        return rooms;
    }
}
