using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;

public class PathNode
{
    private Grid grid;
    public int x;
    public int y;

    public int gCost;
    public int hCost;
    public int fCost;

    public bool isWalkable;
    public PathNode parent;
    public PathNode(Grid grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        isWalkable = true;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public override string ToString()
    {
        return x + "," + y;
    }

}

public class Pathfinding
{
    private const int MOVE_STRAIGHT = 10;
    private const int MOVE_DIAG = 14;

    private Grid grid;
    private bool[,] walls;
    private List<PathNode> open;
    private List<PathNode> close;
    public Pathfinding(int width, int height)
    {
        grid = new Grid(width, height, 1.33f);
        walls = new bool[width, height];
        for (int i = 0; i < width; i++) { 
            for(int j = 0; j < height; j++)
            {
                walls[i,j] = false;
            }
        }
    }

    public List<Vector3> FindPath(Vector3 startPos, Vector3 endPos)
    {
        grid.GetXY(startPos, out int startX, out int startY);
        grid.GetXY(endPos, out int endX, out int endY);

        List<PathNode> path = FindPath(startX, startY, endX, endY);
        if (path == null) return null;

        List<Vector3> worldPath = new List<Vector3>();
        foreach(PathNode node in path)
        {
            worldPath.Add(new Vector3(node.x + 0.5f, 0 , node.y + 0.5f) * grid.getCellSize());
        }
        return worldPath;
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        PathNode startNode = new PathNode(grid, startX, startY);
        PathNode endNode = new PathNode(grid, endX, endY);

        open = new List<PathNode> { startNode };
        close = new List<PathNode>();

        for (int x = 0; x < grid.getWidth(); x++)
            for(int y = 0; y < grid.getHeight(); y++)
            {
                PathNode newNode = new PathNode(grid,x,y);
                newNode.gCost = int.MaxValue;
                newNode.CalculateFCost();
                newNode.parent = null;
                if (walls[x, y]) newNode.isWalkable = false;
                grid.SetTile(x, y, newNode);
            }

        startNode.gCost = 0;
        startNode.hCost = findDistanceCost(startNode, endNode);
        startNode.CalculateFCost();
        grid.SetTile(startX, startY, startNode);

        while(open.Count > 0)
        {
            PathNode current = getCheapestNode(open);
            if(current.x == endX && current.y == endY)
            {
                //destination reached
                return createPath(current);
            }

            open.Remove(current);
            close.Add(current);

            foreach(PathNode node in getNieghbors(current))
            {
                if (close.Contains(node)) continue;
                if (!node.isWalkable)
                {
                    close.Add(node);
                    continue;
                }

                int possibleGcost = current.gCost + findDistanceCost(current, node);
                if(possibleGcost < node.gCost)
                {
                    node.parent = current;
                    node.gCost = possibleGcost;
                    node.hCost = findDistanceCost(node, endNode);
                    node.CalculateFCost();

                    if (!open.Contains(node))
                    {
                        open.Add(node);
                    }
                }
            }
        }
        return null;
    }

    public Grid getGrid()
    {
        return grid;
    }

   
    private List<PathNode> getNieghbors(PathNode curr)
    {
        List<PathNode> list = new List<PathNode>();

        if(curr.x - 1 >= 0)
        {
            list.Add(grid.GetTile(curr.x - 1, curr.y));
            if (curr.y - 1 >= 0) list.Add(grid.GetTile(curr.x - 1, curr.y - 1));
            if (curr.y + 1 < grid.getHeight()) list.Add(grid.GetTile(curr.x - 1, curr.y + 1));
        }
        if(curr.x + 1 < grid.getWidth())
        {
            list.Add(grid.GetTile(curr.x + 1, curr.y));
            if (curr.y - 1 >= 0) list.Add(grid.GetTile(curr.x + 1, curr.y - 1));
            if (curr.y + 1 < grid.getHeight()) list.Add(grid.GetTile(curr.x + 1, curr.y + 1));
        }
        if (curr.y - 1 >= 0) list.Add(grid.GetTile(curr.x, curr.y - 1));
        if (curr.y + 1 < grid.getHeight()) list.Add(grid.GetTile(curr.x, curr.y + 1));

        return list;

    }
    private List<PathNode> createPath(PathNode end)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(end);
        PathNode curr = end;
        while(curr.parent != null)
        {
            path.Add(curr.parent);
            curr = curr.parent;
        }
        path.Reverse();
        return path;
    }
    private int findDistanceCost(PathNode a, PathNode b)
    {
        int xDist = Mathf.Abs(a.x - b.x);
        int yDist = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDist - yDist);
        return MOVE_DIAG * Mathf.Min(xDist, yDist) + MOVE_STRAIGHT * remaining;
    }

    private PathNode getCheapestNode(List<PathNode> nodeList)
    {
        PathNode cheapest = nodeList[0];
        for(int i = 1; i < nodeList.Count; i++)
        {
            if (nodeList[i].fCost < cheapest.fCost)
            {
                cheapest = nodeList[i];
            }
        }
        return cheapest;
    }

    public void placeWallHoriz(int row, int start, int end)
    {
        for(int i = start; i <= end; i++) 
            walls[row,i] = true;

    }

    public void placeWallVert(int col, int start, int end)
    {
        for (int i = start; i <= end; i++) 
            walls[i,col] = true;
    }

    public void generateWallDebug(Vector3 start, Vector3 end)
    {
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Quad);

        Vector3 midpoint = (start + end)/ 2;

        plane.transform.position = midpoint;
        Vector3 direction = end - start;
        float dist = direction.magnitude;

        plane.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        plane.transform.localScale = new Vector3(dist, 1f, 1f);
    }

    public void drawPathDebug(List<Vector3> path)
    {
        for (int i = 0; i < path.Count - 1; i++)
        {
            Debug.DrawLine(new Vector3(path[i].x, 0, path[i].z), new Vector3(path[i + 1].x, 0, path[i + 1].z), Color.green, 0f);
        }
    }
}
