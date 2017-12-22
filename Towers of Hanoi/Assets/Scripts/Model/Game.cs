using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// coded by: Bill Palmestedt

//using the MVC convention, this class is the "model" part of the code. 
//It does not inherit from MonoBehaviour as to make it more abstract and possibly even reusable outside of unity.
//as game is a fairly common name for a class, I'm putting it in this namespace to seperate it from other game classes
namespace TowersOfHanoi {
	
	public class Game {

		//define class variables as properties
		public Ring activeRing {get; private set;}
		public Pin activePin {get; private set;}

			/// <summary>
			/// Initializes a new instance of the "TowersOfHanoi.Game" class.
			/// </summary>
		public Game(){
			activeRing = null;
			activePin = null;
		}


			/// <summary>
			/// Sets the active ring.
			/// </summary>
			/// <param name="ring">Ring.</param>
		public void setActiveRing(Ring ring){
			activeRing = ring;
		}
			

			/// <summary>
			/// Sets the active pin.
			/// </summary>
			/// <param name="pin">Pin.</param>
		public void setActivePin(Pin pin){
			activePin = pin;
		}


			/// <summary>
			/// Places the ring.
			/// </summary>
			/// <returns><c>true</c>, if ring was placed, <c>false</c> otherwise.</returns>
		public bool PlaceRing(){
			if (activeRing != null && activePin != null) {
				
				//if ring is valid, updates the pins with information about current ring, 
				//and aligns the active ring to be placed properly on the pin
				if (RingIsSmaller ()) {

					activeRing.transform.GetComponentInParent<Pin> ().removeRing (activeRing); 	//remove ring from old pin list of rings
					activeRing.transform.SetParent (activePin.transform); 						//update the hierarcy
					activeRing.transform.position = AlignRing();								//align the active ring with pin
					activePin.AddRing (activeRing); 
					activeRing = null;

					return true;			
				}
			}
				
			return false;
		}


		/// <summary>
		/// Aligns the activeRing to ActivePin.
		/// when released, the ring will "drop" from its current height if not obstructed by another ring, 
		/// in which case it will be dropped from the top of the pin instead.
		/// </summary>
		/// <returns>The aligned position for the active ring.</returns>
		private Vector3 AlignRing(){
			
			Vector3 alignedPosition;		//the position that will be returned
			Vector3 topRingPos;				//the position of the current top ring on the pin, if any


			//if there is no ring on this pin, the top position will be same as the bottom of the pin.
			if (activePin.getTopRing ()) {
				topRingPos = activePin.getTopRing ().transform.position;
			} else {
				topRingPos = new Vector3 (activePin.transform.position.x, activePin.transform.position.y - 2.3f, 0);
			}

			//if the ring is above the top ring, it will drop from its current heigth
			if(activeRing.transform.position.y > topRingPos.y+1.2f){
				alignedPosition = new Vector3 (activePin.transform.position.x, activeRing.transform.position.y, 0);
			} else {
				alignedPosition = new Vector3 (activePin.transform.position.x, activePin.transform.position.y + 2, 0);
			}

			return alignedPosition;
		}

			/// <summary>
			/// Compares if ring is small enough to be placed
			/// </summary>
			/// <returns><c>true</c>, if ring is smaller, <c>false</c> otherwise.</returns>
		public bool RingIsSmaller(){
			
			int topRingSize = activePin.getTopRing () == null ? 0 : activePin.getTopRing ().ringSize;

			if (activeRing.ringSize < topRingSize || topRingSize == 0)
				return true;
			else
				return false;
		}


			/// <summary>
			/// Checks for winning move.
			/// </summary>
			/// <returns><c>true</c>, if winning moe was made, <c>false</c> otherwise.</returns>
			/// <param name="pin">Pin.</param>
		public bool CheckForWin(Pin pin){
			if (!pin.startpin && pin.transform.childCount == 4 ) {
				return true;
			}
			return false;
		}


			/// <summary>
			/// Resets the pins for next game.
			/// </summary>
			/// <param name="pins">Pins.</param>
		public void resetGame(Pin[] pins){
			foreach(Pin p in pins){
				if(p.getListOfRings().Count != 0)
					p.startpin = true;
				else p.startpin = false;
			}
			
		}
	}

}
 