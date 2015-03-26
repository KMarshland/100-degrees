using UnityEngine;
using System.Collections;

namespace KofTools{
	public class SmeltBrass : SmeltAlloy {
		
		Bar copperBar;
		Bar zincBar;
		
		bool hauled;
		bool haulingStarted;
		
		HaulingJob copperHauling;
		HaulingJob zincHauling;
		
		public SmeltBrass(Vector3 pos, Bar nzincBar, Bar ncopperBar):base(pos){
			
			jobType = JobType.SmeltBrass;
			
			copperBar = ncopperBar;
			zincBar = nzincBar;
			
			hauled = false;
			haulingStarted = false;
			
			copperHauling = new HaulingJob(copperBar, pos, this, true);
			zincHauling = new HaulingJob(zincBar, pos, copperHauling, true);
			
			jobLabel = "Smelt Brass";
			
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
					robot.assignJob(zincHauling, true);
					haulingStarted = true;
					Debug.Log ("Sure thing");
				}
			}
			if (isDone){
				GameObject.Destroy(zincBar.item);
				GameObject.Destroy(copperBar.item);
				GameObject obj = (GameObject)GameObject.Instantiate(UnityEngine.Resources.Load("Prefabs/BarPrefab"));
				obj.transform.position = new Vector3(0f, 0f, 20f);
				Bar b = (Bar)obj.GetComponent("Bar");
				b.starterate(new Metal(Metal.MetalType.Brass));
				
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
