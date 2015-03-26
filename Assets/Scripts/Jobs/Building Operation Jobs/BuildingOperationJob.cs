using UnityEngine;
using System.Collections;

namespace KofTools{
	public class BuildingOperationJob : Job {
		
		public BuildingOperationJob(Vector3 pos):base(pos, JobType.BuildingOperationJob){
			isBuildingOperationJob = true;
		}
		
		public new bool workOn(double workspeed, Robot robot){
			bool isDone = base.workOn(workspeed, robot);
			double amount = workspeed;
			percentageComplete += amount;
			return isDone;
		}
	}
}
