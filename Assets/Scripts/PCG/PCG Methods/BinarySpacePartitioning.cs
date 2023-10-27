using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinarySpacePartitioning : AbstractGenerator
{
    [SerializeField]
    private BoundsInt mapSize;

    [SerializeField]
    private Vector2Int minRoomSize;

    [SerializeField]
    private int roomCount, offset, corridorWidth;


    public override void Generate()
    {
        // 1. Inicialize map
        HashSet<Vector2Int> map = new HashSet<Vector2Int>();
        List<BoundsInt> roomList = new List<BoundsInt>();
        Queue<BoundsInt> roomQueue = new Queue<BoundsInt>();

        // 2. Generate splits for future rooms
        // Function resets and tries to cut space into equired number of rooms
        GenerateRooms(roomList, roomQueue);

        // 3. Generate map - including filling out rooms and corridors
        GenerateMap(map, roomList);

        // 4. Create walls

        // 5. Draw the map
        tilemapGen.ClearAll();
        tilemapGen.GenerateTilemap(map);
    }

    private void GenerateRooms(List<BoundsInt> roomList, Queue<BoundsInt> roomQueue)
    {
        // Function resets and tries to cut space into equired number of rooms
        while (roomList.Count != roomCount)
        {
            roomQueue.Clear();
            roomList.Clear();

            roomQueue.Enqueue(mapSize);
            while (roomQueue.Count > 0)
            {
                BoundsInt room = roomQueue.Dequeue();

                int roomX = room.size.x,
                    roomY = room.size.y,
                    twiceMinRoomX = minRoomSize.x * 2,
                    twiceMinRoomY = minRoomSize.y * 2;

                if (roomX >= minRoomSize.x && roomY >= minRoomSize.y)
                {
                    // After dequing we take room size and split it into new rooms
                    //   to make thigns more even we have 50% chance between spliting horizontaly/verticaly first
                    //   if we cant split anymore but room size si big enough we simply add it and continue 
                    if (Random.Range(0f, 1f) < 0.5f) // Start with vertical split
                    {
                        if (roomX >= twiceMinRoomX)
                            VerticalSplit(roomQueue, room);
                        else if (roomY >= twiceMinRoomY)
                            HorizontalSplit(roomQueue, room);
                        else if (roomX >= minRoomSize.x && roomY >= minRoomSize.y)
                            roomList.Add(room);
                    }
                    else // Start with horizontal split
                    {
                        if (roomY >= twiceMinRoomY)
                            HorizontalSplit(roomQueue, room);
                        else if (roomX >= twiceMinRoomX)
                            VerticalSplit(roomQueue, room);
                        else if (roomX >= minRoomSize.x && roomY >= minRoomSize.y)
                            roomList.Add(room);
                    }
                }
            }
        }
    }

    private void HorizontalSplit(Queue<BoundsInt> roomQueue, BoundsInt currentRoom)
    {
        int ySplit = rand.Next(1, currentRoom.size.y);

        BoundsInt newRoom1 = new BoundsInt(currentRoom.min, new Vector3Int(currentRoom.size.x, ySplit, currentRoom.size.z));
        BoundsInt newRoom2 = new BoundsInt(new Vector3Int(currentRoom.min.x, currentRoom.min.y + ySplit, currentRoom.min.z),
                                           new Vector3Int(currentRoom.size.x, currentRoom.size.y - ySplit, currentRoom.size.z));

        roomQueue.Enqueue(newRoom1);
        roomQueue.Enqueue(newRoom2);
    }

    private void VerticalSplit(Queue<BoundsInt> roomQueue, BoundsInt currentRoom)
    {
        int xSplit = rand.Next(1, currentRoom.size.x);

        BoundsInt newRoom1 = new BoundsInt(currentRoom.min, new Vector3Int(xSplit, currentRoom.size.y, currentRoom.size.z));
        BoundsInt newRoom2 = new BoundsInt(new Vector3Int(currentRoom.min.x + xSplit, currentRoom.min.y, currentRoom.min.z),
                                           new Vector3Int(currentRoom.size.x - xSplit, currentRoom.size.y, currentRoom.size.z));

        roomQueue.Enqueue(newRoom1);
        roomQueue.Enqueue(newRoom2);
    }

    private void GenerateMap(HashSet<Vector2Int> map, List<BoundsInt> roomList)
    {
        map.UnionWith(BoundToHashRoom(roomList));
        List<Vector2Int> roomCenters = new List<Vector2Int>();

        foreach (var room in roomList)
        {
            roomCenters.Add(new Vector2Int((int)room.center.x, (int)room.center.y));
        }

        SimpleRoomPlacement.GenerateCorridors(roomCenters, map, corridorWidth);
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
