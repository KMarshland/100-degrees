using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Chunk
{
	// world-log-size is 4 per point. Can be expanded to Vector3I.right * 4 + Vector3I.forward * 4 + Vector3I.up * 4;
	public static Vector3I CHUNK_LOG_SIZE = Vector3I.one * 4;
	
	public static Vector3I CHUNK_SIZE = Vector3I.LShift(1,CHUNK_LOG_SIZE);
	public static Vector3I CHUNK_MASK = CHUNK_SIZE - Vector3I.one;
	
	private GBlock[,,] m_chunkBlocks;
	// location denotes the bottom left back corner of this chunk
	private Vector3I m_location;
	
	private static WorldManager m_WM;
	
	public bool noiseGenerated = false;
	
	public static WorldManager wm{
		set{
			if (m_WM == null){
				m_WM = value;
			}
		}
	}
	
	public Vector3I location{
		get{
			return m_location;
		}
	}
	public GBlock this[Vector3I loc]{
		get{
			return m_chunkBlocks[loc.x,loc.y,loc.z];
		}
		set{
			m_chunkBlocks[loc.x,loc.y,loc.z] = value;
		}
	}
	public GBlock this[int x, int y, int z]{
		get{
			return m_chunkBlocks[x,y,z];
		}
		set{
			m_chunkBlocks[x, y, z] = value;
		}
	}
	
	// if generateImmediate is not specified then the chunk will immediately begin filling out its body
	public Chunk(Vector3I location, bool generateImmediate = true){
		m_location = location;
		
		m_chunkBlocks = new GBlock[CHUNK_SIZE[0], CHUNK_SIZE[1], CHUNK_SIZE[2]];
		
		
		
		if (generateImmediate){
			Vector3I blockPos;
			// assume the Simplex Noise generator has been initialized already, and that a texture has been created that describes the terrain
			for (int x = 0; x < CHUNK_SIZE[0]; x++){
				for (int z = 0; z < CHUNK_SIZE[2]; z++){
					bool solidFound = false;
					for (int y = CHUNK_MASK[1]; y >= 0; y--){
						blockPos = m_location + new Vector3I(x,y,z);
						
						try{
							if (!solidFound){
								if (m_WM.worldSurface[blockPos[0], blockPos[2]] <= blockPos[1]){
									// creates a DirtBlock here
									m_chunkBlocks[x,y,z] = new GBlock(1);
									solidFound = true;
								}else{
									// creates a AirBlock here
									m_chunkBlocks[x,y,z] = new GBlock(0);
								}
							}else{
								m_chunkBlocks[x,y,z] = new GBlock(1);
							}
						}catch{
							Debug.Log(string.Format("<x,y,z> = <{0},{1},{2}>", x,y,z));
							Debug.Log(string.Format("m_Location = <{0},{1},{2}>", m_location[0], m_location[1], m_location[2]));
							throw new Exception();
						}
					}
				}
			}
			noiseGenerated = true;
		}
	}
	
	public static CMesh GenerateChunkMesh(Chunk c){
		Vector3I cPos = c.location;
		Vector3I cSiz = Chunk.CHUNK_SIZE;
		
		float scale = 0.5f;
		CMesh chunkMesh = new CMesh();
		
		List<Vector3> verts = new List<Vector3>();
		List<bool> triangleDirections = new List<bool>();
		List<int> tris = new List<int>();
		
		// for now forego color designations since lighting is not yet implemented. Lighting will need to be done BEFORE generating a mesh
		
		for (int x = 0; x < cSiz.x; x++){
			for (int y = 0; y < cSiz.y; y++){
				for (int z = 0; z < cSiz.z; z++){
					Vector3I workerLoc = new Vector3I(x,y,z);
					GBlock worker = c[workerLoc];
					
					// if this is a solid position then we now need to look to see what faces if any are visible
					if (worker.IsSolid()){
						// BLOCK GETTERS - In each case try to first get the block locally through the chunk itself, only polling the worldmanager if absolutely necessary
						// look for the GBlock on top of the worker
						GBlock top, bottom, right, left, forward, back;
						try{
							top = c[workerLoc + Vector3I.up];
						}catch{
							top = m_WM[cPos + workerLoc + Vector3I.up];
						}
						// Bottom
						try{
							bottom = c[workerLoc + Vector3I.down];
						}catch{
							bottom = m_WM[cPos + workerLoc + Vector3I.down];
						}
						// Right
						try{
							right = c[workerLoc + Vector3I.right];
						}catch{
							right = m_WM[cPos + workerLoc + Vector3I.right];
						}
						// Left
						try{
							left = c[workerLoc + Vector3I.left];
							
						}catch{
							left = m_WM[cPos + workerLoc + Vector3I.left];
						}
						// Forward
						try{
							forward = c[workerLoc + Vector3I.forward];
							
						}catch{
							forward = m_WM[cPos + workerLoc + Vector3I.forward];
						}
						// Backward
						try{
							back = c[workerLoc + Vector3I.back];
							
						}catch{
							back = m_WM[cPos + workerLoc + Vector3I.back];
						}
						
						List<Vector3I> tVerts = new List<Vector3I>();
						if (!top.IsSolid()){
							triangleDirections.Add(GBlock.getFaceTriDir(0));
							tVerts.AddRange(GBlock.GetFace(0));
						}
						if (!bottom.IsSolid()){
							triangleDirections.Add(GBlock.getFaceTriDir(1));
							tVerts.AddRange(GBlock.GetFace(1));
						}
						if (!right.IsSolid()){
							triangleDirections.Add(GBlock.getFaceTriDir(2));
							tVerts.AddRange(GBlock.GetFace(2));
						}
						if (!left.IsSolid()){
							triangleDirections.Add(GBlock.getFaceTriDir(3));
							tVerts.AddRange(GBlock.GetFace(3));
						}
						if (!forward.IsSolid()){
							triangleDirections.Add(GBlock.getFaceTriDir(4));
							tVerts.AddRange(GBlock.GetFace(4));
						}
						if (!back.IsSolid()){
							triangleDirections.Add(GBlock.getFaceTriDir(5));
							tVerts.AddRange(GBlock.GetFace(5));
						}
						
						foreach (Vector3I v in tVerts){
							verts.Add(v * scale + workerLoc*1.0f + cPos * 1.0f);
						}
					}
				}
			}
		}
		
		int vertCount = 0;
		foreach (bool dir in triangleDirections){
			if (dir){
				tris.AddRange(new int[]{vertCount + 2, vertCount +1, vertCount });
				tris.AddRange(new int[]{vertCount + 2, vertCount +3, vertCount +1});
			}else{
				tris.AddRange(new int[]{vertCount  , vertCount +1, vertCount + 2});
				tris.AddRange(new int[]{vertCount + 1, vertCount +3, vertCount +2});
			}
			vertCount += 4;
		}
		// set up the UVs
		Vector2[] uvs = new Vector2[verts.Count];
		Vector2[] uvTemplate = new Vector2[]{new Vector2(0,0), new Vector2(1,0), new Vector2(0,1), new Vector2(1,1)};
		for (int i = 0; i < uvs.Length; i++){
			uvs[i] = uvTemplate[i&3];
		}
		
		chunkMesh.vertices = verts.ToArray();
		chunkMesh.triangles = tris.ToArray();
		chunkMesh.uvs = uvs;
		
		return chunkMesh;
	}
}

