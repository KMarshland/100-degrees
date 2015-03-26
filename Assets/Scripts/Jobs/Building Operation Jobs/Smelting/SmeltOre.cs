using UnityEngine;
using System.Collections;

namespace KofTools{
	public class SmeltOre : SmeltingJob {
		
		Ore ore;
		bool hauled;
		bool haulingStarted;
		
		HaulingJob h;
		
		public SmeltOre(Vector3 pos, Ore nore):base(pos){
			ore = nore;
			jobType = JobType.SmeltOre;
			hauled = false;
			haulingStarted = false;
			h = new HaulingJob(ore, pos, this, true);
			
			Material material = ore.material;
				if (material.materialType == Material.MaterialType.Bauxite){
					jobLabel = "Smelt Aluminum Ore";
				} else if (material.materialType == Material.MaterialType.Malachite){
					jobLabel = "Smelt Copper Ore";
				} else if (material.materialType == Material.MaterialType.Limonite){
					jobLabel = "Smelt Iron Ore";
				} else if (material.materialType == Material.MaterialType.Cassisterite){
					jobLabel = "Smelt Tin Ore";
				} else if (material.materialType == Material.MaterialType.Spheralite){
					jobLabel = "Smelt Zinc Ore";
				} else {
					Debug.LogError (material.GetType());
				}
		}
		
		public new bool workOn(double workspeed, Robot robot){
			bool isDone = false;
			try {
				hauled = h.j.complete;
			} catch {
				hauled = false;
			}
			if (hauled){
				isDone =  base.workOn(workspeed, robot);
			} else {
				if (!haulingStarted){
					robot.assignJob(h, true);
					haulingStarted = true;
				}
			}
			if (isDone){
				
				GameObject obj = (GameObject)GameObject.Instantiate(UnityEngine.Resources.Load("Prefabs/BarPrefab"));
				obj.transform.position = new Vector3(0f, 0f, 20f);
				
				Bar b = (Bar)obj.GetComponent("Bar");
				b.starterate(ore.material);
				
				GameObject.Destroy(ore.item);
				
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
