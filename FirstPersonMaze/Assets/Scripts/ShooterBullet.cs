using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterBullet : MonoBehaviour
{
    public Rigidbody myRigidbody;

    public float bulletSpeed;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody.AddForce(0, 0, bulletSpeed);

        
    }

    // Update is called once per frame
    void Update()
    {

    }

    
}
