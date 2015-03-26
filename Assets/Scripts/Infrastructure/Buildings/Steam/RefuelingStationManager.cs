using UnityEngine;
using System.Collections;
using KofTools;

public class RefuelingStationManager : WorkshopManager {
	
	bool lastActive;
	
	// Use this for initialization
	void Start () {
		lastActive = false;
		active = false;
	}
	
	public void OnMouseUp(){
		if (!active){
			showingPressure = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		lastActive = active;
		calculatePressure(4.2f);
		if (active != lastActive){
			if (active){
				ConstructionController.rechargingStations.Add(this);
			} else {
				ConstructionController.rechargingStations.Remove(this);
			}
		}
	}
	
	void OnGUI(){
		if (showingPressure){
			GUI.Label(new Rect(200, 10, 100, 20), "No steam");
			if (Input.GetMouseButtonDown(0)){
				showingPressure = false;
			}
		}
	}
}
