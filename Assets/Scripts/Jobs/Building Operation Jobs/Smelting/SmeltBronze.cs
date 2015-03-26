using UnityEngine;
using System.Collections;

namespace KofTools{
	public class SmeltBronze : SmeltAlloy {
		
		Bar copperBar;
		Bar tinBar;
		
		bool hauled;
		bool haulingStarted;
		
		HaulingJob copperHauling;
		HaulingJob tinHauling;
		
		public SmeltBronze(Vector3 pos, Bar ntinBar, Bar ncopperBar):base(pos){
			
			jobType = JobType.SmeltBronze;
			
			copperBar = ncopperBar;
			tinBar = ntinBar;
			
			hauled = false;
			haulingStarted = false;
			
			copperHauling = new HaulingJob(copperBar, pos, this, true);
			tinHauling = new HaulingJob(tinBar, pos, copperHauling, true);
			
			jobLabel = "Smelt Bronze";
			
		}
		
		public new bool workOn(double workspeed, Robot robot){
			bool isDone = false;
			try {
				hauled = copperHauling.j.complete;
			} catch {
				hauled = false;
			}
			if (hauled){
				isDone =  base.workOn(workspeed, robot);
			} else {
				if (!haulingStarted){
					robot.assignJob(tinHauling, true);
					haulingStarted = true;
					Debug.Log ("Sure thing");
				}
			}
			if (isDone){
				GameObject.Destroy(copperBar.item);
				GameObject.Destroy(tinBar.item);
				GameObject obj = (GameObject)GameObject.Instantiate(UnityEngine.Resources.Load("Prefabs/BarPrefab"));
				obj.transform.position = new Vector3(0f, 0f, 20f);
				Bar b = (Bar)obj.GetComponent("Bar");
				b.starterate(new Metal(Metal.MetalType.Bronze));
				
			}
			return isDone;
		}
		
		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}
	}
}
