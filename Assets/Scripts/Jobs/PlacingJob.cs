using UnityEngine;
using System.Collections;


namespace KofTools{
	
	public class PlacingJob : Job {
		
		public bool complete;
		
		bool destroyAtEnd;
		Item di;
		
		Job jobAtEnd;
		
		public PlacingJob(Vector3 pos, bool destroy, Item i):base(pos, JobType.PlacingJob){
			JobController.removeJob(this);
			complete = false;
			destroyAtEnd = destroy;
			di = i;
		}
		
		public PlacingJob(Vector3 pos, Job endJob, bool destroy, Item i):base(pos, JobType.PlacingJob){
			JobController.removeJob(this);
			complete = false;
			jobAtEnd = endJob;
			destroyAtEnd = destroy;
			di = i;
		}
		
		public bool workOn(Robot r){
			bool done = base.workOn(1000, r);
			
			if (destroyAtEnd){
				GameObject.Destroy(di.item);
			}
			
			r.hauling = false;
			complete = true;
			try {
				r.assignJob(jobAtEnd, true);
				return false;
			} catch {
				return true;
			}
		}
		
		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}
	}
}
