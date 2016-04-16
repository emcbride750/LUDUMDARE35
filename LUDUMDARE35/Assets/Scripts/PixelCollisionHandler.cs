using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class PixelCollisionHandler : MonoBehaviour
{
    public static string StickyTag = "sticky";
    public static string DissolveTag = "dissolve";
    public static string UnbreakableTag = "unbreakable";

    public bool isControlledByPlayer = false;

    public bool isPlayerPixel = false;

    //public float maxDistance = 2.82842712475f;
    private static int maxConnectors = 4;
    
	private List<RelativeJoint2D> joints = new List<RelativeJoint2D>();
    
    public List<RelativeJoint2D> Joints
    {
        get
        {
            return joints;
        }
    }

    public void AddJoint(PixelCollisionHandler ch)
    {
        //create new joint between both objects and add to internal list on both
        if ((ch != null) && (this != ch) && (ch.Joints.Count <= maxConnectors) && (this.Joints.Count <= maxConnectors) && (!this.GetConnectedCollisionHandlers().Contains(ch)))
        {
            //float dist = Vector2.Distance(this.transform.localPosition, ch.transform.localPosition);
            RelativeJoint2D dj = gameObject.AddComponent(typeof(RelativeJoint2D)) as RelativeJoint2D;
            dj.connectedBody = ch.GetComponent<Rigidbody2D>();
            dj.autoConfigureOffset = false;
            dj.maxTorque = 900;
            this.joints.Add(dj);
            ch.joints.Add(dj);
            //return true;
        } else
        {
			throw new Exception();
            //return false;
        }
    }

    public IEnumerable<PixelCollisionHandler> GetConnectedCollisionHandlers()
    {
        foreach (RelativeJoint2D dj in this.joints)
        {
            PixelCollisionHandler ch1 = dj.connectedBody.GetComponentInParent(typeof(PixelCollisionHandler)) as PixelCollisionHandler;
            if (this == ch1)
            {
                PixelCollisionHandler ch2 = dj.GetComponentInParent(typeof(PixelCollisionHandler)) as PixelCollisionHandler;
                yield return ch2;
            }
            else
            {
                yield return ch1;
            }
        }
    }

    private static void DestroyJoint(RelativeJoint2D dj, bool ignoreUnbreakable)
    {
        if (dj != null)
        {
            PixelCollisionHandler ch1 = dj.connectedBody.GetComponent(typeof(PixelCollisionHandler)) as PixelCollisionHandler;
            PixelCollisionHandler ch2 = dj.GetComponentInParent(typeof(PixelCollisionHandler)) as PixelCollisionHandler;
            if ((ch1 != null) && (ch2 != null))
            {
                if (ignoreUnbreakable || (!string.Equals(ch1.tag, UnbreakableTag, System.StringComparison.InvariantCultureIgnoreCase) &&
                    (!string.Equals(ch2.tag, UnbreakableTag, System.StringComparison.InvariantCultureIgnoreCase))))
                {
                    ch1.joints.Remove(dj);
                    ch2.joints.Remove(dj);
                    Destroy(dj);
                }
                else
                {
                    throw new Exception();
                }
            }
            else
            {
                throw new Exception();
            }
        }
        throw new Exception();
    }

    public static void DestroyJoint(RelativeJoint2D dj)
    {
        DestroyJoint(dj, false);
    }

    public RelativeJoint2D GetJoint(PixelCollisionHandler ch)
    {
        if ((ch != null) && (this != ch))
        {
            foreach (var j in this.joints)
            {
                PixelCollisionHandler ch1 = j.connectedBody.GetComponentInParent(typeof(PixelCollisionHandler)) as PixelCollisionHandler;
                PixelCollisionHandler ch2 = j.GetComponentInParent(typeof(PixelCollisionHandler)) as PixelCollisionHandler;
                if ((ch == ch1) || (ch == ch2))
                {
                    return j;
                }
            }
            return null;
        } else
        {
            return null;
        }
    }

    // Use this for initialization
    void Start()
    {
        //joints = new List<RelativeJoint2D>();
        PlayerController playerController = GetComponent<PlayerController>();
        playerController.setOwningPixel(this); 
        playerController.enabled = isControlledByPlayer;
       
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Stick to nearby pixels or get destroyed by laser
    /// </summary>
    /// <param name="coll"></param>
    void OnCollisionEnter2D(Collision2D coll)
    {
        PixelCollisionHandler otherPixel = coll.gameObject.GetComponent<PixelCollisionHandler>();

        if ((otherPixel != null) && 
            (string.Equals(this.gameObject.tag, StickyTag, System.StringComparison.InvariantCultureIgnoreCase) ||
            string.Equals(otherPixel.tag, StickyTag, System.StringComparison.InvariantCultureIgnoreCase))
            ){
            AddJoint(otherPixel);
        } else if (string.Equals(coll.gameObject.tag, DissolveTag,System.StringComparison.InvariantCultureIgnoreCase) &&
            !string.Equals(this.tag, UnbreakableTag, System.StringComparison.InvariantCultureIgnoreCase))
        {
            Destroy(this);
        }
    }

    /// <summary>
    /// Kill this pixel and all joints connected to it.
    /// </summary>
    void OnDestroy()
    {
        RelativeJoint2D[] l = new RelativeJoint2D[this.joints.Count];
        this.joints.CopyTo(l);
        foreach (RelativeJoint2D dj in l)
        {
            try {
                DestroyJoint(dj, true);
            } catch
            {
                //ignore
            }
        }
    }

    public void addHorizontalForce(float force)
    {
        // This one is easy! apply the force to the current objet
        Rigidbody2D body = GetComponent<Rigidbody2D>();

        body.AddForce(new Vector2(force, 0));       
    }

    public void addVerticalForce(float force)
    {
        // This one is easy! apply the force to the current objet
        Rigidbody2D body = GetComponent<Rigidbody2D>();

        body.AddForce(new Vector2(0, force));
    }

    public void addRotationalForce(float force)
    {
        // Currently only the player pixel can rotate
        if (!isPlayerPixel)
        {
            return;
        }

        // This one is easy! apply the force to the current objet
        Rigidbody2D body = GetComponent<Rigidbody2D>();

        body.AddTorque(force);
    }

}