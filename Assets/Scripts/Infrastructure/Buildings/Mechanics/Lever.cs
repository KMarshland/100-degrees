using UnityEngine;
using System.Collections;
using KofTools;

public class Lever : TriggerObject {
	
	public bool pulled;
	
	int linkingTo;
	
	// Use this for initialization
	new void Start () {
		linkingTo = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnMouseUp(){
		showingGUI = true;
	}
	
	void OnGUI(){
		if (showingGUI){
			GUI.Box(new Rect(10, 10, 180, 85), "Lever");
			if (GUI.Button (new Rect (159, 12, 25, 20), "X")){
				showingGUI = false;
			} else if (GUI.Button (new Rect (15, 40, 160, 20), "Pull")){
				new PullLeverJob(transform.position, this);
				showingGUI = false;
			} else if (GUI.Button (new Rect (15, 65, 160, 20), "Link")){
				bool b = this.linkTo((Door)ConstructionController.doors[linkingTo]);
				while (!b){
					linkingTo ++;
					b = this.linkTo((Door)ConstructionController.doors[linkingTo]);
				}
				showingGUI = false;
			}
			//Debug.Log(objectsToTrigger.Count);
		}
	}
	
	public new void trigger(){
		base.trigger();
		pulled = !pulled;
	}
	
}
