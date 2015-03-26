using UnityEngine;
using System.Collections;

public class Flow : MonoBehaviour {
	
	public Vector2 grid;
	
	private GameObject[,] bouys;
	
	public Mesh bouyMesh;
	public UnityEngine.Material bouyMaterial;
	
	public float timeSkipSeconds = 5;
	public float skipSoFar = 0;
	
	private FlowGrid fGrid;
	// Use this for initialization
	void Start () {
		// create grid
		bouys = new GameObject[(int)grid.x, (int)grid.y];
		
		fGrid = new FlowGrid((int)grid.x, (int)grid.y);
		
		for (int i = 0; i < grid.x; i++){
			for (int j = 0; j < grid.y; j++){
				GameObject temp = new GameObject(string.Format("Bouy [{0},{1}]", i,j));
				
				// add mesh to it
				temp.AddComponent<MeshFilter>().sharedMesh = bouyMesh;
				
				// add renderer to it
				temp.AddComponent<MeshRenderer>();
				temp.GetComponent<Renderer>().sharedMaterial = bouyMaterial;
				
				// move it to location in fGrid
				temp.transform.position = fGrid[i,j];
				
				bouys[i,j] = temp;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (skipSoFar <= 0){
			fGrid.update();
			for (int j = 0; j < grid.y; j++){
				for (int i = 0; i < grid.x; i++){
					bouys[i,j].transform.position = fGrid[i,j];
				}
			}
			skipSoFar = timeSkipSeconds;
		}else{
			skipSoFar -= Time.deltaTime;
		}
	}
}
