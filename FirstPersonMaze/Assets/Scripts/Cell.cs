using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    North,
    South,
    East,
    West
}

public class Cell : MonoBehaviour
{
    public float WallHeight;
    public GameObject[] Walls = new GameObject[] { null, null, null, null };
    public bool[] WallsRemoved = new bool[] { false, false, false, false };

    private int cellColumn;
    private int cellRow;
    private float cellSizeX;
    private float cellSizeZ;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void RemoveWall(Direction dir)
    {
        GameObject wallToRemove = Walls[(int)dir];

        if(wallToRemove != null)
        {
            wallToRemove.SetActive(false);
        }
    }

    public void Init(float SizeX, float SizeZ, int Column, int Row)
    {
        cellSizeX = SizeX;
        cellSizeZ = SizeZ;
        cellColumn = Column;
        cellRow = Row;

        Walls[(int)Direction.North].transform.localScale = new Vector3(cellSizeX, WallHeight, 1.0f);
        Walls[(int)Direction.East].transform.localScale = new Vector3(cellSizeZ, WallHeight, 1.0f);
        Walls[(int)Direction.South].transform.localScale = new Vector3(cellSizeX, WallHeight, 1.0f);
        Walls[(int)Direction.West].transform.localScale = new Vector3(cellSizeZ, WallHeight, 1.0f);

        float wallDistX = cellSizeX * 0.5f + 0.5f;
        float wallDistZ = cellSizeZ * 0.5f + 0.5f;

        Walls[(int)Direction.North].transform.localPosition = new Vector3(0.0f, 0.0f, wallDistZ);
        Walls[(int)Direction.East].transform.localPosition = new Vector3(wallDistX, 0.0f, 0.0f);
        Walls[(int)Direction.South].transform.localPosition = new Vector3(0.0f, 0.0f, -wallDistZ);
        Walls[(int)Direction.West].transform.localPosition = new Vector3(-wallDistX, 0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
