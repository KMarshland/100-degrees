using UnityEngine;
using System.Collections;

namespace KofTools{
	public class ForgeRobotComponents : BuildingOperationJob {
		
		Bar bar;
		Metal material;
		int ftype; 
		/// 0 = head
		/// 1 = arms
		/// 2 = legs
		/// 3 = torso
		/// 4 = tank
		/// 5 = mining tool
		/// 6 = construction tool
		/// 7 = deconstruction tool
		
		bool hauled;
		bool haulingStarted;
		
		HaulingJob h;
		
		public ForgeRobotComponents(Bar nbar, Vector3 pos, int nftype):base(pos){
			bar = nbar;
			material = (Metal)bar.getMaterial();
			
			ftype = nftype;
			if (ftype < 0 | ftype > 7){
				ftype = 0;
			}
			
			jobType = JobType.ForgeRobotComponents;
			
			hauled = false;
			haulingStarted = false;
			
			h = new HaulingJob(bar, pos, this, true);
			
			if (ftype == 0){
				jobLabel = "Forge Head";
			} else if (ftype == 1){
				jobLabel = "Forge Arms";
			} else if (ftype == 2){
				jobLabel = "Forge Legs";
			} else if (ftype == 3){
				jobLabel = "Forge Torso";
			} else if (ftype == 4){
				jobLabel = "Forge Tank";
			} else if (ftype == 5){
				jobLabel = "Forge Mining Tool";
			} else if (ftype == 6){
				jobLabel = "Forge Construction Tool";
			} else if (ftype == 7){
				jobLabel = "Forge Deconstruction Tool";
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
				
				GameObject.Destroy(bar.item);
				
				if (ftype == 0){
					Head item;
					GameObject obj = (GameObject)GameObject.Instantiate(UnityEngine.Resources.Load("Prefabs/HeadPrefab"));
					item = (Head)obj.GetComponent("Head");
					item.starterate(material);
					obj.transform.position = new Vector3(Position.x, Position.y + 5, Position.z + 5);
				} else if (ftype == 1){
					Arms item;
					GameObject obj = (GameObject)GameObject.Instantiate(UnityEngine.Resources.Load("Prefabs/ArmsPrefab"));
					item = ((Arms)obj.GetComponent("Arms"));
					item.starterate(material);
					obj.transform.position = new Vector3(Position.x, Position.y + 5, Position.z + 5);
				} else if (ftype == 2){
					Legs item;
					GameObject obj = (GameObject)GameObject.Instantiate(UnityEngine.Resources.Load("Prefabs/LegsPrefab"));
					item = ((Legs)obj.GetComponent("Legs"));
					item.starterate(material);
					obj.transform.position = new Vector3(Position.x, Position.y + 5, Position.z + 5);
				} else if (ftype == 3){
					Torso item;
					GameObject obj = (GameObject)GameObject.Instantiate(UnityEngine.Resources.Load("Prefabs/TorsoPrefab"));
					item = ((Torso)obj.GetComponent("Torso"));
					item.starterate(material);
					obj.transform.position = new Vector3(Position.x, Position.y + 5, Position.z + 5);
				} else if (ftype == 4){
					Tank item;
					GameObject obj = (GameObject)GameObject.Instantiate(UnityEngine.Resources.Load("Prefabs/TankPrefab"));
					item = ((Tank)obj.GetComponent("Tank"));
					item.starterate(material);
					obj.transform.position = new Vector3(Position.x + 5, Position.y + 5, Position.z);
				} else if (ftype == 5){
					MiningTool item;
					GameObject obj = (GameObject)GameObject.Instantiate(UnityEngine.Resources.Load("Prefabs/MiningToolPrefab"));
					item = ((MiningTool)obj.GetComponent("MiningTool"));
					item.starterate(material);
					obj.transform.position = new Vector3(Position.x + 10, Position.y + 5, Position.z);
				} else if (ftype == 6){
					ConstructionTool item;
					GameObject obj = (GameObject)GameObject.Instantiate(UnityEngine.Resources.Load("Prefabs/ConstructionToolPrefab"));
					item = ((ConstructionTool)obj.GetComponent("ConstructionTool"));
					item.starterate(material);
					obj.transform.position = new Vector3(Position.x, Position.y + 5, Position.z + 5);
				} else if (ftype == 7){
					DeconstructionTool item;
					GameObject obj = (GameObject)GameObject.Instantiate(UnityEngine.Resources.Load("Prefabs/DeconstructionToolPrefab"));
					item = ((DeconstructionTool)obj.GetComponent("DeconstructionTool"));
					item.starterate(material);
					obj.transform.position = new Vector3(Position.x, Position.y + 5, Position.z + 5);
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
