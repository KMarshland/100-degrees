using UnityEngine;
using System.Collections;

namespace KofTools{
	public class Ore : Boulder {
		
		
	    public Ore():base(){
			
	    }
		
		public new void starterate(Material mat){
			material = mat;
			reEnqueue();
			
		}
		
		public new void reEnqueue(){
			if (material.materialType == Material.MaterialType.Bauxite){
				ItemList.aluminumOre.Enqueue(this);
			} else if (material.materialType == Material.MaterialType.Malachite){
				ItemList.copperOre.Enqueue(this);
			} else if (material.materialType == Material.MaterialType.Tetrahedrite){
				ItemList.copperOre.Enqueue(this);
			} else if (material.materialType == Material.MaterialType.Limonite){
				ItemList.ironOre.Enqueue(this);
			} else if (material.materialType == Material.MaterialType.Cassisterite){
				ItemList.tinOre.Enqueue(this);
			} else if (material.materialType == Material.MaterialType.Spheralite){
				ItemList.zincOre.Enqueue(this);
			} else if (material.materialType == Material.MaterialType.Coal){
				ItemList.coalChunks.Enqueue(this);
				description = "This is a chunk of coal. It can be used at a boiler to provide steam.";
			} else {
				Debug.LogError (material.materialType);
			}
			
			item.GetComponent<Renderer>().material = material.renderingMat;
		}
	        
		// Use this for initialization
		void Start () {
			if (material.materialType != Material.MaterialType.Coal){
				description = "This is a chunk of " + material.materialType.ToString() + " ore";
			}
		}
		
		// Update is called once per frame
		void Update () {
		
		}
	}
}
