using UnityEngine;
using System.Collections;


namespace KofTools{
	
	public class EquipJob : Job {
		
		Tool tool;
		
		public EquipJob(Tool t, Vector3 pos):base(pos, JobType.EquipJob){
			tool = t;
		}
		
		public bool workOn(Robot robot){
			
			robot.tools.Add(tool);
			percentageComplete = 200;
			
			if (tool.isConstructionTool){
				robot.addConstructionTool((ConstructionTool)(tool));
				robot.hasConstructionTool = true;
			} else if (tool.isDeconstructionTool){
				robot.addDeconstructionTool((DeconstructionTool)(tool));
				robot.hasDeconstructionTool = true;
			} else if (tool.isMiningTool){
				robot.addMiningTool((MiningTool)(tool));
				robot.hasMiningTool = true;
			}
			bool isDone = base.workOn(1, robot);
			JobController.removeTool(tool);
			return true;
		}
	
		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}
	}
}
