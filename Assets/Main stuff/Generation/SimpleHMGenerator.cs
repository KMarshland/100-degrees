using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using NoiseLibrary;
using System.Diagnostics;

public class SimpleHMGenerator : MonoBehaviour
{
	
	public int Height, Width;
	public int NumberOfOctaves;
	public int subOctaves;
	
	public float NoisePermanence;
	public float subPerm;
	
	private Texture2D hmTexture;
	private Texture2D subBandTexture;
	
	public Texture2D combinedTexture;
	
	public int min = 128, max = 128;
	
	public int maxHeight = 100;
	public int subBandMaxHeight = 100;
	
	public int chunkSizeX, chunkSizeY;
	
	public int ColorSaturation = 0;
	public int TerrainFloor = 30;
	
	public Material shaderToUse;
	
	private float[,,] worldMap;
	
	private Vector2 loc;
	private int groupCountW = 0, groupCountH = 0;
	
	private int sectionDividerH;// = Height / chunkSizeY;
	private int sectionDividerW;// = Width / chunkSizeX;
	
	private Stopwatch swatch;
	public bool Flat = true;
	
	private StandardCube sCube;
	public int largestMeshNumberVerts = 0;
	
	// Use this for initialization
	void Start ()
	{
		swatch = new Stopwatch();
		sCube = new StandardCube(0.5f);
		
		// make a HM of size given as a texture and then build it as a series of meshes
		if (ColorSaturation > 255) ColorSaturation = 255;
		if (ColorSaturation < 0) ColorSaturation = 0;
		
		if (TerrainFloor >= maxHeight) TerrainFloor = 0;
		
		shaderToUse.SetFloat("_Div", 255 - ColorSaturation);
		SimplexNoise.init();
		
		sectionDividerH = Height/chunkSizeY;
		sectionDividerW = Width /chunkSizeX;
		
		//hmTexture = new Texture2D(Width, Height);
		//hmTexture.filterMode = FilterMode.Point;
		
		//subBandTexture = new Texture2D(Width, Height);
		
		/*
		NoiseInstruction[] instructions = new NoiseInstruction[3];
		
		for (int i = 0; i < instructions.Length; i++){
			instructions[i] = new NoiseInstruction(SimplexNoise.GenerateWhiteNoise(Width, Height), _INSTRUCTIONTYPE.ADD, NumberOfOctaves, NoisePermanence);
		}
		//*/
		
		float[,] WhiteNoise2D = SimplexNoise.GenerateWhiteNoise(Width, Height);
		
		
		float[,] hmPoints = SimplexNoise.GenerateSmoothSimplex(WhiteNoise2D, NumberOfOctaves, NoisePermanence);
		
		swatch.Start();
		worldMap = new float[Width, maxHeight, Height];
		for (int h = 0; h < Height; h++){
			for (int w = 0; w < Width; w++){
				for (int d = 0; d < maxHeight; d++){
					worldMap[w,d,h] = WhiteNoise2D[w,h];
				}
			}
		}
		worldMap = SimplexNoise.GenerateSmoothSimplex(worldMap, NumberOfOctaves, NoisePermanence);
		swatch.Stop();
		
		combinedTexture = new Texture2D(Width, Height);
		for (int h = 0; h < Height; h++){
			for (int w = 0; w < Width; w++){
				float p = hmPoints[w,h]*maxHeight;
				
				//byte pVal = (byte)((int)Mathf.Max(hmPoints[w,h]*maxHeight - subPoints[w,h]*subBandMaxHeight + ULBand[w,h]*ulMax, 0.0f));
				byte pVal = (byte)((int)(Mathf.Max(p, 0.0f)));
				
				combinedTexture.SetPixel(w,h,new Color32(pVal, pVal, (pVal <= TerrainFloor? (byte)100 : pVal), 255));
			}
		}
		
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		for (int i = 0; i < 2; i++){
			if (groupCountH < sectionDividerH){
				swatch.Start(); // Start Timer
				
				Mesh terrainMesh = new Mesh();
				
				int rX = 0, rY = 0;
				
				//GenFlatMesh(out terrainMesh, out rX, out rY);
				if (Flat){
					GenFlatMesh(out terrainMesh, out rX, out rY);	
				}else{
					//GenCubeMesh(out terrainMesh, out rX, out rY);
					GenChunk2(out terrainMesh, out rX, out rY);
				}
				
				GameObject go = new GameObject("Terrain");
				go.AddComponent<MeshFilter>().mesh = terrainMesh;
				if (shaderToUse == null){
					go.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Diffuse"));
					
				}else{
					go.AddComponent<MeshRenderer>().sharedMaterial = shaderToUse;
					
				}
				
				go.transform.position = new Vector3(rX, 0, rY);
				
				groupCountW ++;
				
				if (groupCountW == sectionDividerW){
					groupCountW = 0;
					groupCountH++;
				}
				
				if (groupCountW == 0){
					Resources.UnloadUnusedAssets();
				}
				if (terrainMesh.vertexCount > largestMeshNumberVerts) largestMeshNumberVerts = terrainMesh.vertexCount;
				
				swatch.Stop(); // Stop Timer
			}else{
				// increase and decrease terrain brightness, like as a time of day cycle
				//shaderToUse.SetFloat("_Div", Mathf.PingPong(Time.time, 10)*10*2.35f + 20);
				
			}
			UnityEngine.Debug.Log(string.Format("Swatch time = {0}sec", swatch.ElapsedMilliseconds/1000f)); // Report Timer
		}
	}
	
