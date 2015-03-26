using UnityEngine;
using System.Collections;


namespace KofTools{
	
	public class RechargingJob : BuildingOperationJob {
		
		public RechargingJob(RefuelingStationManager nBoiler):base(nBoiler.transform.position){
			this.jobType = JobType.RechargingJob;
		}
		
		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}
		
		public bool workOn(Robot robot){
			bool isDone = base.workOn(1, robot);
			if (isDone){
				try {
					robot.pressureInTank = robot.tank.maxPressure;
				} catch {
					robot.pressureInTank = 3000;
				}
			}
			return isDone;
		}
	
		
	}
}
