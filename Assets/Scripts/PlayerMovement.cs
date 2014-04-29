using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlayerMovement : MonoBehaviour {

	public float playerSpeed = 10;
	public float playerAcceleration = 10;
	public float spriteHeight = 0.4f;
	public bool godMode = false;
	public float jumpStrength = 150;
	public float smashDelay = 0.5f;
	public float moveDownDelay = 0.5f;

	public bool jumping;
	public bool leftGround;

	public bool mobile = true;
	bool aboutToMove = false;

	public List<GameObject> currentlyTouchingObjects;

	bool shouldPunch = false;
	bool hasKey = false;
	Animator animator;
	Vector3 defaultScale;
	bool atOpenDoor = false;
	Camera MainCamera;
	AudioManager audioManager;

	public bool facingRight = true;

	// Use this for initialization
	void Start () {
		animator = GetComponentInChildren<Animator> ();
		defaultScale = transform.localScale;
		MainCamera = GameObject.FindGameObjectWithTag ("MainCamera").camera;
		audioManager = MainCamera.GetComponent<AudioManager> ();
	}
	
	// Update is called once per frame
	void Update () {

		//Restrict movement during door animation

		if (Input.GetKeyDown (KeyCode.E) && atOpenDoor && !aboutToMove) {
			EnterDoor();
			atOpenDoor = false;
		}

		if (Input.GetKeyDown (KeyCode.H) && godMode) {
			EnterDoor();
		}

		if (godMode) {
			hasKey = true;
			mobile = true;
		}
		//Input.GetAxis("Mouse ScrollWheel")
		if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && mobile) {
			
			Vector2 vel = rigidbody2D.velocity;
			vel.x *= 0.1f;
			rigidbody2D.velocity = vel;
			Punch();
		}
		else{
			animator.SetBool("Punch", false);
		}

		float speed = rigidbody2D.velocity.x;
		if (speed < 0){
			speed *= -1;
		}

		if(hasKey){
			MainCamera.GetComponent<CameraMovement>().shouldDisplayKey = true;
		}
		else{
			MainCamera.GetComponent<CameraMovement>().shouldDisplayKey = false;
		}

		animator.SetFloat ("speed", speed);
	}

	void EnterDoor(){
		aboutToMove = true;
		atOpenDoor = false;
		currentlyTouchingObjects.Clear ();
		mobile = false;
		rigidbody2D.velocity = new Vector2 ();
		animator.SetTrigger ("DoorWalk");
		Invoke ("MoveDownLevel", moveDownDelay);
	}
	void MoveDownLevel(){
		aboutToMove = false;
		CameraMovement move = MainCamera.GetComponentInChildren<CameraMovement> ();
		move.MoveDownLevel ();
		BuildingManager manager = GameObject.Find ("BuildingManager").GetComponentInChildren<BuildingManager> ();
		Vector2 pos = transform.position;
		pos.y -= manager.buildingHeight ();
		transform.position = pos;
		mobile = true;

	}

	void FixedUpdate(){
		//Left
		if(Input.GetKey(KeyCode.A) && rigidbody2D.velocity.x < playerSpeed && mobile){
			rigidbody2D.AddForce(-Vector2.right * playerAcceleration);
		}
		//Right
		if(Input.GetKey(KeyCode.D) && rigidbody2D.velocity.x < playerSpeed && mobile){
			rigidbody2D.AddForce(Vector2.right * playerAcceleration);
		}

		//Jump
		if(Input.GetKey(KeyCode.W) && isGrounded() && mobile) {
			rigidbody2D.AddForce(Vector2.up * jumpStrength);
			jumping = true;
			animator.SetBool("Jumping", true);
		}


		//Facing right -> left
		if(rigidbody2D.velocity.x < 0){
			Vector3 scale = defaultScale;
			scale.x *= -1;
			transform.localScale = scale;
			if(facingRight) {
				Vector2 pos = transform.position;
				//pos.x += gameObject.GetComponentInChildren<SpriteRenderer>().bounds.size.x/2;
				transform.position = pos;
			}
			facingRight = false;
		}
		//Facing left -> right
		else if (rigidbody2D.velocity.x > 0){
			transform.localScale = defaultScale;

			if(!facingRight) {
				Vector2 pos = transform.position;
				//pos.x -= gameObject.GetComponentInChildren<SpriteRenderer>().bounds.size.x;
				transform.position = pos;
			}
			facingRight = true;
		}
		if (jumping) {
			if(!leftGround && !isGrounded()){
				leftGround = true;
			}
			if(leftGround && isGrounded()){
				leftGround = false;
				jumping = false;
				animator.SetBool("Jumping", false);
			}
		}
	}

	void Punch() {
		animator.SetBool("Punch", true);
		Invoke ("DamageItem", smashDelay);
	}
	void DamageItem(){
		foreach (GameObject item in currentlyTouchingObjects) {
			item.gameObject.GetComponentInChildren<ItemHandler>().DoDamage();
		}
		currentlyTouchingObjects.Clear();
	}

	bool isGrounded(){
		Vector2 myPos = new Vector2 (transform.position.x, transform.position.y);
		Vector2 groundCheckPos = new Vector2 (myPos.x, myPos.y - spriteHeight);
		bool result = Physics2D.Linecast(myPos, groundCheckPos, 1 << LayerMask.NameToLayer("Building"));
		if (result) {
			Debug.DrawLine(new Vector3(myPos.x, myPos.y, 5) , new Vector3(groundCheckPos.x, groundCheckPos.y, 5) , Color.green, 0.1f, false);
		}
		else {
			Debug.DrawLine(new Vector3(myPos.x, myPos.y, 5) , new Vector3(groundCheckPos.x, groundCheckPos.y, 5) , Color.red, 0.1f, false);
		}
		return result;
	}

	void OnTriggerEnter2D(Collider2D collider){
		if(collider.tag.Equals("Door")){
			Animator animator = collider.gameObject.GetComponentInChildren<Animator>();
			if(hasKey && !animator.GetBool("Open")){
				animator.SetTrigger("Open");
				hasKey = false;
				audioManager.playSound("KeyEngage");
				audioManager.playSound("DoorOpen");
				atOpenDoor = true;
			}
			if(animator.GetBool("Open")){
				atOpenDoor = true;
			}
		}
		if (collider.gameObject.tag.Equals ("Key")) {
			Destroy(collider.gameObject);
			hasKey = true;
			audioManager.playSound("KeyPickup");


		}
		if(!currentlyTouchingObjects.Contains(collider.gameObject) && collider.gameObject.tag.Equals("DestroyableItem")){
			currentlyTouchingObjects.Add(collider.gameObject);
		}
	}
	void OnTriggerStay2D(Collider2D collider) {
		if(!currentlyTouchingObjects.Contains(collider.gameObject) && collider.gameObject.tag.Equals("DestroyableItem")){
			currentlyTouchingObjects.Add(collider.gameObject);
		}
		if(collider.tag.Equals("Door")){
			Animator animator = collider.gameObject.GetComponentInChildren<Animator>();
			if(animator.GetBool("Open")){
				atOpenDoor = true;
			}
		}
	}
	void OnTriggerExit2D(Collider2D collider){
		if(collider.tag.Equals("Door")){
			atOpenDoor = false;
		}
		if(currentlyTouchingObjects.Contains(collider.gameObject)){
			currentlyTouchingObjects.Remove(collider.gameObject);
		}
	}
}
