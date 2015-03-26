using UnityEngine;
using System.Collections;

namespace KofTools{
	public class WallManager : ConstructionManager {
		
		Material mat;
		UnityEngine.Material material;
		
		// Use this for initialization
		void Start () {
			active = true;
		}
		
		// Update is called once per frame
		void Update () {
		
		}
		
		public void starterate(Material nmat){
			mat = nmat;
			
			if ((mat.GetType()).ToString() == "Aluminum"){
				material = (UnityEngine.Material)UnityEngine.Resources.Load("Materials/Aluminum");
			} else if ((mat.GetType()).ToString() == "Copper"){
				material = (UnityEngine.Material)UnityEngine.Resources.Load("Materials/Copper");
			} else if ((mat.GetType()).ToString() == "Iron"){
				material = (UnityEngine.Material)UnityEngine.Resources.Load("Materials/Iron");
			} else if ((mat.GetType()).ToString() == "Tin"){
				material = (UnityEngine.Material)UnityEngine.Resources.Load("Materials/Tin");
			} else if ((mat.GetType()).ToString() == "Zinc"){
				material = (UnityEngine.Material)UnityEngine.Resources.Load("Materials/Zinc");
			} else if ((mat.GetType()).ToString() == "Brass"){
				material = (UnityEngine.Material)UnityEngine.Resources.Load("Materials/Brass");
			} else {
				Debug.LogError (material.GetType());
			}
			
			setMat(material);
			
		}
		
		public void setMat(UnityEngine.Material nmat){
			material = nmat;
			GetComponent<Renderer>().material = material;
		}
	}
}
