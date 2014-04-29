using UnityEngine;
using System.Collections;

public class MenuDisplay : MonoBehaviour {

	public GameObject scoreOnes;
	public GameObject scoreTens;
	public GameObject highOnes;
	public GameObject highTens;
	public GameObject scoreLabel;
	public Sprite[] numbers;

	// Use this for initialization
	void Start () {
		hideScore ();
		displayHighScore( PlayerPrefs.GetInt ("HighScore", 00));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void hide(){
		scoreOnes.GetComponentInChildren<SpriteRenderer> ().enabled = false;
		scoreTens.GetComponentInChildren<SpriteRenderer> ().enabled = false;
		scoreLabel.GetComponentInChildren<SpriteRenderer> ().enabled = false;
		highOnes.GetComponentInChildren<SpriteRenderer> ().enabled = false;
		highTens.GetComponentInChildren<SpriteRenderer> ().enabled = false;
		transform.GetComponentInChildren<SpriteRenderer> ().enabled = false;
	}
	public void show(){
		scoreOnes.GetComponentInChildren<SpriteRenderer> ().enabled = true;
		scoreTens.GetComponentInChildren<SpriteRenderer> ().enabled = true;
		scoreLabel.GetComponentInChildren<SpriteRenderer> ().enabled = true;
		highOnes.GetComponentInChildren<SpriteRenderer> ().enabled = true;
		highTens.GetComponentInChildren<SpriteRenderer> ().enabled = true;
		transform.GetComponentInChildren<SpriteRenderer> ().enabled = true;
	}

	public void showScore(){
		scoreOnes.GetComponentInChildren<SpriteRenderer> ().enabled = true;
		scoreTens.GetComponentInChildren<SpriteRenderer> ().enabled = true;
		scoreLabel.GetComponentInChildren<SpriteRenderer> ().enabled = true;
	}
	public void hideScore(){
		scoreOnes.GetComponentInChildren<SpriteRenderer> ().enabled = false;
		scoreTens.GetComponentInChildren<SpriteRenderer> ().enabled = false;
		scoreLabel.GetComponentInChildren<SpriteRenderer> ().enabled = false;
	}
	public void displayScore(int score){
		int tens = Mathf.FloorToInt(score / 10);
		int ones = score % 10;
		scoreOnes.GetComponentInChildren<SpriteRenderer> ().sprite = numbers [ones];
		scoreTens.GetComponentInChildren<SpriteRenderer> ().sprite = numbers [tens];
	}
	public void displayHighScore(int score){
		int tens = Mathf.FloorToInt(score / 10);
		int ones = score % 10;
		highOnes.GetComponentInChildren<SpriteRenderer> ().sprite = numbers [ones];
		highTens.GetComponentInChildren<SpriteRenderer> ().sprite = numbers [tens];
	}
}
