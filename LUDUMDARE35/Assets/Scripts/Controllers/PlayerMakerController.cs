using UnityEngine;
using System.Collections;

public class PlayerMakerController : MonoBehaviour {

	//The pixel prefab!
	public GameObject pixelPrefab;

	//The player
	public GameObject playerPixel;

	//Number of pixes to go from the center (left/right) (up/down)
	public int STARTING_HALF_WIDTH = 50;
	public int STARTING_HALF_HEIGHT = 50;

	//The height and width
	//TODO: GET THIS FROM THE ACTUAL PIXEL CLASS OR SOEMTHING
	public float PIXEL_WIDTH = .01f;
	public float PIXEL_HEIGHT = .01f;

	//The list of all the pixels
	public ArrayList pixelColumns = new ArrayList();

	// Use this for initialization
	void Start () {

		//Make all the pixels
		this.MakePixels();

		//Connect all the pixels
		this.ConnectPixels();
	}

	//Makes the pixels
	void MakePixels()
	{
		//The actual total rows and columns
		int totalColums = STARTING_HALF_WIDTH * 2 + 1;
		int totalRows = STARTING_HALF_HEIGHT * 2 + 1;

		//Where is the player starting??
		Vector3 playerStart = playerPixel.transform.localPosition;

		//The top left X Y
		Vector2 topLeft = new Vector2(playerStart.x - STARTING_HALF_WIDTH * PIXEL_WIDTH, playerStart.y - STARTING_HALF_HEIGHT * PIXEL_WIDTH);

		//The total columns is one plus the starting
		for (int i = 0; i < totalColums; ++i)
		{
			//We want to add a column for this
			ArrayList pixelRow = new ArrayList();
			pixelColumns.Add(pixelRow);

			//For this column, go through each row
			for (int j = 0; j < totalRows; ++j)
			{
				//If we are the middle one, do nothing since that is the player.
				if (i == STARTING_HALF_WIDTH && j == STARTING_HALF_HEIGHT)
				{
					//We're in the middle. This is the player!
					pixelRow.Add(playerPixel);
				}
				else
				{
					//Now we want to fill it with new pixels
					//Make the new pixel
					//TODO: INSTANTIATE A PREFAB DUMMY
					GameObject newPixel = Instantiate(pixelPrefab);

					//Set it's position
					newPixel.transform.localPosition = new Vector3(topLeft.x + i * PIXEL_WIDTH, topLeft.y + j * PIXEL_HEIGHT, 0);

					//Add it to our list
					pixelRow.Add(newPixel);
				}
			}
		}
	}

	//Connect all them pix
	void ConnectPixels()
	{
		//We want to go through all the pixels

		//For each column
		for (int i = 0; i < pixelColumns.Count; ++i)
		{
			//For each row in that column
			for (int j = 0; j < ((ArrayList)pixelColumns[i]).Count; ++j)
			{
				//We're looking at a pixel
				//Try to connect all 4 adjacent ones
				GameObject pixel = (pixelColumns[i] as ArrayList)[j] as GameObject;
				PixelCollisionHandler pixelCollisionHandler = pixel.GetComponent<PixelCollisionHandler>();

				//Get all 4 adjacent ones
				//Left and right
				for (int u = -1; u <= 1; ++u)
				{
					//Above and below
					for (int v = -1; v <= 1; ++v)
					{
						//Try to get that pixel
						try
						{
							//Get it, maybe?
							GameObject adjacentPixel = (pixelColumns[i + u] as ArrayList)[j + v] as GameObject;
							PixelCollisionHandler adjacentPixelCollisionHandler = adjacentPixel.GetComponent<PixelCollisionHandler>();

							//Connect it
							pixelCollisionHandler.AddJoint(adjacentPixelCollisionHandler);
						}
						catch
						{
							//We just pretend that didn't happen
							print("WOOOPDO");
						}
					}
				}
			}
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
