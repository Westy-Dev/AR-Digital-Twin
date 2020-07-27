using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ARSessionManager : MonoBehaviour
{
    //Provide in order of build
    [Tooltip("Provide AR Models for Instructions")]
    public string[] ModelsForInstructions;
    //The position to instantiate the models in the world
    public Vector3 AssemblyPosition;
    private Vector3 assemblyInitialScale;

    private int currentInstructionIndex;
    private int lastInstructionIndex;
    private GameObject currentModel;
    // Start is called before the first frame update
    void Start()
    {

        currentInstructionIndex = 0;
        lastInstructionIndex = ModelsForInstructions.Length - 1;
        // Instantiates a Prefab located in any Resources
        // folder in your project's Assets folder with the name provided in the inspector
        currentModel = Instantiate(Resources.Load(ModelsForInstructions[currentInstructionIndex], typeof(GameObject))) as GameObject;
        //-0.048f, -0.038f, 0.287f
        currentModel.transform.position = AssemblyPosition;
        assemblyInitialScale = currentModel.transform.localScale;
    }

    public void loadNextInstruction()
    {
        if (currentInstructionIndex != lastInstructionIndex)
        {
            currentInstructionIndex++;
            Destroy(currentModel);

            currentModel = Instantiate(Resources.Load(ModelsForInstructions[currentInstructionIndex], typeof(GameObject))) as GameObject;
            currentModel.transform.position = AssemblyPosition;
        }

    }

    public void loadPreviousInstruction()
    {
        if (currentInstructionIndex != 0)
        {
            currentInstructionIndex--;
            Destroy(currentModel);

            currentModel = Instantiate(Resources.Load(ModelsForInstructions[currentInstructionIndex], typeof(GameObject))) as GameObject;
            currentModel.transform.position = AssemblyPosition;
        }
    }

    public void resetPosition()
    {
        currentModel.transform.localPosition = AssemblyPosition;
        currentModel.transform.localScale = assemblyInitialScale;
    }
}
