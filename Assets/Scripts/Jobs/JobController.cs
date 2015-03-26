using UnityEngine;
using System.Collections;
using System.Collections.Generic;

      
namespace KofTools{
	
	public static class JobController{
		
		private static List<Job> unassignedJobs = new List<Job>();
		private static List<Robot> idlingRobots = new List<Robot>();
		private static List<Tool> unusedTools = new List<Tool>();
		
		// Use this for initialization
		public static void Start () {

		}
		
		// Update is called once per frame
		public static void Update () {
			if ((int)(idlingRobots.Count) != 0){
				if (unassignedJobs.Count != 0){
					Robot bestRobot = (Robot)(idlingRobots[0]);
					Job jobToBeAssigned = (Job) (unassignedJobs[0]);
					int bestCost = getCostOf(bestRobot, jobToBeAssigned);;
					
					foreach(Robot rob in idlingRobots){
						if ((getCostOf(rob, jobToBeAssigned)) < bestCost){
							bestCost = getCostOf(rob, jobToBeAssigned);
							bestRobot = rob;
						}
					}
					
					bestRobot.assignJob(jobToBeAssigned);
					unassignedJobs.Remove(jobToBeAssigned);
					idlingRobots.Remove(bestRobot);
				}
			}
		}
		
		
		public static void addJob(Job j){
			unassignedJobs.Add(j);
		}
		
		public static void addRobot(Robot r){
			idlingRobots.Add (r);
		}
		
		public static void finishedJob(Robot r, Job j){
			try {
				idlingRobots.Add(r);
			} catch {
				
			}
		}
		
		public static void removeRobot(Robot robot){
			idlingRobots.Remove(robot);
		}
		
		public static List<Tool> UnusedTools{
			get {
				return unusedTools;
			}
		}
		
		public static void addUnusedTool(Tool t){
			unusedTools.Add(t);
		}
		
		public static void removeTool(Tool t){
			unusedTools.Remove(t);
		}
		
		public static int getCostOf(Robot robot, Job job){
			float cost = 0;
			cost += Mathf.Abs(robot.Position.x - job.Position.x);
			cost += Mathf.Abs(robot.Position.y - job.Position.y);
			cost += Mathf.Abs(robot.Position.z - job.Position.z);
			
			if (job.jobType == Job.JobType.ConstructionJob){
				cost -= (3 * (robot.rateOfConstruction));
			} else if (job.jobType == Job.JobType.DeconstructionJob){
				cost -= (3 * (robot.rateOfDeconstruction));
			} else if (job.jobType == Job.JobType.MiningJob){
				cost -= (3 * (robot.rateOfMining));
			}
			
			return (int)cost;
		}
		
		public static void removeJob(Job j){
			unassignedJobs.Remove(j);
		}
	}
}
