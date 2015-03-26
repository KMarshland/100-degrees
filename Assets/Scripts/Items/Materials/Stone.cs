using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace KofTools{

	public class Stone : Mineral {

		public enum StoneType{
			Undefined,

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
			Schist
		}
		
		static Dictionary<StoneType, string> renderingMaterials = new Dictionary<StoneType, string>(){
			{StoneType.Basalt, "Materials/Basalt"},
			{StoneType.Chalk, "Materials/Chalk"},
			{StoneType.Conglomerate, "Materials/Conglomerate"},
			{StoneType.Gabbro, "Materials/Gabbro"},
			{StoneType.Granite, "Materials/Granite"},
			{StoneType.Mudstone, "Materials/Mudstone"},
			{StoneType.Orthoclase, "Materials/Orthoclase"},
			{StoneType.Phyllite, "Materials/Phyllite"},
			{StoneType.Quartzite, "Materials/Quartzite"},
			{StoneType.Sandstone, "Materials/Sandstone"},
			{StoneType.Schist, "Materials/Schist"}
		};
		
		static Dictionary<StoneType, MaterialType> typeMapping = new Dictionary<StoneType, MaterialType>(){
			{StoneType.Basalt, MaterialType.Basalt},
			{StoneType.Chalk, MaterialType.Chalk},
			{StoneType.Conglomerate, MaterialType.Conglomerate},
			{StoneType.Gabbro, MaterialType.Gabbro},
			{StoneType.Granite, MaterialType.Granite},
			{StoneType.Mudstone, MaterialType.Mudstone},
			{StoneType.Orthoclase, MaterialType.Orthoclase},
			{StoneType.Phyllite, MaterialType.Phyllite},
			{StoneType.Quartzite, MaterialType.Quartzite},
			{StoneType.Sandstone, MaterialType.Sandstone},
			{StoneType.Schist, MaterialType.Schist}
		};
		
		public Stone(StoneType sType):base(){
			this.matType = typeMapping[sType];
			
			this.renderingMat = (UnityEngine.Material)Resources.Load(renderingMaterials[sType]);
		}

	}
}
