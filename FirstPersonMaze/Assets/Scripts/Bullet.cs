using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
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
        if(other.gameObject.tag != "Player" && other.gameObject.tag != "Trigger")
        {
            Destroy(this.gameObject);
            Debug.Log(other.gameObject.name);
        }

    }
}
