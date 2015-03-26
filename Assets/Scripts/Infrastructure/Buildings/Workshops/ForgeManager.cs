using UnityEngine;
using System.Collections;
using KofTools;

public class ForgeManager : WorkshopManager {
	
	bool matSelected;
	Bar bar;
	
	// Use this for initialization
	void Start () {
		queue = new ArrayList();
		active = true;
	}
	
	// Update is called once per frame
	void Update () {
		calculatePressure(4.2f);
		if (active){
			if (queue.Count > 0){
				Job j = (Job)queue[0];
				if (j.percentageComplete > 100){
					queue.RemoveAt(0);
					if (queue.Count > 0){
						JobController.addJob((Job)queue[0]);
					}
				}
			}
		}
	}
	
	public void OnMouseUp(){
		if (active){
			queueJob();
		} else {
			showingPressure = true;
		}
	}
	
	public void queueJob(){
		clicked = true;
	}
	
	void OnGUI(){
		if (clicked && !matSelected){
			GUI.Box(new Rect(10, 10, 180, 190), "Forge");
			
			if (queue.Count != 0){
				ArrayList toBeRemoved = new ArrayList();
				GUI.Box(new Rect(10, 205, 180, 23 * queue.Count + 25), "Queue");
				int i = 0;
				foreach (Job job in queue){
					if (GUI.Button(new Rect(15, 230 + 23 * i, 160, 20), (string) job.jobLabel)){
						toBeRemoved.Add(job);
					}
					i++;
				}
				
				foreach (Job job in toBeRemoved){
					queue.Remove(job);
				}
			} else {
				
			}
			
			try {
				if (GUI.Button (new Rect (159, 12, 25, 20), "X")){
					clicked = false;
				} else if (GUI.Button (new Rect (20, 40, 160, 21), "Copper")) {
					matSelected = true;
					bar = (Bar)ItemList.copperBars.Dequeue();
				} else if (GUI.Button (new Rect (20, 62, 160, 21), "Brass")) {
					matSelected = true;
					bar = (Bar)ItemList.brassBars.Dequeue();
				} else if (GUI.Button (new Rect (20, 84, 160, 21), "Bronze")) {
					matSelected = true;
					bar = (Bar)ItemList.bronzeBars.Dequeue();
				} else if (GUI.Button (new Rect (20, 106, 160, 21), "Aluminum")) {
					matSelected = true;
					bar = (Bar)ItemList.aluminumBars.Dequeue();
				} else if (GUI.Button (new Rect (20, 128, 160, 21), "Iron")) {
					matSelected = true;
					bar = (Bar)ItemList.ironBars.Dequeue();
				} else if (GUI.Button (new Rect (20, 150, 160, 21), "Steel")) {
					matSelected = true;
					bar = (Bar)ItemList.steelBars.Dequeue();
				} else if (GUI.Button (new Rect (20, 172, 160, 21), "Galvanized Steel")) {
					matSelected = true;
					bar = (Bar)ItemList.galvanizedSteelBars.Dequeue();
				}
			} catch {
				Debug.Log("No Bars");
				matSelected = false;
			}
		}
		if (matSelected && clicked){
			GUI.Box(new Rect(10, 10, 180, 190), "Forge");
			//try {
				if (GUI.Button (new Rect (159, 12, 25, 20), "X")){
					matSelected = false;
				} else if (GUI.Button (new Rect (20, 40, 160, 21), "Forge head")) {
					clicked = false;
					matSelected = false;
					queue.Add(new ForgeRobotComponents(bar, transform.position, 0));
				} else if (GUI.Button (new Rect (20, 62, 160, 21), "Forge arms")) {
					clicked = false;
					matSelected = false;
					queue.Add(new ForgeRobotComponents(bar, transform.position, 1));
				} else if (GUI.Button (new Rect (20, 84, 160, 21), "Forge legs")) {
					clicked = false;
					matSelected = false;
					queue.Add(new ForgeRobotComponents(bar, transform.position, 2));
				} else if (GUI.Button (new Rect (20, 106, 160, 21), "Forge torso")) {
					clicked = false;
					matSelected = false;
					queue.Add(new ForgeRobotComponents(bar, transform.position, 3));
				} else if (GUI.Button (new Rect (20, 128, 160, 21), "Forge tank")) {
					clicked = false;
					matSelected = false;
					queue.Add(new ForgeRobotComponents(bar, transform.position, 4));
				} else if (GUI.Button (new Rect (20, 150, 160, 21), "Forge mining tool")) {
					clicked = false;
					matSelected = false;
					queue.Add(new ForgeRobotComponents(bar, transform.position, 5));
				} else if (GUI.Button (new Rect (20, 172, 160, 21), "Forge construction tool")) {
					clicked = false;
					matSelected = false;
					queue.Add(new ForgeRobotComponents(bar, transform.position, 6));
				}/* else if (GUI.Button (new Rect (20, 194, 160, 21), "Forge deconstruction tool")) {
					clicked = false;
					matSelected = false;
					queue.Add(new ForgeRobotComponents(bar, transform.position, 7));
				}*/
				if (queue.Count > 1){
					try {
						JobController.removeJob((Job)queue[queue.Count - 1]);
					} catch {
						
					}
				}
			//} catch {
				//Debug.LogError("Bar in use");
			//}
		} else if (showingPressure){
			GUI.Label(new Rect(200, 10, 100, 20), "No steam");
			if (Input.GetMouseButtonDown(0)){
				showingPressure = false;
			}
		}
	}
}
