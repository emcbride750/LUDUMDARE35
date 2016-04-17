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

    private float kAxisAbsoluteMaximum = 1f;

    public static float kHorizontalForceMaximum = 1f;
    public static float kVerticalForceMaximum = 1f;
    public static float kRotationalForceMaximum = 1f;


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

        // Now we can use these values to apply a force value to the pixel that we own
        var horizontalForce = kHorizontalForceMaximum * (horizontalValue / kAxisAbsoluteMaximum) * connectedPixelsCount;
        var verticalForce = kVerticalForceMaximum * (verticalValue / kAxisAbsoluteMaximum) * connectedPixelsCount;
        var rotationalForce = kRotationalForceMaximum * (rotationValue / kAxisAbsoluteMaximum) * connectedPixelsCount;

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
