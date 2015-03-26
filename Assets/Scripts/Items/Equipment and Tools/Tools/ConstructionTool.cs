using UnityEngine;
using System.Collections;
using KofTools;

public class ConstructionTool : Tool {
	
	
	
	public ConstructionTool():base(){
		
	}
	
	public void starterate(Metal nmaterial){
		material = nmaterial;
		speedWithTool = (float)material.workSpeedModifier * 0.5f;
		JobController.addUnusedTool(this);
		isConstructionTool = true;
		description = "This is a " + material.ToString() + " construction tool. It allows robots to build at a speed of " + speedWithTool;

	}
	
	// Use this for initialization
	void Start () {
		stopEmitting();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void startEmitting(){
		base.startEmitting((ParticleEmitter) item.transform.Find("Sparks").GetComponent("ParticleEmitter"));
	}
	
	public void stopEmitting(){
		base.stopEmitting((ParticleEmitter) item.transform.Find("Sparks").GetComponent("ParticleEmitter"));
	}
}
