using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using KofTools;

public class MenuControl : MonoBehaviour {
	
	public static bool gamePaused;
	public static bool scrollingDisabled;
	bool scrollOnClose;
	static string game;
	public static bool robotsPrintPosition;

	Seeker seeker;
	
	bool onMainMenu;
	bool onCreditsScreen;
	bool onBackStoryScreen;
	bool onHelpScreen;
	bool onAboutScreen;
	bool onOptionsScreen;
	bool onLoadingScreen;
	public bool onSavingScreen;
	
	int timeOfSaving;
		
	bool onBuildingMenu;
	bool buildingSelected;
	GameObject building;
	bool matSelectionButtons;
	ArrayList materials;
	int materialsRequired;
	int buildingType;
	float addedHeight;
	
	Vector3 startLoc;
	bool firstDone;
	float firstTime;
	
	float amountFaded;
	
	public static readonly string credits = "Dedicated to our cats: Calypso, Fluffy, and CeeCee\n\nCredits:\n\n  Programming:\n    Kofthefens (Robots, jobs, sounds, buildings, items, menus)\n    GalenEvil (Terrain, camera)\n    Virex (Rails)\n     Saving - UnitySerializer by Mike Talbot - 2012 http://whydoidoit.com/\n\n  Modeling:\n    MulletMaster (Robots, robot components, tools, and buildings)\n\n  Music:\n    Kevin MacLeod (incompetech.com) Licensed under Creative Commons Attribution 3.0 http://creativecommons.org/licenses/by/3.0/\n\n  Textures:\n    Skyru (Zinc)";
	public static readonly string about = "About 100°: \n\n100° is completely volunteer work. The game Dwarf Fortress, by Bay12 Games, inspired it heavily. It runs in the Unity Engine (Unity3d.com)" + "\n\n100° was first conceived on the Bay12 forums (http://www.bay12forums.com), by Virex. The idea for having a community game in general, however, was imagined by DrPoo, on April 14, 2012. A dozen people signed up, but many dropped out. A small, basic foundation of code was created, but it lay mostly dormant until the end of May that year.\n"+ "\nAt this point, 100° really got going. Mulletmaster created several models, and Kofthefens was able to test some of his code. The game has progressed at a steady rate since."+ "";
	
	
	// Use this for initialization
	void Start () {
		gamePaused = false;
		scrollingDisabled = true;
		scrollOnClose = false;
		robotsPrintPosition = false;
		
		onMainMenu = false;
		onCreditsScreen = false;
		onBackStoryScreen = false;
		onHelpScreen = false;
		onAboutScreen = false;
		onOptionsScreen = false;
		onLoadingScreen = false;
		onSavingScreen = false;
		
		onBuildingMenu = false;
		buildingSelected = false;
		matSelectionButtons = false;
		firstDone = false;
		
		//Time.timeScale = 0;
		
		amountFaded = 0f;
		game = "Region 1";
		//LevelSerializer.PlayerName = "Me";

		if (this.GetComponent<Seeker>() == null){
			this.gameObject.AddComponent<Seeker>();
		}
		seeker = this.GetComponent<Seeker>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (buildingSelected){
			try {				
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit[] hits = Physics.RaycastAll(ray);
				
				//building.rigidbody.constraints = RigidbodyConstraints.None;
				building.GetComponent<Rigidbody>().transform.position = new Vector3(hits[0].point.x, hits[0].point.y + addedHeight, hits[0].point.z);
				building.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
				
				if (Input.GetKey(KeyCode.Escape)){
					buildingSelected = false;
					gamePaused = false;
					matSelectionButtons = false;
					onBuildingMenu = false;
					GameObject.Destroy(building);
					scrollingDisabled = !scrollOnClose;
					materialsRequired = 1000;
				}
			} catch {
				
			}
			if (Input.GetMouseButton(0)){
				
				if (buildingType != 6 | (firstDone && ((Time.time - firstTime) > 0.2))){
					buildingSelected = false;
					gamePaused = true;
					matSelectionButtons = true;
					onBuildingMenu = false;
					firstDone = false;
					Debug.Log (buildingType);
				} else {
					firstDone = true;
					startLoc = building.transform.position;
					firstTime = Time.time;
					Debug.Log(firstTime);
				}
			}
		} else if (onMainMenu){
			if (Input.GetKey(KeyCode.Escape)){
				gamePaused = false;
				onMainMenu = false;
				scrollingDisabled = !scrollOnClose;
			}
			
			if (Input.GetKey(KeyCode.O)){
				onCreditsScreen = false;
				onBackStoryScreen = false;
				onHelpScreen = false;
				onAboutScreen = false;
				onOptionsScreen = true;
				onLoadingScreen = false;
				amountFaded = 0f;
			}
			
			if (Input.GetKey(KeyCode.S)){
				onCreditsScreen = false;
				onBackStoryScreen = false;
				onHelpScreen = false;
				onAboutScreen = false;
				onOptionsScreen = false;
				onLoadingScreen = false;
				onSavingScreen = true;
				timeOfSaving = CommandLineControl.counter;
			}
			
			if (Input.GetKey(KeyCode.L)){
				onCreditsScreen = false;
				onBackStoryScreen = false;
				onHelpScreen = false;
				onAboutScreen = false;
				onOptionsScreen = false;
				onLoadingScreen = true;
			}
			
			if (Input.GetKey(KeyCode.Q)){
				onCreditsScreen = false;
				onBackStoryScreen = false;
				onHelpScreen = false;
				onAboutScreen = false;
				onOptionsScreen = false;
				onLoadingScreen = false;
				save();
				Application.Quit();
			}
			
		} else if (onBuildingMenu){
			if (Input.GetKey(KeyCode.Escape)){
				onBuildingMenu = false;
				scrollingDisabled = !scrollOnClose;
			}
			
			if (Input.GetKey(KeyCode.M)){
				scrollingDisabled = !scrollOnClose;
				buildingSelected = true;
				onBuildingMenu = false;
				building = (GameObject) GameObject.Instantiate(UnityEngine.Resources.Load("Prefabs/smelterPrefab"));
				WorkshopManager s = (WorkshopManager)building.GetComponent("WorkshopManager");
				s.active = false;
				materialsRequired = 3;
				buildingType = 1;
				addedHeight = 0;
			}
			
			if (Input.GetKey(KeyCode.F)){
				scrollingDisabled = !scrollOnClose;
				buildingSelected = true;
				onBuildingMenu = false;
				building = (GameObject) GameObject.Instantiate(UnityEngine.Resources.Load("Prefabs/ForgingPressPrefab"));
				WorkshopManager s = (WorkshopManager)building.GetComponent("WorkshopManager");
				s.active = false;
				materialsRequired = 3;
				buildingType = 2;
				addedHeight = 0;
			}
			
			if (Input.GetKey(KeyCode.E)){
				scrollingDisabled = !scrollOnClose;
				buildingSelected = true;
				onBuildingMenu = false;
				building = (GameObject) GameObject.Instantiate(UnityEngine.Resources.Load("Prefabs/AssemblyLinePrefab"));
				WorkshopManager s = (WorkshopManager)building.GetComponent("WorkshopManager");
				s.active = false;
				materialsRequired = 3;
				buildingType = 3;
				addedHeight = 2.5f;
			}
			
			if (Input.GetKey(KeyCode.O)){
				scrollingDisabled = !scrollOnClose;
				buildingSelected = true;
				onBuildingMenu = false;
				building = (GameObject) GameObject.Instantiate(UnityEngine.Resources.Load("Prefabs/Boiler"));
				materialsRequired = 3;
				buildingType = 4;
				addedHeight = 0;
			}
			
			if (Input.GetKey(KeyCode.L)){
				scrollingDisabled = !scrollOnClose;
				buildingSelected = true;
				onBuildingMenu = false;
				building = (GameObject) GameObject.Instantiate(UnityEngine.Resources.Load("Prefabs/WallPrefab"));
				materialsRequired = 1;
				buildingType = 5;
				addedHeight = 1.0f;
			}
			
			if (Input.GetKey(KeyCode.P)){
				scrollingDisabled = !scrollOnClose;
				buildingSelected = true;
				onBuildingMenu = false;
				building = (GameObject) GameObject.Instantiate(UnityEngine.Resources.Load("Prefabs/StraightPipePrefab"));
				Pipe p = (Pipe) building.GetComponent("Pipe");
				p.activated = false;
				materialsRequired = 0;
				buildingType = 6;
				addedHeight = 0.55f;
			}
			
			if (Input.GetKey(KeyCode.R)){
				scrollingDisabled = !scrollOnClose;
				buildingSelected = true;
				onBuildingMenu = false;
				building = (GameObject) GameObject.Instantiate(UnityEngine.Resources.Load("Prefabs/RefuelingStopPrefab"));
				materialsRequired = 3;
				buildingType = 7;
				addedHeight = 0f;
			}
			
		} else {
			if (Input.GetKey(KeyCode.M)){
				onMainMenu = true;
				onCreditsScreen = false;
				onBackStoryScreen = false;
				onHelpScreen = false;
				onAboutScreen = false;
				onOptionsScreen = false;
				onLoadingScreen = false;
			}
			
			if (Input.GetKey(KeyCode.B)){
				onMainMenu = false;
				onBuildingMenu = true;
				scrollingDisabled = true;
				materials = new ArrayList();
			}
		}
		
		if (onSavingScreen && (timeOfSaving < CommandLineControl.counter)){
			save();
		}
	}
	
