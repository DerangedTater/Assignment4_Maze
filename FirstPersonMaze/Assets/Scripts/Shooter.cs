using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public float MoveSpeed;
    public float TurnSpeed;
    public float ShotDelay;

    private Cell currentCell;
    private Cell destCell;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(destCell == null)
        {
            // Decide on the next cell to move to
            destCell = MazeGenerator.Instance.GetRandomAdjacentCellTo(currentCell);
        }
        else
        {
            if(!IsFacingDestination())
            {
                TurnTowardDestinationCell();
            }
            MoveToDestinationCell();

            // Move toward the destination cell
            
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

    private void TurnTowardDestinationCell()
    {
        Vector3 facing = transform.forward;
        Vector3 toTarget = destCell.transform.position - this.transform.position;
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

    public void SetStartingCell(Cell startCell)
    {
        currentCell = startCell;
    }
}
//DOT PRODUCT:
//calculates the angle of difference between two vectors
//
//Vec1 dot Vec2 = cos(angle)
