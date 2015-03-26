using UnityEngine;
using System.Collections;
using KofTools;

public class WeatherControl : MonoBehaviour {
	
	public static WeatherControl weather;
	
	public static ArrayList weatherSystems;
	
	public static readonly string[] weatherIDs = { //list of strings to spawn weathers
		/*0*/"CLOUDS/AS_cloudBig", "CLOUDS/AS_cloudMedium",
		/*2*/"CLOUDS/AS_cloudSmall", "CLOUDS/AS_cloudBigDark",  
		/*4*/"CLOUDS/AS_cloudSmallDark", "CLOUDS_rainClouds/AS_rainCloudBig",  "CLOUDS/AS_cloudMediumDark",
		/*7*/"CLOUDS_rainClouds/AS_rainCloudSmallDark", "CLOUDS_rainClouds/AS_rainCloudSmallGrey", 
		/*9*/"CLOUDS_rainClouds/AS_rainCloudSmallMoving",
		/*10*/"FOG/AS_fog1", "FOG/AS_fog2slowlyMoving", "FOG/AS_fog3",
		/*13*/"HAIL/AS_hailStorm1", "HAIL/AS_hailStorm2", "HAIL/AS_hailStorm3",
		/*16*/"LEAVES/AS_leavesInTheWind1", "LEAVES/AS_leavesInTheWind2", "LEAVES/AS_leavesInTheWind3",
		/*18*/"LIGHTNING/AS_lightningCloudSimple1", "LIGHTNING/AS_lightningCloudSimple2", 
		/*20*/"LIGHTNING/AS_lightningCloudSimple3", "LIGHTNING/AS_lightningCloudSimple4",
		/*22*/"LIGHTNING/AS_lightningCloudWithScript1", "LIGHTNING/AS_lightningCloudWithScript2", 
		/*24*/"LIGHTNING/AS_lightningCloudWithScript3", "LIGHTNING/AS_lightningCloudWithScript4",
		/*27*/"RAIN/AS_rainWithCollider1", "RAIN/AS_rainWithCollider1v2", "RAIN/AS_rainWithCollider2",
		/*30*/"RAIN/AS_rainWithCollider2v2", "RAIN/AS_rainWithCollider3", "RAIN/AS_rainWithCollider3v2",
		/*33*/"SNOW/AS_snowstorm1large", "SNOW/AS_snowstorm2", "SNOW/AS_snowstorm3"
	};
	
	public static bool rainOn;
	public static bool raining;
	public bool isRaining;
	
	public static float frequency; //lower = more frequent
	public static float frequencyVariance;
	public static float duration; //lower = longer
	public static float durationVariance;
	public static float intensity; //lower = more intense
	public static float intensityVariance;
	
	public float variedFrequency;
	public float variedDuration;
	public float variedIntensity;
	
	bool previouslyRaining;
	public float previousIntensity;
	
	int timer;
	
	GameObject systemAbove;
	ArrayList lightnings;
	ArrayList clouds;
	ArrayList fogs;
	
	GameObject rainSound;
	
	public bool rain;
	public bool hail;
	public bool snow;
	public bool fog;
	
