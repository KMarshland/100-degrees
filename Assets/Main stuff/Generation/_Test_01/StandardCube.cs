using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct StandardCube
{
	private Vector3[][] faces;
	
	public StandardCube(float scale){
		faces = new Vector3[6][];
		faces[0] = new Vector3[]{new Vector3(-1,1,-1) *scale, new Vector3(-1,1,1) *scale, new Vector3(1,1,-1) *scale, new Vector3(1,1,1) *scale};
		faces[1] = new Vector3[]{new Vector3(-1,-1,-1)*scale, new Vector3(-1,-1,1)*scale, new Vector3(1,-1,-1)*scale, new Vector3(1,-1,1)*scale};
		faces[2] = new Vector3[]{new Vector3(1,-1,-1) *scale, new Vector3(1,-1,1) *scale, new Vector3(1,1,-1) *scale, new Vector3(1,1,1) *scale};
		faces[3] = new Vector3[]{new Vector3(-1,-1,-1)*scale, new Vector3(-1,-1,1)*scale, new Vector3(-1,1,-1)*scale, new Vector3(-1,1,1)*scale};
		faces[4] = new Vector3[]{new Vector3(-1,-1,1) *scale, new Vector3(-1,1,1) *scale, new Vector3(1,-1,1) *scale, new Vector3(1,1,1) *scale};
		faces[5] = new Vector3[]{new Vector3(-1,-1,-1)*scale, new Vector3(-1,1,-1)*scale, new Vector3(1,-1,-1)*scale, new Vector3(1,1,-1)*scale};
	}
	public Vector3[] getFace(int face){
		return faces[face];
	}
	public Vector3[] getFaces(int[] faces){
		List<Vector3> ret = new List<Vector3>();
		
		for (int i = 0; i < faces.Length; i++){
			ret.AddRange(getFace(faces[i]));
		}
		
		return ret.ToArray();
	}
	public Vector3[][] GetAllFaces(Vector3 loc){
		Vector3[][] retFaces = new Vector3[6][];
		for (int i = 0; i < 6; i++){
			retFaces[i] = new Vector3[4];
			for (int j = 0; j < 4; j++){
				retFaces[i][j] = faces[i][j] + loc;
			}
		}
		return retFaces;
	}
	public Vector3[] getFaceAroundPoint(int[] faces, Vector3 loc){
		Vector3[] faceVerts = getFaces(faces);
		
		for (int i = 0; i < faceVerts.Length; i++){
			faceVerts[i] += loc;
		}
		
		return faceVerts;
	}
	/*
	public int[] pY;// = new int[]{-1,1,-1, -1,1,1, 1,1,-1, 1,1,1};
	public int[] nY;//= new int[]{-1,-1,-1, -1,-1,1, 1,-1,-1, 1,-1,1};
	public int[] pX;//= new int[]{1,-1,-1, 1,-1,1, 1,1,-1, 1,1,1};
	public int[] nX;// = new int[]{-1,-1,-1, -1,-1,1, -1,1,-1, -1,1,1};
	public int[] pZ;// = new int[]{-1,-1,1, -1,1,1, 1,-1,1, 1,1,1};
	public int[] nZ;// = new int[]{-1,-1,-1, -1,1,-1, 1,-1,-1, 1,1,-1};
	
	public StandardCube(bool doesNothing){
		pY = new int[]{-1,1,-1, -1,1,1, 1,1,-1, 1,1,1};
		nY= new int[]{-1,-1,-1, -1,-1,1, 1,-1,-1, 1,-1,1};
		pX= new int[]{1,-1,-1, 1,-1,1, 1,1,-1, 1,1,1};
		nX = new int[]{-1,-1,-1, -1,-1,1, -1,1,-1, -1,1,1};
		pZ = new int[]{-1,-1,1, -1,1,1, 1,-1,1, 1,1,1};
		nZ = new int[]{-1,-1,-1, -1,1,-1, 1,-1,-1, 1,1,-1};
	}
	
	public float[] getFace(int face, float scale){
		float[] faceVals = new float[12];
		
		if (face == 0){
			for (int i = 0; i < 12; i++){
				faceVals[i] = pY[i]*scale;
			}
		}else if(face == 1){
			for (int i = 0; i < 12; i++){
				faceVals[i] = nY[i]*scale;
			}
		}else if(face == 2){
			for (int i = 0; i < 12; i++){
				faceVals[i] = pX[i]*scale;
			}
		}else if(face == 3){
			for (int i = 0; i < 12; i++){
				faceVals[i] = nX[i]*scale;
			}
		}else if(face == 4){
			for (int i = 0; i < 12; i++){
				faceVals[i] = pZ[i]*scale;
			}
		}else if(face == 5){
			for (int i = 0; i < 12; i++){
				faceVals[i] = nZ[i]*scale;
			}
		}else{
			return null;
		}
		
		return faceVals;
	}
	public float[] getFaces(int[] faces, float scale){
		List<float> faceVals = new List<float>();
		
		for (int i = 0; i < faces.Length; i++){
			faceVals.AddRange(getFace(faces[i], scale));
		}
		
		return faceVals.ToArray();
	}
	*/
}

