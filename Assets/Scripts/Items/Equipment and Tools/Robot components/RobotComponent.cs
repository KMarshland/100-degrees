using UnityEngine;
using System.Collections;

namespace KofTools{
	public class RobotComponent : Item {
		
		public Metal material;
		int damage; // How much damage the piece has taken, starts at 0, goes to 100
		
		public RobotComponent():base(){
	
		}
		
		public void Degrade(){
			if (Random.Range(0, 1000000) == 1){
				damage += 1;
			}
		}
		
		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}
	}
}
