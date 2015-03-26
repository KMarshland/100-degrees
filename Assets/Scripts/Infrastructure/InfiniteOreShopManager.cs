using UnityEngine;
using System.Collections;
using KofTools;

public class InfiniteOreShopManager : WorkshopManager {

	// Use this for initialization
	void Start () {
		active = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void OnMouseUp(){
		if (active){
			queueJob();
		}
	}
	
	public void queueJob(){
		new MiningJob(transform.position);
	}
}
