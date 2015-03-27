using UnityEngine;
using System.Collections;
using KofTools;

public class BoilerManager : SteamObject {
	
	public bool hasWater;
	public int coalAmount;
	
	bool jobAssigned;
	HaulingJob restock;
	
	bool clicked;
	
	// Use this for initialization
	void Start () {
		ConstructionController.boilers.Add(this);
		hasWater = false;
		coalAmount = 50000;
		jobAssigned = false;
		updatePos();
	}
	
	// Update is called once per frame
	void Update () {
		coalAmount -= 1;
		
		if ((coalAmount < 10000) & !jobAssigned){
			if (coalAmount < 0){
				coalAmount = 0;
			}
			try {
				restock = new HaulingJob((Item)ItemList.coalChunks.Dequeue(), transform.position, true);
				jobAssigned = true;
			} catch {
				jobAssigned = false;
			}
		}
		
		try {
			if (restock.j.complete & jobAssigned){
				coalAmount += 10000;
			}
			jobAssigned = !restock.j.complete;
		} catch {
			jobAssigned = false;
		}
		
		if ((coalAmount % 5 == 0)){
			calculateAdjacentSteams();
			calculateWater();
			if (isCarryingWater){
				isCarryingWater = false;
				hasWater = true;
			}
			
			if (coalAmount > 0 && hasWater){
				pressure = 1000;
			}
		}
		
	}
	
	void OnMouseDown(){
		clicked = true;
	}
	
	void OnGUI(){
		if (clicked){
			GUI.Box(new Rect(10, 10, 180, 110), "Boiler");
			
			if (GUI.Button (new Rect (159, 12, 25, 20), "X")){
				clicked = false;
			}
			
			GUI.Label(new Rect(15, 40, 160, 20), "Steam Production: " + pressure);
			GUI.Label(new Rect(15, 65, 185, 20), "Coal Remaining: " + coalAmount);
			GUI.Label(new Rect(15, 90, 210, 20), "Has water");
			GUI.Toggle(new Rect(78, 92, 210, 20), hasWater, "");
			
		}
	}
	
}
