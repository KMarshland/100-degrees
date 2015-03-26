using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using KofTools;

public class Robot : MonoBehaviour {
	
	Job currentJob;
	public bool hasAJob;
	public float pressureInTank;
	bool addedStatustoList;
	public bool atJobSite;
	//double x, y, z;
	//int jobX, jobY, jobZ;

	Seeker seeker;
	Path path;//The calculated path
	float nextWaypointDistance = 3;//The max distance from the AI to a waypoint for it to continue to the next waypoint
	int currentWaypoint = 0;//the waypoint we're currently moving towards
	
	float rateOfMovement;
	static float baseSpeed = 0.3f; //Higher is faster, must be below 1
	static float turnSpeed = 5f; //Degrees/frame that the robot can rotate
	
	public Tank tank;
	Torso torso;
	Legs legs;
	Arms arms;
	Head head;
	
	public bool hasMiningTool, hasConstructionTool, hasDeconstructionTool;
	public float rateOfConstruction, rateOfDeconstruction, rateOfMining;
	public List<Tool> tools;
	public MiningTool miningTool;
	public ConstructionTool constructionTool;
	public DeconstructionTool deconstructionTool;
	
	public Item hauledItem;
	public bool hauling;
	
	// Use this for initialization
	void Start () {
		hasAJob = false;
		currentJob = null;
		pressureInTank = 3000;
		seeker = GetComponent<Seeker>();

		hasMiningTool = false;
		hasConstructionTool = false;
		hasDeconstructionTool = false;
		
		rateOfConstruction = 1;
		rateOfMining = 1;
		rateOfDeconstruction = 1;
		
		tools = new List<Tool>();
		
		hauling = false;
		
		miningTool = (MiningTool)transform.FindChild("jackhammer").GetComponent("MiningTool");
		
		this.transform.FindChild("jackhammer").GetComponent<Renderer>().enabled = false;
		for (int i = 0; i < this.transform.FindChild("jackhammer").childCount; i++){
			try {
				this.transform.FindChild("jackhammer").GetChild(i).GetComponent<Renderer>().enabled = false;
			} catch {
				
			}
		}
		
		constructionTool = (ConstructionTool)transform.FindChild("torch2").GetComponent("ConstructionTool");

		this.transform.FindChild("torch2").GetComponent<Renderer>().enabled = false;
		for (int i = 0; i < this.transform.FindChild("torch2").childCount; i++){
			try {
				this.transform.FindChild("torch2").GetChild(i).GetComponent<Renderer>().enabled = false;
			} catch {
				
			}
		}
		
		hasMiningTool = false;
		hasConstructionTool = false;
		hasDeconstructionTool = false;
		
		
	}
	