	private void GenFlatMesh(out Mesh mesh, out int rX, out int rY){
		mesh = new Mesh();
		
		List<Vector3> verts = new List<Vector3>();
		List<int> tris = new List<int>();
		List<Color32> colors = new List<Color32>();
		
		rY = chunkSizeY * groupCountH;
		rX = chunkSizeX * groupCountW;
		
		Vector3[] quad;
		int quadIndex;
		int vertCount = 0;
		int wi, hj;
		int pixel;
		
		for (int h = rY; h < rY + chunkSizeY; h++){
			for (int w = rX; w < rX + chunkSizeX; w++){
				quad = new Vector3[4];
				quadIndex = 0;
				
				for (int j = 0; j < 2; j++){
					for (int i = 0; i < 2; i++){
						wi = w + i;
						hj = h + j;
						
						//pixel = (int)(hmTexture.GetPixel(wi, hj).r*maxHeight);
						pixel = (int)(combinedTexture.GetPixel(wi, hj).r*255);
						if (pixel < TerrainFloor) pixel = TerrainFloor;
						
						quad[quadIndex++] = new Vector3(wi - rX, (int)(pixel), hj - rY);
						// assign colors based on vertex heights
						colors.Add(new Color32(0, (byte)((int)(pixel & 255)), 0, 255));
						
						/*
						quad[quadIndex++] = new Vector3(wi - rX, (int)(hmTexture.GetPixel(wi, hj).r*maxHeight), hj - rY);
						// assign colors based on vertex heights
						colors.Add(new Color32(0, (byte)((int)(hmTexture.GetPixel(wi, hj).r*maxHeight)&255), 0, 255));
						//*/
					}
				}
				
				verts.AddRange(quad);
				tris.AddRange(new int[]{vertCount + 2, vertCount+1, vertCount});
				tris.AddRange(new int[]{vertCount + 2, vertCount+3, vertCount+1});
				
				
				vertCount += 4;
			}
		}
		
		Vector3[] vertArray = verts.ToArray();
		int[] triArray = tris.ToArray();
		Vector2[] uvArray = new Vector2[vertArray.Length];
		
		for (int i = 0; i < uvArray.Length; i++){
			uvArray[i] = new Vector2(vertArray[i].x, vertArray[i].z);
		}
		
		mesh.vertices = vertArray;
		mesh.triangles = triArray;
		mesh.uv = uvArray;
		mesh.colors32 = colors.ToArray();
		
		mesh.RecalculateNormals();
			
	}
	
