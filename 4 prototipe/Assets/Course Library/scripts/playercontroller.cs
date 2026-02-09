using UnityEngine;
using System.Collections;

public class playercontroller : MonoBehaviour
{
    private Rigidbody rbPlayer;
    public float speed;
    private GameObject Focalpoint;

    

    public bool hasPowerup;

    private float poewrupStrength;
    public GameObject powerup;
    
    public PowerUpType currentPowerUp = PowerUpType.None; 
    public GameObject rocketPrefab; 
    private GameObject tmpRocket; 
    private Coroutine powerupCountdown;
    
    public float hangTime; 
    public float smashSpeed; 
    public float explosionForce; 
    public float explosionRadius; 
    bool smashing = false; 
    float floorY;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbPlayer = gameObject.GetComponent<Rigidbody>();
        Focalpoint = GameObject.Find("Focalpoint");
    }

    

    // Update is called once per frame
    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        rbPlayer.AddForce(Focalpoint.transform.forward * forwardInput * speed);
        powerup.transform.position = transform.position + new Vector3(0, -0.5f, 0);
        
        if (currentPowerUp == PowerUpType.Rockets && Input.GetKeyDown(KeyCode.F)) 
        { 
            LaunchRockets(); 
        }
        if (currentPowerUp == PowerUpType.Smash && Input.GetKeyDown(KeyCode.Space)&&!smashing) 
        { 
            smashing = true; 
            StartCoroutine(Smash());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            currentPowerUp = other.gameObject.GetComponent<PowerUp>().powerUpType;
            hasPowerup = true;
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
            
            powerup.gameObject.SetActive(true);
            
            if(powerupCountdown != null) 
            { 
                StopCoroutine(powerupCountdown); 
            } 
            powerupCountdown = StartCoroutine(PowerupCountdownRoutine());
            
        }
    }

    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerup = false;
        powerup.gameObject.SetActive(false);
        
        currentPowerUp = PowerUpType.None; 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && currentPowerUp 
            == PowerUpType.Pushback)
        {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);
            
            Debug.Log("Collided with" + collision.gameObject.name +  " with powerup set to: " + currentPowerUp.ToString());
            enemyRigidbody.AddForce(awayFromPlayer * poewrupStrength, ForceMode.Impulse);
        }
    }
    void LaunchRockets() 
    { 
        foreach(var enemy in FindObjectsOfType<enemy>()) 
        { 
            tmpRocket = Instantiate(rocketPrefab, transform.position + Vector3.up, 
                Quaternion.identity); 
            tmpRocket.GetComponent<rocketbehavior>().Fire(enemy.transform); 
        } 
    } 
    IEnumerator Smash() 
    { 
        var enemies = FindObjectsOfType<enemy>(); 
//Store the y position before taking off 
        floorY = transform.position.y; 
//Calculate the amount of time we will go up 
        float jumpTime = Time.time + hangTime; 
        while(Time.time < jumpTime) 
        { 
//move the player up while still keeping their x velocity. 
            rbPlayer.linearVelocity = new Vector2(rbPlayer.linearVelocity.x, 
                smashSpeed); yield return null; 
        } 
//Now move the player down 
        while(transform.position.y > floorY) 
        { 
            rbPlayer.linearVelocity = new Vector2(rbPlayer.linearVelocity.x, -smashSpeed * 
                                                                 2); yield return null; 
        } 
//Cycle through all enemies. 
        for (int i = 0; i < enemies.Length; i++) 
        { 
//Apply an explosion force that originates from our position. 
            if(enemies[i] != null) 
                enemies[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, 
                    transform.position, explosionRadius, 0.0f, ForceMode.Impulse); 
        } 
//We are no longer smashing, so set the boolean to false 
        smashing = false; 
    } 
}
