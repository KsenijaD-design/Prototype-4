using UnityEngine;

public class rotatecamera : MonoBehaviour
{
    
    public float speed = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        transform.Rotate(0, horizontal * speed * Time.deltaTime, 0);
    }
}
