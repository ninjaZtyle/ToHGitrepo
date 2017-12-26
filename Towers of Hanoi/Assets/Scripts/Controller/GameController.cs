using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TowersOfHanoi;

//code by: Bill Palmestedt

//Using the MVC conventon, this is the "Control" part of the code.
//passes information between the model and view components, and handles the UI text
public class GameController : MonoBehaviour {

	public static GameController instance;		//static reference as part of a singleton pattern

	public Game game = new Game (); 					//create an instance of the abstract "model" class Game
	public Text gameText;								// displays messages to the player
	public Pin[] pins { get; private set;} 				//reference to the pins

	// Use this for initialization
	void Start () {

		if(instance == null){							//simple singleton pattern
			instance = this;							//checks if instance already exists
		} else if(instance != this){					//sets it if it does not,
			Destroy (gameObject);						//destroy it if it does
		}



		gameText.enabled = false;						//hide text on startup
		pins = GetComponentsInChildren<Pin> ();			//get all pins in an array

	}


	void Update(){

		//drag the active ring, if any
		if(game.activeRing){
			Vector3 pos = Camera.main.ScreenToWorldPoint (new Vector3(Input.mousePosition.x,Input.mousePosition.y,10));

			//if ring is on a pin, the movement is restricted to up and down the pin
			if(game.activeRing.onPin){
				game.activeRing.transform.position = new Vector3 (game.activeRing.pinPos.x, pos.y, pos.z);
			} else {
				game.activeRing.transform.position = pos;
			}
		}

		//pressing backbutton on device exits the game
		if(Input.GetButtonDown("Cancel")){
			Application.Quit ();
		}
	}
		

		/// <summary>
		/// a Ring is pressed. only the topmost ring on a stack of rings will be valid
		/// </summary>
		/// <param name="ring">Ring.</param>
	public void RingIsPressed(Ring ring){

		//hides any gametext if present
		if (gameText.enabled)
			gameText.enabled = false;

		//selects the ring to make it draggable
		game.setActiveRing (ring);
	}


		/// <summary>
		/// a Ring is released. It will either go on a new pin if valid or return to its previous position
		/// </summary>
		/// <param name="ring">Ring.</param>
	public void RingIsReleased(Ring ring){

		//if not active ring, do nothing
		if (ring != game.activeRing)
			return;
		
		//if the ring is released on a pin, it will attempt to placed on it. if invalid the ring will return and a message is displayed
		if(ring.onPin){
			if (!game.PlaceRing ()) {
				ring.transform.position = ring.lastPos;
				setGameText ("Invalid move");
			}
		} else {
			ring.transform.position = ring.lastPos;
		}

		//resets the active ring
		game.setActiveRing (null);


		//if the ring was placed on a pin, a check for win is made. if winconditions are completed a message will be displayed
		if (game.activePin) {
			if (game.CheckForWin (game.activePin)) {
				game.resetGame (pins);
				setGameText ("Game won!");
			}
		}
	}


		/// <summary>
		/// display the game text.
		/// </summary>
		/// <param name="newText">New text.</param>
	public void setGameText(string newText){
		gameText.text =newText;
		gameText.enabled = true;
	}


		/// <summary>
		/// Sets the active pin.
		/// </summary>
		/// <param name="pin">Pin.</param>
	public void setActivePin(Pin pin){
		game.setActivePin (pin);
	}


		/// <summary>
		/// Gets the active ring.
		/// </summary>
		/// <returns>The active ring.</returns>
	public Ring getActiveRing(){
		return game.activeRing;
	}
}
