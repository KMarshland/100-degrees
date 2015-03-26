using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace KofTools{
	public class Metal : Material{

		public enum MetalType {
			Undefined,
			Aluminum,
			Brass,
			Bronze,
			Copper,
			GalvanizedSteel,
			Iron,
			Steel,
			Tin,
			Zinc
		}

		static Dictionary<MetalType, Dictionary<string, double>> modifiers = new Dictionary<MetalType, Dictionary<string, double>>(){
			{
				MetalType.Aluminum, new Dictionary<string, double>(){
					{"weight", 1},
					{"strength", 4},
					{"corrosion", 5},
					{"workSpeed", 10}
				}
			},
			{
				MetalType.Brass, new Dictionary<string, double>(){
					{"weight", 4},
					{"strength", 2},
					{"corrosion", 4},
					{"workSpeed", 4}
				}
			},
			{
				MetalType.Bronze, new Dictionary<string, double>(){
					{"weight", 5},
					{"strength", 5},
					{"corrosion", 3.5},
					{"workSpeed", 3}
				}
			},
			{
				MetalType.Copper, new Dictionary<string, double>(){
					{"weight", 2},
					{"strength", 1},
					{"corrosion", 3},
					{"workSpeed", 6}
				}
			},
			{
				MetalType.GalvanizedSteel, new Dictionary<string, double>(){
					{"weight", 2},
					{"strength", 7},
					{"corrosion", 6},
					{"workSpeed", 15}
				}
			},
			{
				MetalType.Iron, new Dictionary<string, double>(){
					{"weight", 2.5},
					{"strength", 4},
					{"corrosion", 1},
					{"workSpeed", 6.5}
				}
			},
			{
				MetalType.Steel, new Dictionary<string, double>(){
					{"weight", 2},
					{"strength", 6},
					{"corrosion", 2},
					{"workSpeed", 9}
				}
			},
			{
				MetalType.Tin, new Dictionary<string, double>(){
					{"weight", 0},
					{"strength", 0},
					{"corrosion", 0},
					{"workSpeed", 0}
				}
			},
			{
				MetalType.Zinc, new Dictionary<string, double>(){
					{"weight", 0},
					{"strength", 0},
					{"corrosion", 0},
					{"workSpeed", 0}
				}
			}
		};

		static Dictionary<MetalType, string> renderingMaterials = new Dictionary<MetalType, string>(){
			{MetalType.Aluminum, "Materials/Aluminum"},
			{MetalType.Brass, "Materials/Brass"},
			{MetalType.Bronze, "Materials/Bronze"},
			{MetalType.Copper, "Materials/Copper"},
			{MetalType.GalvanizedSteel, "Materials/GalvanizedSteel"},
			{MetalType.Iron, "Materials/Iron"},
			{MetalType.Steel, "Materials/Steel"},
			{MetalType.Tin, "Materials/Tin"},
			{MetalType.Zinc, "Materials/Zinc"}
		};

		static Dictionary<MetalType, MaterialType> typeMapping = new Dictionary<MetalType, MaterialType>(){
			{MetalType.Aluminum, MaterialType.Aluminum},
			{MetalType.Brass, MaterialType.Brass},
			{MetalType.Bronze, MaterialType.Bronze},
			{MetalType.Copper, MaterialType.Copper},
			{MetalType.GalvanizedSteel, MaterialType.GalvanizedSteel},
			{MetalType.Iron, MaterialType.Iron},
			{MetalType.Steel, MaterialType.Steel},
			{MetalType.Tin, MaterialType.Tin},
			{MetalType.Zinc, MaterialType.Zinc}
		};


		public double strengthModifier;
		public double workSpeedModifier;
		public double corrosionResistance;
		public double weightModifier;

		public Metal(MetalType mType){
			this.matType = typeMapping[mType];

			var mods = modifiers[mType];
			this.weightModifier = mods["weight"];
			this.strengthModifier = mods["strength"];
			this.corrosionResistance = mods["corrosion"];
			this.workSpeedModifier = mods["workSpeed"];

			this.renderingMat = (UnityEngine.Material)Resources.Load(renderingMaterials[mType]);
		}
	}
}
