using UnityEngine;
using System.Collections;

public class GoalController : MonoBehaviour {

	//The score handler
	public ScoreHandler scoreHandler;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is csalled once per frame
	void Update () {
	
	}

	//When colliding...
	void OnTriggerEnter2D(Collider2D coll)
	{
		//Are we colliding with a pixel
		PixelCollisionHandler pixel = coll.gameObject.GetComponent<PixelCollisionHandler>();

		//Was there one?
		if (pixel)
		{
			//We need to tell it that it is part of the goal
			pixel.inGoal = true;


			//Recalculate the score
			scoreHandler.currentScore += 1;
			scoreHandler.CalculateScore();
		}
	}

	//When leaving...
	void OnTriggerExit2D(Collider2D coll)
	{
		//Are we colliding with a pixel
		PixelCollisionHandler pixel = coll.gameObject.GetComponent<PixelCollisionHandler>();

		//Was there one?
		if (pixel)
		{
			//We need to tell it that it is part of the goal
			pixel.inGoal = false;

			//Lose a score
			scoreHandler.currentScore -= 1;
		}

		//We can't win if something leaves
		scoreHandler.SetScoreCheck(false);
	}
}