	private void GenChunk2(out Mesh mesh, out int rX, out int rY){
		rX = chunkSizeX * groupCountW;
		rY = chunkSizeY * groupCountH;
		
		List<int> facesToGrab = new List<int>();
		List<Vector3> verts = new List<Vector3>();
		List<int> tris = new List<int>();
		
		bool[] dirVals = new bool[]{false, true, true, false, true, false};
		
		int vertCount = 0;
		int locPoint, checkPoint;
		
		Vector3[] quads;
		Vector3[][] locFaces;
		
		for (int h = rY; h < rY + chunkSizeY; h++){
			for (int w = rX; w < rX + chunkSizeX; w++){
				facesToGrab = new List<int>();
				
				//locPoint = (int)(hmTexture.GetPixel(w,h).r*maxHeight);
				locPoint = (int)(combinedTexture.GetPixel(w,h).r*255);
				// adjacent points
				int[] adjPoints = new int[4];
				try{
					//adjPoints[0] = (int)(hmTexture.GetPixel(w+1,h).r*maxHeight);
					adjPoints[0] = (int)(combinedTexture.GetPixel(w+1, h).r*255);
				}catch{
					adjPoints[0] = 0;
				}
				try{
					//adjPoints[1] = (int)(hmTexture.GetPixel(w-1,h).r*maxHeight);
					adjPoints[1] = (int)(combinedTexture.GetPixel(w-1,h).r*255);
				}catch{
					adjPoints[1] = 0;
				}
				try{
					//adjPoints[2] = (int)(hmTexture.GetPixel(w,h+1).r*maxHeight);
					adjPoints[2] = (int)(combinedTexture.GetPixel(w, h+1).r*255);
				}catch{
					adjPoints[2] = 0;
				}
				try{
					//adjPoints[3] = (int)(hmTexture.GetPixel(w,h-1).r*maxHeight);
					adjPoints[3] = (int)(combinedTexture.GetPixel(w, h-1).r*255);
				}catch{
					adjPoints[3] = 0;
				}
				
				
				for (checkPoint = 0; checkPoint <= locPoint; checkPoint++){
					// find the faces that need to be grabbed!
					// for now we always need the top face
					// do we need the top? for now yes
					if (checkPoint == locPoint){
						facesToGrab.Add(0);
					}
					// do we need the bottom? for now NO
					//facesToGrab.Add(1);
					
					// do we need the right?
					try{
						if (checkPoint > adjPoints[0]){
							facesToGrab.Add(2);
						}
					}catch{ facesToGrab.Add (2);}
					// do we need the left?
					try{
						if (checkPoint > adjPoints[1]){
							facesToGrab.Add(3);
						}
					}catch{ facesToGrab.Add (3);}
					// do we need the front?
					try{
						if (checkPoint > adjPoints[3]){
							facesToGrab.Add(5);
						}
					}catch{ facesToGrab.Add (5);}
					// do we need the back?
					try{
						if (checkPoint > adjPoints[2]){
							facesToGrab.Add(4);
						}
					}catch{ facesToGrab.Add(4);}
					
					if (facesToGrab.Count == 0){
						continue;
					}
					
					locFaces = sCube.GetAllFaces(new Vector3(w-rX, checkPoint, h-rY));
					
					for (int i = 0; i < facesToGrab.Count; i++){
						int faceNum = facesToGrab[i];
						
						bool dir = dirVals[faceNum];
						
						// get Triangles
						if (dir){
							tris.AddRange(new int[]{vertCount + 2, vertCount +1, vertCount });
							tris.AddRange(new int[]{vertCount + 2, vertCount +3, vertCount +1});
						}else{
							tris.AddRange(new int[]{vertCount  , vertCount +1, vertCount + 2});
							tris.AddRange(new int[]{vertCount + 1, vertCount +3, vertCount +2});
						}
						
						verts.AddRange(locFaces[faceNum]);
						vertCount+= 4;
					}
				}
				
				/*
				quads = sCube.getFaceAroundPoint(facesToGrab.ToArray(), new Vector3(w-rX, (int)(hmTexture.GetPixel(w,h).r*maxHeight), h-rY));
				
				bool dir = true;
				
				// get Triangles
				for (int t = 0; t < 24; t+= 4){
					if (dir){
						tris.AddRange(new int[]{vertCount + 2, vertCount +1, vertCount });
						tris.AddRange(new int[]{vertCount + 2, vertCount +3, vertCount +1});
					}else{
						tris.AddRange(new int[]{vertCount  , vertCount +1, vertCount + 2});
						tris.AddRange(new int[]{vertCount + 1, vertCount +3, vertCount +2});
					}
					
					vertCount += 4;
					dir = !dir;
				}
				
				verts.AddRange(quads);
				*/
			}
		}
		mesh = new Mesh();
		
		Vector2[] uvTemplate = new Vector2[]{new Vector2(0,0), new Vector2(1,0), new Vector2(0,1), new Vector2(1,1)};
		
		Vector3[] vertArray = verts.ToArray();
		int[] triArray = tris.ToArray();
		Vector2[] uvArray = new Vector2[vertArray.Length];
		
		for (int i = 0; i < uvArray.Length; i++){
			//uvArray[i] = new Vector2(vertArray[i].x, vertArray[i].z);
			uvArray[i] = uvTemplate[i&3];
		}
		
		mesh.vertices = vertArray;
		mesh.triangles = triArray;
		mesh.uv = uvArray;
		
		mesh.RecalculateNormals();
	}
	