	// Use this for initialization
	void Start () {
		
		rainOn = false;
		weatherSystems = new ArrayList();
		
		weather = this;
		frequency = 0.6f;
		frequencyVariance = 0.1f;
		duration = 0.0015f;
		durationVariance = 0.0005f;
		intensity = 0.15f;
		intensityVariance = 0.03f;
		
		lightnings = new ArrayList();
		clouds = new ArrayList();
		fogs = new ArrayList();
		
		randomize();
		
		spawnWeather(4, new Vector3(CameraControl.x, CameraControl.y + 50, CameraControl.z), true);
				
		for (int i = 0; i < 15; i++){
			clouds.Add(spawnWeather(3, new Vector3(Random.Range(-700, 700), 150, Random.Range(-150, 550)), false));
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		isRaining = raining;
		
		if (rainOn){
			
			timer ++;
			previousIntensity = LightControl.intensity;
			
			//sets the light levels
			LightControl.changeLight(variedFrequency - Mathf.Sin(variedDuration * timer));
			
			//keeps them in decent parameters
			if (LightControl.intensity > 0.35f){
				LightControl.changeLight(0.35f);
			} else if (LightControl.intensity < variedIntensity){
				LightControl.changeLight(variedIntensity);
			}
			
			previouslyRaining = raining;
			
			//sets the raining bool and spawns/ destroys weather above camera if necessary
			if (LightControl.intensity < 0.3f){
				raining = true;
				
				if (rain){ //spawns rain above camera
					if ((LightControl.intensity <= 0.3f && LightControl.intensity > 0.25f) && (previousIntensity > 0.3f || previousIntensity <= 0.25f)){
						destroyAbove();
						clearLightnings();
						systemAbove = spawnWeather(27, new Vector3(CameraControl.x - 8, CameraControl.y + 50, CameraControl.z - 10), true);
					} else if ((LightControl.intensity <= 0.25f && LightControl.intensity > 0.20f) && (previousIntensity > 0.25f || previousIntensity <= 0.20f)){
						destroyAbove();
						clearLightnings();
						systemAbove = spawnWeather(30, new Vector3(CameraControl.x - 8, CameraControl.y + 50, CameraControl.z - 10), true);
					} else if ((LightControl.intensity <= 0.20f && LightControl.intensity > 0.16f) && (previousIntensity > 0.20f || previousIntensity <= 0.16f)){
						destroyAbove();
						clearLightnings();
						systemAbove = spawnWeather(28, new Vector3(CameraControl.x - 8, CameraControl.y + 50, CameraControl.z - 10), true);
					} else if ((LightControl.intensity <= 0.16f && LightControl.intensity > 0f) && (previousIntensity > 0.16f || previousIntensity < 0f)){
						destroyAbove();
						clearLightnings();
						systemAbove = spawnWeather(32, new Vector3(CameraControl.x - 8, CameraControl.y + 50, CameraControl.z - 15), true);
						
						for (int i = 0; i < Random.Range(1, 10); i++){
							lightnings.Add(spawnWeather(Random.Range(19, 22), new Vector3(Random.Range(-700, 700), 150, Random.Range(-150, 550)), false));
						}
						
					}
				} else if (hail){
					if ((LightControl.intensity <= 0.3f && LightControl.intensity > 0.25f) && (previousIntensity > 0.3f || previousIntensity <= 0.25f)){
						destroyAbove();
						clearLightnings();
						systemAbove = spawnWeather(15, new Vector3(CameraControl.x - 8, CameraControl.y + 50, CameraControl.z - 5), true);
					} else if ((LightControl.intensity <= 0.25f && LightControl.intensity > 0.18f) && (previousIntensity > 0.25f || previousIntensity <= 0.20f)){
						destroyAbove();
						clearLightnings();
						systemAbove = spawnWeather(14, new Vector3(CameraControl.x - 8, CameraControl.y + 50, CameraControl.z - 5), true);
					} else if ((LightControl.intensity <= 0.18f && LightControl.intensity > 0f) && (previousIntensity > 0.16f || previousIntensity < 0f)){
						destroyAbove();
						clearLightnings();
						systemAbove = spawnWeather(13, new Vector3(CameraControl.x - 8, CameraControl.y + 50, CameraControl.z - 5), true);
						
						for (int i = 0; i < Random.Range(1, 10); i++){
							//lightnings.Add(spawnWeather(Random.Range(19, 22), new Vector3(Random.Range(-700, 700), 150, Random.Range(-150, 550)), false));
						}
						
					}
				} else if (snow){
					if ((LightControl.intensity <= 0.3f && LightControl.intensity > 0.25f) && (previousIntensity > 0.3f || previousIntensity <= 0.25f)){
						destroyAbove();
						clearLightnings();
						systemAbove = spawnWeather(33, new Vector3(CameraControl.x - 10, CameraControl.y + 5, CameraControl.z - 7), true);
					} else if ((LightControl.intensity <= 0.25f && LightControl.intensity > 0.18f) && (previousIntensity > 0.25f || previousIntensity <= 0.20f)){
						destroyAbove();
						clearLightnings();
						systemAbove = spawnWeather(35, new Vector3(CameraControl.x - 10, CameraControl.y + 5, CameraControl.z - 7), true);
					} else if ((LightControl.intensity <= 0.18f && LightControl.intensity > 0f) && (previousIntensity > 0.16f || previousIntensity < 0f)){
						destroyAbove();
						clearLightnings();
						systemAbove = spawnWeather(34, new Vector3(CameraControl.x - 10, CameraControl.y + 5, CameraControl.z - 7), true);
						
					}
				} else if (fog){
					randomize();
					LightControl.changeLight(0.3f);
				}
				
				
				
			} else {
				raining = false;
			}
			
			//checks to see if the current rain cycle is over
			if (raining != previouslyRaining){
				if (!raining){
					randomize();
					try {
						weatherSystems.Remove(systemAbove);
						Destroy(systemAbove);
						rainSound.GetComponent<AudioSource>().mute = true;
						//Destroy(rainSound);
					} catch {
						
					}
				} else if (rain){
					rainSound = SoundController.playEffect("Rain", false, true);
					rainSound.GetComponent<AudioSource>().mute = false;
				}
			}
			
			
			
		} else {
			LightControl.changeLight(0.3f);
		}
		
	}
	
	void randomize(){ //resets various variables for next rain cycle
		variedFrequency = frequency + (-1 * frequencyVariance) + (Random.value * (2 * frequencyVariance));
		variedDuration = duration + (-1 * durationVariance) + (Random.value * (2 * durationVariance));
		variedIntensity = intensity + (-1 * intensityVariance) + (Random.value * (2 * intensityVariance));
		
		int type = Random.Range(0, 11);
		
		rain = false;
		hail = false;
		snow = false;
		fog = false;
		
		
		if (type < 7){
			rain = true;
			clearFogs();
		} else if (type >= 7 && type < 9){
			hail = true;
			clearFogs();
		} else if (type >= 9 && type < 10){
			snow = true;
			clearFogs();
		} else if (type >= 10 && type < 12){
			fog = true;
			for (int i = 0; i < Random.Range(1, 10); i++){
				fogs.Add(spawnWeather(11, new Vector3(Random.Range(-700, 700), 0, Random.Range(-150, 550)), false));
			}
		}
		
	}
	
	public void destroyAbove(){ //removes the rain above the camera
		try {
			weatherSystems.Remove(systemAbove);
			Destroy(systemAbove);
		} catch {
			
		}
	}
	
	public void clearLightnings(){ //deletes active lightning clouds
		foreach (GameObject o in lightnings){
			Destroy(o);
		}
		
		lightnings = new ArrayList();
	}
	
	public void clearClouds(){ //deletes active clouds
		foreach (GameObject o in clouds){
			Destroy(o);
		}
		
		clouds = new ArrayList();
	}
	
	public void clearFogs(){ //deletes active fogs
		foreach (GameObject o in fogs){
			Destroy(o);
		}
		
		fogs = new ArrayList();
	}
	
	public static void spawnWeather(string type){//creates a new weather
		GameObject cloud = (GameObject)GameObject.Instantiate(Resources.Load("Weather Prefabs/" + type));
		weatherSystems.Add(cloud);
	}
	
	public static void spawnWeather(int type){//creates a new weather
		if (type >= 0 & type < weatherIDs.Length){
			GameObject cloud = (GameObject)GameObject.Instantiate(Resources.Load("Weather Prefabs/" + weatherIDs[type]));
			weatherSystems.Add(cloud);
		}
	}
	
	public static void spawnWeather(string type, Vector3 v){//creates a new weather
		GameObject cloud = (GameObject)GameObject.Instantiate(Resources.Load("Weather Prefabs/" + type));
		cloud.transform.position = v;
		weatherSystems.Add(cloud);
	}
	
	public static GameObject spawnWeather(int type, Vector3 v, bool follow){ //creates a new weather
		if (type >= 0 & type < weatherIDs.Length){
			GameObject cloud = (GameObject)GameObject.Instantiate(Resources.Load("Weather Prefabs/" + weatherIDs[type]));
			cloud.transform.position = v;
			if (follow){
				cloud.AddComponent<FollowCamera>();
				FollowCamera f = (FollowCamera)cloud.transform.GetComponent("FollowCamera");
				f.xOffset = f.transform.position.x - CameraControl.x;
				f.yOffset = f.transform.position.y - CameraControl.y;
				f.zOffset = f.transform.position.z - CameraControl.z;
			}
			weatherSystems.Add(cloud);
			return cloud;
		}
		return null;
	}
	
}
