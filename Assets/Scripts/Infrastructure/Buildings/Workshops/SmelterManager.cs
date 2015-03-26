using UnityEngine;
using System.Collections;
using KofTools;

public class SmelterManager : WorkshopManager {

	// Use this for initialization
	void Start () {
		queue = new ArrayList();
		active = true;
	}
	
	// Update is called once per frame
	void Update () {
		calculatePressure(8.5f);
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
		if (clicked){
			GUI.Box(new Rect(10, 10, 180, 230), "Smelter");
			
			if (queue.Count != 0){	
				GUI.Box(new Rect(10, 245, 180, 23 * queue.Count + 25), "Queue");
				int i = 0;
				
				ArrayList toBeRemoved = new ArrayList();
				
				foreach (Job job in queue){
					
					if (GUI.Button(new Rect(15, 270 + 23 * i, 160, 20), (string) job.jobLabel)){
						toBeRemoved.Add(job);
					}
					i++;
				}
				
				foreach (Job job in toBeRemoved){
					queue.Remove(job);
				}
				
			}
			
			try {
				if (GUI.Button (new Rect (159, 12, 25, 20), "X")){
					clicked = false;
				} else if (GUI.Button (new Rect (20, 40, 160, 20), "Smelt Iron")) {
					queue.Add(new SmeltOre(transform.position, (Ore) (ItemList.ironOre.Dequeue())));
					clicked = false;
				} else if (GUI.Button (new Rect (20, 62, 160, 20), "Smelt Tin")) {
					queue.Add(new SmeltOre(transform.position, (Ore) (ItemList.tinOre.Dequeue())));
					clicked = false;
				} else if (GUI.Button (new Rect (20, 84, 160, 20), "Smelt Aluminum")) {
					queue.Add(new SmeltOre(transform.position, (Ore) (ItemList.aluminumOre.Dequeue())));
					clicked = false;
				} else if (GUI.Button (new Rect (20, 106, 160, 20), "Smelt Copper")) {
					queue.Add(new SmeltOre(transform.position, (Ore) (ItemList.copperOre.Dequeue())));
					clicked = false;
				} else if (GUI.Button (new Rect (20, 128, 160, 20), "Smelt Zinc")) {
					queue.Add(new SmeltOre(transform.position, (Ore) (ItemList.zincOre.Dequeue())));
					clicked = false;
				} else if (GUI.Button (new Rect (20, 150, 160, 20), "Smelt Brass")) {
					if (ItemList.copperBars.Count > 0 & ItemList.zincBars.Count > 0){
						queue.Add(new SmeltBrass(transform.position, (Bar) ItemList.zincBars.Dequeue(), (Bar) (ItemList.copperBars.Dequeue())));
						clicked = false;
					}
				} else if (GUI.Button (new Rect (20, 172, 160, 20), "Smelt Bronze")) {
					if (ItemList.copperBars.Count > 0 & ItemList.tinBars.Count > 0){
						queue.Add(new SmeltBronze(transform.position, (Bar) (ItemList.tinBars.Dequeue()), (Bar) (ItemList.copperBars.Dequeue())));
						clicked = false;
					}
				} else if (GUI.Button (new Rect (20, 194, 160, 20), "Smelt Steel")) {
					if (ItemList.ironBars.Count > 0 & ItemList.coalChunks.Count > 0){
						queue.Add(new SmeltSteel(transform.position, (Bar) ItemList.ironBars.Dequeue(), (Ore) (ItemList.coalChunks.Dequeue())));
						clicked = false;
					}
				} else if (GUI.Button (new Rect (20, 216, 160, 20), "Galvanize Steel")) {
					if (ItemList.steelBars.Count > 0 & ItemList.zincBars.Count > 0){
						queue.Add(new SmeltGalvanizedSteel(transform.position, (Bar) ItemList.zincBars.Dequeue(), (Bar) (ItemList.steelBars.Dequeue())));
						clicked = false;
					}
				} if (queue.Count > 1){
					try {
						JobController.removeJob((Job)queue[queue.Count - 1]);
					} catch {
						
					}
				}
			} catch {
				Debug.Log("No ore");
				clicked = true;
			}
		} else if (showingPressure){
			GUI.Label(new Rect(200, 10, 100, 20), "No steam");
			if (Input.GetMouseButtonDown(0)){
				showingPressure = false;
			}
		}
	}
}
