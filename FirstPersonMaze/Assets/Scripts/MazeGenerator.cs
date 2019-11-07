using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MazeGenerator : MonoBehaviour
{
    public float MazeSizeX;
    public float MazeSizeZ;

    public int NumCellsX;
    public int NumCellsZ;
    public GameObject CellPrefab;

    private float cellSizeX;
    private float cellSizeZ;

    private bool movedOn = true;

    private int currentCol = 0;
    private int currentRow = 0;
    private int cellsDone = 0;
    List<Cell> processedCells = new List<Cell>();
    List<Cell> cellStack = new List<Cell>();

    Cell currentCell;
    int nextCol;
    int nextRow;
    Cell nextCell;

    private List<Cell> Cells = new List<Cell>();

    public static MazeGenerator Instance = null;
    // Start is called before the first frame update
    void Start()
    {
        cellSizeX = MazeSizeX / NumCellsX;
        cellSizeZ = MazeSizeZ / NumCellsZ;
        CreateCells();
        GenerateMazeRecursive();

        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }



    private void GenerateMazeRecursive()
    {
        Direction dir = Direction.North;
        currentCell = GetCellAt(currentCol, currentRow);
        nextCell = null;

        while(nextCell == null)
        {
            dir = (Direction)Random.Range(0, 4);
            nextCol = currentCol;
            nextRow = currentRow;
            nextCell = null;

            switch (dir)
            {
                case Direction.North:
                    nextRow++;
                    break;
                case Direction.South:
                    nextRow--;
                    break;
                case Direction.East:
                    nextCol++;
                    break;
                case Direction.West:
                    nextCol--;
                    break;
            }
            nextCell = GetCellAt(nextCol, nextRow);
        }
        //check if that direction is valid. A direction is invalid if:
        //1. it would lead to a cell that is nonexistant (off the map)
        //2. it would lead to a cell that has already been processed
        if(nextCell != null && !processedCells.Contains(nextCell))
        {
            currentCell.RemoveWall(dir);
            Direction nextCellDir = dir;
            switch(dir)
            {
                case Direction.North:
                    nextCellDir = Direction.South;
                    break;
                case Direction.South:
                    nextCellDir = Direction.North;
                    break;
                case Direction.East:
                    nextCellDir = Direction.West;
                    break;
                case Direction.West:
                    nextCellDir = Direction.East;
                    break;
            }

            nextCell.RemoveWall(nextCellDir);

            processedCells.Add(currentCell);
            cellStack.Add(currentCell);
            cellsDone++;
            currentCell = nextCell;

            currentCol = nextCol;
            currentRow = nextRow;
            //movedOn = true;
        }
        else
        {
/*
            processedCells.RemoveAt(cellsDone);
            cellsDone--;
            if(cellsDone < 0)
            {
                cellsDone = 0;
            }*/
            //movedOn = false;   
        }
    }

    public Cell GetCellAt(int column, int row)
    {
        if (column < 0 || column >= NumCellsX || row < 0 || row >= NumCellsZ)
        {
            return null;
        }

        int CellIndex = row * NumCellsX + column;
        if(CellIndex < 0 || CellIndex >= Cells.Count)
        {
            return null;
        }
        return Cells[CellIndex];
    }

    public Cell GetRandomAdjacentCellTo(Cell cell)
    {
        Cell adjacentCell = null;
        int curColumn = cell.cellColumn;
        int curRow = cell.cellRow;
        int adjColumn = cell.cellColumn;
        int adjRow = cell.cellRow;

        Direction adjDir = (Direction)(Random.Range(0, 4));
        while(adjacentCell == null)
        {
            
            if(cell.canMoveInDirection(adjDir))
            {
                switch(adjDir)
                {
                    case Direction.North:
                        adjRow++;
                        break;
                    case Direction.East:
                        adjColumn++;
                        break;
                    case Direction.West:
                        adjColumn--;
                        break;
                    case Direction.South:
                        adjRow--;
                        break;
                }

                adjacentCell = GetCellAt(adjColumn, adjRow);
            }
        }
        return adjacentCell;
    }

    public void CreateCells()
    {
        Cells.Clear();

        float BaseCellPosX = -(MazeSizeX * 0.5f) + (cellSizeX * 0.5f);
        float BaseCellPosZ = -(MazeSizeZ * 0.5f) + (cellSizeZ * 0.5f);

        int NumTotalCells = NumCellsX * NumCellsZ;
        int CellColumn = 0;
        int CellRow = 0;
        for(int i =0; i < NumTotalCells; i++)
        {
            float cellX = BaseCellPosX + (CellColumn * cellSizeX);
            float cellZ = BaseCellPosZ + (CellRow * cellSizeZ);

            GameObject newCellObj = Instantiate(CellPrefab, this.transform);
            newCellObj.transform.position = new Vector3(cellX, 0.0f, cellZ);

            Cell newCell = newCellObj.GetComponent<Cell>();
            newCell.Init(cellSizeX, cellSizeZ, CellColumn, CellRow);
            Cells.Add(newCell);
            
            if(CellColumn >  NumCellsX - 2)
            {
                CellColumn = 0;
                CellRow++;
            }
            else
            {
                CellColumn++;
            }
        }

    }
}
