using UnityEngine;
using System.Collections;
using KofTools;

public class MiningTool : Tool {
	
	public MiningTool():base(){
		
	}
	
	public void starterate(Metal nmaterial){
		material = nmaterial;
		speedWithTool = (float)material.workSpeedModifier * 0.5f;
		JobController.addUnusedTool(this);
		isMiningTool = true;
		description = "This is a " + material.ToString() + " mining tool. It allows robots to mine at a speed of " + speedWithTool;
	}
	
	// Use this for initialization
	void Start () {
		stopEmitting();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void startEmitting(){
		base.startEmitting((ParticleEmitter) item.transform.Find("Dust Storm").GetComponent("ParticleEmitter"));
	}
	
	public void stopEmitting(){
		base.stopEmitting((ParticleEmitter) item.transform.Find("Dust Storm").GetComponent("ParticleEmitter"));
	}
	
	public void emitFor(int time){
		base.emitFor((ParticleEmitter) item.transform.Find("Dust Storm").GetComponent("ParticleEmitter"), time);
	}
}
