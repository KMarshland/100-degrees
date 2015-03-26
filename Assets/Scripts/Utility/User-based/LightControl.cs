using UnityEngine;
using System.Collections;
using KofTools;

public class LightControl : MonoBehaviour {
	
	static LightControl lc;
	public static float intensity;
	
	// Use this for initialization
	void Start () {
		lc = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public static void changeLight(float light){
		lc.GetComponent<Light>().intensity = light;
		intensity = light;
	}
	
}