	void OnGUI(){
		
		if (gamePaused){
			Time.timeScale = 0;
			scrollingDisabled = true;
		} else {
			Time.timeScale = 1;
		}
		
		if (onMainMenu){
			gamePaused = true;
			
			if (onSavingScreen){
				GUI.Label(new Rect(Screen.width/2 - 25, 150, 100, 25), "Saving...");
			}
			
			GUI.Box(new Rect(10, 10, Screen.width - 20, Screen.height - 20), "Menu");
			if (GUI.Button(new Rect(Screen.width - 100, 75, 75, 20), "Close")){
				gamePaused = false;
				onMainMenu = false;
				scrollingDisabled = !scrollOnClose;
			}
			
			if (GUI.Button(new Rect(25, 75, 75, 20), "Credits")){
				onCreditsScreen = true;
				onBackStoryScreen = false;
				onHelpScreen = false;
				onAboutScreen = false;
				onOptionsScreen = false;
				onLoadingScreen = false;
				amountFaded = 0f;
			}
			
			if (GUI.Button(new Rect(Screen.width - 100, 100, 75, 20), "Load")){
				onCreditsScreen = false;
				onBackStoryScreen = false;
				onHelpScreen = false;
				onAboutScreen = false;
				onOptionsScreen = false;
				onLoadingScreen = true;
			}
			
			if (GUI.Button(new Rect(Screen.width - 100, 125, 75, 20), "Save")){
				onCreditsScreen = false;
				onBackStoryScreen = false;
				onHelpScreen = false;
				onAboutScreen = false;
				onOptionsScreen = false;
				onLoadingScreen = false;
				onSavingScreen = true;
				timeOfSaving = CommandLineControl.counter;
			}
			
			if (GUI.Button(new Rect(Screen.width - 100, 150, 75, 20), "Quit")){
				onCreditsScreen = false;
				onBackStoryScreen = false;
				onHelpScreen = false;
				onAboutScreen = false;
				onOptionsScreen = false;
				onLoadingScreen = false;
				onSavingScreen = true;
				timeOfSaving = CommandLineControl.counter;
				Application.Quit();
			}
			
			if (GUI.Button(new Rect(25, 100, 75, 20), "Story")){
				onCreditsScreen = false;
				onBackStoryScreen = true;
				onHelpScreen = false;
				onAboutScreen = false;
				onOptionsScreen = false;
				onLoadingScreen = false;
				amountFaded = 0f;
			}
			
			if (GUI.Button(new Rect(25, 125, 75, 20), "Help")){
				onCreditsScreen = false;
				onBackStoryScreen = false;
				onHelpScreen = true;
				onAboutScreen = false;
				onOptionsScreen = false;
				onLoadingScreen = false;
				amountFaded = 0f;
			}
			
			if (GUI.Button(new Rect(25, 150, 75, 20), "About")){
				onCreditsScreen = false;
				onBackStoryScreen = false;
				onHelpScreen = false;
				onAboutScreen = true;
				onOptionsScreen = false;
				amountFaded = 0f;
			}
			
			if (GUI.Button(new Rect(25, 175, 75, 20), "Options")){
				onCreditsScreen = false;
				onBackStoryScreen = false;
				onHelpScreen = false;
				onAboutScreen = false;
				onOptionsScreen = true;
				onLoadingScreen = false;
				amountFaded = 0f;
			}
			
			if (GUI.Button(new Rect(Screen.width - 100, 175, 75, 20), "New Game")){
				onCreditsScreen = false;
				onBackStoryScreen = false;
				onHelpScreen = false;
				onAboutScreen = false;
				onOptionsScreen = false;
				onLoadingScreen = false;
				amountFaded = 0f;
				newGame();
			}
			
			if (onCreditsScreen){
				GUI.contentColor = new Color(1f, 1f, 1f, amountFaded);
				if (amountFaded < 1.0f){ 
					amountFaded += 0.003f;
				}
				string str;
				str = credits;
				GUI.TextArea(new Rect(125, 75, Screen.width - 250, Screen.height - 150), str);
			}
			
			if (onAboutScreen){
				GUI.contentColor = new Color(1f, 1f, 1f, amountFaded);
				if (amountFaded < 1.0f){ 
					amountFaded += 0.003f;
				}
				string str = about;
				GUI.TextArea(new Rect(125, 75, Screen.width - 250, Screen.height - 150), str);
			}
			
			if (onBackStoryScreen){
				if (amountFaded < 3.5f){ 
					amountFaded += 0.003f;
				}
				
				GUI.contentColor = new Color(1f, 1f, 1f, amountFaded);
				string str1 = "\n    They died by the hundreds;";
				GUI.TextArea(new Rect(125, 75, Screen.width - 250, Screen.height - 150), str1);
				
				GUI.contentColor = new Color(1f, 1f, 1f, amountFaded - 0.5f);
				string str2 = str1 + " they died by the thousands.";
				GUI.TextArea(new Rect(125, 75, Screen.width - 250, Screen.height - 150), str2);
				
				GUI.contentColor = new Color(1f, 1f, 1f, amountFaded - 1.5f);
				string str3 = str2 + "\n\n    In the first second of the war, the neutron bombs wiped out half the world. After the first minute, it was over; only one patch of humanity remained. Into the depths of ignorance spiraled the greatest knowledge of humankind. The last humans knew nothing practical: a scholar who had made steam his life's work, and an artificial intelligence expert. None of them knew how to survive in the barren landscape of the world, but together they eked out a living, working on their project, always working. But it could not last; they died from radiation poisoning, yet not before they finished their work: the robots.";
				GUI.TextArea(new Rect(125, 75, Screen.width - 250, Screen.height - 150), str3);
				
				GUI.contentColor = new Color(1f, 1f, 1f, amountFaded - 2.5f);
				string str4 = str3 + "\n\n    The humans have disappeared; only the steam-driven robots, living in their mines, remain. Steam is the lifeblood of society. Without it, everything grinds to a halt. But steam needs coal, and unless you can find the fabled earth-heat, you will run out.";
				GUI.TextArea(new Rect(125, 75, Screen.width - 250, Screen.height - 150), str4);
			}
			
			if (onHelpScreen){
				GUI.contentColor = new Color(1f, 1f, 1f, amountFaded);
				if (amountFaded < 1.0f){ 
					amountFaded += 0.003f;
				}
				string str = "How to play:\n\nMovement:\nUse the arrow keys to move left, right, forward, and backwards.\nUse WASD to rotate the camera.\nYou can also move your mouse to the edge of the screen to move. (This can be adjusted on the options screen)" 
					+ "\n\nGeneral advice:\nCheck out our wiki! http://code.google.com/p/100-degrees-celsius/wiki/MainPage\nWhen in doubt, click on it.\nItems can be right-clicked for a description.\nThere are also several hot keys: http://code.google.com/p/100-degrees-celsius/wiki/Hotkeys";
				GUI.TextArea(new Rect(125, 75, Screen.width - 250, Screen.height - 150), str);
			}
			
			if (onOptionsScreen){
				GUI.contentColor = new Color(1f, 1f, 1f, amountFaded);
				if (amountFaded < 1.0f){ 
					amountFaded += 0.003f;
				}
				
				SoundController.musicOn = GUI.Toggle(new Rect((Screen.width / 2) - 50, 100, 100, 20), SoundController.musicOn, "Music");
				SoundController.effectsOn = GUI.Toggle(new Rect((Screen.width / 2) - 50, 125, 100, 20), SoundController.effectsOn, "Effects");
				SoundController.volume = GUI.HorizontalSlider(new Rect((Screen.width / 2) - 50, 150, 200, 20), SoundController.volume, 0.0f, 1.0f);
				GUI.Label(new Rect((Screen.width / 2) - 100, 145, 55, 20), "Volume");
				SoundController.soundControl.updateSounds();
				
				scrollOnClose = GUI.Toggle(new Rect((Screen.width / 2) - 50, 175, 100, 20), scrollOnClose, "Scrolling");
				//robotsPrintPosition = GUI.Toggle(new Rect((Screen.width / 2) - 50, 200, 100, 20), robotsPrintPosition, "Position");
				
				Pipe.pipesPerFrame = (int)GUI.HorizontalSlider(new Rect((Screen.width / 2) - 50, 200, 200, 20), Pipe.pipesPerFrame, 50, 1.0f);
				GUI.Label(new Rect((Screen.width / 2) - 145, 195, 550, 25), "Pipe Checks");
				//CommandLineControl.pipeCheck = (int)GUI.HorizontalSlider(new Rect((Screen.width / 2) - 50, 225, 200, 20), CommandLineControl.pipeCheck, 1.0f, 100000.0f);
				//GUI.Label(new Rect((Screen.width / 2) - 145, 220, 550, 25), "Pipe Smoothing");
				
				WeatherControl.rainOn = GUI.Toggle(new Rect((Screen.width / 2) - 50, 250, 100, 20), WeatherControl.rainOn, "Rain");
				CommandLineControl.printingFPS = GUI.Toggle(new Rect((Screen.width / 2) - 50, 275, 100, 20), CommandLineControl.printingFPS, "FPS");
				
				Water.updatingFrequency = (int)GUI.HorizontalSlider(new Rect((Screen.width / 2) - 50, 300, 200, 20), Water.updatingFrequency, 1f, 250f);
				GUI.Label(new Rect((Screen.width / 2) - 145, 295, 550, 25), "Water Updating");
				
			}
			
			if (onLoadingScreen){
				/*if(LevelSerializer.SavedGames.Count > 0) {
					GUI.Label(new Rect((Screen.width / 2) - 50, 100, 200, 25), "Available saved games");
					int i = 0;
					foreach(var g in LevelSerializer.SavedGames["Me"]){
						if(GUI.Button(new Rect((Screen.width / 2) - 150, 125 + 25 * i, 300, 20), g.Caption)){
							g.Load();
							string[] words = CommandLineControl.splitString(LevelSerializer.SavedGames["Me"][i].Caption);
							int newRegionNumber =  int.Parse(words[1]);
							game = "Region " + newRegionNumber;
						}
						i++;
					}
				}*/
			}
			
		}
		
		if (!onMainMenu){
			if (!onBuildingMenu){
				if (!buildingSelected){
					if (GUI.Button(new Rect(Screen.width - 110, 10, 100, 20), "Main Menu")){
						onMainMenu = true;
						onCreditsScreen = false;
						onBackStoryScreen = false;
						onHelpScreen = false;
						onAboutScreen = false;
						onOptionsScreen = false;
						onLoadingScreen = false;
					}
					
					if (GUI.Button(new Rect(Screen.width - 110, 35, 100, 20), "Buildings")){
						onMainMenu = false;
						onBuildingMenu = true;
						scrollingDisabled = true;
						materials = new ArrayList();
					}
				}
			} else {
				if (GUI.Button(new Rect(Screen.width - 110, 10, 100, 20), "Back")){
					onBuildingMenu = false;
					scrollingDisabled = !scrollOnClose;
				}
				
				if (GUI.Button(new Rect(Screen.width - 110, 35, 100, 20), "Smelter")){
					scrollingDisabled = !scrollOnClose;
					buildingSelected = true;
					onBuildingMenu = false;
					building = (GameObject) GameObject.Instantiate(UnityEngine.Resources.Load("Prefabs/smelterPrefab"));
					WorkshopManager s = (WorkshopManager)building.GetComponent("WorkshopManager");
					s.active = false;
					materialsRequired = 3;
					buildingType = 1;
					addedHeight = 0;
				}
				
				if (GUI.Button(new Rect(Screen.width - 110, 60, 100, 20), "Forge")){
					scrollingDisabled = !scrollOnClose;
					buildingSelected = true;
					onBuildingMenu = false;
					building = (GameObject) GameObject.Instantiate(UnityEngine.Resources.Load("Prefabs/ForgingPressPrefab"));
					WorkshopManager s = (WorkshopManager)building.GetComponent("WorkshopManager");
					s.active = false;
					materialsRequired = 3;
					buildingType = 2;
					addedHeight = 0;
				}
				
				if (GUI.Button(new Rect(Screen.width - 110, 85, 100, 20), "Assembly Line")){
					scrollingDisabled = !scrollOnClose;
					buildingSelected = true;
					onBuildingMenu = false;
					building = (GameObject) GameObject.Instantiate(UnityEngine.Resources.Load("Prefabs/AssemblyLinePrefab"));
					WorkshopManager s = (WorkshopManager)building.GetComponent("WorkshopManager");
					s.active = false;
					materialsRequired = 3;
					buildingType = 3;
					addedHeight = 2.5f;
				}
				
				if (GUI.Button(new Rect(Screen.width - 110, 110, 100, 20), "Boiler")){
					scrollingDisabled = !scrollOnClose;
					buildingSelected = true;
					onBuildingMenu = false;
					building = (GameObject) GameObject.Instantiate(UnityEngine.Resources.Load("Prefabs/Boiler"));
					materialsRequired = 3;
					buildingType = 4;
					addedHeight = 0;
				}
				
				if (GUI.Button(new Rect(Screen.width - 110, 135, 100, 20), "Wall")){
					scrollingDisabled = !scrollOnClose;
					buildingSelected = true;
					onBuildingMenu = false;
					building = (GameObject) GameObject.Instantiate(UnityEngine.Resources.Load("Prefabs/WallPrefab"));
					materialsRequired = 1;
					buildingType = 5;
					addedHeight = 1f;
				}
				
				if (GUI.Button(new Rect(Screen.width - 110, 160, 100, 20), "Pipe")){
					scrollingDisabled = !scrollOnClose;
					buildingSelected = true;
					onBuildingMenu = false;
					building = (GameObject) GameObject.Instantiate(UnityEngine.Resources.Load("Prefabs/StraightPipePrefab"));
					Pipe p = (Pipe) building.GetComponent("Pipe");
					p.activated = false;
					materialsRequired = 0;
					buildingType = 6;
					addedHeight = 0.55f;
				}
				
				if (GUI.Button(new Rect(Screen.width - 110, 185, 100, 20), "Refueling")){
					scrollingDisabled = !scrollOnClose;
					buildingSelected = true;
					onBuildingMenu = false;
					building = (GameObject) GameObject.Instantiate(UnityEngine.Resources.Load("Prefabs/RefuelingStopPrefab"));
					materialsRequired = 3;
					buildingType = 7;
					addedHeight = 0f;
				}
				
				if (GUI.Button(new Rect(Screen.width - 110, 210, 100, 20), "Block Cutter")){
					scrollingDisabled = !scrollOnClose;
					buildingSelected = true;
					onBuildingMenu = false;
					building = (GameObject) GameObject.Instantiate(UnityEngine.Resources.Load("Prefabs/BlockCutterPrefab"));
					materialsRequired = 3;
					buildingType = 8;
					addedHeight = 0f;
				}
			}
			
			if (matSelectionButtons){
				if (materials.Count < materialsRequired){
					if (ItemList.tinBars.Count > 0){
						if (GUI.Button(new Rect(10, 25, 100, 20), "Tin")){
							materials.Add(ItemList.tinBars.Dequeue());
						}
					}
					
					if (ItemList.aluminumBars.Count > 0){
						if (GUI.Button(new Rect(10, 50, 100, 20), "Aluminum")){
							materials.Add(ItemList.aluminumBars.Dequeue());
						}
					}
					
					if (ItemList.brassBars.Count > 0){
						if (GUI.Button(new Rect(10, 75, 100, 20), "Brass")){
							materials.Add(ItemList.brassBars.Dequeue());
						}
					}
					
					if (ItemList.bronzeBars.Count > 0){
						if (GUI.Button(new Rect(10, 100, 100, 20), "Bronze")){
							materials.Add(ItemList.bronzeBars.Dequeue());
						}
					}
					
					if (ItemList.copperBars.Count > 0){
						if (GUI.Button(new Rect(10, 125, 100, 20), "Copper")){
							materials.Add(ItemList.copperBars.Dequeue());
						}
					}
					
					if (ItemList.galvanizedSteelBars.Count > 0){
						if (GUI.Button(new Rect(10, 150, 100, 20), "Galvanized Steel")){
							materials.Add(ItemList.galvanizedSteelBars.Dequeue());
						}
					}
					
					if (ItemList.ironBars.Count > 0){
						if (GUI.Button(new Rect(10, 175, 100, 20), "Iron")){
							materials.Add(ItemList.ironBars.Dequeue());
						}
					}
					
					if (ItemList.steelBars.Count > 0){
						if (GUI.Button(new Rect(10, 200, 100, 20), "Steel")){
							materials.Add(ItemList.steelBars.Dequeue());
						}
					}
					
					if (ItemList.zincBars.Count > 0){
						if (GUI.Button(new Rect(10, 225, 100, 20), "Zinc")){
							materials.Add(ItemList.zincBars.Dequeue());
						}
					}
					
					if (ItemList.blocks.Count > 0){
						if (GUI.Button(new Rect(10, 250, 100, 20), "Blocks")){
							materials.Add(ItemList.blocks.Dequeue());
						}
					}
					
					if (GUI.Button(new Rect(10, 275, 100, 20), "Cancel")){
						matSelectionButtons = false;
						gamePaused = false;
						scrollingDisabled = !scrollOnClose;
						GameObject.Destroy(building);
					}
					
					if (Input.GetKey(KeyCode.Escape)){
						matSelectionButtons = false;
						gamePaused = false;
						scrollingDisabled = !scrollOnClose;
						GameObject.Destroy(building);
					}
					
				} else {
					if (buildingType != 6){
						matSelectionButtons = false;
						gamePaused = false;
						scrollingDisabled = !scrollOnClose;
						new ConstructionJob(building.transform.position, materials, buildingType);
						GameObject.Destroy(building);
						materials = new ArrayList();
					} else {
						Vector3 goalLoc = building.transform.position;
						//ArrayList nodes = Point.aStar(startLoc, goalLoc);
						getPath(startLoc, goalLoc, delegate (Pathfinding.Path p) {
							bool doingThis = true;
							int i = 0;
							var nodes = p.vectorPath;
							foreach (Vector3 node in nodes){
								if (doingThis){
									new ConstructionJob(node, materials, 6);
									doingThis = false;
									if ((i > 1)){
										if ((node.x != (nodes[i-2]).x) && (node.z != (nodes[i-2]).z)){
											new ConstructionJob(new Vector3(node.x, node.y, (int)(nodes[i-2]).z), materials, 6);
										}
									}
								} else {
									doingThis = true;
								}
								
								i++;
							}
						});

						matSelectionButtons = false;
						gamePaused = false;
						scrollingDisabled = !scrollOnClose;
						GameObject.Destroy(building);
						materials = new ArrayList();
					}
				}
				
			}
		}
	}

