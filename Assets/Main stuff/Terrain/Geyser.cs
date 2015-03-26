using UnityEngine;
using System.Collections;

namespace KofTools{
	public class Geyser : MonoBehaviour {
		
		int timer; // how often it erupts
		int regularity; // how random the timing is
		int intensity; // how high it erupts
		int variance; // how much the height varies
		
		ParticleEmitter particles;
		
		int regularness;
		
		// Use this for initialization
		void Start () {
			regularness = Random.Range(-1 * regularity, regularity);
		}
		
		// Update is called once per frame
		void Update () {
			if ((CommandLineControl.counter % (timer + regularness)) == 0){
				erupt();
			}
		}
		
		public void erupt(){
			regularness = Random.Range(-1 * regularity, regularity);
			particles.Emit();
		}
		
	}
}
