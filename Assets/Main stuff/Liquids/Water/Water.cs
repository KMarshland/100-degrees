using UnityEngine;
using System.Collections;

namespace KofTools{
	public class Water : Liquid {
		
		public static ArrayList waters;
		public static readonly int numberToCheckBehind = 10;
		public static readonly float viscosity = 0.8f; //higher = less viscous = more ripples. Too high will cause odd occilations
		public static readonly string objectName = "WaterFake";
		
		private static bool updatingMeshNextFrame;
		private static bool isFirstFrame;
		
		public static GameObject[] waterPlanes;
		public static int[] meshSizes;
		
		public static int normalRecalculationFrequency = 30;
		public static int updatingFrequency = 30;
		
		static Water[] waterses = new Water[0];
		public static int waterNum = 0;
		
		static int goingMeshID;
		
		ArrayList neighbors;
		
		bool hasSoilBelow;
		SoilBelow soil;
		
		float[] previousDepths;	
		
		public int id;
		public int localID;
		public int meshID;
		
		public int[] triple1;
		public int[] triple2;
		public bool hasTriple1;
		public bool hasTriple2;
		
		public Water(int nx, float ny, int nz):base(nx, nz){
			surfaceLevel = ny;
			start();
		}
		
		public Water(int nx, float ny, int nz, SoilBelow s):base(nx, nz){
			surfaceLevel = ny;
			hasSoilBelow = true;
			soil = s;
			start();
		}
		
		public Water(int nx, float ny, int nz, float ndepth):base(nx, nz){
			surfaceLevel = ny;
			changeDepth(ndepth);
			start();
		}
		
		public Water(int nx, float ny, int nz, float ndepth, SoilBelow s):base(nx, nz){
			surfaceLevel = ny;
			hasSoilBelow = true;
			soil = s;
			changeDepth(ndepth);
			start();
		}
		
		private void start(){
			waters.Add(this);
			id = waters.Count - 1;
			previousDepths = new float[numberToCheckBehind];
			checkSurface();
			//generateTriples();
			updatingMeshNextFrame = true;
			if (!isFirstFrame){
				//meshID = goingMeshID;
				addNeighbors();
				generateTriples();
				generateNeighborTriples();
			}
			calculateMeshGroup();
			calculateLocalID();
		}
		
		public void update(){
			
			checkShowing();
			checkSurface();
			checkFlows();
			//spread();
			
			
			if (!hasTriple1 || !hasTriple2){
				generateTriples();
			}
			
			changeMesh();
			
			if (hasSoilBelow){
				updateLower();
			}
			
		}
		
		// compares depths with all of its neighbors. This is where water flows
		public void checkFlows(){
			try {
				
				// prevents it from checking one it has settled
				bool checking = false;
				
				foreach (float f in previousDepths){
					if (f != depth){
						checking = true;
						break;
					}
				}
				
				if (checking){
					foreach (Water neighbor in neighbors){
						normalizeFlows(neighbor, viscosity);
					}
					//spread();
					//normalizeFlows(Water.arrayListToArray(neighbors));
				}
				
				//makes sure it has neighbors
				if (neighbors.Count < 1){
					addNeighbors();
				}
				
				// updates list of depths so that it can check for settling
				for (int i = 0; i < numberToCheckBehind - 1; i++){
					previousDepths[i] = previousDepths[i + 1];
				}
				
				previousDepths[numberToCheckBehind - 1] = depth;
				
			} catch {
				Debug.Log("No neighbors");
				addNeighbors();	
			}
		}
		
		public void whenRaining(){
			changeDepth(0.0015f / WeatherControl.weather.variedIntensity);
		}
		
		public void whenNotRaining(){
			changeDepth(-0.001f);
		}
		
		public void updateLower(){
			float change = (0.05f * depth) / (100f * soil.saturation);
			soil.saturation += change;
			changeDepth(-1f * change);
			soil.updateLower();
		}
		
		public void addNeighbors(){
			neighbors = new ArrayList();
			removeDuplicate();
			
			for (int nx = -1; nx <= 1; nx++){
				for (int nz = -1; nz <= 1; nz++){
					if (nx != 0 | nz != 0){
						try {
							Water tba = closestTo(waterAt(x + nx, z + nz), surfaceLevel);
							//if (tba.meshID == this.meshID){
								neighbors.Add(tba);
							//}
						} catch {
							//Debug.Log("No water at: " + (x + nx) + ", " + (z + nz));
						}
					}
				}
			}
			
		}
		
