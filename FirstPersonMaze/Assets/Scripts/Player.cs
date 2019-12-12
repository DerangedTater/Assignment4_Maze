using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CharacterController PlayerController;
    public float MovementSpeed;
    public float TurningSpeed;
    public float MaximumPitch;
    public float GravityForce;
    public float shotDelay;
    public Camera PlayerCamera;
    public GameObject playerBullet;
    public GameObject bulletSpawner;

    private Vector3 bulletRotation;
    private bool hasTreasure = false;
    private bool GameOver = false;
    private float shotTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        Cell StartCell = MazeGenerator.Instance.GetCellAt(0, 0);
        this.gameObject.transform.position = StartCell.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = Vector3.zero; 
        //Forward/Back/Left/Right Movement
        float frameMovementSpeed = Time.deltaTime * MovementSpeed;
        float moveX = Input.GetAxis("Horizontal") * frameMovementSpeed;
        float moveZ = Input.GetAxis("Vertical") * frameMovementSpeed;
        move = new Vector3(moveX, 0.0f, moveZ);
        move = Vector3.ClampMagnitude(move, frameMovementSpeed);
        move = this.transform.TransformVector(move);
        move.y = -GravityForce;
        PlayerController.Move(move);

        // Yaw Rotation (Player)
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * TurningSpeed;
        Vector3 playerRot = transform.rotation.eulerAngles;
        playerRot.y += mouseX;
        transform.rotation = Quaternion.Euler(playerRot);
        
        // Mouse Look (Camera)
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * TurningSpeed;
        Vector3 cameraRot = PlayerCamera.transform.rotation.eulerAngles;
        if(cameraRot.x > MaximumPitch)
        {
            cameraRot.x -= 360.0f;
        }
        if(cameraRot.x < -MaximumPitch)
        {
            cameraRot.x += 360.0f;
        }
        cameraRot.x -= mouseY;
        cameraRot.x = Mathf.Clamp(cameraRot.x, -MaximumPitch, MaximumPitch);
        PlayerCamera.transform.rotation = Quaternion.Euler(cameraRot);

        bulletRotation.x = PlayerCamera.transform.rotation.x;
        bulletRotation.y = transform.rotation.y;

        shotTimer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && shotTimer >= shotDelay && !GameOver)
        {
            Shoot();
            shotTimer = 0;
        }

        if(this.transform.position.y > 5)
        {
            Cell StartCell = MazeGenerator.Instance.GetCellAt(0, 0);
            this.gameObject.transform.position = StartCell.transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Shooter" || other.gameObject.tag == "Brawler" || other.gameObject.tag == "Ghost" || other.gameObject.tag == "ShooterBullet")
        {
            GameOver = true;
            MazeGenerator.Instance.GameOver();
        }
        else if(other.gameObject.tag == "Treasure")
        {
            hasTreasure = true;
            Destroy(other.transform.parent.gameObject);
        }
        else if(other.gameObject.tag == "Finish")
        {
            if(hasTreasure)
            {
                GameOver = true;
                MazeGenerator.Instance.Win();
            }
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(playerBullet);
        PlayerBullet PBullet = bullet.GetComponent<PlayerBullet>();
        bullet.transform.position = bulletSpawner.transform.position;
        Vector3 playerRot = transform.eulerAngles;
        Vector3 cameraRot = PlayerCamera.transform.eulerAngles;

        bullet.transform.rotation = bulletSpawner.transform.rotation; //this.gameObject.transform.rotation;
        PBullet.OnceInstantiated();
    }
}
