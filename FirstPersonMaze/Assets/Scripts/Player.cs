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
    public Camera PlayerCamera;
    public GameObject playerBullet;
    public GameObject bulletSpawner;

    private Vector3 bulletRotation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = Vector3.zero;
        move.y = -GravityForce;

        //Forward/Back/Left/Right Movement
        float frameMovementSpeed = Time.deltaTime * MovementSpeed;
        float moveX = Input.GetAxis("Horizontal") * frameMovementSpeed;
        float moveZ = Input.GetAxis("Vertical") * frameMovementSpeed;
        move = new Vector3(moveX, 0.0f, moveZ);
        move = Vector3.ClampMagnitude(move, frameMovementSpeed);
        move = this.transform.TransformVector(move);
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

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(playerBullet);
        Bullet PBullet = bullet.GetComponent<Bullet>();
        bullet.transform.position = bulletSpawner.transform.position;
        Vector3 playerRot = transform.eulerAngles;
        Vector3 cameraRot = PlayerCamera.transform.eulerAngles;

        bullet.transform.rotation = bulletSpawner.transform.rotation; //this.gameObject.transform.rotation;
        PBullet.OnceInstantiated();
    }
}