		public void generateTriples(){ //generates its neighbors for mesh generation
			triple1 = new int[3];
			triple2 = new int[3];
			hasTriple1 = false;
			hasTriple2 = false;
			
			bool zp1 = false;
			bool xp1 = false;
			
			bool zm1 = false;
			bool xm1 = false;
			
			Water ZP1 = null; //holder for z-1
			Water XP1 = null; //holder for x+1
			
			Water ZM1 = null; //holder for z-1
			Water XM1 = null; //holder for x-1
			
			foreach (Water neighbor in neighbors){ //checks its neighbors to see if they are right
				if (neighbor.meshID == meshID){
					if ((neighbor.z == z + 1) && (neighbor.x == x)){
						zp1 = true;
						ZP1 = neighbor;
					} else if ((neighbor.z == z) && (neighbor.x == x + 1)){
						xp1 = true;
						XP1 = neighbor;
					} else if ((neighbor.z == z - 1) && (neighbor.x == x)){
						zm1 = true;
						ZM1 = neighbor;
					} else if ((neighbor.z == z) && (neighbor.x == x - 1)){
						xm1 = true;
						XM1 = neighbor;
					}
				}
			}
			
			if (zp1 && xp1){ //creates triple 1 if valid
				hasTriple1 = true;
				triple1[0] = this.localID;
				triple1[1] = ZP1.localID;
				triple1[2] = XP1.localID;
			}
			
			if (zm1 && xm1){ //creates triple 2 if valdi
				hasTriple2 = true;
				triple2[0] = this.localID;
				triple2[1] = ZM1.localID;
				triple2[2] = XM1.localID;
			}
			
		}
		
		public void removeDuplicate(){
			Water[] ws = waterAt(x, z);
			foreach (Water w in ws){
				if (!w.Equals(this)){
					if (surfaceLevel > w.surfaceLevel){
						if (bottom < w.surfaceLevel){
							removeWater(w);
						}
					} else {
						if (w.bottom < surfaceLevel){
							removeWater(this);
						}
					}
				}
			}
		}
		
		public void spread(){ //checks if more waters need to be created or if this needs to be deleted
			if (depth < 0.001f){
				//delete();
			} else {
				if (neighbors.Count < 8){
					//figures out where there could be waters
					Vector2[] possiblities = new Vector2[8];
					int i = 0;
					for (int nx = -1; nx <= 1; nx++){
						for (int nz = -1; nz <= 1; nz++){
							if (nx != 0 || nz != 0){
								possiblities[i] = new Vector2(nx + x, nz + z);
								i++;
							}
						}
					}
					
					//figures out where waters are missing and places them
					foreach (Vector2 v in possiblities){
						bool needed = true;
						
						foreach (Water n in neighbors){ //checks if a water is necessary
							if (v.x == n.x && v.y == n.z){
								needed = false;
							}
						}
						
						if (needed){ //places the water
							Water w = new Water((int)v.x, surfaceLevel, (int)v.y);
							w.addNeighbors();
							w.generateTriples();
							w.generateNeighborTriples();
							addNeighbors();
							w.addTris();
							//updatingMeshNextFrame = true;
						}
					}
				}
				
			}
		}
		
		public void delete(){ //deletes this water and update other waters accordingly
			//remove from list
			waters.Remove(this);
			
			//update ids
			for (int i = id + 1; i < waters.Count + 1; i++){
				((Water)waters[i]).id --;
				if (((Water)waters[i]).meshID == this.meshID && ((Water)waters[i]).localID > this.localID){
					((Water)waters[i]).localID --;
				}
			}
			
			meshSizes[meshID] --;
			
			//edit neighbors neighbor lists and triples
			foreach (Water n in neighbors){
				n.addNeighbors();
				n.generateTriples();
			}
		}
		
		public void generateNeighborTriples(){
			foreach (Water w in neighbors){
				w.generateTriples();
			}
		}
		
		public void addTris(){//adds triangles
			Mesh m = getMesh(meshID); //gets the mesh
			ArrayList tris = new ArrayList(); //creates storage for tris
			
			for (int i = 0; i < m.triangles.Length; i++){ //adds the current ones
				tris.Add(m.triangles[i]);
			}
			
			if (hasTriple1){//adds this ones tris if applicable
				tris.Add(triple1[0]);
				tris.Add(triple1[1]);
				tris.Add(triple1[2]);
			}
			
			if (hasTriple2){//same as before
				tris.Add(triple2[0]);
				tris.Add(triple2[1]);
				tris.Add(triple2[2]);
			}
			
			//applies the changes
			int[] trisApplied = new int[tris.Count];
			for (int i = 0; i < tris.Count; i++){
				trisApplied[i] = (int)tris[i];
			}
			
		}
		
