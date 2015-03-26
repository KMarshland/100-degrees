using UnityEngine;
using System.Collections;

namespace KofTools{
	public class Job{
		
		public double percentageComplete = 0;
		public bool isBuildingOperationJob, isConstructionJob, isDeconstructionJob, 
				isEquipJob, isHaulingJob, isMiningJob;
		//public int x, y, z;
		Vector3 position;

		public enum JobType{
			Job = 0,
			BuildingOperationJob = 1,
			ConstructionJob = 2,
			DeconstructionJob = 3,
			EquipJob = 4,
			HaulingJob = 5,
			MiningJob = 6,
			RechargingJob = 7,
			SmeltOre = 8,
			PlacingJob = 9,
			SmeltBronze = 10,
			SmeltBrass = 11,
			SmeltGalvanizedSteel = 12,
			SmeltSteel = 13,
			ForgeRobotComponents = 14,
			AssembleRobot = 15,
			CutBlockJob = 16,
			LinkingJob = 17,
			PullLeverJob = 18,
			MovementJob = 19
		};
		
		public JobType jobType; 
		
		public string jobLabel;
		
		public Job(Vector3 pos, JobType njobType){
			position = pos;
			percentageComplete = 0;
			
			jobType = njobType;
			if (((int)jobType > 19) || ((int)jobType < 0)){
				jobType = JobType.Job;
			}
			
			JobController.addJob(this);
			
		}

		public Vector3 Position {
			get {
				return position;
			}
		}
		
		public bool workOn(double workspeed, Robot robot){
			
			if (percentageComplete > 100){
				return true;
			} else {
				return false;
			}
		}
		
		
		
	}
}
