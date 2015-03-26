using UnityEngine;
using System.Collections;

namespace KofTools{
	public class Tool : Item{
		
		public Metal material;
		public float speedWithTool;
		public float wearPercent;
		
		public bool isMiningTool, isConstructionTool, isDeconstructionTool;
		
		public Tool():base(){
			isMiningTool = false;
			isConstructionTool = false;
			isDeconstructionTool = false;
			JobController.addUnusedTool(this);
		}
		
		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}
		
		public void startEmitting(ParticleEmitter p){
			p.emit = true;
		}
		
		public void stopEmitting(ParticleEmitter p){
			p.emit = false;
		}
		
		public void emitFor(ParticleEmitter p, int time){
			p.Emit(time);
		}
	}
}
