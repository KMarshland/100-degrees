using UnityEngine;
using System.Collections;

namespace KofTools{
	public class SoilBelow {
		
		public float saturation;
		bool hasSoilBelow;
		SoilBelow soil;
		
		public SoilBelow(){
			saturation = 0.01f;
		}
		
		// Use this for initialization
		void Start () {
			saturation = 0.01f;
		}
		
		// Update is called once per frame
		void Update () {
		
		}
		
		public void whenMined(){
			//new Water((int)transform.position.x, transform.position.y, (int)transform.position.z);
		}
		
		public void updateLower(){
			if (hasSoilBelow){
				float change = (0.05f * saturation) / (100f * soil.saturation);
				soil.saturation += change;
				saturation -= change;
				soil.updateLower();
			}
		}
		
	}
}
