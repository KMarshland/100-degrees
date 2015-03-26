using UnityEngine;
using System.Collections;

namespace KofTools{
	public class Arms : RobotComponent {
		
		public Arms():base(){
			
		}
		
		public void starterate(Metal nmaterial){
			material = nmaterial;
			ItemList.arms.Enqueue(this);
		}
		
		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}
	}
}
