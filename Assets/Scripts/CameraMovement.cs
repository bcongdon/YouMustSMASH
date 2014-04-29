using UnityEngine;
using System.Collections;
public class CameraMovement : MonoBehaviour {

	Vector2 targetLocation;
	public float cameraIncriment = 0.1f;
	public Sprite keySprite;
	public Sprite traySprite;
	public bool shouldDisplayKey;

	public float trayLocationX;
	public float trayLocationY;
	public float traySize;

	public float keyLocationX;
	public float keyLocationY;
	public float keySize;

	bool cameraMoved = false;
	bool gameStarted = false;
	float gameTime = 60;
	MenuDisplay menu;

	static bool gameStartedOnce = false;
	public GameObject[] hideAfterFirstTime;

	public GameObject[] GUIElements;

	public Sprite[] numbers;
	public int score = 0;

	bool hasPassedUnder10 = false;

	public bool menuOut = true;
	public GameObject control;

	// Use this for initialization
	void Start () {
		menu = transform.Find ("Menu").GetComponent<MenuDisplay> ();
		targetLocation = transform.position;
		hideGUI ();
		if (gameStartedOnce){
			menu.hide();
			GameObject.Find("Player").GetComponentInChildren<PlayerMovement>().mobile = true;
			menuOut = false;
			showGUI();
			foreach(GameObject gObject in hideAfterFirstTime){
				gObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
			}
		}
		gameStartedOnce = true;
	}

	void hideGUI(){
		foreach(GameObject gObject in GUIElements){
			gObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
		}
	}

	void showGUI(){
		foreach(GameObject gObject in GUIElements){
			gObject.GetComponentInChildren<SpriteRenderer>().enabled = true;
		}
	}

	// Update is called once per frame
	void Update () {
		if(menuOut){
			GameObject.Find("Player").GetComponentInChildren<PlayerMovement>().mobile = false;
		}
		if (menuOut && Input.GetKeyDown(KeyCode.F)) {
			showGUI();
			menu.hide();
			GameObject.Find("Player").GetComponentInChildren<PlayerMovement>().mobile = true;
			menuOut = false;
			transform.GetComponent<AudioManager>().playSound("Start");
		}
		if(gameStarted ){
			gameTime -= Time.deltaTime;
		}
		if(gameTime <= 0){
			menu.show();
			menu.displayScore(score - 1);
			if(score - 1 > PlayerPrefs.GetInt("HighScore", 0)){
				PlayerPrefs.SetInt("HighScore", score - 1);
			}
			menu.displayHighScore(PlayerPrefs.GetInt("HighScore",0));
			hideGUI();
			gameTime = 0;
			GameObject.Find("Player").GetComponentInChildren<PlayerMovement>().mobile = false;
			if(Input.GetKeyDown (KeyCode.F)){
				Application.LoadLevel(Application.loadedLevel);
				transform.GetComponent<AudioManager>().playSound("Start");
			}
		}
		int tens = Mathf.FloorToInt (gameTime / 10);
		int ones = Mathf.FloorToInt (gameTime % 10);
		int tenths = Mathf.FloorToInt ((gameTime * 10) % 10);
		transform.Find ("Tens").GetComponentInChildren<SpriteRenderer> ().sprite = numbers [tens];
		transform.Find ("Ones").GetComponentInChildren<SpriteRenderer> ().sprite = numbers [ones];
		transform.Find ("Tenths").GetComponentInChildren<SpriteRenderer> ().sprite = numbers [tenths];
		if(gameTime <= 10) {
			if(!hasPassedUnder10){
				transform.GetComponent<AudioManager>().playSound("Alert");
				transform.GetComponent<AudioManager>().playSound("Alert");
				transform.GetComponent<AudioManager>().playSound("Alert");
			}
			hasPassedUnder10 = true;
			transform.Find("Tens").GetComponentInChildren<SpriteRenderer>().color = Color.red;
			transform.Find("Ones").GetComponentInChildren<SpriteRenderer>().color = Color.red;
			transform.Find("Tenths").GetComponentInChildren<SpriteRenderer>().color = Color.red;
			transform.Find("Dot").GetComponentInChildren<SpriteRenderer>().color = Color.red;
		}
	}

	void FixedUpdate(){
		if (transform.position.y > targetLocation.y) {
			Vector3 pos = transform.position;
			pos.y -= cameraIncriment;
			transform.position = pos;
		}
	}

	public void MoveDownLevel() {
		if(!gameStarted){
			transform.GetComponent<AudioManager>().playSound("StartTimer");
		}
		gameStarted = true;
		BuildingManager manager = GameObject.Find ("BuildingManager").GetComponentInChildren<BuildingManager> ();
		Vector2 target = targetLocation;
		float test = manager.buildingHeight ();
		target.y -= manager.buildingHeight();
		targetLocation = target;
		manager.CreateNewBuilding ();
		cameraMoved = true;
		if(score > 0){
			manager.buildings [score - 1].transform.Find ("Number1").GetComponent<SpriteRenderer> ().color = Color.green;
			manager.buildings [score - 1].transform.Find ("Number2").GetComponent<SpriteRenderer> ().color = Color.green;
		}
		score++;
	}

	void OnGUI(){
		GUIStyle style = new GUIStyle();
		style.normal.textColor = Color.black;
		GameObject key = GameObject.Find ("IndicatorKey");
		if(shouldDisplayKey){
			key.GetComponentInChildren<SpriteRenderer>().sortingLayerName = "GUI";
		}
		else{
			key.GetComponentInChildren<SpriteRenderer>().sortingLayerName = "Default";
		}
	
	}

}
