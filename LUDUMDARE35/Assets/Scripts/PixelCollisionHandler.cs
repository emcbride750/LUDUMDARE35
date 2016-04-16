using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody2D))]
public class PixelCollisionHandler : MonoBehaviour
{
    public static string StickyTag = "sticky";
    public static string DissolveTag = "dissolve";
    public static string UnbreakableTag = "unbreakable";

    private static int maxConnectors = 4;
    
	private List<FixedJoint2D> joints = new List<FixedJoint2D>();
    
    public List<FixedJoint2D> Joints
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
            FixedJoint2D dj = gameObject.AddComponent(typeof(FixedJoint2D)) as FixedJoint2D;
            dj.connectedBody = ch.GetComponent<Rigidbody2D>();
            //dj.autoConfigureOffset = false;
            //dj.maxTorque = 900;
            this.joints.Add(dj);
            ch.joints.Add(dj);

            //tell player that we are connected
            GameObject pc = PlayerController.getPlayer();
            if (pc != null)
            {
                PlayerController pco = pc.GetComponent<PlayerController>();
                if (pco.isConnectedTo(ch) || pco.isConnectedTo(this))
                {
                    ExecuteEvents.Execute<IPixelConnectionTarget>(pc, null, (x, y) => x.AddPixel(this));
                    ExecuteEvents.Execute<IPixelConnectionTarget>(pc, null, (x, y) => x.AddPixel(ch));
                }
            }
        } else
        {
			throw new Exception();
        }
    }

    public IEnumerable<PixelCollisionHandler> GetConnectedCollisionHandlers()
    {
        foreach (FixedJoint2D dj in this.joints)
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

    private static void DestroyJoint(FixedJoint2D dj, bool ignoreUnbreakable)
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

                    //TODO: remove pixels on player object
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

    public static void DestroyJoint(FixedJoint2D dj)
    {
        DestroyJoint(dj, false);
    }

    public FixedJoint2D GetJoint(PixelCollisionHandler ch)
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
        FixedJoint2D[] l = new FixedJoint2D[this.joints.Count];
        this.joints.CopyTo(l);
        foreach (FixedJoint2D dj in l)
        {
            try {
                DestroyJoint(dj, true);
            } catch
            {
                //ignore
            }
        }
        //tell player that we are not connected
        GameObject pc = PlayerController.getPlayer();
        if (pc != null)
        {
            ExecuteEvents.Execute<IPixelConnectionTarget>(pc, null, (x, y) => x.RemovePixel(this));
        }
    }



}