using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace KofTools{
	public class EconomicMineral : Mineral {

		public enum EconomicMineralType{
			Undefined,
			
			Bauxite,
			Cassisterite,
			Limonite,
			Malachite,
			Spheralite,
			Tetrahedrite,

			Coal,
			Anthracite,
			BituminousCoal,
			Lignite
		}
		
		static Dictionary<EconomicMineralType, string> renderingMaterials = new Dictionary<EconomicMineralType, string>(){
			{EconomicMineralType.Bauxite, "Materials/Bauxite"},
			{EconomicMineralType.Cassisterite, "Materials/Cassisterite"},
			{EconomicMineralType.Limonite, "Materials/Limonite"},
			{EconomicMineralType.Malachite, "Materials/Malachite"},
			{EconomicMineralType.Spheralite, "Materials/Spheralite"},
			{EconomicMineralType.Tetrahedrite, "Materials/Tetrahedrite"},

			{EconomicMineralType.Coal, "Materials/Coal"},
			{EconomicMineralType.Anthracite, "Materials/Coal"},
			{EconomicMineralType.BituminousCoal, "Materials/Coal"},
			{EconomicMineralType.Lignite, "Materials/Coal"}
		};
		
		static Dictionary<EconomicMineralType, MaterialType> typeMapping = new Dictionary<EconomicMineralType, MaterialType>(){
			{EconomicMineralType.Bauxite, MaterialType.Bauxite},
			{EconomicMineralType.Cassisterite, MaterialType.Cassisterite},
			{EconomicMineralType.Limonite, MaterialType.Limonite},
			{EconomicMineralType.Malachite, MaterialType.Malachite},
			{EconomicMineralType.Spheralite, MaterialType.Spheralite},
			{EconomicMineralType.Tetrahedrite, MaterialType.Tetrahedrite},

			{EconomicMineralType.Coal, MaterialType.Coal},
			{EconomicMineralType.Anthracite, MaterialType.Anthracite},
			{EconomicMineralType.BituminousCoal, MaterialType.BituminousCoal},
			{EconomicMineralType.Lignite, MaterialType.Lignite}
		};

		public EconomicMineral(EconomicMineralType sType):base(){
			this.matType = typeMapping[sType];
			
			this.renderingMat = (UnityEngine.Material)Resources.Load(renderingMaterials[sType]);
		}
	}
}
