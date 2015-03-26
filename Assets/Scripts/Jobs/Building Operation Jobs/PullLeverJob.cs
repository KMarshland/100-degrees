using UnityEngine;
using System.Collections;

namespace KofTools{
	public class PullLeverJob : BuildingOperationJob {
		
		Lever lever;
		
		public PullLeverJob(Vector3 pos, Lever nlever):base(pos){
			lever = nlever;
			jobType = JobType.PullLeverJob;
		}
		
		public new bool workOn(double workspeed, Robot robot){
			bool isDone = true;
			
			lever.trigger();
			
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
