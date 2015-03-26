using UnityEngine;
using System.Collections;

namespace KofTools{
	public class Legs : RobotComponent {
		
		public Legs():base(){
			
		}
		
		public void starterate(Metal nmaterial){
			material = nmaterial;
			ItemList.legs.Enqueue(this);
		}
		
		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}
	}
}
