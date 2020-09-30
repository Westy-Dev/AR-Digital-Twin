//Created By Ben Westcott, 2020
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Script that controls the colour and speed of highlight flashing
/// </summary>
public class HighlightFlash : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private Color startColor;
    [SerializeField]
    private Color endColor;

    private void FixedUpdate()
    {
        //Linearly interpolate between the given colours at the given speed
        GetComponent<Renderer>().material.color = Color.Lerp(startColor, endColor,
                                                    Mathf.PingPong(Time.time * speed, 1));
    }

}
