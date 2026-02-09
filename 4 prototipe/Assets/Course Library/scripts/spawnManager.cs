using UnityEngine;

public class spawnManager : MonoBehaviour
{
    

    private float spawnRange = 9;
    public int  enemyCount = 4;

    public int waveNumber = 1;

    public GameObject[] powerupPrefab;
    public GameObject[] enemyPrefab;
    
    public GameObject bossPrefab; 
    public GameObject[] miniEnemyPrefabs; 
    public int bossRound;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnEnemyWave(waveNumber);
        int randomPowerup = Random.Range(0, powerupPrefab.Length); 
        Instantiate(powerupPrefab[randomPowerup], GenerateRandomPosition(), 
            powerupPrefab[randomPowerup].transform.rotation); 
    }

  


    private Vector3 GenerateRandomPosition()
    {
        float spawnX = Random.Range(-spawnRange, spawnRange);
        float spawnZ = Random.Range(-spawnRange, spawnRange);
        Vector3 randomPosition = new Vector3(spawnX, 0, spawnZ);
        return randomPosition;
    }
    
    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsOfType<enemy>().Length;
        if (enemyCount == 0)
        {
            waveNumber++;
//waveNumber = waveNumber + 1;
//waveNumber += 1;
//Spawn a boss every x number of waves  
            if (waveNumber % bossRound == 0)  
            {  
                SpawnBossWave(waveNumber);  
            }  
            else  
            {  
                SpawnEnemyWave(waveNumber);  
            } 
            
            SpawnEnemyWave(waveNumber);
            int rand2 = Random.Range(0, powerupPrefab.Length);
            Instantiate(powerupPrefab[rand2], GenerateRandomPosition(), powerupPrefab[rand2].transform.rotation);
        }
    }

    void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            int rand = Random.Range(0, enemyPrefab.Length);
            Instantiate(enemyPrefab[rand], GenerateRandomPosition(), enemyPrefab[rand].transform.rotation);
        } 
    }
    void SpawnBossWave(int currentRound) 
    { 
        int miniEnemysToSpawn; 
//We dont want to divide by 0! 
        if (bossRound != 0) 
        { 
            miniEnemysToSpawn = currentRound / bossRound; 
        } 
        else 
        { 
            miniEnemysToSpawn = 1; 
        } 
        var boss = Instantiate(bossPrefab, GenerateRandomPosition(), 
            bossPrefab.transform.rotation); 
        boss.GetComponent<enemy>().miniEnemySpawnCount = miniEnemysToSpawn; 
    } 
    public void SpawnMiniEnemy(int amount) 
    { 
        for(int i = 0; i < amount; i++)  
        {  
            int randomMini = Random.Range(0, miniEnemyPrefabs.Length); 
            Instantiate(miniEnemyPrefabs[randomMini], GenerateRandomPosition(), 
                miniEnemyPrefabs[randomMini].transform.rotation);  
        }  
    } 
    
}
