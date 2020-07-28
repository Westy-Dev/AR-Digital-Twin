using UnityEngine;


public class TouchGestureManager : MonoBehaviour
{
    [SerializeField]
    private Camera arCamera;
    [SerializeField]
    private float movementSensitivity = 0.001f;
    [SerializeField]
    private float rotateSensitivity = 0.1f;

    private GameObject arObject;


    float initialFingersDistance;
    Vector3 initialArObjectScale;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.touches[0];
            Touch touch2 = Input.touches[1];

            if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
            {
                initialFingersDistance = Vector2.Distance(touch1.position, touch2.position);
                Ray ray = arCamera.ScreenPointToRay(touch1.position);
                RaycastHit hitARObject;
                //Debug.Log("Ray Fired");
                if (Physics.Raycast(ray, out hitARObject))
                {
                    arObject = hitARObject.transform.gameObject;
                    initialArObjectScale = arObject.transform.localScale;
                }
            }
            else if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                var currentFingersDistance = Vector2.Distance(touch1.position, touch2.position);
                var scaleFactor = currentFingersDistance / initialFingersDistance;
                if (arObject != null)
                {
                    arObject.transform.localScale = initialArObjectScale * scaleFactor;
                }

            }

        }
        else if (Input.touchCount == 1)
        {
            //Debug.Log("Touch Phase = " + touch.phase);

            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = touch.position;

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = arCamera.ScreenPointToRay(touchPosition);
                RaycastHit hitARObject;
                //Debug.Log("Ray Fired");
                if (Physics.Raycast(ray, out hitARObject))
                {
                    arObject = hitARObject.transform.gameObject;
                    if (arObject != null)
                    {
                        // Debug.Log("Hit Object");
                        //  Debug.Log("Position = " + arObject.transform.position);
                    }
                } else
                {
                    arObject = null;
                }
            }

            if (touch.phase == TouchPhase.Moved)
            {
                if (arObject != null)
                {
                    Vector2 touchMovement = (touch.deltaPosition) * movementSensitivity;

                    //For Transforming position
                    //Debug.Log("Delta Touch Position = " + touchMovement);
                    //arObject.transform.position = new Vector3(
                    //            arObject.transform.position.x + touchMovement.x,
                    //            arObject.transform.position.y + touchMovement.y,
                    //           arObject.transform.position.z);
                    //Debug.Log("New Position = " + arObject.transform.position);
                    Debug.Log("Rotating");
                    Vector2 touchRotate = (touch.deltaPosition);

                    //Check if the object is upside down or not
                    if(Vector3.Dot(arObject.transform.up, Vector3.up) >= 0)
                    {
                        arObject.transform.Rotate(arObject.transform.up, -Vector3.Dot(touchRotate, Camera.main.transform.right) * rotateSensitivity, Space.World);
                    } else
                    {
                        arObject.transform.Rotate(arObject.transform.up, Vector3.Dot(touchRotate, Camera.main.transform.right) * rotateSensitivity, Space.World);
                    }
           
                    arObject.transform.Rotate(Camera.main.transform.right, Vector3.Dot(touchRotate, Camera.main.transform.up) * rotateSensitivity, Space.World);
                    //For Rotating Position
                }
            }

            if (touch.phase == TouchPhase.Stationary)
            {
           
            }
        }
    }
}
