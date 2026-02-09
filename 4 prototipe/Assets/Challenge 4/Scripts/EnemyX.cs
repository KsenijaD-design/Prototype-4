using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyX : MonoBehaviour
{
    public float speed;
    private Rigidbody enemyRb;
    private GameObject player;
    private SpawnManagerX spawnManager;
    public ParticleSystem end;

    // Start is called before the first frame update
    void Start()
    {
      
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManagerX>();
    }

    // Update is called once per frame
    void Update()
    {
        // Set enemy direction towards player goal and move there
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        enemyRb.AddForce(lookDirection * speed);

    }

    private void OnCollisionEnter(Collision other)
    {
        // If enemy collides with either goal, destroy it
        if (other.gameObject.name == "Enemy Goal")
        {
            spawnManager.EnemyReachedGoal();
            Destroy(gameObject);
        } 
        else if (other.gameObject.name == "Player Goal")
        {
            Instantiate(end, transform.position, end.transform.rotation).Play();

            Destroy(gameObject);
            spawnManager.waveCount++;
            spawnManager.SpawnEnemyWave(spawnManager.waveCount); 
            
            
        }

    }

}
