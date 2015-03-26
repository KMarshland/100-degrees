using UnityEngine;
using System.Collections;

namespace KofTools{
	public class Bar : Item {
		
		public Material material;
		
		public Bar():base(){
		}
		
		public void starterate(Material mat){
			material = mat;
			if (material.materialType == Material.MaterialType.Bauxite){
				material = new Metal(Metal.MetalType.Aluminum);
			} else if (material.materialType == Material.MaterialType.Malachite){
				material = new Metal(Metal.MetalType.Copper);
			} else if (material.materialType == Material.MaterialType.Limonite){
				material = new Metal(Metal.MetalType.Iron);
			} else if (material.materialType == Material.MaterialType.Cassisterite){
				material = new Metal(Metal.MetalType.Tin);
			} else if (material.materialType == Material.MaterialType.Spheralite){
				material = new Metal(Metal.MetalType.Zinc);
			} else if (material.materialType == Material.MaterialType.Tetrahedrite){
				material = new Metal(Metal.MetalType.Copper);
			}
			reEnqueue();
			
		}
		
		public void reEnqueue(){
			if (material.materialType == Material.MaterialType.Aluminum){
				ItemList.aluminumBars.Enqueue(this);
			} else if (material.materialType == Material.MaterialType.Copper){
				ItemList.copperBars.Enqueue(this);
			} else if (material.materialType == Material.MaterialType.Iron){
				ItemList.ironBars.Enqueue(this);
			} else if (material.materialType == Material.MaterialType.Tin){
				ItemList.tinBars.Enqueue(this);
			} else if (material.materialType == Material.MaterialType.Zinc){
				ItemList.zincBars.Enqueue(this);
			} else if (material.materialType == Material.MaterialType.Brass){
				ItemList.brassBars.Enqueue(this);
			} else if (material.materialType == Material.MaterialType.Bronze){
				ItemList.bronzeBars.Enqueue(this);
			} else if (material.materialType == Material.MaterialType.Steel){
				ItemList.steelBars.Enqueue(this);
			} else if (material.materialType == Material.MaterialType.GalvanizedSteel){
				ItemList.galvanizedSteelBars.Enqueue(this);
			} else {
				Debug.LogError(material.GetType());
			}
			item.GetComponent<Renderer>().material = material.renderingMat;
	
		}
		
		// Use this for initialization
		void Start () {
			description = "This is a " + material.materialType.ToString() + " bar";
		}
		
		// Update is called once per frame
		void Update () {
		
		}
		
		public Material getMaterial(){
			return material;
		}
	}
}
