using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARSessionManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Instantiates a Prefab named "enemy" located in any Resources
        // folder in your project's Assets folder.
        GameObject instance = Instantiate(Resources.Load("SportsCar31100", typeof(GameObject))) as GameObject;
        instance.transform.position = new Vector3(-0.048f, -0.038f, 0.287f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