	private void GenChunk(out Mesh mesh, out int rX, out int rY){
		List<Vector3> verts = new List<Vector3>();
		List<int> tris = new List<int>();
		List<Color32> colors = new List<Color32>();
		
		rX = chunkSizeX * groupCountW;
		rY = chunkSizeY * groupCountH;
		
		int[,] pNeighbors = new int[3,3];
		int pMin;
		
		Vector3[] quads;
		
		int quadAppSet = 0;
		int vertCount = 0;
		
		
		
		for (int h = rY; h < rY + chunkSizeY; h++){
			for (int w = rX; w < rX + chunkSizeX; w++){
				pMin = maxHeight;
				// fill in pNeighbors from h-1 to h+1 and w-1 to w+1; any out of bounds errors should be resolved by setting the pNeighbor value to 0 height
				for (int i = 0; i < 3; i++){
					for (int j = 0; j < 3; j++){
						try{
							pNeighbors[i,j] = (int)(hmTexture.GetPixel(w+j-1, h+i-1).r*maxHeight);
						}catch{
							pNeighbors[i,j] = 0;
						}
						
						if (pNeighbors[i,j] < pMin) pMin = pNeighbors[i,j];
						
						//UnityEngine.Debug.Log(string.Format("lx={5},ly={6}\tP={0},N={1},E={2},S={3},W={4}",pNeighbors[1,1], pNeighbors[1,0], pNeighbors[2,1], pNeighbors[1,2], pNeighbors[0,1], w,h));
						// pNeighbors[1,1] is the pixel we are positioning the active cubes at
					}
				}
				
				// build cubes from pMin to pNeighbors[1,1] and only keep the faces LRFB when the corresponding neighbor has a height BELOW the working height
				for (int d = pMin; d <= pNeighbors[1,1]; d++){	// <= so that at least 1 cube will always be generated
					// generate the cube faces for this position
					GenCube(w,d,h,rX,rY,d,out quads); // quads are in the form Top, Bottom, Right, Left, Front, Back or +y,-y,+x,-x,-z,+z
					
					/*
					bool dir = true;
					
					// get Triangles
					for (int t = 0; t < 24; t+= 4){
						if (dir){
							tris.AddRange(new int[]{vertCount + 2, vertCount +1, vertCount });
							tris.AddRange(new int[]{vertCount + 2, vertCount +3, vertCount +1});
						}else{
							tris.AddRange(new int[]{vertCount  , vertCount +1, vertCount + 2});
							tris.AddRange(new int[]{vertCount + 1, vertCount +3, vertCount +2});
						}
						
						vertCount += 4;
						dir = !dir;
					}
					
					verts.AddRange(quads);
					*/
					
					
				}
			}
		}
		
		mesh = new Mesh();
		
		Vector2[] uvTemplate = new Vector2[]{new Vector2(0,0), new Vector2(1,0), new Vector2(0,1), new Vector2(1,1)};
		
		Vector3[] vertArray = verts.ToArray();
		int[] triArray = tris.ToArray();
		Vector2[] uvArray = new Vector2[vertArray.Length];
		
		for (int i = 0; i < uvArray.Length; i++){
			//uvArray[i] = new Vector2(vertArray[i].x, vertArray[i].z);
			uvArray[i] = uvTemplate[i&3];
		}
		
		mesh.vertices = vertArray;
		mesh.triangles = triArray;
		mesh.uv = uvArray;
		mesh.colors32 = colors.ToArray();
		
		mesh.RecalculateNormals();
	}
	
