using UnityEngine;
using System.Collections;

namespace KofTools{
	public class HaulingJob : Job {
		
		GameObject item;
		Item realItem;
		//int haulingToX, haulingToY, haulingToZ;
		
		public PlacingJob j;
		Job jobAtEnd;
		
		bool destroyAtEnd;
		
		public HaulingJob(Item nitem, Vector3 haulingTo, bool destroy):base(haulingTo, JobType.HaulingJob){
			item = nitem.item;
			realItem = nitem;
			
			destroyAtEnd = destroy;
			
			if (item == null){
				Debug.LogError("Item is null");
			}
		}
		
		public HaulingJob(Item nitem, Vector3 haulingTo, Job nendJob, bool destroy):base(haulingTo, JobType.HaulingJob){
			JobController.removeJob(this);
			destroyAtEnd = destroy;
			item = nitem.item;
			realItem = nitem;
			jobAtEnd = nendJob;
			
			if (item == null){
				Debug.LogError("Item is null");
			}
		}
		
		public bool workOn(Robot robot){
			//Debug.Log ("At site");
			robot.hauledItem = realItem;
			PlacingJob endJob;
			try {
				endJob = new PlacingJob(Position, jobAtEnd, destroyAtEnd, realItem);
			} catch {
				endJob = new PlacingJob(Position, destroyAtEnd, realItem);
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