		public void newObject(){ //creates a new GameObject for the water
			base.newObject(objectName);
		}
		
		public Vector3 getPosition(){ //creates a Vector3 of the water's position
			return new Vector3(x, surfaceLevel, z);
		}
		
		public void changeMesh(){ //changes the water mesh just for this water
			Mesh mesh = getMesh(meshID); // gets the mesh for the water
			Vector3[] verts = mesh.vertices; //saves the vertices
			
			//changes the vertice
			verts[localID] = getPosition();
			
			//puts everything back in
	
			mesh.vertices = verts;
			//mesh.RecalculateNormals();
		}
		
		public void calculateLocalID(){//assigns the position of the water in the mesh group
			int nlid = id;
			
			for (int i = 0; i < meshID; i++){
				nlid -= meshSizes[i];
			}
			
			localID = nlid;
			
		}
		
		public void calculateMeshGroup(){//checks which mesh group it is
			
			int nmg = waterPlanes.Length - 1;
			
			if (!isFirstFrame){
				if (neighbors.Count > 0){
					nmg = ((Water)neighbors[0]).meshID;
				} else {
					nmg = newMesh();
				}
			}
			
			int nms = meshSizes[nmg] + 1;
			meshSizes[nmg] = nms;
			
			meshID = nmg;
			
		}
		
		public static void onRain(){
			foreach (Water w in waters){
				w.whenRaining();
			}
		}
		
		public static void onCalm(){
			foreach (Water w in waters){
				w.whenNotRaining();
			}
		}
		
		public static Water[] waterAt(int wx, int wz) {
			ArrayList ws = new ArrayList();
			foreach (Water w in waters){
				if (w.x == wx & w.z == wz){
					ws.Add(w);
				}
			}
			return arrayListToArray(ws);
		}
		
		public static Water waterAt(int wx, int wz, bool b) {
			foreach (Water w in waters){
				if (w.x == wx & w.z == wz){
					return w;
				}
			}
			return null;
		}
		
		public static bool isWaterAt(int wx, int wz){
			
			foreach (Water w in waters){
				if (w.x == wx & w.z == wz){
					return true;
				}
			}
			
			return false;
		}
		
		public static void removeWater(Water w){
			try {
				waters.Remove(w);
			} catch {
				Debug.Log("Removal error");
			}
		}
		
		public static Water[] arrayListToArray(ArrayList al){
			
			Water[] ws = new Water[al.Count];
			
			/*for (int i = 0; i < al.Count; i++){
				ws[i] = (Water)(al[i]);
			}*/
			
			int i = 0;
			
			foreach (Water w in al){
				ws[i] = w;
				i++;
			}
			
			return ws;
			
		}
		
		public static Water closestTo(Water[] ws, float h){
			Water best = ws[0];
			
			foreach (Water w in ws){
				if (Mathf.Abs(w.surfaceLevel - h) < Mathf.Abs(best.surfaceLevel - h)){
					//best = w;
				}
			}
			
			return best;
		}
		
		public static void addAllNeighbors(){
			ArrayList ws = waters;
			foreach (Water w in ws){
				w.addNeighbors();
			}
		}
		
		public static void generateAllTriples(){
			ArrayList ws = waters;
			foreach (Water w in ws){
				w.generateTriples();
			}
		}
		
		public static void updateAll(){
			Water[] ws = arrayListToArray(waters);
			foreach (Water w in ws){
				w.update();
			}
		}
		
		public static void updateNext(){
			waterNum ++;
	
			if (waterNum >= waterses.Length){
				waterNum = 0;
				waterses = arrayListToArray(waters);
			}
					
			Water w = waterses[waterNum];
			if (w != null){
				w.update();
				if (WeatherControl.rainOn){
					if (WeatherControl.raining){
						w.whenRaining();
					} else {
						w.whenNotRaining();
					}
				}
			} else {
				//updateNext();
			}
		}
		
		public static Mesh getMesh(int i){
			return ((MeshFilter)waterPlanes[i].GetComponent("MeshFilter")).mesh;
		}
		
