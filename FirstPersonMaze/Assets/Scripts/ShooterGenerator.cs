using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterGenerator : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnDelay;
    public int enemyCap;

    public int maxHealth;
    private int currentHealth;

    private Cell myCell;
    private List<GameObject> liveEnemies = new List<GameObject>();
    private float elapsedSinceSpawn = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
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
                Shooter newShooter = newEnemyObj.GetComponent<Shooter>();
                newShooter.SetStartingCell(myCell);
                newShooter.SetGenerator(this);

                liveEnemies.Add(newEnemyObj);
            }
            elapsedSinceSpawn = 0;
        }
    }

    public void SubHealth()
    {
        currentHealth -= 1;
        Debug.Log(currentHealth);
        if (currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void RemoveEnemy(GameObject deadShooter)
    {
        liveEnemies.Remove(deadShooter);
    }

    public void Init(Cell cell)
    {
        myCell = cell;
    }
}
