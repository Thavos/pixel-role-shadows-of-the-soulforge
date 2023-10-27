using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimpleRandomWalk : AbstractGenerator
{
    //TODO experiment with ways to create double tripple width corridors

    [SerializeField]
    private int corridorIter, corridorLen,
                roomIter, roomLen;

    [SerializeField]
    bool corridorWalkBack = false,
         randomiseRoomStartPos = false;

    [SerializeField]
    [Range(0.1f, 1f)]
    float roomCoveragePercent = 0.5f;

    public override void Generate()
    {
        // 1. Inicialize map
        HashSet<Vector2Int> map = new HashSet<Vector2Int>();
        List<Vector2Int> potentialRoomCenters = new List<Vector2Int>(),
                         deadEnds = new List<Vector2Int>();

        // 2. Create corridors
        GenerateCorridors(map, potentialRoomCenters);

        // 3. Generate rooms
        // 3.1 Find dead ends and create rooms on thoose positions
        FindDeadEnds(map, deadEnds);
        for (int i = 0; i < deadEnds.Count; i++)
        {
            GenerateRoom(map, deadEnds[i]);
        }

        // 3.2 Take % amount of potentialRoomCenter points and create rooms at their pos
        int roomCount = Mathf.RoundToInt(potentialRoomCenters.Count * roomCoveragePercent);
        for (int i = 0; i < roomCount; i++)
        {
            Vector2Int startPos = potentialRoomCenters[rand.Next(0, potentialRoomCenters.Count)];
            potentialRoomCenters.Remove(startPos);
            GenerateRoom(map, startPos);
        }

        // 4. Create walls

        // 5. Draw the map
        tilemapGen.ClearAll();
        tilemapGen.GenerateTilemap(map);
    }

    private void GenerateCorridors(HashSet<Vector2Int> map, List<Vector2Int> potentialRoomCenters)
    {
        Vector2Int curPos = Vector2Int.zero;
        potentialRoomCenters.Add(curPos);

        for (int i = 0; i < corridorIter;)
        {
            HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
            Vector2Int dir = Directions2D.GetRandomCardinalDir(rand);

            // Check if we can walk back, and if not and direction 
            //   would make us to walk back then ignore this
            //   iteration and rerun it
            if (corridorWalkBack == false && map.Contains(curPos + dir))
                continue;

            for (int step = 0; step < corridorLen; step++)
            {
                curPos += dir;
                corridor.Add(curPos);
            }

            // Last position on corridor is good place for a room
            //   therefore add it to potential room centers for later user
            potentialRoomCenters.Add(curPos);
            map.UnionWith(corridor);

            i++;
        }
    }

    private void GenerateRoom(HashSet<Vector2Int> map, Vector2Int startPos)
    {
        HashSet<Vector2Int> room = new HashSet<Vector2Int>();
        Vector2Int curPos = startPos;

        for (int i = 0; i < roomIter; i++)
        {
            for (int step = 0; step < roomLen; step++)
            {
                curPos += Directions2D.GetRandomCardinalDir(rand);
                if (room.Contains(curPos) == false)
                    room.Add(curPos);
            }

            // Check for randomising room start pos
            //   false = we will generate more circular rooms
            //   true  = rooms will be spread around more but this often leads to thinner rooms
            if (randomiseRoomStartPos)
                curPos = room.ElementAt(rand.Next(0, room.Count));
            else
                curPos = startPos;
        }

        map.UnionWith(room);
    }

    public static void FindDeadEnds(HashSet<Vector2Int> map, List<Vector2Int> deadEnds)
    {
        foreach (var pos in map)
        {
            int neighboursCount = 0;

            foreach (var dir in Directions2D.CardinalDir)
            {
                Vector2Int checkPos = pos + dir;
                if (map.Contains(checkPos))
                    neighboursCount++;

                // Dead-ends can have only one neighbouring tile
                if (neighboursCount > 1)
                    break;
            }

            if (neighboursCount == 1)
                deadEnds.Add(pos);
        }
    }
}
