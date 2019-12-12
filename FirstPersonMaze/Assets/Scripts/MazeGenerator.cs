using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
    public int MinEnemyDistFromPlayer;
    public int MinTresureDistFromPlayer;

    public int numBrawlerGenerators;
    public int numShooterGenerators;
    public int numGhostGenerators;

    public int numBrawlersSpawned;
    public int numGhostSpawned;
    public int numShootersSpawned;

    public int NumCellsX;
    public int NumCellsZ;
    public GameObject CellPrefab;
    public GameObject shooterPrefab;
    public GameObject ghostPrefab;
    public GameObject brawlerPrefab;
    public GameObject shooterGeneratorPrefab;
    public GameObject brawlerGeneratorPrefab;
    public GameObject ghostGeneratorPrefab;
    public GameObject treasurePrefab;
    public GameObject flagPrefab;
    public GameObject floor;
    public Camera minimapCamera;

    public Text GameOverText;
    public Text RestartText;

    public GameObject Player;

    private float cellSizeX;
    private float cellSizeZ;

    List<Cell> processedCells = new List<Cell>();

    Cell currentCell;
    int nextCol;
    int nextRow;
    Cell nextCell;

    private bool isGameOver = false;
    private bool didWin = false;

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
        //PlaceEnemyGenerator();
        SpawnEnemiesInMaze();
        //OnMazeGenerationComplete();
        minimapCamera.orthographicSize = (MazeSizeX  + MazeSizeZ) / 3.5f;

        Vector3 floorScale = new Vector3(MazeSizeX, 1, MazeSizeZ);
        floor.transform.localScale = floorScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(isGameOver)
        {
            GameOver();
        }

        if(didWin)
        {
            Win();
        }
    }

    private void SpawnEnemiesInMaze()
    {
        for(int i = 0; i < numBrawlersSpawned; i++)
        {
            SpawnBrawler();
        }
        for(int i = 0; i < numGhostSpawned; i++)
        {
            SpawnGhosts();
        }
        for(int i = 0; i < numShootersSpawned; i++)
        {
            SpawnShooters();
        }
    }

    private void SpawnShooters()
    {
        int numColumns = NumCellsX;
        int numRows = NumCellsZ;

        int spawnColumn = Random.Range(MinEnemyDistFromPlayer, numColumns);
        int spawnRow = Random.Range(MinEnemyDistFromPlayer, numRows);

        Cell shooterSpawnCell = GetCellAt(spawnColumn, spawnRow);
        Vector3 cellpos = shooterSpawnCell.transform.position;
        Vector3 spawnPos = new Vector3(cellpos.x, 0.0f, cellpos.z);
        GameObject shooterObj = Instantiate(shooterPrefab);
        shooterObj.transform.position = shooterSpawnCell.transform.position;

        Shooter newShooter = shooterObj.GetComponent<Shooter>();
        newShooter.SetStartingCell(shooterSpawnCell);
    }

    private void SpawnGhosts()
    {
        int numColumns = NumCellsX;
        int numRows = NumCellsZ;

        int spawnColumn = Random.Range(MinEnemyDistFromPlayer, numColumns);
        int spawnRow = Random.Range(MinEnemyDistFromPlayer, numRows);

        Cell ghostSpawnCell = GetCellAt(spawnColumn, spawnRow);
        Vector3 cellpos = ghostSpawnCell.transform.position;
        Vector3 spawnPos = new Vector3(cellpos.x, 0.0f, cellpos.z);
        GameObject ghostObj = Instantiate(ghostPrefab);
        ghostObj.transform.position = spawnPos;

        Ghost newGhost = ghostObj.GetComponent<Ghost>();
        newGhost.SetStartingCell(ghostSpawnCell);
    }

    private void SpawnBrawler()
    {
        int numColumns = NumCellsX;
        int numRows = NumCellsZ;

        int spawnColumn = Random.Range(MinEnemyDistFromPlayer, numColumns);
        int spawnRow = Random.Range(MinEnemyDistFromPlayer, numRows);

        Cell brawlerSpawnCell = GetCellAt(spawnColumn, spawnRow);
        Vector3 cellpos = brawlerSpawnCell.transform.position;
        Vector3 spawnPos = new Vector3(cellpos.x, 0.0f, cellpos.z);
        GameObject BrawlerObj = Instantiate(brawlerPrefab);
        BrawlerObj.transform.position = spawnPos;

        Brawler newBrawler = BrawlerObj.GetComponent<Brawler>();
        newBrawler.SetStartingCell(brawlerSpawnCell);
    }

    private void OnMazeGenerationComplete()
    {
        SetSpawn(Player);
        PlaceEnemyGenerator();
        PlaceTreasure();
    }

    private void PlaceEnemyGenerator()
    {
        int MaxCols = NumCellsX;
        int MaxRows = NumCellsZ;

        int GenColumn = Random.Range(0, NumCellsX);
        int GenRow = Random.Range(0, NumCellsZ);

        List<Cell> generatorCells = new List<Cell>();
        Cell checkCell = GetCellAt(GenColumn, GenRow);

        for (int i = 0; i < numShooterGenerators; i++)
        {


            while ((GenColumn < MinGenDistFromPlayer && GenRow < MinGenDistFromPlayer) || generatorCells.Contains(checkCell))
            {
                GenColumn = Random.Range(0, NumCellsX);
                GenColumn = Random.Range(0, NumCellsZ);

                checkCell = GetCellAt(GenColumn, GenRow);
            }

            Cell GeneratorCell = GetCellAt(GenColumn, GenRow);
            generatorCells.Add(GeneratorCell);
            GameObject generatorObj = Instantiate(shooterGeneratorPrefab);
            generatorObj.transform.position = GeneratorCell.transform.position;
            ShooterGenerator generator = generatorObj.GetComponent<ShooterGenerator>();

            generator.Init(GeneratorCell);

            GenColumn = Random.Range(0, NumCellsX);
            GenColumn = Random.Range(0, NumCellsZ);
            checkCell = GetCellAt(GenColumn, GenRow);
        }

        for (int i = 0; i < numBrawlerGenerators; i++)
        {


            while ((GenColumn < MinGenDistFromPlayer && GenRow < MinGenDistFromPlayer) || generatorCells.Contains(checkCell))
            {
                GenColumn = Random.Range(0, NumCellsX);
                GenColumn = Random.Range(0, NumCellsZ);

                checkCell = GetCellAt(GenColumn, GenRow);
            }

            Cell GeneratorCell = GetCellAt(GenColumn, GenRow);
            generatorCells.Add(GeneratorCell);
            GameObject generatorObj = Instantiate(brawlerGeneratorPrefab);
            generatorObj.transform.position = GeneratorCell.transform.position;
            BrawlerGenerator generator = generatorObj.GetComponent<BrawlerGenerator>();

            generator.Init(GeneratorCell);

            GenColumn = Random.Range(0, NumCellsX);
            GenColumn = Random.Range(0, NumCellsZ);
            checkCell = GetCellAt(GenColumn, GenRow);
        }



        for (int i = 0; i < numGhostGenerators; i++)
        {


            while ((GenColumn < MinGenDistFromPlayer && GenRow < MinGenDistFromPlayer) || generatorCells.Contains(checkCell))
            {
                GenColumn = Random.Range(0, NumCellsX);
                GenColumn = Random.Range(0, NumCellsZ);

                checkCell = GetCellAt(GenColumn, GenRow);
            }

            Cell GeneratorCell = GetCellAt(GenColumn, GenRow);
            generatorCells.Add(GeneratorCell);
            GameObject generatorObj = Instantiate(ghostGeneratorPrefab);
            generatorObj.transform.position = GeneratorCell.transform.position;
            GhostGenerator generator = generatorObj.GetComponent<GhostGenerator>();

            generator.Init(GeneratorCell);
        }

    }

    private void PlaceTreasure()
    {
        int GenColumn = Random.Range(MinTresureDistFromPlayer, NumCellsX);
        int GenRow = Random.Range(MinTresureDistFromPlayer, NumCellsZ);

        Cell TreasureCell = GetCellAt(GenColumn, GenRow);
        GameObject treasureObj = Instantiate(treasurePrefab);
        treasureObj.transform.position = TreasureCell.transform.position;
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        GameOverText.text = "You Died";
        RestartText.text = "Press Space to Restart";

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("SampleScene");
            GameOverText.text = "";
            RestartText.text = "";
            isGameOver = false;
        }
        else
        {
            isGameOver = true;
        }
    }

    public void Win()
    {
        GameOverText.text = "You Won";
        RestartText.text = "Press Space to Restart";
        Time.timeScale = 0;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("SampleScene");
            GameOverText.text = "";
            RestartText.text = "";
            didWin = false;
        }
        else
        {
            didWin = true;
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
        OnMazeGenerationComplete();
    }

    public void SetSpawn(GameObject Player)
    {
        Cell startingCell = GetCellAt(0, 0);
        Player.transform.position = startingCell.transform.position;
        GameObject flagObj = Instantiate(flagPrefab);
        flagObj.transform.position = startingCell.transform.position;
    }
}
