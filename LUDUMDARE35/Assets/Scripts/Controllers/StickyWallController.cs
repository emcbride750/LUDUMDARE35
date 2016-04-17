using UnityEngine;
using System.Collections;

public class StickyWallController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	//We were hit
	void OnCollisionEnter2D(Collision2D coll)
	{
		//What did we hit?
		PixelCollisionHandler pixel = coll.gameObject.GetComponent<PixelCollisionHandler>();

		//Was it a pixel?
		if (pixel)
		{
			//Set it to sticky
			pixel.sticky = true;
			pixel.GetComponent<SpriteRenderer>().color = Color.green;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
