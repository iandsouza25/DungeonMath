using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class Grid{

    private int width;
    private int height;
    private float cellSize;
    private PathNode[,] gridArray;
    private TextMesh[,] debugTextArray;
    private Vector3 origin;
    
    public TextMesh placeTileText(string text, Vector3 pos)
    {
        GameObject go = new GameObject("Tile_Text", typeof(TextMesh));

        Transform transform = go.transform;
        transform.position = pos;
        transform.rotation = Quaternion.Euler(90, 0, 0);

        TextMesh textMesh = go.GetComponent<TextMesh>();
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.alignment = TextAlignment.Center;
        textMesh.color = Color.white;
        textMesh.text = text;
        textMesh.fontSize = 10;

        return textMesh;
    }

    public Grid(int width, int height, float cellSize, Vector3 origin)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.origin = origin;

        gridArray = new PathNode[width, height];
        debugTextArray = new TextMesh[width, height];
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, 0, y) * this.cellSize + origin;
    }


    public void GetXY(Vector3 worldPos, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPos - origin).x / cellSize);
        y = Mathf.FloorToInt((worldPos - origin).z / cellSize);
    }

    public void SetTile(int x, int y, PathNode value)
    {
        if(x >= 0 &&y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            //debugTextArray[x, y].text = gridArray[x,y].fCost;
        }
    }

    public void SetTile(Vector3 worldPos, PathNode value)
    {
        int x, y;
        GetXY(worldPos, out x, out y);
        SetTile(x, y, value);
    }

    public PathNode GetTile(int x , int y)
    {
        if(x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x,y];
        }
        return default(PathNode);
    }

    public PathNode GetTile(Vector3 worldPos)
    {
        int x, y;
        GetXY(worldPos, out x, out y);
        return GetTile(x, y);
    }

    public int getWidth()
    {
        return width;
    }

    public int getHeight()
    {
        return height;
    }

    public float getCellSize()
    {
        return cellSize;
    }
}
