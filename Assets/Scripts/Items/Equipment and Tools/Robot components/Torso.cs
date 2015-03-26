using UnityEngine;
using System.Collections;

namespace KofTools{
	public class Torso : RobotComponent {
		
		public Torso():base(){
			
		}
		
		public void starterate(Metal nmaterial){
			material = nmaterial;
			ItemList.torsos.Enqueue(this);
		}
		
		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}
	}
}
