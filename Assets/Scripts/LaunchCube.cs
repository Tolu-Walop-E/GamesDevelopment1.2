using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchCube : MonoBehaviour
{
    public float bounceForce = 15f;
    public string playerTag = "Player";

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(playerTag))
        {
            Rigidbody playerRigidBody = collision.collider.GetComponent<Rigidbody>();

            if (playerRigidBody != null)
            {
                Vector3 launchDirection = new Vector3(0, bounceForce, 0);
                playerRigidBody.AddForce(launchDirection, ForceMode.Impulse);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

