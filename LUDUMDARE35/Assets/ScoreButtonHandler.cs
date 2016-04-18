using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ScoreButtonHandler : MonoBehaviour {

	//The score handler
	ScoreController scoreHandler;

	// Use this for initialization
	void Start () {
		//Look for it
		scoreHandler = GameObject.Find("ScoreHandler").GetComponent<ScoreController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void BeatLevel()
	{
		SceneManager.LoadScene("levelSelect");
		/**
		//Can we beat the level?
		if (scoreHandler.canWin)
		{
			//We did it. Go back to level select
			SceneManager.LoadScene("levelSelect");
		}
		**/
	}
}
