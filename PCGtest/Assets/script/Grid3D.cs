using UnityEngine;

public class Grid3D
{
    private int[,,] grid;
    public int Width { get; private set; }
    public int Height { get; private set; }
    public int Depth { get; private set; }

    public Grid3D(int width, int height, int depth)
    {
        Width = width;
        Height = height;
        Depth = depth;
        grid = new int[width, height, depth];
    }

    // Indexer to access the grid using Vector3
    public int this[Vector3 index]
    {
        get
        {
            int x = Mathf.FloorToInt(index.x);
            int y = Mathf.FloorToInt(index.y);
            int z = Mathf.FloorToInt(index.z);
            return grid[x, y, z];
        }
        set
        {
            int x = Mathf.FloorToInt(index.x);
            int y = Mathf.FloorToInt(index.y);
            int z = Mathf.FloorToInt(index.z);
            grid[x, y, z] = value;
        }
    }

    // Indexer to access the grid using individual x, y, z coordinates
    public int this[int x, int y, int z]
    {
        get
        {
            return grid[x, y, z];
        }
        set
        {
            grid[x, y, z] = value;
        }
    }

    // Method to initialize the grid with some values
    public void InitializeGrid()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int z = 0; z < Depth; z++)
                {
                    grid[x, y, z] = 0;
                }
            }
        }
    }

    // Method to print the grid values
    public void PrintGrid()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int z = 0; z < Depth; z++)
                {
                    Debug.Log($"grid[{x}, {y}, {z}] = {grid[x, y, z]}");
                }
            }
        }
    }
}
