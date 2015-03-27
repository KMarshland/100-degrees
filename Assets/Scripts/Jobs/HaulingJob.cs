using UnityEngine;
using System.Collections;

namespace KofTools{
	public class HaulingJob : Job {
		
		GameObject item;
		Item realItem;
		Vector3 haulingTo;
		
		public PlacingJob j;
		Job jobAtEnd;
		
		bool destroyAtEnd;
		
		public HaulingJob(Item nitem, Vector3 nhaulingTo, bool destroy):base(nitem.transform.position, JobType.HaulingJob){
			JobController.removeJob(this);
			item = nitem.item;
			realItem = nitem;
			
			destroyAtEnd = destroy;
			haulingTo = nhaulingTo;
			
			if (item == null){
				Debug.LogError("Item is null");
			}
		}
		
		public HaulingJob(Item nitem, Vector3 nhaulingTo, Job nendJob, bool destroy):base(nitem.transform.position, JobType.HaulingJob){
			JobController.removeJob(this);
			destroyAtEnd = destroy;
			item = nitem.item;
			realItem = nitem;
			jobAtEnd = nendJob;
			haulingTo = nhaulingTo;
			
			if (item == null){
				Debug.LogError("Item is null");
			}
		}
		
		public bool workOn(Robot robot){
			//Debug.Log ("At site");
			robot.hauledItem = realItem;
			PlacingJob endJob;
			try {
				endJob = new PlacingJob(haulingTo, jobAtEnd, destroyAtEnd, realItem);
			} catch {
				endJob = new PlacingJob(haulingTo, destroyAtEnd, realItem);
			}
			endJob.percentageComplete = 101;
			robot.assignJob(endJob, true);
			robot.hauling = true;
			j = endJob;
			return false;
		}
	
		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}
	}
}
