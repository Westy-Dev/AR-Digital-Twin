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

    public Transform InstructionsStartPosition;
    private Vector3 assemblyInitialPosition;
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

        uiManager.prevButton.SetActive(false);
        // Instantiates a Prefab located in any Resources
        // folder in your project's Assets folder with the name provided in the inspector
        modelInstructions = Resources.LoadAll<GameObject>(InstructionFolderName);

        Array.Sort(modelInstructions, delegate (GameObject x, GameObject y) { return int.Parse(x.name).CompareTo(int.Parse(y.name)); });

        if (modelInstructions !=null || modelInstructions.Length > 0)
        {
            lastInstructionIndex = modelInstructions.Length - 1;

            currentModel = Instantiate(modelInstructions[currentInstructionIndex],InstructionsStartPosition);
            currentModel.transform.localPosition = Vector3.zero;

            assemblyInitialPosition = currentModel.transform.localPosition;
            assemblyInitialScale = currentModel.transform.localScale;
            assemblyInitialRotation = currentModel.transform.rotation;

            updateUIText();
            
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

        updateUIButtons();
    }

    public void loadPreviousInstruction()
    {
        if (currentInstructionIndex != 0)
        {
            currentInstructionIndex--;
            loadNewModel(currentInstructionIndex);
        }

        updateUIButtons();
    }

    private void updateUIButtons()
    {

        if(currentInstructionIndex == 0)
        {
            uiManager.prevButton.SetActive(false);
        } 
        else if (!uiManager.prevButton.activeSelf)
        {
            uiManager.prevButton.SetActive(true);
        }

        if(currentInstructionIndex == lastInstructionIndex)
        {
            uiManager.nextButton.SetActive(false);
        }
        else if (!uiManager.nextButton.activeSelf)
        {
            uiManager.nextButton.SetActive(true);
        }

    }
    
    private void updateUIText()
    {
        int numberOfMovingPartsForInstruction = getNumberOfMovingPartsForInstruction(currentModel);
        uiManager.UpdateNumberOfMovingPartsForInstruction(numberOfMovingPartsForInstruction);
        uiManager.UpdateStepNumber(currentInstructionIndex + 1);
    }

    public void resetPosition()
    {
        currentModel.transform.localPosition = assemblyInitialPosition;
        currentModel.transform.localScale = assemblyInitialScale;
        currentModel.transform.rotation = assemblyInitialRotation;
    }

    private void loadNewModel(int currentInstructionIndex)
    {
        Transform currentTransform = currentModel.transform;
        Destroy(currentModel);
        currentModel = Instantiate(modelInstructions[currentInstructionIndex], InstructionsStartPosition);
        currentModel.transform.localPosition = currentTransform.localPosition;
        currentModel.transform.localScale = currentTransform.localScale;
        currentModel.transform.rotation = currentTransform.rotation;

        updateUIText();
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