	void getPath(Vector3 start, Vector3 destination, OnPathDelegate callback){
		seeker.StartPath(start, destination, callback);
	}
	
	public void save(){		
		
		/*int regionNumber = getRegionNumber();
		
		for (int i = 0; i < LevelSerializer.SavedGames["Me"].Count; i++){
			string[] words = CommandLineControl.splitString(LevelSerializer.SavedGames["Me"][i].Caption);
			int newRegionNumber =  int.Parse(words[1]);
			if (newRegionNumber == regionNumber){
				LevelSerializer.SavedGames["Me"][i].Delete();
			}
		}
		LevelSerializer.PlayerName = "Me";
		LevelSerializer.SaveGame(game);
		onSavingScreen = false;*/
	}
	
	public void load(string toLoad){
		/*LevelSerializer.SavedGames[toLoad][0].Load();*/
	}
	
	public static int getRegionNumber(){
		/*string[] thisWords = CommandLineControl.splitString(game);
		int regionNumber =  int.Parse(thisWords[1]);
		return regionNumber;*/
		return 0;
	}
	
	public void newGame(){
		
		//generate terrain
		
		//embark
		
		//save
		game = "Region " + (getRegionNumber() + 1);
		save();
	}
	
	public static void clearAllSavedGames(){
		/*for (int i = 0; i < LevelSerializer.SavedGames["Me"].Count; i++){
			LevelSerializer.SavedGames["Me"][i].Delete();
		}*/
	}
	
}
