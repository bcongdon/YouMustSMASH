using UnityEngine;
using System.Collections;

public class ItemHandler : MonoBehaviour {

	public Sprite[] damageStates;
	public GameObject key;
	int damageState = 0;
	public bool containsKey;
	public float yShift;
	public float keyShiftY;
	public float keyShiftX;
	public float spriteWidth;
	public bool spawnedKey = false;

	public AudioClip[] smashClips;
	public AudioClip breakClip;

	// Use this for initialization
	void Start () {
		IterateDamageState ();
		spriteWidth = gameObject.GetComponentInChildren<SpriteRenderer> ().bounds.size.x;
	}
	
	// Update is called once per frame
	void Update () {

	}

	void IterateDamageState(){
		if(damageState < damageStates.Length){
			if(damageState != 0 && damageState != damageStates.Length - 1){
				transform.GetComponentInChildren<AudioSource>().clip = smashClips[Random.Range(0, smashClips.Length)];
				transform.GetComponentInChildren<AudioSource>().Play();
			}
			gameObject.GetComponentInChildren<SpriteRenderer> ().sprite = damageStates [damageState];
			if(Random.Range(0, 8) == 5 && damageState != 0 && containsKey) {
				SpawnKey();
			}
		}
		if (damageState == damageStates.Length - 1) {
			transform.GetComponentInChildren<AudioSource>().clip = breakClip;
			transform.GetComponentInChildren<AudioSource>().Play();
			if(containsKey){
				SpawnKey();
			}
		}
		damageState++;
	}

	void SpawnKey() {
		if(spawnedKey){
			return;
		}
		Vector2 pos = transform.position;
		pos.y += keyShiftY;
		pos.x += keyShiftX;
		Instantiate (key, pos, Quaternion.identity);
		spawnedKey = true;
		AudioManager aManager = GameObject.Find ("Main Camera").GetComponent<AudioManager> ();
		aManager.playSound ("KeyFound");
	}

	public void DoDamage(){
		IterateDamageState ();
	}
	public float getMinX(float buildingMinX){
		return buildingMinX + (spriteWidth);
	}
	public float getMaxX(float buildingMaxX){
		return buildingMaxX - (spriteWidth);
	}
}
