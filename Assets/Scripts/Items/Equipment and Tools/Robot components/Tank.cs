using UnityEngine;
using System.Collections;

namespace KofTools{
	public class Tank : RobotComponent {
		
		public int maxPressure;
		
		public Tank():base(){
			
		}
		
		public void starterate(Metal nmaterial){
			material = nmaterial;
			ItemList.tanks.Enqueue(this);
			maxPressure = (int)(1000 * material.corrosionResistance * material.strengthModifier);
		}
		
		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}
	}
}
