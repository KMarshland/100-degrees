using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace KofTools{
	public class TriggerObject : MonoBehaviour {
		
		protected List<TriggerableObject> objectsToTrigger = new List<TriggerableObject>();
		
		public bool showingGUI = false;
		
		// Use this for initialization
		public void Start () {

		}
		
		// Update is called once per frame
		void Update () {
		
		}
		
		public void trigger(){
			foreach (TriggerableObject t in objectsToTrigger){
				if (t.type == 1){
					((Door)t).triggered();
				} else {
					(t).triggered();
				}
			}
		}
		
		public bool linkTo(TriggerableObject t){
			if (!objectsToTrigger.Contains(t)){
				new LinkingJob(this, t);
				return true;
			} else {
				return false;
			}
		}
		
		public void addLinked(TriggerableObject t){
			objectsToTrigger.Add(t);
			Debug.Log("Ye");
		}
		
	}
}
