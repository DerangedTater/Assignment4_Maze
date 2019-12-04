using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrawlerGenerator : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnDelay;
    public int enemyCap;

    private Cell myCell;
    private List<GameObject> liveEnemies = new List<GameObject>();
    private float elapsedSinceSpawn = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        //
    }

    // Update is called once per frame
    void Update()
    {
        elapsedSinceSpawn += Time.deltaTime;
        if(elapsedSinceSpawn >= spawnDelay)
        {
            if(liveEnemies.Count < enemyCap)
            {
                GameObject newEnemyObj = Instantiate(enemyPrefab);
                newEnemyObj.transform.position = myCell.transform.position;
                Brawler newBrawler = newEnemyObj.GetComponent<Brawler>();
                newBrawler.SetStartingCell(myCell);

                liveEnemies.Add(newEnemyObj);
            }
            elapsedSinceSpawn = 0;
        }
    }

    public void Init(Cell cell)
    {
        myCell = cell;
    }
}
