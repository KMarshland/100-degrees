using UnityEngine;
using System.Collections;

public struct GBlock{
	private byte lightNatural;
	private byte[] lightArtificial; // R B and G channels
	
	private byte type; // block Type
	
	private static Vector3I[][] faces;
	private static bool[] triDirs = new bool[]{false, true, true, false, true, false};
	
	public Color32 light{
		get{
			return new Color32(lightArtificial[0], lightArtificial[1], lightArtificial[2], lightNatural);
		}
	}
	
	public GBlock(byte type){
		this.type = type;
		this.lightNatural = 0;
		this.lightArtificial = new byte[]{0,0,0};
	}
	
	public bool IsSolid(){
		return BlockTypes.GetBlockType(type).IsSolid();
	}
	/*
		faces[0] = new Vector3[]{new Vector3(-1,1,-1) *scale, new Vector3(-1,1,1) *scale, new Vector3(1,1,-1) *scale, new Vector3(1,1,1) *scale};
		faces[1] = new Vector3[]{new Vector3(-1,-1,-1)*scale, new Vector3(-1,-1,1)*scale, new Vector3(1,-1,-1)*scale, new Vector3(1,-1,1)*scale};
		faces[2] = new Vector3[]{new Vector3(1,-1,-1) *scale, new Vector3(1,-1,1) *scale, new Vector3(1,1,-1) *scale, new Vector3(1,1,1) *scale};
		faces[3] = new Vector3[]{new Vector3(-1,-1,-1)*scale, new Vector3(-1,-1,1)*scale, new Vector3(-1,1,-1)*scale, new Vector3(-1,1,1)*scale};
		faces[4] = new Vector3[]{new Vector3(-1,-1,1) *scale, new Vector3(-1,1,1) *scale, new Vector3(1,-1,1) *scale, new Vector3(1,1,1) *scale};
		faces[5] = new Vector3[]{new Vector3(-1,-1,-1)*scale, new Vector3(-1,1,-1)*scale, new Vector3(1,-1,-1)*scale, new Vector3(1,1,-1)*scale};
	*/
	public static Vector3I[] GetFace(int face){
		// first check to see if 'faces' has been initialized
		if (faces == null){
			faces = new Vector3I[6][];
			
			faces[0] = new Vector3I[]{new Vector3I(-1,1,-1) , new Vector3I(-1,1,1) , new Vector3I(1,1,-1) , new Vector3I(1,1,1) };
			faces[1] = new Vector3I[]{new Vector3I(-1,-1,-1), new Vector3I(-1,-1,1), new Vector3I(1,-1,-1), new Vector3I(1,-1,1)};
			faces[2] = new Vector3I[]{new Vector3I(1,-1,-1) , new Vector3I(1,-1,1) , new Vector3I(1,1,-1) , new Vector3I(1,1,1) };
			faces[3] = new Vector3I[]{new Vector3I(-1,-1,-1), new Vector3I(-1,-1,1), new Vector3I(-1,1,-1), new Vector3I(-1,1,1)};
			faces[4] = new Vector3I[]{new Vector3I(-1,-1,1) , new Vector3I(-1,1,1) , new Vector3I(1,-1,1) , new Vector3I(1,1,1) };
			faces[5] = new Vector3I[]{new Vector3I(-1,-1,-1), new Vector3I(-1,1,-1), new Vector3I(1,-1,-1), new Vector3I(1,1,-1)};
		}
		
		return faces[face];
	}
	public static bool getFaceTriDir(int face){
		return triDirs[face];
	}
}