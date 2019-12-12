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

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnceInstantiated()
    {
        myRigidbody.AddRelativeForce(transform.forward * bulletSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag != "Trigger" && other.gameObject.tag != "Shooter" && other.gameObject.tag != "PlayerBullet")
        {
            Destroy(this.gameObject);
            Debug.Log(other.gameObject.name);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }
    }
}
