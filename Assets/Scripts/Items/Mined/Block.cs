using UnityEngine;
using System.Collections;

namespace KofTools{
	public class Block : Item {
		
		public Material material;
		
		public Block():base(){
			
	    }
		
		public void starterate(Material mat){
			material = mat;
			reEnqueue();
		}
		
		public void reEnqueue(){
			ItemList.blocks.Enqueue(this);
			
			item.GetComponent<Renderer>().material = material.renderingMat;
		}
		
		// Use this for initialization
		void Start () {
			description = "This is a block of " + material.GetType().ToString();
		}
		
		// Update is called once per frame
		void Update () {
		
		}
	}
}
