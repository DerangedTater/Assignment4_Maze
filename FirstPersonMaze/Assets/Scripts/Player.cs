using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CharacterController PlayerController;
    public float MovementSpeed;
    public float TurningSpeed;
    public float MaximumPitch;
    public Camera PlayerCamera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Forward/Back/Left/Right Movement
        float frameMovementSpeed = Time.deltaTime * MovementSpeed;
        float moveX = Input.GetAxis("Horizontal") * frameMovementSpeed;
        float moveZ = Input.GetAxis("Vertical") * frameMovementSpeed;
        Vector3 move = new Vector3(moveX, 0.0f, moveZ);
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
    }
}
