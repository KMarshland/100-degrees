using UnityEngine;
using System.Collections;

public class ControllerCaller : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Pipe.EachFrame();
	}
}
