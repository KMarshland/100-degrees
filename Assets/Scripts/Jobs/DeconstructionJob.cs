using UnityEngine;
using System.Collections;


namespace KofTools{
	
	public class DeconstructionJob : Job {
		
		public DeconstructionJob(Vector3 pos):base(pos, JobType.DeconstructionJob){
			
		}
		
		public new bool workOn(double workspeed, Robot robot){
			bool isDone = base.workOn(workspeed, robot);
			double amount = workspeed * (robot.rateOfDeconstruction);
			percentageComplete += amount;
			
			robot.deconstructionTool.wearPercent += (float)(0.1/robot.deconstructionTool.material.strengthModifier);
			if (robot.deconstructionTool.wearPercent > 100){
				robot.tools.Remove(robot.deconstructionTool);
				GameObject.Destroy(robot.deconstructionTool.item);
				robot.deconstructionTool = null;
				robot.hasDeconstructionTool = false;
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
