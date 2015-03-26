using UnityEngine;
using System.Collections;

namespace KofTools{
	public class SmeltSteel : SmeltAlloy {
		
		Bar ironBar;
		Ore coal;
		
		bool hauled;
		bool haulingStarted;
		
		HaulingJob ironHauling;
		HaulingJob coalHauling;
		
		public SmeltSteel(Vector3 pos, Bar nironBar, Ore ncoal):base(pos){
			jobType = JobType.SmeltSteel;
			
			ironBar = nironBar;
			coal = ncoal;
			
			hauled = false;
			haulingStarted = false;
			
			ironHauling = new HaulingJob(ironBar, pos, this, true);
			coalHauling = new HaulingJob(coal, pos, ironHauling, true);
			
			jobLabel = "Smelt Steel";
		}
		
		public new bool workOn(double workspeed, Robot robot){
			bool isDone = false;
			try {
				hauled = ironHauling.j.complete;
			} catch {
				hauled = false;
			}
			if (hauled){
				isDone =  base.workOn(workspeed, robot);
			} else {
				if (!haulingStarted){
					robot.assignJob(coalHauling, true);
					haulingStarted = true;
					Debug.Log ("Sure thing");
				}
			}
			if (isDone){
				GameObject.Destroy(ironBar.item);
				GameObject.Destroy(coal.item);
				GameObject obj = (GameObject)GameObject.Instantiate(UnityEngine.Resources.Load("Prefabs/BarPrefab"));
				obj.transform.position = new Vector3(0f, 0f, 20f);
				Bar b = (Bar)obj.GetComponent("Bar");
				b.starterate(new Metal(Metal.MetalType.Steel));
				
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
