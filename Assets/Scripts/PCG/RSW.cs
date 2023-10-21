using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RSW : AbstractGenerator
{
    [SerializeField]
    int cItterations, cWalkLength = 0;

    [SerializeField]
    bool corridorWalkBack = false;

    [SerializeField]
    RandomStepWalkSO roomParams;

    [SerializeField]
    [Range(0.1f, 1f)]
    float roomCoverage = 0.5f;

    public override void Generate()
    {
        HashSet<Vector2Int> map = new HashSet<Vector2Int>();

        HashSet<Vector2Int> rooms = new HashSet<Vector2Int>();
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();

        List<Vector2Int> potentialRoomPos = new List<Vector2Int>();
        List<Vector2Int> deadEnds = new List<Vector2Int>();

        GenerateCorridors(corridors, potentialRoomPos);
        FindDeadEnds(corridors, deadEnds);

        int roomsCount = (int)Mathf.Floor(potentialRoomPos.Count * roomCoverage);

        for (int i = 0; i < roomsCount; i++)
        {
            GenerateRoom(rooms, potentialRoomPos[i], roomParams);
        }

        for (int i = 0; i < potentialRoomPos.Count; i++)
        {
            int randListId = rand.Next(0, potentialRoomPos.Count);
            Vector2Int pos = potentialRoomPos[randListId];
            potentialRoomPos.Remove(pos);

            GenerateRoom(rooms, pos, roomParams);
        }

        foreach (var pos in deadEnds)
        {
            GenerateRoom(rooms, pos, roomParams);
        }

        map.UnionWith(rooms);
        map.UnionWith(corridors);
        HashSet<Vector2Int> walls = WallsGenerator.GenerateWalls(map);

        tilemapGen.ClearAll();
        tilemapGen.GenerateTilemap(map);
        tilemapGen.GenerateWalls(walls);
    }

    public static void GenerateRoom(HashSet<Vector2Int> rooms, Vector2Int startPos, RandomStepWalkSO roomParams)
    {
        HashSet<Vector2Int> room = new HashSet<Vector2Int>();
        Vector2Int curPos = startPos;

        for (int i = 0; i < roomParams.rIterrations; i++)
        {
            for (int step = 0; step < roomParams.rWalkLength; step++)
            {
                curPos += Directions2D.GetRandomCardinalDir(rand);
                room.Add(curPos);
            }

            if (roomParams.randomiseItteration)
                curPos = room.ElementAt(rand.Next(0, room.Count));
            else
                curPos = startPos;
        }

        rooms.UnionWith(room);
    }

    private void GenerateCorridors(HashSet<Vector2Int> corridors, List<Vector2Int> potentialRoomPos)
    {
        Vector2Int curPos = Vector2Int.zero;
        potentialRoomPos.Add(curPos);

        for (int i = 0; i < cItterations; i++)
        {
            HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
            Vector2Int dir = Directions2D.GetRandomCardinalDir(rand);

            if (corridorWalkBack == false && corridors.Contains(curPos + dir))
            {
                i--;
                continue;
            }

            for (int step = 0; step < cWalkLength; step++)
            {
                curPos += dir;
                corridor.Add(curPos);
            }

            potentialRoomPos.Add(curPos);
            corridors.UnionWith(corridor);
        }
    }

    public static void FindDeadEnds(HashSet<Vector2Int> corridors, List<Vector2Int> deadEnds)
    {
        foreach (var pos in corridors)
        {
            int tilesAround = 0;

            for (int i = 0; i < Directions2D.CardinalDir.Count(); i++)
            {
                Vector2Int checkPos = pos + Directions2D.CardinalDir[i];
                if (corridors.Contains(checkPos))
                    tilesAround++;

                if (tilesAround > 1)
                    break;
            }

            if (tilesAround == 1)
            {
                deadEnds.Add(pos);
            }
        }
    }
}