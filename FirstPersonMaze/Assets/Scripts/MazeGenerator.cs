using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MazeGenerator : MonoBehaviour
{
    // Singleton pattern
    private static MazeGenerator _instance = null;
    public static MazeGenerator Instance
    {
        get
        {
            return _instance;
        }
    }

    public float MazeSizeX;
    public float MazeSizeZ;

    public int MinGenDistFromPlayer;

    public int numBrawlerGenerators;
    public int numShooterGenerators;
    public int numGhostGenerators;

    public int NumCellsX;
    public int NumCellsZ;
    public GameObject CellPrefab;
    public GameObject shooterPrefab;
    public GameObject ghostPrefab;
    public GameObject shooterGeneratorPrefab;
    public GameObject brawlerGeneratorPrefab;
    public GameObject ghostGeneratorPrefab;

    public GameObject Player;

    private float cellSizeX;
    private float cellSizeZ;

    List<Cell> processedCells = new List<Cell>();

    Cell currentCell;
    int nextCol;
    int nextRow;
    Cell nextCell;

    private List<Cell> Cells = new List<Cell>();    

    // Start is called before the first frame update
    void Start()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

        cellSizeX = MazeSizeX / NumCellsX;
        cellSizeZ = MazeSizeZ / NumCellsZ;
        CreateCells();
        GenerateMazeRecursive( GetCellAt(0,0) );
        //SpawnShooters();
        //PlaceEnemyGenerator();
        //SpawnGhosts();
        OnMazeGenerationComplete();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnShooters()
    {
        int numColumns = NumCellsX;
        int numRows = NumCellsZ;

        int spawnColumn = Random.Range(0, numColumns);
        int spawnRow = Random.Range(0, numRows);

        Cell shooterSpawnCell = GetCellAt(1, 1);
        Vector3 cellpos = shooterSpawnCell.transform.position;
        Vector3 spawnPos = new Vector3(cellpos.x, 0.0f, cellpos.z);
        GameObject shooterObj = Instantiate(shooterPrefab);
        shooterObj.transform.position = spawnPos;

        Shooter newShooter = shooterObj.GetComponent<Shooter>();
        newShooter.SetStartingCell(shooterSpawnCell);
    }

    /*private void SpawnGhosts()
    {
        int numColumns = NumCellsX;
        int numRows = NumCellsZ;

        int spawnColumn = Random.Range(0, numColumns);
        int spawnRow = Random.Range(0, numRows);

        Cell ghostSpawnCell = GetCellAt(2, 2);
        Vector3 cellpos = ghostSpawnCell.transform.position;
        Vector3 spawnPos = new Vector3(cellpos.x, 0.0f, cellpos.z);
        GameObject ghostObj = Instantiate(ghostPrefab);
        ghostObj.transform.position = spawnPos;

        Shooter newGhost = ghostObj.GetComponent<Shooter>();
        newGhost.SetStartingCell(ghostSpawnCell);
    }*/

    private void OnMazeGenerationComplete()
    {
        ResetPlayer(Player);
        PlaceEnemyGenerator();
    }

    private void PlaceEnemyGenerator()
    {
        int MaxCols = NumCellsX;
        int MaxRows = NumCellsZ;

        int GenColumn = Random.Range(0, NumCellsX);
        int GenRow = Random.Range(0, NumCellsZ);

        for (int i = 0; i < numShooterGenerators; i++)
        {


            while (GenColumn < MinGenDistFromPlayer && GenRow < MinGenDistFromPlayer)
            {
                GenColumn = Random.Range(0, NumCellsX);
                GenColumn = Random.Range(0, NumCellsZ);
            }

            Cell GeneratorCell = GetCellAt(GenColumn, GenRow);
            GameObject generatorObj = Instantiate(shooterGeneratorPrefab);
            generatorObj.transform.position = GeneratorCell.transform.position;
            ShooterGenerator generator = generatorObj.GetComponent<ShooterGenerator>();

            generator.Init(GeneratorCell);
        }
        
        GenColumn = Random.Range(0, NumCellsX);
        GenColumn = Random.Range(0, NumCellsZ);

        for (int i = 0; i < numBrawlerGenerators; i++)
        {


            while (GenColumn < MinGenDistFromPlayer && GenRow < MinGenDistFromPlayer)
            {
                GenColumn = Random.Range(0, NumCellsX);
                GenColumn = Random.Range(0, NumCellsZ);
            }

            Cell GeneratorCell = GetCellAt(GenColumn, GenRow);
            GameObject generatorObj = Instantiate(brawlerGeneratorPrefab);
            generatorObj.transform.position = GeneratorCell.transform.position;
            BrawlerGenerator generator = generatorObj.GetComponent<BrawlerGenerator>();

            generator.Init(GeneratorCell);
        }

        GenColumn = Random.Range(0, NumCellsX);
        GenColumn = Random.Range(0, NumCellsZ);

        for (int i = 0; i < numGhostGenerators; i++)
        {


            while (GenColumn < MinGenDistFromPlayer && GenRow < MinGenDistFromPlayer)
            {
                GenColumn = Random.Range(0, NumCellsX);
                GenColumn = Random.Range(0, NumCellsZ);
            }

            Cell GeneratorCell = GetCellAt(GenColumn, GenRow);
            GameObject generatorObj = Instantiate(ghostGeneratorPrefab);
            generatorObj.transform.position = GeneratorCell.transform.position;
            GhostGenerator generator = generatorObj.GetComponent<GhostGenerator>();

            generator.Init(GeneratorCell);
        }

    }

    private void GenerateMazeRecursive(Cell currentCell)
    {
        processedCells.Add(currentCell);

        List<bool> DirectionsChecked = new List<bool>();
        for( int i = 0; i < 4; ++i )
        {
            DirectionsChecked.Add(false);
        }

        Direction dir = Direction.North;
        int dirNumber = 0;
        nextCell = null;

        while(DirectionsChecked.Contains(false))
        {
            while (nextCell == null && DirectionsChecked.Contains(false))
            {
                dirNumber = Random.Range(0, 4);
                while (DirectionsChecked[dirNumber] == true)
                {
                    dirNumber = Random.Range(0, 4);
                }
                dir = (Direction)dirNumber;
                nextCol = currentCell.cellColumn;
                nextRow = currentCell.cellRow;
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
                DirectionsChecked[dirNumber] = true;
            }

            //check if that direction is valid. A direction is invalid if:
            //1. it would lead to a cell that is nonexistant (off the map)
            //2. it would lead to a cell that has already been processed
            if (nextCell != null && !processedCells.Contains(nextCell))
            {
                currentCell.RemoveWall(dir);
                Direction nextCellDir = dir;
                switch (dir)
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
                GenerateMazeRecursive(nextCell);
            }

            nextCell = null;
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

    public Cell GetRandomAdjacentCellTo(Cell cell, bool isGhost)
    {
        Cell adjacentCell = null;
        int curColumn = cell.cellColumn;
        int curRow = cell.cellRow;
        int adjColumn = cell.cellColumn;
        int adjRow = cell.cellRow;

        List<bool> DirectionsChecked = new List<bool>();
        for(int i = 0; i < 4; i++)
        {
            DirectionsChecked.Add(false);
        }

        Direction adjDir = (Direction)(Random.Range(0, 4));
        while(adjacentCell == null)
        {
            if(!isGhost)
            {
                if (cell.canMoveInDirection(adjDir))
                {
                    switch (adjDir)
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

                else
                {
                    DirectionsChecked[(int)adjDir] = true;

                    while (DirectionsChecked[(int)adjDir] == true)
                    {
                        adjDir = (Direction)(Random.Range(0, 4));
                    }
                }
            }

            else
            {
                if (cell.canMoveInDirectionGhost(adjDir))
                {
                    switch (adjDir)
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

                else
                {
                    DirectionsChecked[(int)adjDir] = true;

                    while (DirectionsChecked[(int)adjDir] == true)
                    {
                        adjDir = (Direction)(Random.Range(0, 4));
                    }
                }
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

    public void ResetPlayer(GameObject Player)
    {
        Cell startingCell = GetCellAt(0, 0);
        Player.transform.position = startingCell.transform.position;
    }
}
