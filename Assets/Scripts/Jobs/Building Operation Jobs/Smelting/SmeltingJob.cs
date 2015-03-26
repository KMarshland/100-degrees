using UnityEngine;
using System.Collections;


namespace KofTools{
	
	public class SmeltingJob : BuildingOperationJob {
		
		public SmeltingJob(Vector3 pos):base(pos){
		}
		
		public new bool workOn(double workspeed, Robot robot){
			bool isDone =  base.workOn(workspeed, robot);
			return isDone;
		}
		
	}
}
