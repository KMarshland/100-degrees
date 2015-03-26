using UnityEngine;
using System.Collections;

namespace KofTools{
	public class MovementJob : Job {
		
		Job jobAtEnd;
		
		public MovementJob(Vector3 pos):base(pos, JobType.MovementJob){
			jobType = JobType.MovementJob;
		}
		
		public MovementJob(Vector3 pos, Job njobAtEnd):base(pos, JobType.MovementJob){
			jobType = JobType.MovementJob;
			jobAtEnd = njobAtEnd;
		}
		
		public bool workOn(Robot r){
			base.workOn(1000, r);
			
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
