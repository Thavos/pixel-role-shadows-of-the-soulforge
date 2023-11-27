using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PathfindingAStar
{
    public static List<Vector2Int> AStar(Vector2Int start, Vector2Int goal, HashSet<Vector2Int> map)
    {
        HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>();
        PriorityQueue<Vector2Int> openSet = new PriorityQueue<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        Dictionary<Vector2Int, float> heauristicScore = new Dictionary<Vector2Int, float>();
        Dictionary<Vector2Int, float> distanceScore = new Dictionary<Vector2Int, float>();

        openSet.Enqueue(start, 0);
        heauristicScore[start] = CalculateHeuristic(start, goal);
        distanceScore[start] = 0;

        while (openSet.Count > 0)
        {
            Vector2Int current = openSet.Dequeue();

            if (current == goal)
                return ReconstructPath(cameFrom, current);

            closedSet.Add(current);

            foreach (Vector2Int neighbor in GetNeighbors(current))
            {
                if (!map.Contains(neighbor) || closedSet.Contains(neighbor))
                    continue;

                float tentativeGScore = distanceScore[current] + Vector2Int.Distance(current, neighbor);

                if (!distanceScore.ContainsKey(neighbor) || tentativeGScore < distanceScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    distanceScore[neighbor] = tentativeGScore;
                    heauristicScore[neighbor] = tentativeGScore + CalculateHeuristic(neighbor, goal);

                    if (!openSet.Contains(neighbor))
                        openSet.Enqueue(neighbor, heauristicScore[neighbor]);
                }
            }
        }

        return new List<Vector2Int>();
    }

    private static float CalculateHeuristic(Vector2Int start, Vector2Int finish)
    {
        return Mathf.Abs(start.x - finish.x) + Mathf.Abs(start.y - finish.y);
    }

    private static float CalculateDistance(Vector2Int start, Vector2Int finish)
    {
        return Vector2Int.Distance(start, finish);
    }

    private static List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        while (cameFrom.ContainsKey(current))
        {
            path.Add(current);
            current = cameFrom[current];
        }
        path.Reverse();
        return path;
    }

    public static List<Vector2Int> GetNeighbors(Vector2Int current)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();
        neighbors.Add(current + Vector2Int.right);
        neighbors.Add(current + Vector2Int.left);
        neighbors.Add(current + Vector2Int.up);
        neighbors.Add(current + Vector2Int.down);

        return neighbors;
    }
}
