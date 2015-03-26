using UnityEngine;
using System.Collections;
using KofTools;

public class Pipe : SteamObject {

	static GameObject PipeParent;

	float corrosion;
	Metal material;
	public int type;
	public int lastType;
	
	public float pipelength;
	public float offSet; //How far to move to the right
	GameObject straightPipe;
	
		
	public bool northOpen, southOpen, eastOpen, westOpen, topOpen, bottomOpen;
	
	public void starterate(Metal mat){
		if (PipeParent == null){
			PipeParent = new GameObject("Pipe Parent");
		}

		material = mat;
		/*calculateAdjacentSteams();
		calculatePressure();
		calculateWater();
		updatePos();
		checkSurroundings();*/
		base.updatePos();
		reEnqueue();

		this.transform.parent = PipeParent.transform;
	}
	
	public void reEnqueue(){
		ConstructionController.pipes.Add(this);
	}
	
	public void disEnqueue(){
		//Debug.Log("First: " + ConstructionController.pipes.Count);
		//Debug.Log(ConstructionController.pipes.Contains(this));
		ConstructionController.pipes.Remove(this);
		//Debug.Log("Second: " + ConstructionController.pipes.Count);
	}
	
	public void corrode(){
		corrosion += 0.01f/(float)material.corrosionResistance;
		if (corrosion >= 100.0f){
			ConstructionController.pipes.Remove(this);
			GameObject.Destroy(this.gameObject);
		}
	}
	
	public new void calculatePressure(){
		base.calculatePressure();
		pressure -= (int)(corrosion / 10);
		// Reduce pressure if it's a junction
		if (type > 7 && type < 12){
			pressure /= 2;
		} else if (type == 16){
			pressure /= 3;
		}
	}
	
	public void checkSurroundings(){
		/// Types http://i.imgur.com/pb2Uf.png
		/// 0 = EoWc |-
		/// 1 = WoEc -|
		/// 2 = NoSc u
		/// 3 = SoNc n
		/// 4 = UoDc ..u
		/// 5 = DoUc ..n
		/// 6 = NS ||
		/// 7 = EW =
		/// 8 = EWS T
		/// 9 = EWN
		/// 10 = NSE
		/// 11 = NSW
		/// 12 = ES
		/// 13 = EN
		/// 14 = WN
		/// 15 = WS
		/// 16 = EWNS
		/// 17 = UD
		/// 18 = UDNSEW
		/// 19 = ND
		/// 20 = SD
		/// 21 = ED
		/// 22 = WD
		/// 23 = NU
		/// 24 = SU
		/// 25 = EU
		/// 26 = WU
		
		lastType = type;
		
		northOpen = false;
		southOpen = false;
		eastOpen = false;
		westOpen = false;
		topOpen = false;
		bottomOpen = false;
		
		foreach (SteamObject s in adjacentSteams){
			if (s.x > x){
				if (s.y == y && s.z == z){
					eastOpen = true;
				}
			} else if (s.x < x){
				if (s.y == y && s.z == z){
					westOpen = true;
				}
			}
			
			if (s.y > y){
				if (s.x == x && s.z == z){
					topOpen = true;
				}
			} else if (s.y < y){
				if (s.x == x && s.z == z){
					bottomOpen = true;
				}
			}
			
			if (s.z > z){
				if (s.y == y && s.x == x){
					northOpen = true;
				}
			} else if (s.z < z){
				if (s.y == y && s.x == x){
					southOpen = true;
				}
			}
		}
		
		type = -1;
		
		if (!topOpen && !bottomOpen && !northOpen && !southOpen && eastOpen && !westOpen){
			type = 0;
		}
		
		if (!topOpen && !bottomOpen && !northOpen && !southOpen && !eastOpen && westOpen){
			type = 1;
		}
		
		if (!topOpen && !bottomOpen && northOpen && !southOpen && !eastOpen && !westOpen){
			type = 2;
		}
		
		if (!topOpen && !bottomOpen && !northOpen && southOpen && !eastOpen && !westOpen){
			type = 3;
		}
		
		if (topOpen && !bottomOpen && !northOpen && !southOpen && !eastOpen && !westOpen){
			type = 4;
		}
		
		if (!topOpen && bottomOpen && !northOpen && !southOpen && !eastOpen && !westOpen){
			type = 5;
		}
		
		if (!topOpen && !bottomOpen && northOpen && southOpen && !eastOpen && !westOpen){
			type = 6;
		}
		
		if (!topOpen && !bottomOpen && !northOpen && !southOpen && eastOpen && westOpen){
			type = 7;
		}
		
		if (!topOpen && !bottomOpen && !northOpen && southOpen && eastOpen && westOpen){
			type = 8;
		}
		
		if (!topOpen && !bottomOpen && northOpen && !southOpen && eastOpen && westOpen){
			type = 9;
		}
		
		if (!topOpen && !bottomOpen && northOpen && southOpen && eastOpen && !westOpen){
			type = 10;
		}
		
		if (!topOpen && !bottomOpen && northOpen && southOpen && !eastOpen && westOpen){
			type = 11;
		}
		
		if (!topOpen && !bottomOpen && !northOpen && southOpen && !eastOpen && westOpen){
			type = 12;
		}
		
		if (!topOpen && !bottomOpen && northOpen && !southOpen && !eastOpen && westOpen){
			type = 13;
		}
		
		if (!topOpen && !bottomOpen && northOpen && !southOpen && eastOpen && !westOpen){
			type = 14;
		}
		
		if (!topOpen && !bottomOpen && !northOpen && southOpen && eastOpen && !westOpen){
			type = 15;
		}
		
		if (!topOpen && !bottomOpen && northOpen && southOpen && eastOpen && westOpen){
			type = 16;
		}
		
		if (topOpen && bottomOpen && !northOpen && !southOpen && !eastOpen && !westOpen){
			type = 17;
			transform.position = new Vector3((float)x, (float)(y - 0.55f), (float)z);
		}
		
		if (topOpen && bottomOpen && northOpen && southOpen && eastOpen && westOpen){
			type = 18;
		}
		
		if (!topOpen && bottomOpen && northOpen && !southOpen && !eastOpen && !westOpen){
			type = 19;
		}
		
		if (!topOpen && bottomOpen && !northOpen && southOpen && !eastOpen && !westOpen){
			type = 20;
		}
		
		if (!topOpen && bottomOpen && !northOpen && !southOpen && eastOpen && !westOpen){
			type = 21;
		}
		
		if (!topOpen && bottomOpen && !northOpen && !southOpen && !eastOpen && westOpen){
			type = 22;
		}
		
		if (topOpen && !bottomOpen && northOpen && !southOpen && !eastOpen && !westOpen){
			type = 23;
		}
		
		if (topOpen && !bottomOpen && !northOpen && southOpen && !eastOpen && !westOpen){
			type = 24;
		}
		
		if (topOpen && !bottomOpen && !northOpen && !southOpen && eastOpen && !westOpen){
			type = 25;
		}
		
		if (topOpen && !bottomOpen && !northOpen && !southOpen && !eastOpen && westOpen){
			type = 26;
		}

		
	}
	
