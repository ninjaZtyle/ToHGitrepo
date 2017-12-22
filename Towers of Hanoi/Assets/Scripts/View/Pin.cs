using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//coded by: Bill Palmestedt

//Using the MVC convention, this is one of the "View" parts of the code.
//a small class, the player will drag the rings to and from the pins, who will keep track and provide information to the gameController
public class Pin : MonoBehaviour {

	//declare variables
	private List<Ring> rings = new List<Ring>(); 	//list of rings currently on this pin
	private GameController gc;  					//reference to gameController

	public bool startpin; 							//used for checking winning conditions

	// Use this for initialization
	void Start () {
		if (gc == null) {
			gc = transform.GetComponentInParent<GameController> ();
		}

		//assigning any rings to their starting pin, if any
		for(int i = 0; i < transform.childCount; i++)
			rings.Add(transform.GetChild(i).GetComponent<Ring>()) ;	
	}
	

	/// <summary>
	/// Gets the top ring, if any.
	/// </summary>
	/// <returns>The top ring.</returns>
	public Ring getTopRing(){
		if (rings.Count == 0)
			return null;
		return rings [rings.Count - 1];
	}


	/// <summary>
	/// Removes the ring from this pin.
	/// </summary>
	/// <param name="ring">Ring.</param>
	public void removeRing(Ring ring){
		rings.Remove (ring);
	}


	/// <summary>
	/// Adds the ring to this pin.
	/// </summary>
	/// <param name="ring">Ring.</param>
	public void AddRing(Ring ring){
		rings.Add (ring);	
	}


	/// <summary>
	/// Gets the list of rings for this pin.
	/// </summary>
	/// <returns>The list of rings.</returns>
	public List<Ring> getListOfRings(){
		return rings;
	}
}
