using UnityEngine;
using System.Collections;

namespace KofTools{
	public class WorkshopManager : MonoBehaviour {
		
		protected bool clicked;
		protected bool showingPressure;
		protected ArrayList queue;
		public bool active = true;
		
		public void calculatePressure(){
			
			active = false;
			
			Collider[] cols = Physics.OverlapSphere(transform.position, 4.2f);
			foreach (Collider c in cols){
				
				try {
					string str = c.gameObject.ToString();
					if (str.Equals("StraightPipePrefab (UnityEngine.GameObject)") 
						| str.Equals("StraightPipePrefab(Clone) (UnityEngine.GameObject)")
						| str.Equals("StraightPipePrefab(Clone) (UnityEngine.GameObject)")
						| str.Equals("EndPipePrefab(Clone) (UnityEngine.GameObject)")
						| str.Equals("TBendPipePrefab(Clone) (UnityEngine.GameObject)")
						| str.Equals("LBendPipePrefab(Clone) (UnityEngine.GameObject)")){
						Pipe p = (Pipe) c.gameObject.GetComponent("Pipe");
						if (p.pressure > 100){
							active = true;
						}
						
						if (this.transform.position.x > p.x){
							p.eastOpen = true;
						} else if (this.transform.position.x < p.x){
							p.westOpen = true;
						} else if (this.transform.position.y > p.y){
							p.topOpen = true;
						} else if (this.transform.position.y < p.y){
							p.bottomOpen = true;
						} else if (this.transform.position.z > p.z){
							p.northOpen = true;
						} else if (this.transform.position.z < p.z){
							p.southOpen = true;
						}
					}
				} catch {
					
				}
			}
		}
		
		public void calculatePressure(float rad){
			
			active = false;
			
			Collider[] cols = Physics.OverlapSphere(transform.position, rad);
			foreach (Collider c in cols){
				
				try {
					string str = c.gameObject.ToString();
					if (str.Equals("StraightPipePrefab (UnityEngine.GameObject)") 
						| str.Equals("StraightPipePrefab(Clone) (UnityEngine.GameObject)")
						| str.Equals("StraightPipePrefab(Clone) (UnityEngine.GameObject)")
						| str.Equals("EndPipePrefab(Clone) (UnityEngine.GameObject)")
						| str.Equals("TBendPipePrefab(Clone) (UnityEngine.GameObject)")
						| str.Equals("LBendPipePrefab(Clone) (UnityEngine.GameObject)")){
						Pipe p = (Pipe) c.gameObject.GetComponent("Pipe");
						if (p.pressure > 100){
							active = true;
						}
						
						if (this.transform.position.x > p.x){
							p.eastOpen = true;
						} else if (this.transform.position.x < p.x){
							p.westOpen = true;
						} else if (this.transform.position.y > p.y){
							p.topOpen = true;
						} else if (this.transform.position.y < p.y){
							p.bottomOpen = true;
						} else if (this.transform.position.z > p.z){
							p.northOpen = true;
						} else if (this.transform.position.z < p.z){
							p.southOpen = true;
						}
					}
				} catch {
					
				}
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
