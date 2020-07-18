using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegoPieceMovement: MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 endPosition;
    public float speed;
    public float distance;
    public enum Direction { X, Y, Z}
    public Direction direction;

    private float percentageOfMovementElapsed = 0;
    private bool resetting;
    // Start is called before the first frame update
    void Start()
    {
        SetMovementPositions(direction);

    }

    void SetMovementPositions(Direction direction)
    {
        endPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
        
        switch (direction)
        {
            case Direction.X:
                startPosition = endPosition + new Vector3(distance, 0, 0);
                break;
            case Direction.Y:
                startPosition = endPosition + new Vector3(0, distance, 0);
                break;
            case Direction.Z:
                startPosition = endPosition + new Vector3(0, 0, distance);
                break;
            default:
                break;
        }

        transform.localPosition = startPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {    
        if (transform.localPosition != endPosition)
        {
            percentageOfMovementElapsed += Time.deltaTime / speed;
            transform.localPosition = Vector3.Lerp(startPosition, endPosition, percentageOfMovementElapsed);
        } else
        {
            if (!resetting)
            {
                StartCoroutine(ResetPosition());
            } 
        }   
    }

    IEnumerator ResetPosition()
    {
        resetting = true;
        yield return new WaitForSeconds(1);
        percentageOfMovementElapsed = 0;
        transform.localPosition = startPosition;
        resetting = false;
    }
}