	public void updateGraphics(){
		disEnqueue();
		if (type == 0){
			switchTo("Prefabs/EndPipePrefab", new Vector3(-90f, 0f, 90f));
		} else if (type == 1){
			switchTo("Prefabs/EndPipePrefab",new Vector3(-90f, 0f, 270f));
		} else if (type == 2){
			switchTo("Prefabs/EndPipePrefab",new Vector3(-90f, 0f, 0f));
		} else if (type == 3){
			switchTo("Prefabs/EndPipePrefab", new Vector3(-90f, 0f, 180f));
			//p.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
		} else if (type == 4){
			switchTo("Prefabs/EndPipePrefab",new Vector3(180f, 0f, 0f));
		} else if (type == 5){
			switchTo("Prefabs/EndPipePrefab",new Vector3(0f, 0f, 0f));
		} else if (type == 6){
			switchTo("Prefabs/StraightPipePrefab",new Vector3(90f, 0f, 0f));
		} else if (type == 7){
			switchTo("Prefabs/StraightPipePrefab",new Vector3(0f, 0f, 90f));
		} else if (type == 8){
			switchTo("Prefabs/TBendPipePrefab",new Vector3(-90f, 0f, 270f));
		} else if (type == 9){
			switchTo("Prefabs/TBendPipePrefab",new Vector3(-90f, 0f, 90f));
		} else if (type == 10){
			switchTo("Prefabs/TBendPipePrefab",new Vector3(-90f, 0f, 180f));
		} else if (type == 11){
			switchTo("Prefabs/TBendPipePrefab",new Vector3(90f, 0f, 0f));
		} else if (type == 12){
			switchTo("Prefabs/LBendPipePrefab",new Vector3(0f, 270f, 90f));
		} else if (type == 13){
			switchTo("Prefabs/LBendPipePrefab",new Vector3(0f, 0f, 90f));
		} else if (type == 14){
			switchTo("Prefabs/LBendPipePrefab",new Vector3(0f, 90f, 90f));
		} else if (type == 15){
			switchTo("Prefabs/LBendPipePrefab",new Vector3(0f, 180f, 90f));
		} else if (type == 16){
			
		} else if (type == 17){
			switchTo("Prefabs/StraightPipePrefab",new Vector3(0f, 90f, 0f));
		} else if (type == 18){
			
		} else if (type == 19){
			switchTo("Prefabs/LBendPipePrefab",new Vector3(0f, 0f, 180f));
		} else if (type == 20){
			switchTo("Prefabs/LBendPipePrefab",new Vector3(180f, 0f, 180f));
		} else if (type == 21){
			switchTo("Prefabs/LBendPipePrefab",new Vector3(90f, 0f, 90f));
		} else if (type == 22){
			switchTo("Prefabs/LBendPipePrefab",new Vector3(0f, 270f, 180f));
		} else if (type == 23){
			switchTo("Prefabs/LBendPipePrefab",new Vector3(0f, 0f, 0f));
		} else if (type == 24){
			switchTo("Prefabs/LBendPipePrefab",new Vector3(180f, 0f, 0f));
		} else if (type == 25){
			switchTo("Prefabs/LBendPipePrefab",new Vector3(90f, 0f, 0f));
		} else if (type == 26){
			switchTo("Prefabs/LBendPipePrefab",new Vector3(270f, 0f, 0f));
		}
		
	}

