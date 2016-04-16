using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PixelCollisionHandler))]
public class PlayerController : MonoBehaviour
{
    private string kHorizontalAxisString = "Horizontal";
    private string kRotationalAxisString = "Rotation";
    private string kVerticalAxisString = "Vertical";

    private float kAxisAbsoluteMaximum = 1;

    public static float kHorizontalForceMaximum = 5;
    public static float kVerticalForceMaximum = 5;
    public static float kRotationalForceMaximum = 5;

    private Rigidbody2D pixelBody;

    private void Start()
    {
        pixelBody = GetComponent<PixelCollisionHandler>().GetComponent<Rigidbody2D>();
    }

    private void Awake()
    {

    }

    
    private void Update()
    {
        // First of all, is this pixel alive?
        if (pixelBody == null)
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

        this.addHorizontalForce(horizontalForce);
        this.addVerticalForce(verticalForce);
        this.addRotationalForce(rotationalForce);
    }


    private void FixedUpdate()
    {
    }


    public void addHorizontalForce(float force)
    {
        // This one is easy! apply the force to the current objet
        pixelBody.AddForce(new Vector2(force, 0));
    }

    public void addVerticalForce(float force)
    {
        // This one is easy! apply the force to the current objet
        pixelBody.AddForce(new Vector2(0, force));
    }

    public void addRotationalForce(float force)
    {
        // This one is easy! apply the force to the current objet
        pixelBody.AddTorque(force);
    }
}
