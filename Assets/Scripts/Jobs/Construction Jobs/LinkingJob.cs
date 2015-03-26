using UnityEngine;
using System.Collections;

namespace KofTools{
	public class LinkingJob : Job {
		
		TriggerObject trigger;
		TriggerableObject triggered;
		
		bool assigned;
		MovementJob mj;
		
		public LinkingJob(TriggerObject ntrigger, TriggerableObject ntriggered):base(ntriggered.transform.position, JobType.LinkingJob){
			trigger = ntrigger;
			triggered = ntriggered;
			
			assigned = false;
			
			mj = new MovementJob(ntrigger.transform.position, this);
			
		}
		
		public new bool workOn(double workspeed, Robot r){
			
			bool isDone = false;
			
			if (!assigned){
				assigned = true;
				r.assignJob(mj, true);
			} else {
				percentageComplete += (0.25 * workspeed);
				isDone = base.workOn(workspeed, r);
			}
			
			if (isDone){
				trigger.addLinked(triggered);
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
