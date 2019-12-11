using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
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
        if(other.gameObject.tag != "Trigger" && other.gameObject.tag != "Player")
        {
            switch(other.gameObject.tag)
            {
                case "Shooter":
                    Shooter hitShooter = other.GetComponentInParent<Shooter>();
                    hitShooter.SubHealth();
                    break;
                case "Brawler":
                    Brawler hitBrawler = other.GetComponentInParent<Brawler>();
                    hitBrawler.SubHealth();
                    break;
                case "Ghost":
                    Ghost hitGhost = other.GetComponentInParent<Ghost>();
                    hitGhost.SubHealth();
                    break;
                case "BrawlerGenerator":
                    BrawlerGenerator hitBrawlerGenerator = other.GetComponentInParent<BrawlerGenerator>();
                    hitBrawlerGenerator.SubHealth();
                    break;
                case "GhostGenerator":
                    GhostGenerator hitGhostGenerator = other.GetComponentInParent<GhostGenerator>();
                    hitGhostGenerator.SubHealth();
                    break;
                case "ShooterGenerator":
                    ShooterGenerator hitShooterGenerator = other.GetComponentInParent<ShooterGenerator>();
                    hitShooterGenerator.SubHealth();
                    break;
                default:
                    break;
            }

            Destroy(this.gameObject);
            Debug.Log(other.gameObject.name);
        }

    }
}
