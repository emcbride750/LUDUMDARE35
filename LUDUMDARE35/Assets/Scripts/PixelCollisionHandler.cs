using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.EventSystems;


[RequireComponent(typeof(ScoringObject))]
[RequireComponent(typeof(FixedJoint2D))]
public class PixelJoint : MonoBehaviour
{
    public FixedJoint2D joint;

    private void CleanUp()
    {
        if (joint != null)
        {
            var cb1 = joint.connectedBody;
            if (cb1 != null)
            {
                PixelCollisionHandler ch1 = cb1.GetComponentInParent(typeof(PixelCollisionHandler)) as PixelCollisionHandler;
                if (ch1 != null)
                {
                    ch1.removeJoint(this);
                }
            }
            
            PixelCollisionHandler ch2 = joint.GetComponentInParent(typeof(PixelCollisionHandler)) as PixelCollisionHandler;
            if (ch2 != null)
            {
                ch2.removeJoint(this);
            }
        }
    }
    void OnJointBreak(float breakForce)
    {
        Debug.Log("Joint Broke!, force: " + breakForce);
        CleanUp();
        if (joint != null)
        {
            Destroy(joint);
        }
    }

    void OnDestroy()
    {
        CleanUp();
        if (joint != null)
        {
            Destroy(joint);
        }
    }
}
[RequireComponent(typeof(Rigidbody2D))]
public class PixelCollisionHandler : MonoBehaviour
{
	//Is the player?
	public bool isPlayer = false;

    //Is it sticky?
    public bool sticky = false;
	
    public static string StickyTag = "sticky";

    private static int maxConnectors = 40;
    private static float maxSpeed = 100.0f;
    private static float breakForce = 900.0f;

    private List<PixelJoint> joints = new List<PixelJoint>();

    public void removeJoint(PixelJoint j)
    {
        this.joints.Remove(j);
    }

    public List<PixelJoint> Joints
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
            FixedJoint2D fj = gameObject.AddComponent(typeof(FixedJoint2D)) as FixedJoint2D;
            PixelJoint dj = gameObject.AddComponent(typeof(PixelJoint)) as PixelJoint;
            dj.joint = fj;
            dj.joint.connectedBody = ch.GetComponent<Rigidbody2D>();
            //dj.autoConfigureOffset = false;
            //dj.maxTorque = 1000;
            //dj.maxForce = 1000;
            //dj.correctionScale = 1;
            //dj.frequency = 1000000f;
            //dj.dampingRatio = 1;
            dj.joint.autoConfigureConnectedAnchor = false;
            dj.joint.enableCollision = false;
            dj.joint.breakForce = breakForce;
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
            else
            {
                print("no player to add " + this.name + " to.");
            }
        }
        else
        {
            throw new Exception();
        }
    }

    public IEnumerable<PixelCollisionHandler> GetConnectedCollisionHandlers()
    {
        foreach (PixelJoint dj in this.joints)
        {
            PixelCollisionHandler ch1 = dj.joint.connectedBody.GetComponentInParent(typeof(PixelCollisionHandler)) as PixelCollisionHandler;
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

    public static void DestroyJoint(PixelCollisionHandler pixel1, PixelCollisionHandler pixel2)
    {
        //Look for all the joints involving these two
        if ((pixel1 != null) && (pixel2 != null)) 
        {
            foreach (PixelJoint dj in pixel1.joints)
            {
                PixelCollisionHandler ch = dj.joint.connectedBody.GetComponentInParent(typeof(PixelCollisionHandler)) as PixelCollisionHandler;
                if (pixel1 == ch)
                {
                    ch = dj.GetComponentInParent(typeof(PixelCollisionHandler)) as PixelCollisionHandler;
                }
                if (ch == pixel2)
                {
                    Destroy(dj);
                    return;
                }
            }
            foreach (PixelJoint dj in pixel2.joints)
            {
                PixelCollisionHandler ch = dj.joint.connectedBody.GetComponentInParent(typeof(PixelCollisionHandler)) as PixelCollisionHandler;
                if (pixel2 == ch)
                {
                    ch = dj.GetComponentInParent(typeof(PixelCollisionHandler)) as PixelCollisionHandler;
                }
                if (ch == pixel1)
                {
                    Destroy(dj);
                    return;
                }
            }
        }
    }

    public PixelJoint GetJoint(PixelCollisionHandler ch)
    {
        if ((ch != null) && (this != ch))
        {
            foreach (var j in this.joints)
            {
                PixelCollisionHandler ch1 = j.joint.connectedBody.GetComponentInParent(typeof(PixelCollisionHandler)) as PixelCollisionHandler;
                PixelCollisionHandler ch2 = j.GetComponentInParent(typeof(PixelCollisionHandler)) as PixelCollisionHandler;
                if ((ch == ch1) || (ch == ch2))
                {
                    return j;
                }
            }
            return null;
        }
        else
        {
            return null;
        }
    }

    // Use this for initialization
    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //enforce maximum velocity on pixels.
        var rb = this.GetComponent<Rigidbody2D>();
        if (rb.velocity.magnitude > maxSpeed)
        {
            if (rb.velocity.magnitude > maxSpeed * 5)
            {
                Destroy(this);
            }
            else {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }
        }

		//Set our color if we are in the goal
		Color targetColor = Color.white;
        ScoringObject sc = GetComponent<ScoringObject>();
        if (sc != null)
        {
            if (sc.State == ScoringObject.goalState.INSIDE)
            {
                //We want to be gold
                targetColor = Color.yellow;
            } else if (sc.State == ScoringObject.goalState.DOORFRAME)
            {
                //We want to be gold
                targetColor = Color.magenta;
            }
        }
		

		//Is it the player?
		if (this.isPlayer)
		{
			//Its red
			targetColor = Color.red;
		}

		//Is it sticky?
		if (this.sticky)
		{
			//We are gren
			targetColor = Color.green;
		}

		//Set the color
		this.gameObject.GetComponent<SpriteRenderer>().color = targetColor;
    }

    /// <summary>
    /// Stick to nearby pixels or get destroyed by laser
    /// </summary>
    /// <param name="coll"></param>
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll != null)
        {
            GameObject g = coll.gameObject;
            if (g != null)
            {
                PixelCollisionHandler otherPixel = g.GetComponent<PixelCollisionHandler>();

                if ((otherPixel != null) &&
                    (this.sticky ||
                    otherPixel.sticky
                    ))
                {
                    AddJoint(otherPixel);
                }
            }

        }
    }

    /// <summary>
    /// Kill this pixel and all joints connected to it.
    /// </summary>
    void OnDestroy()
    {
        PixelJoint[] l = new PixelJoint[this.joints.Count];
        this.joints.CopyTo(l);
        foreach (PixelJoint dj in l)
        {
            if (dj != null)
            {
                Destroy(dj);
            }
        }
        //tell player that we are not connected
        GameObject pc = PlayerController.getPlayer();
        if (pc != null)
        {
            ExecuteEvents.Execute<IPixelConnectionTarget>(pc, null, (x, y) => x.RemovePixel(this));
        }
        else
        {
            //print("no player to remove " + this.name + " from.");
        }
    }
}