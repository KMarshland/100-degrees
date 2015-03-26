using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Diagnostics;

using NoiseLibrary;

public class WorldManager : MonoBehaviour
{
	public Texture2D worldTexture;
	public Vector3 worldChunkNumbers;
	
	public Vector3I clampedChunkNumbers{
		get{
			return Vector3I.CastToVector3I(worldChunkNumbers);
		}
	}
	
	public float[,] worldSurface;
	
	Chunk[,,] worldChunks;
	Queue<Chunk> chunkQueue;
	
	public Material chunkMat;
	private Stopwatch swatch;
	private Stopwatch allTime;
	
	public GBlock this[Vector3I loc]{
		get{
			Vector3I cLoc = Vector3I.RShift(loc, Chunk.CHUNK_LOG_SIZE);
			
			if (cLoc[0] < 0 || cLoc[0] >= clampedChunkNumbers[0]){
				return new GBlock(0);
			}
			if (cLoc[1] < 0 || cLoc[1] >= clampedChunkNumbers[1]){
				return new GBlock(0);
			}
			if (cLoc[2] < 0 || cLoc[2] >= clampedChunkNumbers[2]){
				return new GBlock(0);
			}
			
			Chunk chunk = worldChunks[cLoc[0], cLoc[1], cLoc[2]];
			
			Vector3I bLoc = Vector3I.AShift(loc, Chunk.CHUNK_MASK);
			
			return chunk[bLoc]; // temporary measure so I can figure out what to do exactly hehe
		}
	}
	
	// Use this for initialization
	void Start ()
	{
		swatch = new Stopwatch();
		allTime = new Stopwatch();
		
		allTime.Start();
		
		swatch.Start();
		SimplexNoise.init();
		Chunk.wm = this;
		
		worldChunks = new Chunk[clampedChunkNumbers[0], clampedChunkNumbers[1], clampedChunkNumbers[2]];
		
		//float[,] noise = SimplexNoise.GenerateSmoothSimplex(SimplexNoise.GenerateWhiteNoise(clampedChunkNumbers[0] * Chunk.CHUNK_SIZE[0], clampedChunkNumbers[2]*Chunk.CHUNK_SIZE[2]), 8, 0.4f);
		worldSurface = SimplexNoise.GenerateSmoothSimplex(SimplexNoise.GenerateWhiteNoise(clampedChunkNumbers[0] * Chunk.CHUNK_SIZE[0], clampedChunkNumbers[2]*Chunk.CHUNK_SIZE[2]), 8, 0.4f);
		//worldSurface = new float[clampedChunkNumbers[0]*Chunk.CHUNK_SIZE[0], clampedChunkNumbers[2]*Chunk.CHUNK_SIZE[2]];
		// normalize values in WorldSurface to between 0 and worldChunkNumbers[1]*Chunk.CHUNK_SIZE[1]-1
		for (int x = 0; x < worldSurface.GetLength(0); x++){
			for (int z = 0; z < worldSurface.GetLength(1); z++){
				worldSurface[x,z] = (int)(worldSurface[x,z]*(clampedChunkNumbers[1]*Chunk.CHUNK_SIZE[1]));
			}
		}
		
		// create the chunks!
		chunkQueue = new Queue<Chunk>();
		for (int x = 0; x < worldChunks.GetLength(0); x++){
			for (int y = 0; y < worldChunks.GetLength(1); y++){
				for (int z = 0; z < worldChunks.GetLength(2); z++){
					worldChunks[x,y,z] = new Chunk(new Vector3I(x,y,z) * Chunk.CHUNK_SIZE, true);
					
					// add the working chunk to a queue so that it can be processed into a mesh
					chunkQueue.Enqueue(worldChunks[x,y,z]);
				}
			}
		}
		swatch.Stop();
		UnityEngine.Debug.Log(string.Format("{0}ms to generate all chunks", swatch.ElapsedMilliseconds));
		
		swatch.Reset();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (chunkQueue.Count > 0){
			swatch.Start();
			Chunk c = chunkQueue.Dequeue();
			
			if (c.noiseGenerated){
				
				CMesh cMesh = Chunk.GenerateChunkMesh(c);
				
				GameObject cObject = new GameObject("Chunk");
				
				cObject.transform.parent = transform;
				
				Mesh cOMesh = new Mesh();
				cOMesh.vertices = cMesh.vertices;
				cOMesh.triangles = cMesh.triangles;
				cOMesh.uv = cMesh.uvs;
				cOMesh.RecalculateNormals();
				
				cObject.AddComponent<MeshFilter>().mesh = cOMesh;
				cObject.AddComponent<MeshRenderer>().sharedMaterial = chunkMat;
				swatch.Stop();
				
				UnityEngine.Debug.Log(string.Format("{0}ms to generate Chunk mesh - {1} remaining", swatch.ElapsedMilliseconds, chunkQueue.Count));
				swatch.Reset();
				
				if (chunkQueue.Count == 0){
					allTime.Stop();
					UnityEngine.Debug.Log(string.Format("{0}ms to complete everything", allTime.ElapsedMilliseconds));
			}
			}else{
				chunkQueue.Enqueue(c);
			}
		}
	}
}

