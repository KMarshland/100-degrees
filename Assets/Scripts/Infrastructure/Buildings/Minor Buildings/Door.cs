using UnityEngine;
using System.Collections;
using KofTools;

public class Door : TriggerableObject {

	// Use this for initialization
	void Start () {
		type = 1;
		ConstructionController.doors.Add(this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public new void triggered(){
		base.triggered();
		if (on){
			this.gameObject.SetActive(false);
		} else {
			this.gameObject.SetActive(true);
		}
	}
	
}
