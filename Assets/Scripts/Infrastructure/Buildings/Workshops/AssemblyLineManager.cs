using UnityEngine;
using System.Collections;
using KofTools;

public class AssemblyLineManager : WorkshopManager {

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
					queue.Remove(j);
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
			GUI.Box(new Rect(10, 10, 180, 60), "Assembly Line");
			
			ArrayList toBeRemoved = new ArrayList();
			
			if (queue.Count != 0){	
				GUI.Box(new Rect(10, 75, 180, 23 * queue.Count + 25), "Queue");
				int i = 0;
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
				} else if (GUI.Button (new Rect (15, 40, 160, 20), "Assemble Robot")){
					
					if (ItemList.arms.Count > 0 & ItemList.legs.Count > 0 & ItemList.heads.Count > 0 & ItemList.torsos.Count > 0 & ItemList.tanks.Count > 0){
						clicked = false;
						queue.Add(new AssembleRobot(transform.position, (Arms)ItemList.arms.Dequeue(), (Legs)ItemList.legs.Dequeue(), 
							(Head)ItemList.heads.Dequeue(), (Torso)ItemList.torsos.Dequeue(), (Tank)ItemList.tanks.Dequeue()));
					}
					if (queue.Count > 1){
						JobController.removeJob((Job)queue[queue.Count - 1]);
					}
				}
			} catch {
				Debug.Log("No components");
			}
		} else if (showingPressure){
			GUI.Label(new Rect(200, 10, 100, 20), "No steam");
			if (Input.GetMouseButtonDown(0)){
				showingPressure = false;
			}
		}
	}
	
}
