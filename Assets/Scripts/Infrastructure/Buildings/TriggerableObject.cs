using UnityEngine;
using System.Collections;

namespace KofTools{
	public class TriggerableObject : MonoBehaviour {
		
		public int type;
		
		public bool on;
		
		/// type list
		/// 0 = default
		/// 1 = door
		
		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}
		
		public void triggered(){
			on = !on;
		}
		
	}
}
