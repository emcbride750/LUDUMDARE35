using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelPickerController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//Load a level
	public void LoadLevel(string level)
	{
		//Load it
		SceneManager.LoadScene(level);
	}
}
