using UnityEngine;
using System.Collections;

namespace KofTools{
	public class DeconstructionTool : Tool {
		
		public DeconstructionTool():base(){
	
		}
		
		public void starterate(Metal nmaterial){
			material = nmaterial;
			speedWithTool = (float)material.workSpeedModifier * 0.5f;
			JobController.addUnusedTool(this);
			isDeconstructionTool = true;
			description = "This is a " + material.ToString() + " deconstruction tool. It allows robots to destroy buildings at a speed of " + speedWithTool;
	
		}
		
		// Use this for initialization
		void Start () {
		
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
}
