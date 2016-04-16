using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class CollisionHandler : MonoBehaviour
{
    private static int maxConnectors = 4;

    private List<DistanceJoint2D> joints;

    public List<DistanceJoint2D> Joints
    {
        get
        {
            return joints;
        }
    }

    public void AddJoint(CollisionHandler ch)
    {
        //create new joint between both objects and add to internal list on both
        if ((ch != null) && (ch.Joints.Count < maxConnectors) && (this.Joints.Count < maxConnectors))
        {
            DistanceJoint2D dj = gameObject.AddComponent(typeof(DistanceJoint2D)) as DistanceJoint2D;
            dj.connectedBody = ch.GetComponent<Rigidbody2D>();
            this.joints.Add(dj);
            ch.joints.Add(dj);
        }
    }

    public static void DestroyJoint(DistanceJoint2D dj)
    {
        CollisionHandler ch1 = dj.connectedBody.GetComponentInParent(typeof(CollisionHandler)) as CollisionHandler;
        CollisionHandler ch2 = dj.GetComponentInParent(typeof(CollisionHandler)) as CollisionHandler;
    }

    // Use this for initialization
    void Start()
    {
        joints = new List<DistanceJoint2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnCollisionEnter2D(Collision2D coll)
    {
        CollisionHandler otherPixel = coll.gameObject.GetComponent<CollisionHandler>();

        if ((otherPixel != null) && (otherPixel.Joints.Count < maxConnectors) && (this.Joints.Count < maxConnectors)){
            //create new hinge object
        }


    }
}