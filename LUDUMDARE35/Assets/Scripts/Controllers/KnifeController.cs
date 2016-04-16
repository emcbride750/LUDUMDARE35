using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Collider2D))]
public class KnifeController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        touching = new List<PixelCollisionHandler>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private List<PixelCollisionHandler> touching;

    void OnCollisionEnter2D(Collision2D coll)
    {
        PixelCollisionHandler aPixel = coll.gameObject.GetComponent<PixelCollisionHandler>();
        if (aPixel != null)
        {
            if (!touching.Contains(aPixel))
            {
                foreach (var p in touching)
                {
                    PixelCollisionHandler.DestroyJoint(aPixel.GetJoint(p));
                }
                touching.Add(aPixel);
            }
        }
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        PixelCollisionHandler aPixel = coll.gameObject.GetComponent<PixelCollisionHandler>();
        if (aPixel != null)
        {
            touching.Remove(aPixel);
        }
            
    }
}
