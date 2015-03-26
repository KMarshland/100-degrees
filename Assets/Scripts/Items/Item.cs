using UnityEngine;
using System.Collections;

namespace KofTools{
	public class Item : MonoBehaviour{
		
		//public int x, y, z;
		
		//public GameObject item;
		
		public string description;
		bool clicked;
		
		public Item(){
			
		}
		
		/*public bool isAt(Point point){
			bool isAt = false;
			
			if ((x == point.x) & (y == point.y) & (z == point.z)){
				isAt = true;
			}
			
			return isAt;
		}*/
		
		// Use this for initialization
		void Start () {

		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public GameObject item{
			get {
				return this.gameObject;
			}
		}

		public Vector3 Position{
			get {
				return transform.position;
			} set {
				setPosition(value);
			}
		}
		
		public void setPosition(Vector3 v){
			item.transform.position = v;
		}
		
		void examine(){
			clicked = true;
			//Debug.Log("Yep");
		}
		
		void OnGUI(){
			if (clicked){
				if (GUI.Button(new Rect(10, 10, 10 + (7 * description.Length), 25), description)){
					clicked = false;
				}
			}
		}
		
		void OnMouseOver(){
			//Debug.Log("Clicked");
			if (Input.GetMouseButton(1)){
				examine();
			}
		}
	}
}
