using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private string kHorizontalAxisString = "Horizontal";
    private string kRotationalAxisString = "Rotation";
    private string kVerticalAxisString = "Vertical";

    private double kAxisAbsoluteMaximum = 1;

    public static double kHorizontalForceMaximum = 5;
    public static double kVerticalForceMaximum = 5;
    public static double kRotationalForceMaximum = 5;


    private pixel parentPixel = null;
    
    private void Awake()
    {

    }

    public void setOwningPixel(pixel owningPixel)
    {
        parentPixel = owningPixel;
    }
    
    private void Update()
    {
        // First of all, is this pixel alive?
        if (parentPixel == null)
        {
            // Do nothing, no owning pixel!
            // TODO: Should this be killed? I bet that when a pixel dies
            // it should kill its player controller if it has one
            return;
        }
        var horizontalValue = Input.GetAxis(kHorizontalAxisString);
        var verticalValue = Input.GetAxis(kVerticalAxisString);
        var rotationValue = Input.GetAxis(kRotationalAxisString);

        // Now we can use these values to apply a force value to the pixel that we own
        var horizontalForce = kHorizontalForceMaximum * (horizontalValue / kAxisAbsoluteMaximum);
        var verticalForce = kVerticalForceMaximum * (verticalValue / kAxisAbsoluteMaximum);
        var rotationalForce = kRotationalForceMaximum * (rotationValue / kAxisAbsoluteMaximum);

        parentPixel.addHorizontalForce(horizontalForce);
        parentPixel.addVerticalForce(verticalForce);
        parentPixel.addRotationalForce(rotationalForce);
    }


    private void FixedUpdate()
    {
    }
}