	private void GenCubeMesh(out Mesh mesh, out int rX, out int rY){
		mesh = new Mesh();
		
		List<Vector3> verts = new List<Vector3>();
		List<int> tris = new List<int>();
		List<Color32> colors = new List<Color32>();
		
		rY = chunkSizeY * groupCountH;
		rX = chunkSizeX * groupCountW;
		
		Vector3[] quad;
		int quadIndex;
		int vertCount = 0;
		int wi, hj;
		int pixel;
		
		// go through each point in the hm and make a cube at that position
		for (int h = rY; h < rY + chunkSizeY; h++){
			for (int w = rX; w < rX + chunkSizeX; w++){
				
				// get Verts
				GenCube(w,(int)(hmTexture.GetPixel(w, h).r*maxHeight), h, rX, rY, TerrainFloor-1, out quad);
				//Debug.Log(quad.Length);
				bool dir = true;
				
				// get Triangles
				for (int t = 0; t < 24; t+= 4){
					if (dir){
						tris.AddRange(new int[]{vertCount + 2, vertCount +1, vertCount });
						tris.AddRange(new int[]{vertCount + 2, vertCount +3, vertCount +1});
					}else{
						tris.AddRange(new int[]{vertCount  , vertCount +1, vertCount + 2});
						tris.AddRange(new int[]{vertCount + 1, vertCount +3, vertCount +2});
					}
					
					vertCount += 4;
					dir = !dir;
				}
				
				/*
				quad = new Vector3[4];
				quadIndex = 0;
				
				for (int j = 0; j < 2; j++){
					for (int i = 0; i < 2; i++){
						wi = w + i;
						hj = h + j;
						
						pixel = (int)(hmTexture.GetPixel(wi, hj).r*maxHeight);
						if (pixel < TerrainFloor) pixel = TerrainFloor;
						
						quad[quadIndex++] = new Vector3(wi - rX, (int)(pixel), hj - rY);
						// assign colors based on vertex heights
						colors.Add(new Color32(0, (byte)((int)(pixel & 255)), 0, 255));
						
						
						quad[quadIndex++] = new Vector3(wi - rX, (int)(hmTexture.GetPixel(wi, hj).r*maxHeight), hj - rY);
						// assign colors based on vertex heights
						colors.Add(new Color32(0, (byte)((int)(hmTexture.GetPixel(wi, hj).r*maxHeight)&255), 0, 255));
						
					}
				}
				//*/
				verts.AddRange(quad);
				//tris.AddRange(new int[]{vertCount + 2, vertCount+1, vertCount});
				//tris.AddRange(new int[]{vertCount + 2, vertCount+3, vertCount+1});
				
				
				//vertCount += 24;
			}
		}
		Vector2[] uvTemplate = new Vector2[]{new Vector2(0,0), new Vector2(1,0), new Vector2(0,1), new Vector2(1,1)};
		
		Vector3[] vertArray = verts.ToArray();
		int[] triArray = tris.ToArray();
		Vector2[] uvArray = new Vector2[vertArray.Length];
		
		for (int i = 0; i < uvArray.Length; i++){
			//uvArray[i] = new Vector2(vertArray[i].x, vertArray[i].z);
			uvArray[i] = uvTemplate[i&3];
		}
		
		mesh.vertices = vertArray;
		mesh.triangles = triArray;
		mesh.uv = uvArray;
		mesh.colors32 = colors.ToArray();
		
		mesh.RecalculateNormals();
			
	}
	private void GenCube(int pX, int pY, int pZ, int rX, int rY, int pYLow, out Vector3[] vertArray){
		// create vertices (x24)
		List<Vector3> verts = new List<Vector3>();
		// top +y
		Vector3 dP = Vector3.one;
		
		float aX, aY, aZ;
		
		dP.y = +0.5f;
		
		for (int z = -1; z < 2; z+=2){
			for (int x = -1; x < 2; x+=2){
				aX = x*0.5f+pX;
				aY = dP.y + pY;
				aZ = z*0.5f+pZ;
				
				verts.Add(new Vector3(aX, aY, aZ));
			}
		}
		
		// bottom -y
		dP.y = -0.5f;
		
		for (int z = -1; z < 2; z+=2){
			for (int x = -1; x < 2; x+=2){
				aX = x*0.5f+pX;
				aY = dP.y + pYLow;
				aZ = z*0.5f+pZ;
				
				verts.Add(new Vector3(aX, aY, aZ));
			}
		}
		
		// right +x
		dP.x = +0.5f;
		
		for (int y = -1; y < 2; y+=2){
			for (int z = -1; z < 2; z+=2){
				aX = dP.x + pX;
				if (y == -1){
					aY = y*0.5f+pYLow;
				}else{
					aY = y*0.5f+pY;
				}
				aZ = z*0.5f+pZ;
				verts.Add(new Vector3(aX, aY, aZ));
			}
		}
		
		// left -x
		dP.x = -0.5f;
		
		for (int y = -1; y < 2; y+=2){
			for (int z = -1; z < 2; z+=2){
				aX = dP.x + pX;
				if (y == -1){
					aY = y*0.5f+pYLow;
				}else{
					aY = y*0.5f+pY;
				}
				aZ = z*0.5f+pZ;
				
				verts.Add(new Vector3(aX, aY, aZ));
			}
		}
		
		// front -z
		dP.z = -0.5f;
		
		for (int y = -1; y < 2; y+=2){
			for (int x = -1; x < 2; x+=2){
				aX = x*0.5f + pX;
				if (y == -1){
					aY = y*0.5f+pYLow;
				}else{
					aY = y*0.5f+pY;
				}
				aZ = dP.z + pZ;
				
				verts.Add(new Vector3(aX, aY, aZ));
			}
		}
		
		// back +z
		dP.z = +0.5f;
		
		for (int y = -1; y < 2; y+=2){
			for (int x = -1; x < 2; x+=2){
				aX = x*0.5f + pX;
				if (y == -1){
					aY = y*0.5f+pYLow;
				}else{
					aY = y*0.5f+pY;
				}
				aZ = dP.z + pZ;
				
				verts.Add(new Vector3(aX, aY, aZ));
			}
		}
		
		vertArray = verts.ToArray();
		
		for (int i = 0; i < 24; i++){
			vertArray[i].x -= rX;
			vertArray[i].z -= rY;
		}
		
		
		
	}
}

