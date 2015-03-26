using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace KofTools{
	public static class ItemList {
		
		//These are the lists of items that can be turned into something else. Some things are finished products, 
		//like tools, and hence not on here
		
		public static Queue<Item> allItems = new Queue<Item>();
		
		//boulders
		public static Queue<Boulder> boulders = new Queue<Boulder>();
		
		//blocks
		public static Queue<Block> blocks = new Queue<Block>();
		
		//ores
		public static Queue<Ore> ironOre = new Queue<Ore>();
		public static Queue<Ore> tinOre = new Queue<Ore>();
		public static Queue<Ore> aluminumOre = new Queue<Ore>();
		public static Queue<Ore> copperOre = new Queue<Ore>();
		public static Queue<Ore> zincOre = new Queue<Ore>();
		
		//bars
		public static Queue<Bar> ironBars = new Queue<Bar>();
		public static Queue<Bar> tinBars = new Queue<Bar>();
		public static Queue<Bar> aluminumBars = new Queue<Bar>();
		public static Queue<Bar> copperBars = new Queue<Bar>();
		public static Queue<Bar> zincBars = new Queue<Bar>();
		public static Queue<Bar> brassBars = new Queue<Bar>();
		public static Queue<Bar> bronzeBars = new Queue<Bar>();
		public static Queue<Bar> coalBars = new Queue<Bar>();
		public static Queue<Bar> galvanizedSteelBars = new Queue<Bar>();
		public static Queue<Bar> steelBars = new Queue<Bar>();
		
		//robotstuff
		public static Queue<Arms> arms = new Queue<Arms>();
		public static Queue<Head> heads = new Queue<Head>();
		public static Queue<Legs> legs = new Queue<Legs>();
		public static Queue<Tank> tanks = new Queue<Tank>();
		public static Queue<Torso> torsos = new Queue<Torso>();
		
		//steam stuff
		public static Queue<Item> coalChunks = new Queue<Item>();
		
		
	}
}
