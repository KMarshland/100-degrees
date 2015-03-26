using UnityEngine;
using System.Collections;

namespace KofTools{
	public class Head : RobotComponent {
		
		public Head():base(){
			
		}
		
		public void starterate(Metal nmaterial){
			material = nmaterial;
			ItemList.heads.Enqueue(this);
		}
		
		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}
	}
}
