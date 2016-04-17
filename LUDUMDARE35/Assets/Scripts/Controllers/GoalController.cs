using UnityEngine;
using System.Collections;

public class GoalController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is csalled once per frame
	void Update () {
	
	}

	//When colliding...
	void OnTriggerEnter(Collision2D coll)
	{
		//Are we colliding with a pixel
		PixelCollisionHandler pixel = coll.gameObject.GetComponent<PixelCollisionHandler>();
		print(pixel);
		//Was there one?
		if (pixel)
		{
			//We need to tell it that it is part of the goal
			pixel.inGoal = true;
		}
	}

	//When leaving...
	void OnTriggerExit(Collision2D coll)
	{
		//Are we colliding with a pixel
		PixelCollisionHandler pixel = coll.gameObject.GetComponent<PixelCollisionHandler>();

		//Was there one?
		if (pixel)
		{
			//We need to tell it that it is part of the goal
			pixel.inGoal = false;
		}
	}
}
