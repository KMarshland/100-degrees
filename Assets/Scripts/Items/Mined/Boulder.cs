using UnityEngine;
using System.Collections;

namespace KofTools{
	public class Boulder : Item {
		
		public Material material;
		
		public Boulder():base(){
			
		}
		
		public void starterate(Material mat){
			material = mat;
			reEnqueue();
			
		}
		
		public void reEnqueue(){
			ItemList.boulders.Enqueue(this);
			
			item.GetComponent<Renderer>().material = material.renderingMat;
		}
		
		// Use this for initialization
		void Start () {
			description = "This is a boulder made of " + material.materialType.ToString();
		}
		
		// Update is called once per frame
		void Update () {
		
		}
	}
}
