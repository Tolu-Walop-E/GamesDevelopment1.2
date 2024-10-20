using UnityEngine;

public class CubeMove : MonoBehaviour
{
    public float leftSpeed = 2f; 
    public float rightSpeed = 5f; 
    public float distance = 3f; 

    private bool movingRight = true; 
    private Vector3 startPosition; 

    void Start()
    {
        
        startPosition = transform.position;
    }

    void Update()
    {
        MoveCube();
    }

    void MoveCube()
    {
        
        if (movingRight)
        {
            transform.Translate(Vector3.right * rightSpeed * Time.deltaTime);

           
            if (transform.position.x >= startPosition.x + distance)
            {
                movingRight = false; 
            }
        }
        else
        {
            transform.Translate(Vector3.left * leftSpeed * Time.deltaTime);

            
            if (transform.position.x <= startPosition.x - distance)
            {
                movingRight = true; 
            }
        }
    }
}
