using UnityEngine;
using System.Collections;

public static class BlockTypes{
	private static IBlock[] blocks = new IBlock[]{new AirBlock(), new DirtBlock()};
	
	public static IBlock GetBlockType(byte type){
		return blocks[type];
	}
}

