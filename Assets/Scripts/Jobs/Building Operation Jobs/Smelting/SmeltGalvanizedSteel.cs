using UnityEngine;
using System.Collections;

namespace KofTools{
	public class SmeltGalvanizedSteel : SmeltAlloy {
		
		Bar steelBar;
		Bar zincBar;
		
		bool hauled;
		bool haulingStarted;
		
		HaulingJob steelHauling;
		HaulingJob zincHauling;
		
		public SmeltGalvanizedSteel(Vector3 pos, Bar nzincBar, Bar nsteelBar):base(pos){
			
			jobType = JobType.SmeltGalvanizedSteel;
			
			steelBar = nsteelBar;
			zincBar = nzincBar;
			
			hauled = false;
			haulingStarted = false;
			
			steelHauling = new HaulingJob(steelBar, pos, this, true);
			zincHauling = new HaulingJob(zincBar, pos, steelHauling, true);
			
			jobLabel = "Smelt Galvanized Steel";
			
		}
		
		public new bool workOn(double workspeed, Robot robot){
			bool isDone = false;
			try {
				hauled = steelHauling.j.complete;
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
				GameObject.Destroy(steelBar.item);
				GameObject obj = (GameObject)GameObject.Instantiate(UnityEngine.Resources.Load("Prefabs/BarPrefab"));
				obj.transform.position = new Vector3(0f, 0f, 20f);
				Bar b = (Bar)obj.GetComponent("Bar");
				b.starterate(new Metal(Metal.MetalType.GalvanizedSteel));
				
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
