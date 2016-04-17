using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IPixelConnectionTarget : IEventSystemHandler
{
    void AddPixel(PixelCollisionHandler px);
    void RemovePixel(PixelCollisionHandler px);
}

[RequireComponent(typeof(PixelCollisionHandler))]
public class PlayerController : MonoBehaviour, IPixelConnectionTarget
{
    public static string playerTag = "Player";
    private string kHorizontalAxisString = "Horizontal";
    private string kRotationalAxisString = "Rotation";
    private string kVerticalAxisString = "Vertical";

    //private float kAxisAbsoluteMaximum = Mathf.Sqrt(kHorizontalForceMaximum + kVerticalForceMaximum);

    public static float kHorizontalForceMaximum = 200;
    public static float kVerticalForceMaximum = 200;
    public static float kRotationalForceMaximum = 200;
    public float maxSpeed = 60.0f;


    public static GameObject getPlayer()
    {
        try {
            return GameObject.FindGameObjectWithTag(playerTag);
        } catch
        {
            print("couldn't find player");
            return null;
        }
    }

    public int connectedPixelsCount
    {
        get
        {
            return this.connectedPixels.Count;
        }
    }

    private List<PixelCollisionHandler> connectedPixels = new List<PixelCollisionHandler>();

    public bool isConnectedTo(PixelCollisionHandler pch)
    {
        return this.connectedPixels.Contains(pch);
    }

    private Rigidbody2D pixelBody;

    private void Start()
    {
        
    }

    private void Awake()
    {
        this.gameObject.tag = playerTag;
        PixelCollisionHandler pch = GetComponent<PixelCollisionHandler>();
        pixelBody = pch.GetComponent<Rigidbody2D>();
        this.connectedPixels.Add(pch);
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

        //Get current velocities
        var hVel = pixelBody.velocity.x;
        var vVel = pixelBody.velocity.y;
        var rVel = pixelBody.inertia;

        if ((hVel > maxSpeed) && (horizontalValue > 0)){
            horizontalValue = 0;
        }
        if ((hVel < -maxSpeed) && (horizontalValue < 0))
        {
            horizontalValue = 0;
        }
        if ((vVel > maxSpeed) && (verticalValue > 0))
        {
            verticalValue = 0;
        }
        if ((vVel < -maxSpeed) && (verticalValue < 0))
        {
            verticalValue = 0;
        }
        if ((rVel > maxSpeed) && (rotationValue > 0))
        {
            rotationValue = 0;
        }
        if ((rVel < -maxSpeed) && (rotationValue < 0))
        {
            rotationValue = 0;
        }
        // Now we can use these values to apply a force value to the pixel that we own
        var horizontalForce = kHorizontalForceMaximum * horizontalValue;
        var verticalForce = kVerticalForceMaximum * verticalValue;
        var rotationalForce = kRotationalForceMaximum * rotationValue;

        pixelBody.AddForce(new Vector2(horizontalForce, 0));
        pixelBody.AddForce(new Vector2(0, verticalForce));
        pixelBody.AddTorque(rotationalForce);
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

    void IPixelConnectionTarget.AddPixel(PixelCollisionHandler px)
    {
        if (!connectedPixels.Contains(px))
        {
            connectedPixels.Add(px);
            print("connected to player: "+px.name);
        }
    }

    void IPixelConnectionTarget.RemovePixel(PixelCollisionHandler px)
    {
        connectedPixels.Remove(px);
        print("removed from player: " + px.name);
    }
}
