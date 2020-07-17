using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegoPieceInstruction : MonoBehaviour
{
    public Vector3 startPosition;
    private Vector3 endPosition;
    public float speed;
    public bool moveX;
    public bool moveY;
    private float percentageOfMovementElapsed = 0;
    private bool resetting;
    // Start is called before the first frame update
    void Start()
    {
        endPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
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
