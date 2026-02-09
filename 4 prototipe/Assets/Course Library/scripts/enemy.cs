using UnityEngine;

public class enemy : MonoBehaviour
{
    public float speed = 0;
    private Rigidbody rbEnemy;
    private GameObject player;
    
    public bool isBoss = false;
    public float spawnInterval; 
    private float nextSpawn; 
    public int miniEnemySpawnCount; 
    private spawnManager spawnManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      rbEnemy = gameObject.GetComponent<Rigidbody>();
      player = GameObject.Find("Player");
      if (isBoss) 
      { 
          spawnManager = FindObjectOfType<spawnManager>(); 
      } 
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        rbEnemy.AddForce(lookDirection * speed);
        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }
        if(isBoss) 
        { 
            if(Time.time > nextSpawn) 
            { 
                nextSpawn = Time.time + spawnInterval; 
                spawnManager.SpawnMiniEnemy(miniEnemySpawnCount); 
            } 
        } 
    }
}
