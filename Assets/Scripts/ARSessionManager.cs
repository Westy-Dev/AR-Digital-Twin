using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ARSessionManager : MonoBehaviour
{
    //Provide in order of build
    [Tooltip("Provide Folder Name With Model Instructions")]
    public string InstructionFolderName;
    private GameObject[] modelInstructions;
    //The position to instantiate the models in the world
    public Vector3 AssemblyPosition;
    private Vector3 assemblyInitialScale;
    private Quaternion assemblyInitialRotation;

    private int currentInstructionIndex;
    private int lastInstructionIndex;
    private GameObject currentModel;

    [SerializeField]
    private UIManager uiManager;
    // Start is called before the first frame update
    void Start()
    {

        currentInstructionIndex = 0;

        // Instantiates a Prefab located in any Resources
        // folder in your project's Assets folder with the name provided in the inspector
        modelInstructions = Resources.LoadAll<GameObject>(InstructionFolderName);

        Array.Sort(modelInstructions, delegate (GameObject x, GameObject y) { return int.Parse(x.name).CompareTo(int.Parse(y.name)); });

        if (modelInstructions !=null || modelInstructions.Length > 0)
        {
            lastInstructionIndex = modelInstructions.Length - 1;

            currentModel = Instantiate(modelInstructions[currentInstructionIndex]);
            //-0.048f, -0.038f, 0.287f
            currentModel.transform.position = AssemblyPosition;
            assemblyInitialScale = currentModel.transform.localScale;
            assemblyInitialRotation = currentModel.transform.rotation;

            int numberOfMovingPartsForInstruction = getNumberOfMovingPartsForInstruction(currentModel);
            uiManager.UpdateNumberOfMovingPartsForInstruction(numberOfMovingPartsForInstruction);
        } else
        {
            Debug.LogError("Cannot Load Instructions - Instructions Not Found in Folder: " + InstructionFolderName,gameObject);
        }

    }

    public void loadNextInstruction()
    {
        if (currentInstructionIndex != lastInstructionIndex)
        {
            currentInstructionIndex++;
            loadNewModel(currentInstructionIndex);
        }
    }

    public void loadPreviousInstruction()
    {
        if (currentInstructionIndex != 0)
        {
            currentInstructionIndex--;
            loadNewModel(currentInstructionIndex);
        }
    }

    public void resetPosition()
    {
        currentModel.transform.localPosition = AssemblyPosition;
        currentModel.transform.localScale = assemblyInitialScale;
        currentModel.transform.rotation = assemblyInitialRotation;
    }

    private void loadNewModel(int currentInstructionIndex)
    {
        Transform currentTransform = currentModel.transform;
        Destroy(currentModel);
        currentModel = Instantiate(modelInstructions[currentInstructionIndex]);
        currentModel.transform.position = currentTransform.position;
        currentModel.transform.localScale = currentTransform.localScale;
        currentModel.transform.rotation = currentTransform.rotation;
        int numberOfMovingPartsForInstruction = getNumberOfMovingPartsForInstruction(currentModel);
        uiManager.UpdateNumberOfMovingPartsForInstruction(numberOfMovingPartsForInstruction);
    }

    private int getNumberOfMovingPartsForInstruction(GameObject currentModel)
    {
        int numberOfMovingPartsForInstruction = 0;
        foreach(Transform part in currentModel.transform)
        {
            InstructionPieceMovement movementScript = part.GetComponent<InstructionPieceMovement>();
            if (movementScript != null)
            {
                numberOfMovingPartsForInstruction++;
            }
            
            if(part.childCount > 0)
            {
                numberOfMovingPartsForInstruction += getNumberOfMovingPartsForInstruction(part.gameObject);
            }
        }
        return numberOfMovingPartsForInstruction;
    }
}
