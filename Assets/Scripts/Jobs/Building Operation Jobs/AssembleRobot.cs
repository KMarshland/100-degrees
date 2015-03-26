using UnityEngine;
using System.Collections;

namespace KofTools{
	public class AssembleRobot : BuildingOperationJob {
		
		RobotComponent arms, legs, head, torso, tank;
		HaulingJob[] jobs;
		
		bool hauled;
		bool haulingStarted;
		
		public AssembleRobot(Vector3 pos, Arms narms, Legs nlegs, Head nhead, Torso ntorso, Tank ntank):base(pos){
			arms = narms;
			legs = nlegs;
			torso = ntorso;
			head = nhead;
			tank = ntank;
			
			jobType = JobType.AssembleRobot;
			
			hauled = false;
			haulingStarted = false;
			
			jobs = new HaulingJob[5];
			
			jobs[0] = new HaulingJob(arms, Position, this, true);
			jobs[1] = new HaulingJob(legs, Position, jobs[0], true);
			jobs[2] = new HaulingJob(torso, Position, jobs[1], true);
			jobs[3] = new HaulingJob(head, Position, jobs[2],true);
			jobs[4] = new HaulingJob(tank, Position, jobs[3], true);
			
			jobLabel = "Assemble Robot";
			
		}
		
		public new bool workOn(double workspeed, Robot robot){
			bool isDone = false;
			
			try {
				hauled = jobs[0].j.complete;
			} catch {
				hauled = false;
			}
			
			if (hauled){
				isDone = base.workOn(workspeed, robot);
			} else {
				if (!haulingStarted){
					haulingStarted = true;
					robot.assignJob(jobs[4], true);
				}
			}
			
			if (isDone){
				GameObject.DestroyObject(arms.item);
				GameObject.DestroyObject(legs.item);
				GameObject.DestroyObject(head.item);
				GameObject.DestroyObject(torso.item);
				GameObject.DestroyObject(tank.item);
				
				GameObject obj = (GameObject)GameObject.Instantiate(UnityEngine.Resources.Load("Prefabs/RobotPrefab2Tools"));
				obj.transform.position = Position;
				Robot r = (Robot)obj.GetComponent("Robot");
				r.starterate((Arms)arms, (Legs)legs, (Head)head, (Torso)torso, (Tank)tank);
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
