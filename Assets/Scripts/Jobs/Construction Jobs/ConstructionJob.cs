using UnityEngine;
using System.Collections;


namespace KofTools{
	
	public class ConstructionJob : Job {
		
		ArrayList materials;
		bool hauled;
		bool haulingStarted;
		
		HaulingJob h;
		ArrayList haulingjobs;
		
		Vector3 buildingPos;
		
		int type;
		/// 0 = base type
		/// 1 = smelter
		/// 2 = forge
		/// 3 = Assembly line
		/// 4 = boiler
		/// 5 = wall
		
		public ConstructionJob(Vector3 pos, ArrayList nmaterials, int ntype):base(pos, JobType.ConstructionJob){
			type = ntype;
			
			materials = nmaterials;
			hauled = false;
			haulingStarted = false;
			
			try {
				haulingjobs = new ArrayList();
				h = new HaulingJob((Item)materials[0], pos, this, true);
				haulingjobs.Add(h);
				
				for (int i = 1; i < materials.Count; i++){
					haulingjobs.Add(new HaulingJob((Item)materials[i], pos, (Job)haulingjobs[i - 1], true));
				}
				
				h = (HaulingJob)haulingjobs[haulingjobs.Count - 1];
			} catch {
				hauled = true;
			}
			
			buildingPos = pos;
			
			percentageComplete = CommandLineControl.constructionComplete;
			if (type == 6){
				percentageComplete += 50;
			}
			
		}
		
		public new bool workOn(double workspeed, Robot robot){
			bool isDone = base.workOn(workspeed, robot);
			double amount = workspeed * (robot.rateOfConstruction);
			percentageComplete += amount;
			
			try {
				if (!hauled){
					hauled = h.j.complete;
				}
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
			
			try {
				robot.constructionTool.wearPercent += (float)(0.1/robot.constructionTool.material.strengthModifier);
				if (robot.constructionTool.wearPercent > 100){
					robot.tools.Remove(robot.constructionTool);
					GameObject.Destroy(robot.constructionTool.item);
					robot.constructionTool = null;
					robot.hasConstructionTool = false;
				}
			} catch {
				
			}
			
			if (isDone){
				if (type == 1){
					GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/smelterPrefab"));
					obj.transform.position = buildingPos;
				} else if (type == 2){
					GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/ForgingPressPrefab"));
					obj.transform.position = buildingPos;
				} else if (type == 3){
					GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/AssemblyLinePrefab"));
					obj.transform.position = buildingPos;
				} else if (type == 4){
					GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Boiler"));
					obj.transform.position = buildingPos;
				} else if (type == 5){
					GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/WallPrefab"));
					obj.transform.position = buildingPos;
					Material m = (Material)(((Bar)materials[0]).material);
					((WallManager)obj.GetComponent("WallManager")).starterate(m);
				} else if (type == 6){
					GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/StraightPipePrefab"));
					obj.transform.position = buildingPos;
					Pipe p = ((Pipe)obj.GetComponent("Pipe"));
					p.starterate(new Metal(Metal.MetalType.Aluminum));
					p.activated = true;
					foreach (Pipe pi in ConstructionController.pipes){
						try {
							//pi.type = -2;
							//pi.lastType = -2;
						} catch {
							
						}
					}
				} else if (type == 7){
					GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/RefuelingStopPrefab"));
					obj.transform.position = buildingPos;
				} else if (type == 8){
					GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/BlockCutterPrefab"));
					obj.transform.position = buildingPos;
				}
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
