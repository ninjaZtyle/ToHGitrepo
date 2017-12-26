using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// coded by: Bill Palmestedt


//Using the MVC convention, this is one of the "View" parts of the code.
//the player interacts with the rings, and the rings will comunicate with the gameController.
//Im using pointerHandlers as the touch interactions are simple drag and drop mechanics, keeping the code simple without touch specific inputs

public class Ring : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	//defining variables
	public int ringSize; 						//size of this ring, used to determine if a move is valid
	public Vector3 lastPos{ get; private set;} 	//lastPos is used to return ring after an invalid move
	public Vector3 pinPos{ get; private set;} 	//pinPos is used to align rings to the pins
	public bool onPin { get; private set;} 		//is the ring on a pin or floating freely

	private Rigidbody2D rb;


	void Start(){
		//setting up references
		if (rb == null) {
			rb = GetComponent<Rigidbody2D> ();
		}

		//set pinPosition to that of starting pin
		pinPos = transform.parent.position;


		//set lastPosition to current pos
		lastPos = transform.position;
	}


	void Update(){

		//clamp position to keep all rings whitin playing area
		Vector3 pos = transform.position;
		pos.x = Mathf.Clamp (pos.x, -7.5f, 7.5f);
		pos.y = Mathf.Clamp (pos.y, -3f, 4f);
		transform.position = pos;
	}


		/// <summary>
		/// Raises the pointer down event.
		/// when a valid ring is pressed, the current position is stored, the ring is made draggable 
		/// and gameConroller is notified that the ring has been pressed
		/// </summary>
		/// <param name="eventData">Event data.</param>
	void IPointerDownHandler.OnPointerDown (PointerEventData eventData){
		if (GetComponentInParent<Pin> ().getTopRing () == this) {
			lastPos = transform.position;
			rb.bodyType = RigidbodyType2D.Dynamic;
			GameController.instance.RingIsPressed (this);
		}

	}


		/// <summary>
		/// Raises the pointer up event.
		/// gameController is notified that the ring has been released
		/// </summary>
		/// <param name="eventData">Event data.</param>
	void IPointerUpHandler.OnPointerUp(PointerEventData eventData){
		GameController.instance.RingIsReleased (this);
	}


		/// <summary>
		/// when the ring enters a new pin, the pin becomes active. and the rings information is updated with the new active pin.
		/// </summary>
		/// <param name="other">Other.</param>
	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Pin"){
			GameController.instance.setActivePin (other.GetComponent<Pin> ());
			pinPos = other.transform.position;
			onPin = true;
		}
	}


		/// <summary>
		/// updates pin iformation when a ring exits a pin
		/// </summary>
		/// <param name="other">Other.</param>
	void OnTriggerExit2D(Collider2D other){
		if(other.tag == "Pin"){
			GameController.instance.setActivePin(null);
			onPin = false;
		}
	}


		/// <summary>
		/// When a ring collides with another ring or the plate, they become stattic to avoid being pulled out of position
		/// </summary>
		/// <param name="other">Other.</param>
	void OnCollisionEnter2D (Collision2D other){
		if ((other.gameObject.tag== "Ring" || other.gameObject.tag== "Plate") && GameController.instance.getActiveRing() != this){
			rb.bodyType = RigidbodyType2D.Static;
		}
	}
		

		/// <summary>
		/// Makes the ring dynamic to make it fall by gravity.
		/// </summary>
	public void MakeDynamic(){
		rb.bodyType = RigidbodyType2D.Dynamic;
	}
}
