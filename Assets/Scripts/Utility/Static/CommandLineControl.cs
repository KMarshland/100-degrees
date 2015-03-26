using UnityEngine;
using System.Collections;
using KofTools;

	public class CommandLineControl : MonoBehaviour {//place water range -100 1 0 10 10 1
		
		public static bool showingCommandLine;
		bool showingLS;
		string command;
		string previous;
		
		public static int counter;
		public int cntr;
		
		public static int pipesPerSecond;
		public static int pipeCheck;
		public int pipesPerFrame;
		public int pipeCount;
		
		public static double constructionComplete = 100;
		public static double miningComplete = 100;
		
		public static bool printingFPS;
		public float updateInterval = 0.5f;
		private float accum   = 0; // FPS accumulated over the interval
		private int   frames  = 0; // Frames drawn over the interval
		private float timeleft; // Left time for current interval
		string fps;
		
		Water[] ws;
		public int www;
		
		Vector2 scrollPosition;
		
		// Use this for initialization
		void Start () {
			command = "Enter Command";
			showingCommandLine = false;
			showingLS = false;
			counter = 0;
			
			ConstructionController.updatePipes();
			pipesPerSecond = 50;
			pipeCheck = 100000;
			
			printingFPS = true;
			timeleft = updateInterval;
			
			//Water.firstFrame();
		}
		
		// Update is called once per frame
		void Update () {
			if (Input.GetKeyDown(KeyCode.C) && Input.GetKeyDown(KeyCode.L)){
				if (showingCommandLine){
					showingCommandLine = false;
				} else {
					showingCommandLine = true;
				}
			}
			
			counter += 1;
			cntr = counter;
			pipeCount = ConstructionController.pipes.Count;
			
			//update pipes
			if (counter % pipeCheck == 0){
				foreach (Pipe p in ConstructionController.pipes){
					try {
						p.type = -2;
						p.lastType = -2;
					} catch {
						command = "poop";
					}
				}
			}
			try {
				if (ConstructionController.pipes.Count < pipesPerSecond){
					pipesPerSecond = ConstructionController.pipes.Count;
				} else if (pipesPerSecond < 1){
					pipesPerSecond = 1;
				}
				
			
				pipesPerFrame = (int)(ConstructionController.pipes.Count / pipesPerSecond);
			} catch {
				pipesPerFrame = 0;
			}
			
			for (int i = 0; i < pipesPerFrame; i++){
				ConstructionController.updatenextPipe();
			}
			
			// update liquids
			/*www = Water.waters.Count;
			
			if (!MenuControl.gamePaused){
				Water.everyFrame();
			}*/
			
			
			// update FPS
			timeleft -= Time.deltaTime;
			accum += Time.timeScale/Time.deltaTime;
			frames ++;
	 
		    if( timeleft <= 0.0 ){ 
				float fPS = accum/frames;
				fps = System.String.Format("{0:F2}", fPS);
				timeleft = updateInterval;
				accum = 0.0F;
				frames = 0;
			}		
			
		}
		
		void OnGUI(){
			if (showingCommandLine){
				command = (GUI.TextField(new Rect(50, Screen.height - 50, Screen.width - 100, 25), command));
				
				if (Event.current.type == EventType.KeyDown && Event.current.character == "\n"[0]) {
					evaluteCommand(command);
				} else if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.UpArrow) {
					//command = previous;
				}
				
				if (showingLS){
					string str = "ls - prints this list" + 
						"\nclose - closes command line" + 
						"\nclear - clears command line" +
						"\ndie - closes without saving" +
						"\npause - pauses the game" +
						"\nupdate pipe graphics - updates all pipes' graphics" + 
						"\nupdate pipe types - forces all pipes' graphics to be reevaluated next check" +
						"\nupdate pipes all - updates all pipe atributes" +
						"\nscale pipes - resacales all pipes" +
						"\nchange construction speed <x> - sets the construction speed to <x>" +
						"\nchange mining speed <x> - sets the mining speed to <x>" +
						"\nprint position - makes robots print their position in the corner of the screen" +
						"\nincrease boiler coal - increases all boiler's coal amounts by one chunk of coal" +
						"\ndelete all saved games - deletes all saves" + 
						"\nspawn <material> bar <x> - spawns <x> <material> bars" + 
						"\nspawn <material> ore <x> - spawns <x> <material> ores" + 
						"\nspawn robot" + 
						"\nprint fps - toggles whether the FPS counter is visible" +
						"\nplace water <x> <y> <z> <depth> places a tile of water at (x, y, z) with a depth <depth>" + 
						"\nplace water here <depth> places water at camera position with a depth <depth>" +
						"\nplace water range <x> <y> <z> <width> <length> <depth> - places water with those parameters, top left corner at (x, y, z)" +
						"\nnew water mesh - respawns the water graphics" +
						"\nupdate water mesh - updates the water graphics" +
						"\ngenerate all water triples" +
						"\ngenerate all water neighbors" +
						"\ndel weather all - destroys all weather" +
						"\ndel weather above - destroys the rain, snow, or hail" +
						"\n" +
						"\n";
					
					try {
						GUILayout.BeginArea(new Rect(150, 60, Screen.width - 300, Screen.height - 40));
						scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width (Screen.width-300), GUILayout.Height (Screen.height-200));
						
						/*changes made in the below 2 lines */
						
						GUI.skin.box.wordWrap = true;     // set the wordwrap on for box only.
						GUILayout.Box(str);        // just your message as parameter.
						GUILayout.EndScrollView();
		
						GUILayout.EndArea();
						//GUI.TextArea(new Rect(Screen.width/2 - 200, 50, 400, Screen.height - 150), str);
					} catch {
						
					}
				}
			}
			
			if (printingFPS){
				GUI.Label(new Rect(10, Screen.height - 20, 100, 100), "FPS: " + fps);
			}
			
		}
		
		void evaluteCommand(string com){
			Debug.Log(com);
			command = "";
			showingLS = false;
			
			string[] words = null;
			char[] splitchar = { ' ' };
			words = com.Split(splitchar);
			
			//try {
			if (com.ToLower().Equals("close")){
				showingCommandLine = false;
			} else if (com.ToLower().Equals("ls")){
				showingLS = true;
			} else if (com.ToLower().Equals("p")){
				command = previous;
			} else if (com.ToLower().Equals("die")){
				Application.Quit();
			} else if (com.ToLower().Equals("pause")){
				MenuControl.gamePaused = !MenuControl.gamePaused;
			} else if (com.ToLower().Equals("update pipe graphics")){
				foreach (Pipe p in ConstructionController.pipes){
					try {
						p.updateGraphics();
					} catch {
						command = "neerd";
					}
				}
			} else if (com.ToLower().Equals("update pipe types")){
				foreach (Pipe p in ConstructionController.pipes){
					try {
						p.type = -2;
						p.lastType = -2;
					} catch {
						command = "poop";
					}
				}
			} else if (com.ToLower().Equals("scale pipes")){
				foreach (Pipe p in ConstructionController.pipes){
					try {
						p.scale();
					} catch {
						command = "neerd";
					}
				}
			} else if (com.ToLower().Equals("update pipes all")){
				ConstructionController.updatePipes();
			} else if (words[0].ToLower().Equals("change")){
				if (words[1].ToLower().Equals("construction")){
					if (words[2].ToLower().Equals("speed")){
						double speed = double.Parse(words[3]);
						if (speed >= 0 && speed <= 100){
							constructionComplete = speed;
						} else {
							command = "Must be between 0 and 100";
						}
					}
				} else if (words[1].ToLower().Equals("mining")){
					if (words[2].ToLower().Equals("speed")){
						double speed = double.Parse(words[3]);
						if (speed >= 0 && speed <= 100){
							miningComplete = speed;
						} else {
							command = "Must be between 0 and 100";
						}
					}
				}
			} else if (com.ToLower().Equals("enter command")){
				command = "";
			} else if (com.ToLower().Equals("invalid command")){
				command = "";
			} else if (com.ToLower().Equals("clear")){
				command = "";
			} else if (com.ToLower().Equals("print position")){
				MenuControl.robotsPrintPosition = !MenuControl.robotsPrintPosition;
			} else if (com.ToLower().Equals("increase boiler coal")){
				foreach (BoilerManager b in ConstructionController.boilers){
					b.coalAmount += 10000;
				}
			} else if (com.ToLower().Equals("delete all saved games")){
				MenuControl.clearAllSavedGames();
			} else if (words[0].Equals("spawn")){
				if (words.Length > 3){
					if (words[1].Equals("iron") && words[2].Equals("bar")){
						for (int i = 0; i < int.Parse(words[3]); i++){
							GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/BarPrefab"));
							Bar b = (Bar)obj.transform.GetComponent("Bar");
							//b.item = obj;
							b.starterate(new Metal(Metal.MetalType.Iron));
							b.transform.position = transform.position;
						}
					} else if (words[1].Equals("copper") && words[2].Equals("bar")){
						for (int i = 0; i < int.Parse(words[3]); i++){
							GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/BarPrefab"));
							Bar b = (Bar)obj.transform.GetComponent("Bar");
							//b.item = obj;
							b.starterate(new Metal(Metal.MetalType.Copper));
							b.transform.position = transform.position;
						}
					} else if (words[1].Equals("tin") && words[2].Equals("bar")){
						for (int i = 0; i < int.Parse(words[3]); i++){
							GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/BarPrefab"));
							Bar b = (Bar)obj.transform.GetComponent("Bar");
							//b.item = obj;
							b.starterate(new Metal(Metal.MetalType.Tin));
							b.transform.position = transform.position;
						}
					} else if (words[1].Equals("zinc") && words[2].Equals("bar")){
						for (int i = 0; i < int.Parse(words[3]); i++){
							GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/BarPrefab"));
							Bar b = (Bar)obj.transform.GetComponent("Bar");
							//b.item = obj;
							b.starterate(new Metal(Metal.MetalType.Zinc));
							b.transform.position = transform.position;
						}
					} else if (words[1].Equals("steel") && words[2].Equals("bar")){
						for (int i = 0; i < int.Parse(words[3]); i++){
							GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/BarPrefab"));
							Bar b = (Bar)obj.transform.GetComponent("Bar");
							//b.item = obj;
							b.starterate(new Metal(Metal.MetalType.Steel));
							b.transform.position = transform.position;
						}
					} else if (words[1].Equals("brass") && words[2].Equals("bar")){
						for (int i = 0; i < int.Parse(words[3]); i++){
							GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/BarPrefab"));
							Bar b = (Bar)obj.transform.GetComponent("Bar");
							//b.item = obj;
							b.starterate(new Metal(Metal.MetalType.Brass));
							b.transform.position = transform.position;
						}
					} else if (words[1].Equals("bronze") && words[2].Equals("bar")){
						for (int i = 0; i < int.Parse(words[3]); i++){
							GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/BarPrefab"));
							Bar b = (Bar)obj.transform.GetComponent("Bar");
							//b.item = obj;
							b.starterate(new Metal(Metal.MetalType.Bronze));
							b.transform.position = transform.position;
						}
					} else if (words[1].Equals("galvy") && words[2].Equals("bar")){
						for (int i = 0; i < int.Parse(words[3]); i++){
							GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/BarPrefab"));
							Bar b = (Bar)obj.transform.GetComponent("Bar");
							//b.item = obj;
							b.starterate(new Metal(Metal.MetalType.GalvanizedSteel));
							b.transform.position = transform.position;
						}
					}
				
					if (words[1].Equals("copper") && words[2].Equals("ore")){
						for (int i = 0; i < int.Parse(words[3]); i++){
							GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/OrePrefab"));
							Ore o = (Ore)obj.transform.GetComponent("Ore");
							//o.item = obj;
							o.starterate(new EconomicMineral(EconomicMineral.EconomicMineralType.Malachite));
							o.transform.position = transform.position;
						}
					} else if (words[1].Equals("iron") && words[2].Equals("ore")){
						for (int i = 0; i < int.Parse(words[3]); i++){
							GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/OrePrefab"));
							Ore o = (Ore)obj.transform.GetComponent("Ore");
							//o.item = obj;
							o.starterate(new EconomicMineral(EconomicMineral.EconomicMineralType.Limonite));
							o.transform.position = transform.position;
						}
					} else if (words[1].Equals("aluminum") && words[2].Equals("ore")){
						for (int i = 0; i < int.Parse(words[3]); i++){
							GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/OrePrefab"));
							Ore o = (Ore)obj.transform.GetComponent("Ore");
							//o.item = obj;
							o.starterate(new EconomicMineral(EconomicMineral.EconomicMineralType.Bauxite));
							o.transform.position = transform.position;
						}
					} else if (words[1].Equals("zinc") && words[2].Equals("ore")){
						for (int i = 0; i < int.Parse(words[3]); i++){
							GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/OrePrefab"));
							Ore o = (Ore)obj.transform.GetComponent("Ore");
							//o.item = obj;
							o.starterate(new EconomicMineral(EconomicMineral.EconomicMineralType.Spheralite));
							o.transform.position = transform.position;
						}
					} else if (words[1].Equals("tin") && words[2].Equals("ore")){
						for (int i = 0; i < int.Parse(words[3]); i++){
							GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/OrePrefab"));
							Ore o = (Ore)obj.transform.GetComponent("Ore");
							//o.item = obj;
							o.starterate(new EconomicMineral(EconomicMineral.EconomicMineralType.Cassisterite));
							o.transform.position = transform.position;
						}
					}
				}
				
				if (words[1].Equals("robot")){
					((GameObject)GameObject.Instantiate(Resources.Load("Prefabs/RobotPrefab2Tools"))).transform.position = this.transform.position;
				}
			} else if (com.ToLower().Equals("print fps")){
				printingFPS = !printingFPS;
			} else if (com.ToLower().Equals("update water neighbors")){
				Water.addAllNeighbors();
			} else if (words[0].Equals("place")){
				if (words[1].Equals("water")){
					if (words[2].Equals("here")){
						try {
							Water.waterAt((int)transform.position.x, (int)transform.position.z)[0].depth = int.Parse(words[3]);
						} catch {
							new Water((int)transform.position.x, (int)transform.position.y, (int)transform.position.z, int.Parse(words[3]));
						}
					} else if (words[2].Equals("range")){
						int x1 = int.Parse(words[3]);
						int y1 = int.Parse(words[4]);
						int z1 = int.Parse(words[5]);
						
						int width = int.Parse(words[6]);
						int length = int.Parse(words[7]);
						float depth = float.Parse(words[8]);
											
						for (int x = x1; x < x1 + width; x++){
							for (int z = z1; z < z1 + length; z++){
								new Water(x, y1, z, depth);
							}
						}
						
					} else {
						try {
							Water.waterAt(int.Parse(words[2]), int.Parse(words[4]))[0].depth = float.Parse(words[5]);
						} catch {
							new Water(int.Parse(words[2]), int.Parse(words[3]), int.Parse(words[4]), float.Parse(words[5]));
						}
					}
				}
			} else if (com.ToLower().Equals("del weather all")){
				foreach (GameObject o in WeatherControl.weatherSystems){
					Destroy(o);
					WeatherControl.weatherSystems = new ArrayList();
				}
			}  else if (com.ToLower().Equals("del weather above")){
				WeatherControl.weather.destroyAbove();
			} else if (com.ToLower().Equals("new water mesh")){
				Water.newMesh();
			} else if (com.ToLower().Equals("update water mesh")){
				Water.updateAllMeshes();
			} else if (com.ToLower().Equals("generate all water triples")){
				Water.generateAllTriples();
			} else if (com.ToLower().Equals("generate all water neighbors")){
				Water.addAllNeighbors();
			} else {
				command = "Invalid command";
			}
			/*} catch {
				command = "Invalid command";
			}*/
			
			previous = com;
			
		}
		
		public static string[] splitString(string str){
			string[] words = null;
			char[] splitchar = { ' ' };
			words = str.Split(splitchar);
			return words;
		}
		
	}
