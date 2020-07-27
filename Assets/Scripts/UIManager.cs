using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private ARSessionManager arSessionManager;

    [SerializeField]
    private GameObject DebugCanvas;
    private bool debug = false;
    public void loadNextInstruction()
    {
        arSessionManager.loadNextInstruction();
    }

    public void loadPreviousInstruction()
    {
        arSessionManager.loadPreviousInstruction();
    }

    public void toggleDebug()
    {
        debug = !debug;

        DebugCanvas.SetActive(debug);
    }

    public void resetPosition()
    {
        arSessionManager.resetPosition();
    }
}
