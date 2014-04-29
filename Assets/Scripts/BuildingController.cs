using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingController : MonoBehaviour {
	
	private List<GameObject> dooHickeys;
	public BuildingManager manager;
	public GameObject[] dooHickeyTypes;
	public GameObject door;
	public GameObject doorExit;
	public int buildingNumber;
	public Sprite[] numbers;


	// Use this for initialization
	void Start () {
		dooHickeys = new List<GameObject> ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void GenerateDooHickeys(int floorLevel){
		dooHickeys = new List<GameObject> ();
		int numToGenerate = 3;
		int floorNumber = manager.buildings.ToArray ().Length;
		if(floorLevel < 3){
			numToGenerate = Random.Range(1,2);
		}
		else if(floorLevel < 10){
			numToGenerate = Random.Range(3,6);
		}
		else {
			numToGenerate = Random.Range(5,10);
		}
		for(int x = 0; x < numToGenerate; x++){
			GameObject item = dooHickeyTypes[Random.Range(0,dooHickeyTypes.Length)];
			GameObject itemInstance = Instantiate(item, new Vector3(100,100,0), Quaternion.identity) as GameObject;
			bool matchFound = false;
			float itemX = 0;
			ItemHandler handler = itemInstance.GetComponent<ItemHandler>();
			int tries = 0;
			while(!matchFound && tries < 1000){
				float minX = handler.getMinX(manager.firstBuilding.GetComponentInChildren<SpriteRenderer>().bounds.min.x) + 0.3f;
				float maxX = handler.getMaxX(manager.firstBuilding.GetComponentInChildren<SpriteRenderer>().bounds.max.x) - 0.3f;
				itemX = Random.Range(minX, maxX);
				Vector2 pos = transform.position;
				pos.x = itemX - handler.spriteWidth /2;
				pos.y -= handler.yShift;
				if(Physics2D.Raycast(pos, Vector2.right, handler.spriteWidth, 1 << LayerMask.NameToLayer("Door"))){
					continue;
				}
				if(!Physics2D.Raycast(pos, Vector2.right, handler.spriteWidth, 1 << LayerMask.NameToLayer("DooHickey"))){
					matchFound = true;
					Vector2 pos2 = pos;
					pos2.x += handler.spriteWidth;
					//Debug.DrawLine(pos, pos2, Color.red, 100);
				}
				tries ++;
			}
			Vector3 newPos = gameObject.transform.position;
			newPos.y -= handler.yShift;
			newPos.x = itemX;
			itemInstance.transform.position = newPos;
			dooHickeys.Add(itemInstance);
		}
		GameObject keyHolder = dooHickeys [Random.Range (0, dooHickeys.ToArray().Length - 1)];
		keyHolder.GetComponent<ItemHandler>().containsKey = true;

		int number1 = Mathf.FloorToInt(buildingNumber / 10);
		int number2 = buildingNumber % 10;
		transform.Find("Number1").GetComponent<SpriteRenderer>().sprite = numbers[number1];
		transform.Find("Number2").GetComponent<SpriteRenderer>().sprite = numbers[number2];
	}
}
