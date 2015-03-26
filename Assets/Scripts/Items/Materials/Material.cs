using UnityEngine;
using System.Collections;


namespace KofTools{

	public class Material {
		
		public UnityEngine.Material renderingMat;

		public enum MaterialType {
			Undefined,

			//Metals
			Metal,
			Aluminum,
			Brass,
			Bronze,
			Copper,
			GalvanizedSteel,
			Iron,
			Steel,
			Tin,
			Zinc,

			//Stones
			Stone,
			Basalt,
			Chalk,
			Conglomerate,
			Gabbro,
			Granite,
			Mudstone,
			Orthoclase,
			Phyllite,
			Quartzite,
			Sandstone,
			Schist,

			//Coals
			Coal,
			Anthracite,
			BituminousCoal,
			Lignite,

			//Ores
			Ore,
			Bauxite,
			Cassisterite,
			Limonite,
			Malachite,
			Spheralite,
			Tetrahedrite
		};

		protected MaterialType matType;
		
		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		public MaterialType materialType{
			get {
				return matType;
			}
		}
	}
}
