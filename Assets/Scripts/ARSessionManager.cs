//Created By Ben Westcott, 2020
using System;
using UnityEngine;
/// <summary>
/// Manages the position, scale, rotation and display of the <c>CurrentModel</c>
/// </summary>
public class ARSessionManager : MonoBehaviour
{
    //Provide in order of build
    [Tooltip("Provide Folder Name With Model Instruction Prefabs (Prefabs in folder should be in build order)")]
    [SerializeField]
    private string InstructionFolderName;

    [Tooltip("Provide A Start Position For The Model")]
    [SerializeField]
    private Transform InstructionsStartPosition;

    //Holds all prefabs for the model instructions
    private GameObject[] modelInstructions;

    //Initial values used for reset functionality
    private Vector3 assemblyInitialPosition;
    private Vector3 assemblyInitialScale;
    private Quaternion assemblyInitialRotation;

    private int currentInstructionIndex;
    private int lastInstructionIndex;

    //The current displayed prefab
    private GameObject currentModel;

    [SerializeField]
    private UIManager uiManager;
    // Start is called before the first frame update
    void Start()
    {
        //Hide previous button on UI
        uiManager.prevButton.SetActive(false);

        //Loads all model instruction prefabs from resources folder using the given instruction folder name in the inspector
        modelInstructions = Resources.LoadAll<GameObject>(InstructionFolderName);

        //As the resources load all function does not load in order, we need to sort the prefabs by name to get them in order
        Array.Sort(modelInstructions, delegate (GameObject x, GameObject y) { return int.Parse(x.name).CompareTo(int.Parse(y.name)); });

        if (modelInstructions !=null || modelInstructions.Length > 0)
        {
            //Sets the last instruction index
            lastInstructionIndex = modelInstructions.Length - 1;

            //Instantiates the first instruction at the start position set in the inspector
            currentModel = Instantiate(modelInstructions[currentInstructionIndex],InstructionsStartPosition);

            //Set the local position relative to the start position to zero 
            //(this makes sure the model is always located exactly at the instruction start position)
            currentModel.transform.localPosition = Vector3.zero;

            //Set the initial values used for reset functionality
            assemblyInitialPosition = currentModel.transform.localPosition;
            assemblyInitialScale = currentModel.transform.localScale;
            assemblyInitialRotation = currentModel.transform.rotation;

            updateUIText();
            
        } else
        {
            Debug.LogError("Cannot Load Instructions - Instructions Not Found in Folder: " + InstructionFolderName,gameObject);
        }

    }

    /// <summary>
    /// Loads the next instruction from <c>modelInstructions</c>
    /// </summary>
    public void loadNextInstruction()
    {
        if (currentInstructionIndex != lastInstructionIndex)
        {
            currentInstructionIndex++;
            loadNewModel(currentInstructionIndex);
        }

        updateUIButtons();
    }

    /// <summary>
    /// Loads the previous instruction from <c>modelInstructions</c>
    /// </summary>
    public void loadPreviousInstruction()
    {
        if (currentInstructionIndex != 0)
        {
            currentInstructionIndex--;
            loadNewModel(currentInstructionIndex);
        }

        updateUIButtons();
    }

    /// <summary>
    /// Controls the visibility of the previous and next buttons on the UI
    /// </summary>
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
    /// <summary>
    /// Updates the number of parts per step and step number on the UI
    /// </summary>
    private void updateUIText()
    {
        int numberOfMovingPartsForInstruction = getNumberOfMovingPartsForInstruction(currentModel);
        uiManager.UpdateNumberOfMovingPartsForInstruction(numberOfMovingPartsForInstruction);
        uiManager.UpdateStepNumber(currentInstructionIndex + 1);
    }

    /// <summary>
    /// Resets the position of the current instruction model displayed to the original scale, position and rotation.
    /// </summary>
    public void resetModel()
    {
        currentModel.transform.localPosition = assemblyInitialPosition;
        currentModel.transform.localScale = assemblyInitialScale;
        currentModel.transform.rotation = assemblyInitialRotation;
    }

    /// <summary>
    /// Loads the new model instruction from <c>modelInstructions</c> given the instruction index to load
    /// </summary>
    /// <param name="currentInstructionIndex">The instruction index number of the instruction to load</param>
    private void loadNewModel(int currentInstructionIndex)
    {
        //Keep a reference to the current transform of the model
        Transform currentTransform = currentModel.transform;

        //Remove the current model
        Destroy(currentModel);

        //Instantiate the new model instruction at the given instruction start position
        currentModel = Instantiate(modelInstructions[currentInstructionIndex], InstructionsStartPosition);

        //Offset the local position relative to the instruction start position using the original transform reference
        currentModel.transform.localPosition = currentTransform.localPosition;
        currentModel.transform.localScale = currentTransform.localScale;
        currentModel.transform.rotation = currentTransform.rotation;

        updateUIText();
    }

    /// <summary>
    /// Recursively obtains the number of moving parts for the current instruction by checking all game objects 
    /// (and child objects) in the current instruction for the <c>InstructionPieceMovement</c> script
    /// </summary>
    /// <param name="currentModel"></param>
    /// <returns>The number of moving parts for the current instruction model</returns>
    private int getNumberOfMovingPartsForInstruction(GameObject currentModel)
    {
        int numberOfMovingPartsForInstruction = 0;

        // checks every part in the current model to see if it has a movement script attached
        foreach(Transform part in currentModel.transform)
        {
            InstructionPieceMovement movementScript = part.GetComponent<InstructionPieceMovement>();
            if (movementScript != null)
            {
                //If the part has a movement script, it is moving and therefore a step in the current instruction
                numberOfMovingPartsForInstruction++;
            }
            
            if(part.childCount > 0)
            {
                //Recursively check the child objects
                numberOfMovingPartsForInstruction += getNumberOfMovingPartsForInstruction(part.gameObject);
            }
        }
        return numberOfMovingPartsForInstruction;
    }
}