		public static int newMesh(){
			int i = waterPlanes.Length + 1;
			
			GameObject[] gos = new GameObject[i];
			int[] nms = new int[i];
			
			for (int j = 0 ; j < i - 1; j++){
				gos[j] = waterPlanes[j];
				nms[j] = meshSizes[j];
			}
			
			i--;
			
			nms[i] = 0;
			meshSizes = nms;
			
			waterPlanes = gos;
			
			newMesh(i);
			
			return i;
		}
		
		public static void newMesh(int i){
			
			//make game object
			try {
				GameObject.Destroy(waterPlanes[i]);
			} catch {
				
			}
			waterPlanes[i] = new GameObject("Water Plane " + i);
			Mesh mesh = new Mesh();
			
			waterPlanes[i].AddComponent<MeshFilter>();
			waterPlanes[i].AddComponent<MeshRenderer>();
			
			updateMesh(i);
		}
		
		public static void updateMesh(int wn){
			
			Mesh mesh = getMesh(wn);
			
			//make a list of those to check
			ArrayList calculatedWater = new ArrayList();
			foreach (Water w in waters){
				if (w.meshID == wn){
					calculatedWater.Add(w);
				}
			}
			
			//make vertices
			Vector3[] verts = new Vector3[calculatedWater.Count];
			for (int i = 0; i < calculatedWater.Count; i++){
				Water w = (Water) waters[i];
				verts[i] = w.getPosition();
			}
			mesh.vertices = verts;
			
			// make uv
			Vector2[] uvs = new Vector2[mesh.vertices.Length];   
			for (int i = 0; i < uvs.Length; i++) {
				Vector3 wp = ((Water)(calculatedWater[i])).getPosition();
				uvs[i] = new Vector2(wp.x, wp.z);
			}
			mesh.uv = uvs;
			
			//create triangles
			ArrayList triGen = new ArrayList();
			
			foreach (Water w in calculatedWater){
				if (w.hasTriple1){
					triGen.Add(w.triple1[0]);
					triGen.Add(w.triple1[1]);
					triGen.Add(w.triple1[2]);
				}
				
				if (w.hasTriple2){
					triGen.Add(w.triple2[0]);
					triGen.Add(w.triple2[1]);
					triGen.Add(w.triple2[2]);
				}
			}
			
			int[] tris = new int[triGen.Count];
			for (int i = 0; i < triGen.Count; i++){
				tris[i] = (int)triGen[i];
			}
			
			mesh.triangles = tris;
			
			//add everything together
			mesh.RecalculateNormals();
			((MeshFilter)waterPlanes[wn].GetComponent("MeshFilter")).mesh = mesh;
			waterPlanes[wn].GetComponent<Renderer>().material = (UnityEngine.Material)(Resources.Load("Materials/Water"));
			//waterPlanes[wn].layer = 3;
		}
		
		public static void updateAllMeshes(){
			updatingMeshNextFrame = true;
		}
		
		public static void everyFrame(){
			if (updatingMeshNextFrame){
				for (int i = 0; i < waterPlanes.Length; i++){
					updateMesh(i);
				}
				updatingMeshNextFrame = false;
			} else if (CommandLineControl.counter % normalRecalculationFrequency == 0){
				for (int i = 0; i < waterPlanes.Length; i++){
					getMesh(i).RecalculateNormals();
				}
			}
			for (int i = 0; i < (Water.waters.Count / updatingFrequency) + 1; i++){
				try {
					Water.updateNext();
				} catch {
					
				}
			}
		}
		
		public static void firstFrame(){
			isFirstFrame = true;
			goingMeshID = 0;
			//create empty lists
			Water.waterPlanes = new GameObject[1];
			Water.meshSizes = new int[1];
			
			Water.waters = new ArrayList();
			
			//spawn waters
			for (int x = 40; x < 95; x ++){
				for (int z = 5; z < 65; z ++){
					//new Water(x, 10, z, 3.5f);
				}
			}
			
			for (int x = 57; x < 60; x ++){
				for (int z = 65; z < 68; z ++){
					//new Water(x, 10, z, 3.5f);
				}
			}
			
			//update all the waters
			Water.addAllNeighbors();
			Water.generateAllTriples();
			Debug.Log(Water.waters.Count);
			
			//create the mesh
			Water.newMesh(0);
			isFirstFrame = false;
			goingMeshID = 1;
			
			
			for (int x = -100; x < -90; x ++){
				for (int z = 0; z < 10; z ++){
					//new Water(x, 10, z, 0.5f);
				}
			}
			
		}
		
	}
}
