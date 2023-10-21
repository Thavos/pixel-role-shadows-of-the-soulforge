using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LS : AbstractGenerator
{
    // TODO - this needs a check system to determin if map is big enough but not too big (has enough rooms, room overlaps, etc) 
    [SerializeField]
    private string startValue;

    [SerializeField]
    private int itterations, corLength;

    [SerializeField]
    Vector2Int minRoomSize, maxRoomSize;

    public override void Generate()
    {
        string mapLayout = GenerateRules(startValue);
        HashSet<Vector2Int> map = GenerateDungeon(mapLayout);

        List<Vector2Int> deadEnds = new List<Vector2Int>();
        RSW.FindDeadEnds(map, deadEnds);
        foreach (var endPos in deadEnds)
        {
            GenerateRoom(map, endPos);
        }

        HashSet<Vector2Int> walls = WallsGenerator.GenerateWalls(map);

        tilemapGen.ClearAll();
        tilemapGen.GenerateTilemap(map);
        tilemapGen.GenerateWalls(walls);
    }

    private HashSet<Vector2Int> GenerateDungeon(string mapLayout)
    {
        HashSet<Vector2Int> map = new HashSet<Vector2Int>();
        Vector2Int pos = Vector2Int.zero;

        for (int i = 0; i < mapLayout.Length - 1; i++)
        {
            Vector2Int dir = Vector2Int.up;

            switch (mapLayout[i])
            {
                case 'C':
                    if (mapLayout[i - 1] == '+')
                        dir = Vector2Int.right;
                    else if (mapLayout[i - 1] == '-')
                        dir = Vector2Int.left;
                    else if (mapLayout[i - 1] == '.')
                        dir = Vector2Int.down;
                    else
                        dir = Vector2Int.up;

                    HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
                    Vector2Int newPos = pos;
                    for (int step = 0; step < corLength; step++)
                    {
                        newPos += dir;
                        corridor.Add(newPos);
                    }
                    pos = newPos;
                    map.UnionWith(corridor);
                    break;

                case 'S':
                case 'B':
                case 'R':
                    GenerateRoom(map, pos);
                    break;

                default:
                    break;
            }
        }

        return map;
    }

    private void GenerateRoom(HashSet<Vector2Int> map, Vector2Int pos)
    {
        Vector2Int roomSize = new Vector2Int(rand.Next(minRoomSize.x, maxRoomSize.x), rand.Next(minRoomSize.y, maxRoomSize.y));
        HashSet<Vector2Int> room = new HashSet<Vector2Int>();
        Vector2Int roomStart = pos - roomSize / 2;
        for (int row = 0; row < roomSize.x; row++)
        {
            for (int col = 0; col < roomSize.y; col++)
            {
                room.Add(roomStart + new Vector2Int(row, col));
            }
        }
        map.UnionWith(room);
    }

    private string GenerateRules(string mapLayout)
    {
        string layout = mapLayout;

        for (int i = 0; i < itterations; i++)
        {
            int id = rand.Next(0, layout.Length - 1);
            int ruleChance = rand.Next(1, 100);

            switch (layout[id])
            {
                case 'S':
                    if (ruleChance <= 25)
                        layout = layout.Insert(id + 1, "C");
                    else if (ruleChance <= 50)
                        layout = layout.Insert(id + 1, "+C");
                    else if (ruleChance <= 75)
                        layout = layout.Insert(id + 1, "-C");
                    else
                        layout = layout.Insert(id + 1, ".C");
                    break;

                case 'C':
                    if (ruleChance <= 15)
                        layout = layout.Insert(id + 1, "C");
                    else if (ruleChance <= 30)
                        layout = layout.Insert(id + 1, "+C");
                    else if (ruleChance <= 45)
                        layout = layout.Insert(id + 1, "-C");
                    else if (layout[id - 1] != 'R' && layout[id + 1] != 'R')
                    {
                        i++;
                        layout = layout.Insert(id + 1, "R");
                    }
                    break;

                default:
                    break;
            }

            // // string newLayout = "";
            // // for (int j = 0; j < layout.Length; j++)
            // // {
            // //     if (layout[j] == 'F')
            // //         newLayout += "+C+CRG";
            // //     else if (layout[j] == 'G')
            // //         newLayout += ".CF.CH";
            // //     else if (layout[j] == 'H')
            // //         newLayout += "-C-C-CICCH+C.C";
            // //     else if (layout[j] == 'I')
            // //         newLayout += "-CCR+C+C+CG";
            // //     else
            // //         newLayout += layout[j].ToString();
            // // }

            // layout = newLayout;
        }

        return layout;
    }
}
