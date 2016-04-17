using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Collider2D))]
public class KnifeController : MonoBehaviour
{

	//The player
	PixelCollisionHandler player;

    // Use this for initialization
    void Start()
    {
        touching = new List<PixelCollisionHandler>();

		//Get the player
		player = GameObject.Find("Player").GetComponent<PixelCollisionHandler>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private List<PixelCollisionHandler> touching;

    void OnTriggerEnter2D(Collider2D coll)
    {
		print(coll.gameObject.name);
        PixelCollisionHandler aPixel = coll.gameObject.GetComponent<PixelCollisionHandler>();
		aPixel.GetComponent<SpriteRenderer>().color = Color.blue;
        if (aPixel != null)
        {
            bool alreadyPresent = false;
            foreach (var p in touching)
            {
                if (aPixel == p)
                {
                    alreadyPresent = true;
                    break;
                }
                else {

					//Are either of them the player?
					if(aPixel==this.player || p == this.player)
					{
						//Do nothing
						return;
					}

                    try {
                        PixelCollisionHandler.DestroyJoint(aPixel, p);
                    } catch
                    {
						print("ROBOEM");//ignore.
                    }
                }
            }
            if (!alreadyPresent)
            {
                touching.Add(aPixel);
            }
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        PixelCollisionHandler aPixel = coll.gameObject.GetComponent<PixelCollisionHandler>();
        if (aPixel != null)
        {
            touching.Remove(aPixel);
			aPixel.GetComponent<SpriteRenderer>().color = Color.white;
		}

    }
}
