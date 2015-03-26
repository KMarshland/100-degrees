using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {
	
	public float xOffset, yOffset, zOffset;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(CameraControl.x + xOffset, CameraControl.y + yOffset, CameraControl.z + zOffset);
	}
}
