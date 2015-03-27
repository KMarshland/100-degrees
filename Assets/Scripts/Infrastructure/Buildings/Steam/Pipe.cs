using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using KofTools;

public class Pipe : SteamObject {

	static GameObject PipeParent;

	float corrosion;
	Metal material;
	public int type;
	public int lastType;
	public string prefabType;
	
	public float pipelength;
	public float offSet; //How far to move to the right
	GameObject straightPipe;
	
		
	public bool northOpen, southOpen, eastOpen, westOpen, topOpen, bottomOpen;

	Dictionary<string, Pipe> closestOnAxis = new Dictionary<string, Pipe>();

	public void starterate(Metal mat){


		material = mat;
		updateParent();
		reEnqueue();

	}

	public void updateParent(){
		if (PipeParent == null){
			PipeParent = new GameObject("Pipe Parent");
		}
		if (this.transform.parent != PipeParent.transform){
			this.transform.parent = PipeParent.transform;
		}
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
	
	public void checkSurroundings(){//figures out what type it is
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

		if (adjacentSteams == null){
			calculateAdjacentSteams();
		}

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
	
	public void updateGraphics(){//switches the graphics to a given type
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

	void switchTo(string ptype, Vector3 rotation){
		if (ptype == prefabType){//you don't need to regenerate
			transform.rotation = Quaternion.identity;
			transform.Rotate(rotation);
			updateParent();
		} else {
			disEnqueue();
			GameObject obj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(ptype));
			Pipe p = (Pipe)obj.transform.GetComponent<Pipe>();
			transform.rotation = Quaternion.identity;
			transform.Rotate(rotation);
			transferData(p);
			p.prefabType = ptype;
			p.updateParent();
		}
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
		updateParent();
	}
	
	// Update is called once per frame
	void Update () {
		checkSurroundings();
		calculatePressure();
	}

	public void fullUpdate(){
		calculateAdjacentSteams();
		checkSurroundings();
		calculatePressure();
		calculateWater();
		updatePos();
		updateParent();

		updateGraphics();

	}
	
	public new void updatePos(){
		
		scale();
		
		if (type == 7){
			transform.position = new Vector3((float) x, (float) y, (float)z);
		} else if (type == 6){
			transform.position = new Vector3((float) x, (float) y, (float)z);
		}
	}
	
	public void scale(){
		
		this.GetComponent<Renderer>().enabled = true;

		if (adjacentSteams == null || adjacentSteams.Count == 0){
			return;
		}

		if (type == 6){
			scaleAlongAxis("z");
		} else if (type == 7){
			scaleAlongAxis("x");
		}

	}

	void scaleAlongAxis(string axis){
		Pipe[] nearby = findClosest(axis);
		Dictionary<string, Vector3> properRotations = new Dictionary<string, Vector3>(){
			{"x", new Vector3(90, 90, 0)},
			{"y", new Vector3(90, 90, 0)},
			{"z", new Vector3(90, 0, 0)}
		};
		
		if (nearby[0] != null){//it's got something to the right -- calculate pipelength accordingly
			float d = axis == "x" ? x : axis == "y" ? y : axis == "z" ? z : 0f;;
			float closestD = axis == "x" ? nearby[0].x : axis == "y" ? nearby[0].y : axis == "z" ? nearby[0].z : 0f;
			pipelength = nearby[0].pipelength + (closestD - d);
		}
		
		if (nearby[0] == null && nearby[1] == null){//no neighbors on this axis -- leave in in normal form
			if (straightPipe == null){
				GameObject.Destroy(straightPipe);
			}
		} else if (nearby[1] == null){//nothing to the left, something to the right, so it's the parent of the pipe
			offSet = (pipelength) / 2f;
			float nx = x + ((axis == "x" ? 1 : 0) * offSet);
			float ny = y + ((axis == "y" ? 1 : 0) * offSet);
			float nz = z + ((axis == "z" ? 1 : 0) * offSet);
			
			doScale(new Vector3(nx, ny, nz), properRotations[axis]);
		} else {//neighbor on the left
			this.GetComponent<Renderer>().enabled = false;
			if (straightPipe == null){
				GameObject.Destroy(straightPipe);
			}
		}
	}

	Pipe[] findClosest(string axis){
		Pipe closest = null;
		Pipe furthest = null;

		float furthestPossibleNeighbor = 2f;
		float d = axis == "x" ? x : axis == "y" ? y : axis == "z" ? z : 0f;
		
		foreach (SteamObject p in adjacentSteams){
			if (p.GetType().ToString().Equals("Pipe")){
				Pipe pi = (Pipe) p;

				if (pi.type == this.type){
					float piD = axis == "x" ? pi.x : axis == "y" ? pi.y : axis == "z" ? pi.z : 0f;
					float cD = closest == null ? 0f : axis == "x" ? closest.x : axis == "y" ? closest.y : axis == "z" ? closest.z : 0f;
					float fD = furthest == null ? 0f : axis == "x" ? furthest.x : axis == "y" ? furthest.y : axis == "z" ? furthest.z : 0f;
					
					if (piD > d && (piD - d) <= furthestPossibleNeighbor && (closest == null || piD < cD)){//it's to the right of you, and closer than anything else you've found
						closest = pi;
					}

					if (piD < d && (d - piD) <= furthestPossibleNeighbor && (furthest == null || piD > fD)){//it's to the right of you, and closer than anything else you've found
						furthest = pi;
					}
				}
			}
		}

		//closestOnAxis[axis] = closest;
		return new Pipe[]{closest, furthest};
	}

	void doScale(Vector3 newPos, Vector3 newRot){

		if (prefabType == "Prefabs/StraightPipePrefab"){
			this.GetComponent<Renderer>().enabled = false;
		}
					
		if (straightPipe == null){
			straightPipe = (GameObject) GameObject.Instantiate(Resources.Load("Prefabs/StraightPipeSectionPrefab"));
		}
		
		straightPipe.transform.rotation = Quaternion.identity;
		straightPipe.transform.Rotate(newRot);
		straightPipe.transform.position = newPos;

		float normalizedPipeLength = 0.0625f;//when y scale is this, the pipe section is exactly one unit long
		straightPipe.transform.localScale = new Vector3(1f, (normalizedPipeLength * (pipelength + 2f)), 1f);
	}

	void destroyStraightPipeAlongAxis(string axis){
		if (straightPipe != null){
			GameObject.Destroy(straightPipe);

		}
	}

	public static int pipesPerFrame = 10;
	static int pipeIndex;//how far through the cycle of pipes you are

	public static void OnStart(){
		
	}
	
	public static void EachFrame(){
		//update the number of pipes we need -- never more than the number of pipes that exist
		for (int i = 0; i < Mathf.Min(pipesPerFrame, ConstructionController.pipes.Count); i++){
			updateNextPipe();
		}
	}

	public static void updateNextPipe(){
		if (pipeIndex >= ConstructionController.pipes.Count){
			pipeIndex = 0;
		}
		ConstructionController.pipes[pipeIndex].fullUpdate();
		pipeIndex ++;
	}

	public static void updateAll(){
		foreach (Pipe p in ConstructionController.pipes){
			p.fullUpdate();
		}
	}
}
