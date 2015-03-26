using UnityEngine;
using System.Collections;

using NoiseLibrary;
using System.Diagnostics;

public class ChunkGenerator : MonoBehaviour {
	#region Public Properties
	
	public int distanceToCheck = 5;
	public Color specColor = Color.white;
	public Material BlockMat;
	
	#endregion
	
	// Use this for initialization
	void Start () {
		Stopwatch swatch = new Stopwatch();
		swatch.Start();
		Vector3I[] blocks = BlockIdentifier.GetBlockPositionsWithinSphericalRadius(Vector3I.zero, distanceToCheck);
		swatch.Stop();
		
		
		UnityEngine.Debug.Log(string.Format("{1}x Points within {0} distance of Vector3I.zero\nTook {2}ms", distanceToCheck, blocks.Length, swatch.ElapsedMilliseconds));
		
		/*
		foreach (Vector3I b in blocks){
			Debug.Log(string.Format("Block: <{0},{1},{2}>", b.x, b.y, b.z));
		}
		*/
		
		// for visualization purposes, instantiate a cube at each point
		swatch.Reset();
		swatch.Start();
		foreach (Vector3I b in blocks){
			int distance = Vector3I.EDistance(b, Vector3I.zero);
			
			if ((b == Vector3I.zero || distance == distanceToCheck) && b.z >= 0 ){
				GameObject block = GameObject.CreatePrimitive(PrimitiveType.Cube);
				block.transform.position = Vector3I.CastToVector3(b);
				
				
				Color c = specColor/Mathf.Sqrt(distance);
				block.GetComponent<Renderer>().sharedMaterial = BlockMat;
				
				// ColorSet
				Color[] colors = new Color[24];
				
				if (b == Vector3I.zero){
					for (int i = 0; i < 24; i++){
						colors[i] = Color.blue;
					}
					block.GetComponent<MeshFilter>().mesh.colors = colors;
				}else{
					for (int i = 0; i < 24; i++){
						colors[i] = c;
					}
					block.GetComponent<MeshFilter>().mesh.colors = colors;
				}
			}
		}
		swatch.Stop();
		UnityEngine.Debug.Log(string.Format("GOs created: Took {0}ms", swatch.ElapsedMilliseconds));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