	// Update is called once per frame
	void Update(){
		if (!hasAJob){
			
			if (pressureInTank < 500){
				addedStatustoList = true;
				//refill with steam
				Job j = new RechargingJob((RefuelingStationManager)ConstructionController.rechargingStations[0]);
				JobController.removeJob(j);
				this.assignJob(j);
			}
			
			if (!hasConstructionTool && !hasDeconstructionTool && !hasMiningTool){
				if (!hasConstructionTool){
					foreach (Tool t in JobController.UnusedTools){
						if ((t.isConstructionTool) && (!addedStatustoList)){
							addedStatustoList = true;
							currentJob = new EquipJob(t, t.Position);
						}
					}
				} if (!hasDeconstructionTool){
					foreach (Tool t in JobController.UnusedTools){
						if ((t.isDeconstructionTool) && (!addedStatustoList)){
							addedStatustoList = true;
							currentJob = new EquipJob(t, t.Position);
						}
					}
				} if (!hasMiningTool){
					foreach (Tool t in JobController.UnusedTools){
						if ((t.isMiningTool) && (!addedStatustoList)){
							addedStatustoList = true;
							assignJob(new EquipJob(t, t.Position));
						}
					}
				}
			}
			
			if (hasConstructionTool){
				foreach (Tool t in JobController.UnusedTools){
					if ((t.isConstructionTool) && (!addedStatustoList) && (t.speedWithTool > rateOfConstruction)){
						addedStatustoList = true;
						currentJob = new EquipJob(t, t.Position);
						ConstructionTool item;
						GameObject obj = (GameObject)GameObject.Instantiate(UnityEngine.Resources.Load("ConstructionToolPrefab"));
						item = ((ConstructionTool)obj.GetComponent("ConstructionTool"));
						item.starterate(constructionTool.material);
						obj.transform.position = new Vector3(this.Position.x, this.Position.y + 5f, this.Position.z);
						removeConstructionTool();
					}
				}
			} if (hasDeconstructionTool){
				foreach (Tool t in JobController.UnusedTools){
					if ((t.isDeconstructionTool) && (!addedStatustoList) && (t.speedWithTool > rateOfDeconstruction)){
						addedStatustoList = true;
						currentJob = new EquipJob(t, t.Position);
						DeconstructionTool item;
						GameObject obj = (GameObject)GameObject.Instantiate(UnityEngine.Resources.Load("DeconstructionToolPrefab"));
						item = ((DeconstructionTool)obj.GetComponent("DeconstructionTool"));
						item.starterate(deconstructionTool.material);
						obj.transform.position = new Vector3(this.Position.x, this.Position.y + 5f, this.Position.z);
						removeDeconstructionTool();
					}
				}
			} if (hasMiningTool){
				foreach (Tool t in JobController.UnusedTools){
					if ((t.isMiningTool) && (!addedStatustoList) && (t.speedWithTool > rateOfMining)){
						addedStatustoList = true;
						assignJob(new EquipJob(t, t.Position));
						MiningTool item;
						GameObject obj = (GameObject)GameObject.Instantiate(UnityEngine.Resources.Load("MiningToolPrefab"));
						item = ((MiningTool)obj.GetComponent("MiningTool"));
						item.starterate(miningTool.material);
						obj.transform.position = new Vector3(Position.x, Position.y + 5f, Position.z);
						removeMiningTool();
					}
				}
			}
			
			if (!addedStatustoList){
				addedStatustoList = true;
				JobController.addRobot(this);
			}
		} 
	}
	
	
	void FixedUpdate () {
		
		//Deal with rendering of tools
		if (hasMiningTool){
			miningTool.stopEmitting();
		}
		
		if (hasConstructionTool){
			constructionTool.stopEmitting();
		}
		
		if (hasDeconstructionTool){
			deconstructionTool.setPosition(new Vector3(Position.x, Position.y + 2, Position.z));
			deconstructionTool.stopEmitting();
		}
		
		
		//Works on job
		if (atJobSite && hasAJob){
			bool done = false;
			if (currentJob.jobType == Job.JobType.BuildingOperationJob){
				done = ((BuildingOperationJob)(currentJob)).workOn(0.5, this);
			} else if (currentJob.jobType == Job.JobType.ConstructionJob){
				done = ((ConstructionJob)(currentJob)).workOn(0.2, this);
				if (hasConstructionTool){
					constructionTool.startEmitting();
				}
			} else if (currentJob.jobType == Job.JobType.DeconstructionJob){
				done = ((DeconstructionJob)(currentJob)).workOn(0.3, this);
				if (hasDeconstructionTool){
					deconstructionTool.startEmitting();
				}
			} else if (currentJob.jobType == Job.JobType.EquipJob) {
				done = ((EquipJob)(currentJob)).workOn(this);
				done = true;
			} else if (currentJob.jobType == Job.JobType.HaulingJob){
				done = ((HaulingJob)(currentJob)).workOn(this);
			} else if (currentJob.jobType == Job.JobType.MiningJob){
				done = ((MiningJob)(currentJob)).workOn(0.2, this);
				if (hasMiningTool){
					miningTool.startEmitting();
				}
			} else if (currentJob.jobType == Job.JobType.RechargingJob){
				done = ((RechargingJob)(currentJob)).workOn(this);
			} else if (currentJob.jobType == Job.JobType.SmeltOre){
				done = ((SmeltOre)(currentJob)).workOn(0.5, this);
			} else if (currentJob.jobType == Job.JobType.PlacingJob){
				done = ((PlacingJob)(currentJob)).workOn(this);
				if (done){
					hauling = false;
				}
			} else if (currentJob.jobType == Job.JobType.SmeltBronze){
				done = ((SmeltBronze)(currentJob)).workOn(0.5, this);
			} else if (currentJob.jobType == Job.JobType.SmeltBrass){
				done = ((SmeltBrass)(currentJob)).workOn(0.5, this);
			} else if (currentJob.jobType == Job.JobType.SmeltGalvanizedSteel){
				done = ((SmeltGalvanizedSteel)(currentJob)).workOn(0.5, this);
			} else if (currentJob.jobType == Job.JobType.SmeltSteel){
				done = ((SmeltSteel)(currentJob)).workOn(0.5, this);
			} else if (currentJob.jobType == Job.JobType.ForgeRobotComponents){
				done = ((ForgeRobotComponents)(currentJob)).workOn(0.5, this);
			} else if (currentJob.jobType == Job.JobType.AssembleRobot){
				done = ((AssembleRobot)(currentJob)).workOn(0.5, this);
			} else if (currentJob.jobType == Job.JobType.CutBlockJob){
				done = ((CutBlockJob)(currentJob)).workOn(0.5, this);
			} else if (currentJob.jobType == Job.JobType.LinkingJob){
				done = ((LinkingJob)(currentJob)).workOn(0.5, this);
			} else if (currentJob.jobType == Job.JobType.PullLeverJob){
				done = ((PullLeverJob)(currentJob)).workOn(0.5, this);
			} else if (currentJob.jobType == Job.JobType.MovementJob){
				done = ((MovementJob)(currentJob)).workOn(this);
			} else {
				done = true;
				LogControl.LogError("Invalid job type: " + currentJob.jobType);
			}
			if (done){
				JobController.finishedJob(this, currentJob);
				currentJob = null;
				hasAJob = false;
				addedStatustoList = false;
			}
			
		}
		if (hasAJob && !atJobSite){
			move ();			
		}
		
		if (hauling){
			try {
				hauledItem.setPosition(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z + 1f));
			} catch {
				
			}
		}
		

		
	}

	void move(){
		//Debug.Log("Move called");
		rateOfMovement = 0;
		foreach (Tool t in tools){
			rateOfMovement += (float)(t.material.weightModifier);
		}
		
		rateOfMovement = (baseSpeed / (rateOfMovement + 1));

		if (path == null || path.vectorPath.Count == 0) {
			//We have no path to move after yet
			LogControl.LogWarning("No path", this);
			return;
		}
		
		if (currentWaypoint >= path.vectorPath.Count) {
			atJobSite = true;
			//Debug.Log("At job site?");
			return;
		}
		
		//Direction to the next waypoint
		Vector3 dir = (path.vectorPath[currentWaypoint]-transform.position).normalized;
		//Debug.Log(dir);
		dir *= rateOfMovement;// * Time.fixedDeltaTime;
		turnTowards(dir + transform.position);


		transform.position += dir;
		//Debug.Log(dir);

		//release some steam
		pressureInTank -= 1;

		//bounds
		if (Position.x > 1400){
			transform.position = new Vector3(1400f, Position.y, Position.z);
		}
		if (Position.x < -1250){
			transform.position = new Vector3(-1250f, Position.y, Position.z);
		}
		if (Position.y > 200){
			transform.position = new Vector3(Position.x, 200f, Position.z);
		}
		if (Position.y < -25){
			transform.position = new Vector3(Position.x, -25f, Position.z);
		}
		if (Position.z > 1500){
			transform.position = new Vector3(Position.x, Position.y, 1500f);
		}
		if (Position.z < -800){
			transform.position = new Vector3(Position.x, Position.y, -800f);
		}

		//Check if we are close enough to the next waypoint
		//If we are, proceed to follow the next waypoint
		if (Vector3.Distance (transform.position,path.vectorPath[currentWaypoint]) < nextWaypointDistance) {
			currentWaypoint++;
			return;
		}
	}

	float turnTowards(float angle){

		float theta = transform.rotation.eulerAngles.y;
		if (angleDifference(theta, angle) < turnSpeed){
			theta = angle;
			//transform.rotation = Quaternion.Euler(270f, theta, 0f);
		} else {
			if (theta < angle){
				if (angle - theta <= 180f){
					theta += turnSpeed;
				} else {
					theta -= turnSpeed;
				}
			} else {
				if (theta - angle <= 180f){
					theta -= turnSpeed;
				} else {
					theta += turnSpeed;
				}
			}
		}
		
		transform.rotation = Quaternion.Euler(270f, theta, 0f);
		return angleDifference(theta, angle);
	}
	
	float turnTowards(Vector3 v){
		Vector3 tcal = v - transform.position;
		return turnTowards(180f - (Mathf.Atan2(tcal.z, tcal.x) * Mathf.Rad2Deg));
	}
	
	float angleDifference(float a, float b){
		a = normalAngle(a);
		b = normalAngle(b);
		
		Vector2 v1 = new Vector2(Mathf.Cos(Mathf.Deg2Rad * a), Mathf.Sin(Mathf.Deg2Rad * a));
		Vector2 v2 = new Vector2(Mathf.Cos(Mathf.Deg2Rad * b), Mathf.Sin(Mathf.Deg2Rad * b));
		return Vector2.Angle(v1 , v2);
	}
	
	float angleTo(Vector3 v){
		Vector3 tcal = v - transform.position;
		return (Mathf.Atan2(tcal.z, tcal.x) * Mathf.Rad2Deg);
	}

	public static float normalAngle(float a){
		while (a < 0f || a >= 360f){
			if (a < 0f){
				a += 360f;
			} else {
				a -= 360f;
			}
		}
		
		return a;
	}
	
	public void starterate(Arms narms, Legs nlegs, Head nhead, Torso ntorso, Tank ntank){
		arms = narms;
		legs = nlegs;
		head = nhead;
		torso = ntorso;
		tank = ntank;
	}
	
	public void addConstructionTool(ConstructionTool tool){
		constructionTool = tool;
		hasConstructionTool = true;
		rateOfConstruction = tool.speedWithTool;
		tools.Add(tool);
		try {
		JobController.removeTool(tool);
		} catch {
			
		}
		try {
			Destroy(constructionTool.item);
		} catch {
			
		}
		//constructionTool.item = this.transform.FindChild("torch2").gameObject;
		
		constructionTool.item.GetComponent<Renderer>().enabled = true;
		
		for (int i = 0; i < constructionTool.item.transform.childCount; i++){
			try {
				constructionTool.item.transform.GetChild(i).GetComponent<Renderer>().enabled = true;
			} catch {
				
			}
		}
	}
	
	public void addDeconstructionTool(DeconstructionTool tool){
		hasDeconstructionTool = true;
		rateOfDeconstruction = tool.speedWithTool;
		tools.Add(tool);
		deconstructionTool =  tool;
		JobController.removeTool(tool);
	}
	
	public void addMiningTool(MiningTool tool){
		miningTool = tool;
		hasMiningTool = true;
		rateOfMining = tool.speedWithTool;
		tools.Add(tool);
		try {
		JobController.removeTool(tool);
		} catch {
			
		}
		try {
			Destroy(miningTool.item);
		} catch {
			
		}
		//miningTool.item = this.transform.FindChild("jackhammer").gameObject;
		
		miningTool.item.GetComponent<Renderer>().enabled = true;
		
		for (int i = 0; i < miningTool.item.transform.childCount; i++){
			try {
				miningTool.item.transform.GetChild(i).GetComponent<Renderer>().enabled = true;
			} catch {
				
			}
		}
	}
	
	public void removeConstructionTool(){
		hasConstructionTool = false;
		//constructionTool.item = this.transform.FindChild("torch2").gameObject;
		rateOfConstruction = 1;
		tools.Remove(constructionTool);
		constructionTool.item.GetComponent<Renderer>().enabled = false;
		
		for (int i = 0; i < constructionTool.item.transform.childCount; i++){
			try {
				constructionTool.item.transform.GetChild(i).GetComponent<Renderer>().enabled = false;
			} catch {
				
			}
		}
	}
	
	public void removeDeconstructionTool(){
		hasDeconstructionTool = false;
		//deconstructionTool.item = this.transform.FindChild("jackhammer").gameObject;
		rateOfMining = 1;
		tools.Remove(deconstructionTool);
		deconstructionTool.item.GetComponent<Renderer>().enabled = false;
		
		for (int i = 0; i < deconstructionTool.item.transform.childCount; i++){
			try {
				deconstructionTool.item.transform.GetChild(i).GetComponent<Renderer>().enabled = false;
			} catch {
				
			}
		}
	}
	
	public void removeMiningTool(){
		hasMiningTool = false;
		//miningTool.item = this.transform.FindChild("jackhammer").gameObject;
		rateOfMining = 1;
		tools.Remove(miningTool);
		miningTool.item.GetComponent<Renderer>().enabled = false;
		
		for (int i = 0; i < miningTool.item.transform.childCount; i++){
			try {
				miningTool.item.transform.GetChild(i).GetComponent<Renderer>().enabled = false;
			} catch {
				
			}
		}
	}
	
	public void assignJob(Job j){
		if (!hasAJob){
			currentJob = j;
			hasAJob = true;
			atJobSite = false;
			
			JobController.removeRobot(this);
			JobController.removeJob(currentJob);
			
			startPathTo(j);
		} else {
			JobController.addJob(j);
			addedStatustoList = true;
		}
	}
	
	public void assignJob(Job j, bool t){
		currentJob = j;
		hasAJob = true;
		atJobSite = false;
		
		JobController.removeRobot(this);
		JobController.removeJob(currentJob);

		startPathTo(j);

	}

	void startPathTo(Job j){
		startPathTo(j.Position);
	}

	void startPathTo(Vector3 destination){
		//LogControl.Log("Path started");
		atJobSite = false;
		seeker.StartPath(transform.position, destination, OnPathComplete);
	}

	public void OnPathComplete (Path p) {
		if (p.error) {
			LogControl.LogError(p.error, this);
		} else {
			//LogControl.Log("Path complete");
			path = p;
			currentWaypoint = 0;
			atJobSite = false;
		}
	}
	
	public Vector3 Position{
		get {
			return transform.position;
		}
	}
	
	void OnGUI(){
		if (MenuControl.robotsPrintPosition){
			GUI.Label(new Rect(10, 10, 200, 30), transform.position.ToString());
		}
	}
	
}
