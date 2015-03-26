using UnityEngine;
using System.Collections;

namespace KofTools{
	public class CutBlockJob : BuildingOperationJob {
		
		Boulder boulder;
		Material mat;
		
		bool hauled;
		bool haulingStarted;
		
		HaulingJob h;
		
		public CutBlockJob(Boulder nboulder, Vector3 pos):base(pos){
			boulder = nboulder;
			mat = (Material)boulder.material;
			
			jobType = JobType.CutBlockJob;
			
			hauled = false;
			haulingStarted = false;
			
			h = new HaulingJob(boulder, pos, this, true);
			
			jobLabel = "Cut block";
			
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
				Block item;
				GameObject obj = (GameObject)GameObject.Instantiate(UnityEngine.Resources.Load("Prefabs/BlockPrefab"));
				item = ((Block)obj.GetComponent("Block"));
				item.starterate(mat);
				obj.transform.position = new Vector3(Position.x + 5, Position.y + 5, Position.z);
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
