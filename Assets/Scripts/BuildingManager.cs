using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingManager : MonoBehaviour {

	public GameObject firstBuilding;
	public GameObject buildingObject;
	public GameObject firstDoor;

	public Sprite[] buildingMaterials;
	int numIterations = 1;
	Vector3 initialPos;
	public GameObject Door;

	float DoorXMin;
	float DoorXMax;
	float DoorYOffset;


	public List<GameObject> buildings;

	void Start () {

		initialPos = firstBuilding.transform.position;
		buildings = new List<GameObject>();

		DoorXMin = firstBuilding.GetComponentInChildren<SpriteRenderer>().bounds.min.x + firstDoor.GetComponentInChildren<SpriteRenderer>().bounds.size.x / 2;
		DoorXMax = firstBuilding.GetComponentInChildren<SpriteRenderer>().bounds.max.x - firstDoor.GetComponentInChildren<SpriteRenderer>().bounds.size.x / 2;
	
		DoorYOffset = firstBuilding.transform.position.y - firstDoor.transform.position.y;

		for(int x = 0; x < 50; x++){
			CreateNewBuilding();
		}
	}
	
	// Update is called once per frame
	void Update () {

	}

	public float buildingHeight() {
		SpriteRenderer spRenderer = firstBuilding.GetComponentInChildren<SpriteRenderer> ();
		return spRenderer.bounds.size.y;
	}

	public void CreateNewBuilding(){
		Vector3 newPos = initialPos;
		SpriteRenderer spRenderer = firstBuilding.GetComponentInChildren<SpriteRenderer> ();
		newPos.y -= (numIterations) * spRenderer.bounds.size.y;
		GameObject clone = GameObject.Instantiate (buildingObject, newPos, Quaternion.identity) as GameObject;
		spRenderer = clone.GetComponentInChildren<SpriteRenderer> ();
		spRenderer.sprite = buildingMaterials [(int) Random.Range (0f, buildingMaterials.Length)];

		GameObject prevDoor;
		if (buildings.ToArray ().Length != 0) {
			prevDoor = buildings[buildings.ToArray().Length - 1].GetComponentInChildren<BuildingController>().door;
		}
		else{
			prevDoor = firstDoor;
		}


		Vector2 doorExitPos;
		doorExitPos.x = prevDoor.transform.position.x;
		doorExitPos.y = clone.transform.position.y - DoorYOffset;

		GameObject doorExit = Instantiate(Door, doorExitPos, Quaternion.identity) as GameObject;
		doorExit.GetComponentInChildren<Animator>().SetTrigger("Exit");
		doorExit.tag = "Untagged";
		clone.GetComponentInChildren<BuildingController>().doorExit = doorExit;
		

		Vector2 doorPos;
		bool matchFound = false;
		float doorPosX = 0;
		int tries = 0;
		while (!matchFound) {
			doorPosX = Random.Range (DoorXMin, DoorXMax);
			float doorWidth = firstDoor.GetComponentInChildren<SpriteRenderer>().bounds.size.x;
			Vector3 prevDoorPosMin = prevDoor.transform.position;
			prevDoorPosMin.x = doorPosX - doorWidth;
			if(!Physics2D.Raycast(prevDoorPosMin, Vector2.right, doorWidth * 2, 1 << LayerMask.NameToLayer("Door"))){
				matchFound = true;
			}
		}
		doorPos.x = doorPosX;
		doorPos.y = clone.transform.position.y - DoorYOffset;
		GameObject doorClone = Instantiate (Door, doorPos, Quaternion.identity) as GameObject;
		clone.GetComponentInChildren<BuildingController> ().door = doorClone;
		clone.GetComponentInChildren<BuildingController> ().manager = gameObject.GetComponent<BuildingManager> ();
		clone.GetComponentInChildren<BuildingController> ().buildingNumber = numIterations;
		clone.GetComponentInChildren<BuildingController>().GenerateDooHickeys(buildings.ToArray().Length);

		buildings.Add (clone);
		numIterations++;
	}
}
