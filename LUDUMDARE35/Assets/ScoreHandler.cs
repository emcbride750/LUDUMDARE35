using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreHandler : MonoBehaviour {

	//The player we are scoring
	public PixelCollisionHandler player;

	//The current score
	public int currentScore;

	//Sprites
	public Texture goodSprite;
	public Texture badSprite;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//Update the UI
		GameObject.Find("ScoreText").GetComponent<Text>().text = "Score: " + currentScore;
	}

	//Calculate the score
	public void CalculateScore()
	{
		//If pixels does not equal score, we can't win

		//Set the graphic
		this.SetScoreCheck(ScorePixel(player, new ArrayList()));
	}

	private bool ScorePixel(PixelCollisionHandler pixel, ArrayList handledPixels)
	{

        return false;
	}

	public void SetScoreCheck(bool good)
	{
		//We're bad...
		Texture targetSprite = badSprite;

		//Unless we're good
		if (good)
		{
			targetSprite = goodSprite;
		}
		
		//Set it
		GameObject.Find("ScoreGood").GetComponent<RawImage>().texture = targetSprite;
	}
}