	void switchTo(string type, Vector3 rotation){
		GameObject obj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(type));
		Pipe p = (Pipe)obj.transform.GetComponent<Pipe>();
		transform.rotation = Quaternion.identity;
		transform.Rotate(rotation);
		transferData(p);
	}
	
	void transferData(Pipe p){
		p.transform.position = this.transform.position;
		p.type = this.type;
		p.lastType = this.lastType;
		p.transform.rotation = this.transform.rotation;
		p.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
		p.activated = true;
		p.pipelength = this.pipelength;
		p.straightPipe = this.straightPipe;
		p.starterate(this.material);
		disEnqueue();
		GameObject.Destroy(this.gameObject);
	}
	
	// Use this for initialization
	void Start () {
		pressure = 0;
		isCarryingWater = false;
		offSet = 0f;
		base.updatePos();
		updatePos();
		if (this.material == null){
			this.material = new Metal(Metal.MetalType.Aluminum);
			starterate(this.material);
		}
	}
	
	// Update is called once per frame
	void Update () {
		calculateAdjacentSteams();
		calculatePressure();
	}
	
	public new void updatePos(){
		
		try {
			scale();
		} catch {
			//Debug.Log("Poop");
		}
		
		if (type == 7){
			transform.position = new Vector3((float) x, (float) y, (float)z);
		} else if (type == 6){
			transform.position = new Vector3((float) x, (float) y, (float)z);
		}
	}
	
	public void scale(){
		
		float max = 0;
		
		if (type == 6){
			this.GetComponent<Renderer>().enabled = true;
			foreach (SteamObject p in adjacentSteams){
				if (p.GetType().ToString().Equals("Pipe")){
					Pipe pi = (Pipe) p;
					if (pi.type == 6){
						if (p.z > z){
							try {
								pipelength = (1 * pi.pipelength) + 1;
							} catch {
								Debug.Log("SQUAWK");
							}
						} else {
							//this.renderer.enabled = false;
							if (pi.pipelength > max){
								max = pi.pipelength;
							}
						}
					}
				}
			}
			
			this.GetComponent<Renderer>().enabled = false;
			
			try {
				//GameObject.Destroy(straightPipe);
			} catch {
				
			}
			
			if (pipelength >= max){
				offSet = ((pipelength - 0f)/1.2f) + 0.5f;
				
				if (straightPipe == null){
					straightPipe = (GameObject) GameObject.Instantiate(Resources.Load("Prefabs/StraightPipeSectionPrefab"));
				}
					
				straightPipe.transform.rotation = Quaternion.identity;
				straightPipe.transform.Rotate(new Vector3(90f, 0f, 0f));
				straightPipe.transform.position = new Vector3(x, y, z + offSet);
				straightPipe.transform.localScale = new Vector3(1f, (0.14f * pipelength), 1f);
				
				
				//Debug.Log("This: " + pipelength + " Max: " + max);
				
			} else {
				if (straightPipe != null){
					GameObject.Destroy(straightPipe);
				}
			}
		} else if (type == 7){
			this.GetComponent<Renderer>().enabled = true;
			foreach (SteamObject p in adjacentSteams){
				if (p.GetType().ToString().Equals("Pipe")){
					Pipe pi = (Pipe) p;
					if (pi.type == 7){
						if (p.x > x){
							try {
								pipelength = (1 * pi.pipelength) + 1;
							} catch {
								Debug.Log("SQUAWK");
							}
						} else {
							//this.renderer.enabled = false;
							if (pi.pipelength > max){
								max = pi.pipelength;
							}
						}
					}
				}
			}
			
			this.GetComponent<Renderer>().enabled = false;
			
			try {
				//GameObject.Destroy(straightPipe);
			} catch {
				
			}
			
			if (pipelength >= max){
				offSet = ((pipelength - 0f)/1.2f) + 0.75f;
				
				if (straightPipe == null){
					straightPipe = (GameObject) GameObject.Instantiate(Resources.Load("Prefabs/StraightPipeSectionPrefab"));
				}
					
				straightPipe.transform.rotation = Quaternion.identity;
				straightPipe.transform.Rotate(new Vector3(90, 90, 0));
				straightPipe.transform.position = new Vector3(x + offSet, y, z);
				straightPipe.transform.localScale = new Vector3(1f, (0.14f * pipelength), 1f);
				
				
				//Debug.Log("This: " + pipelength + " Max: " + max);
				
			} else {
				if (straightPipe != null){
					GameObject.Destroy(straightPipe);
				}
			}
			
		}
		
	}
}
