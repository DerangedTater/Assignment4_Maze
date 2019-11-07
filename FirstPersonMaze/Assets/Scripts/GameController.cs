using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject shooterPrefab;

    // Start is called before the first frame update
    void Start()
    {
        SpawnShooters();
    }

    private void SpawnShooters()
    {
        int numColumns = MazeGenerator.Instance.NumCellsX;
        int numRows = MazeGenerator.Instance.NumCellsZ;

        int spawnColumn = Random.Range(0, numColumns);
        int spawnRow = Random.Range(0, numRows);

        Cell spawnCell = MazeGenerator.Instance.GetCellAt(spawnColumn, spawnRow);
        Vector3 cellpos = spawnCell.transform.position;
        Vector3 spawnPos = new Vector3(cellpos.x, 0.0f, cellpos.z);
        GameObject shooterObj = Instantiate(shooterPrefab);
        


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
