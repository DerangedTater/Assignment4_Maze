using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public float MoveSpeed;
    public float TurnSpeed;
    public float ShotDelay;

    public GameObject RayCastSource;

    public float scanRange;

    private float shotTimer = 0.0f;

    private Cell currentCell;
    private Cell destCell;

    public GameObject shooterBullet;
    public GameObject gunEnd;

    private MazeGenerator MazeManager;

    private ShooterGenerator myGenerator;
    // Start is called before the first frame update
    void Start()
    {
        //SetStartingCell();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject playerObject = MazeGenerator.Instance.Player;
        bool test = CanSeeTarget(playerObject.transform);

        if (!CanSeeTarget(playerObject.transform))
        {
            if (destCell == null)
            {
                // Decide on the next cell to move to
                destCell = MazeGenerator.Instance.GetRandomAdjacentCellTo(currentCell, false);
            }
            else
            {
                if (!IsFacingDestination())
                {
                    TurnTowardDestinationCell();
                }
                MoveToDestinationCell();

                // Move toward the destination cell

            }
        }
        else
        {
            if (!IsFacingPlayer())
            {
                TurnTowardsPlayer();
            }
            else
            {
                
            }
            FireProjectile();
            ChasePlayer();
        }
    }

    private void FireProjectile()
    {
        shotTimer += Time.deltaTime;

        if(shotTimer >= ShotDelay)
        {
            GameObject bullet = Instantiate(shooterBullet);
            Bullet SBullet = bullet.GetComponent<Bullet>();
            bullet.transform.rotation = this.gameObject.transform.rotation;
            bullet.transform.position = gunEnd.transform.position;
            shotTimer = 0.0f;

            SBullet.OnceInstantiated();
        }
    }

    private bool IsFacingDestination()
    {
        Vector3 facing = transform.forward;
        Vector3 toDest = destCell.transform.position - this.transform.position;
        float dotProduct = Vector3.Dot(facing, toDest);
        float radianAngle = Mathf.Acos(dotProduct);
        float degreesAngle = Mathf.Rad2Deg * radianAngle;

        return (degreesAngle == 0.0f);
    }

    private bool IsFacingPlayer()
    {
        GameObject playerObject = MazeGenerator.Instance.Player;
        Vector3 facing = transform.forward;
        Vector3 toPlayer = playerObject.transform.position - this.transform.position;
        float dotProduct = Vector3.Dot(facing, toPlayer);
        float radianAngle = Mathf.Acos(dotProduct);
        float degreesAngle = Mathf.Rad2Deg * radianAngle;

        return (degreesAngle == 0.0f);
    }

    private void TurnTowardDestinationCell()
    {
        Vector3 facing = transform.forward;
        Vector3 toTarget = destCell.transform.position - this.transform.position;
        Vector3 newFacing = Vector3.RotateTowards(facing, toTarget, TurnSpeed * Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.LookRotation(newFacing);
    }

    private void TurnTowardsPlayer()
    {
        GameObject playerObject = MazeGenerator.Instance.Player;
        Vector3 facing = transform.forward;
        Vector3 toTarget = playerObject.transform.position - this.transform.position;
        Vector3 newFacing = Vector3.RotateTowards(facing, toTarget, TurnSpeed * Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.LookRotation(newFacing);
    }

    private void MoveToDestinationCell()
    {
        Vector3 curPos = transform.position;
        Vector3 destPos = destCell.transform.position;

        if (curPos == destPos)
        {
            currentCell = destCell;
            destCell = null;
        }
        else
        {
            Vector3 moveVec = destPos - curPos;
            float distanceToDestination = moveVec.magnitude;
            moveVec.Normalize();
            moveVec *= MoveSpeed * Time.deltaTime;
            if(moveVec.magnitude > distanceToDestination)
            {
                transform.position = destPos;
            }
            else
            {
                transform.position += moveVec;
            }

        }


    }

    private bool CanSeeTarget(Transform targetTrans)
    {
        Vector3 RayCastStartPos = RayCastSource.transform.position;
        RaycastHit hit;
        Vector3 RayCastDir = targetTrans.position - RayCastStartPos;
        RayCastDir.y = 0.0f;
        RayCastDir.Normalize();
        int layerMask = LayerMask.GetMask("Ignore Raycast");
        layerMask = ~layerMask;

        if (Physics.Raycast(RayCastStartPos, RayCastDir, out hit, scanRange, layerMask))
        {
            //Debug.Log("Raycast Hit: " + hit.collider.gameObject.name);

            Player player = hit.transform.gameObject.GetComponentInParent<Player>();
            if (player != null)
            {
                Debug.DrawRay(RayCastStartPos, RayCastDir * scanRange, Color.red);
                return true;
            }

            else
            {
                Debug.DrawRay(RayCastStartPos, RayCastDir * scanRange, Color.yellow);
                return false;
            }

        }

        else
        {
            Debug.DrawRay(RayCastStartPos, RayCastDir * scanRange, Color.yellow);
            return false;
        }
    }

    private void ChasePlayer()
    {
        GameObject playerObj = MazeGenerator.Instance.Player;

        Vector3 curPos = transform.position;
        Vector3 destPos = playerObj.transform.position;
        Vector3 moveVec = destPos - curPos;
        float distanceToDestination = moveVec.magnitude;
        moveVec.Normalize();
        moveVec *= MoveSpeed * Time.deltaTime;

        transform.position += moveVec;
    }

    public void SetStartingCell(Cell startCell)
    {
        currentCell = startCell;
    }
}
//DOT PRODUCT:
//calculates the angle of difference between two vectors
//
//Vec1 dot Vec2 = cos(angle)
