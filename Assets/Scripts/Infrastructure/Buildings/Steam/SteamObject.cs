using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace KofTools{
	public class SteamObject : MonoBehaviour {
		
		public int pressure;
		public bool isCarryingWater;
		public int len;
		public int x, y, z;
		public bool activated;
		
		protected List<SteamObject> adjacentSteams;
		
		public void calculatePressure(){
			pressure = 0;
			foreach(SteamObject p in adjacentSteams){
				if (p.pressure - 10 > pressure){
					pressure = p.pressure - 10;
				}
			}
		}
		
		public void calculateAdjacentSteams(){
			adjacentSteams = new List<SteamObject>();
			
			Collider[] cols = Physics.OverlapSphere(transform.position, 2.0f);
			foreach (Collider c in cols){
				//string str = c.gameObject.ToString();
				if (c.gameObject.GetComponent<Pipe>() != null){
					SteamObject p = (SteamObject) c.gameObject.GetComponent<Pipe>();
					adjacentSteams.Add(p);
				} else if (c.gameObject.GetComponent<BoilerManager>() != null){
					SteamObject p = (SteamObject) c.gameObject.GetComponent<BoilerManager>();
					adjacentSteams.Add(p);
				} else if (c.gameObject.GetComponent<RefuelingStationManager>() != null){
					//SteamObject p = (SteamObject) c.gameObject.GetComponent<RefuelingStationManager>();
					//adjacentSteams.Add(p);
				} else {
					//Debug.Log(c.gameObject.ToString());
				}
			}
			
			len = adjacentSteams.Count;
			
		}
		
		public void calculateWater(){
			isCarryingWater = false;
			
			foreach(SteamObject p in adjacentSteams){
				if (p.isCarryingWater){
					isCarryingWater = true;
					pressure = 0;
				}
			}
			
			if (!isCarryingWater){
				Collider[] cols = Physics.OverlapSphere(transform.position, 2.7f);
				foreach (Collider c in cols){
					if (c.gameObject.layer == 4){
						isCarryingWater = true;
						pressure = 0;
					}
				}
			}
		}
		
		public void updatePos(){
			x = (int) transform.position.x;
			y = (int) transform.position.y;
			z = (int) transform.position.z;
			transform.position = new Vector3((float) x, (float) y, (float)z);
		}
		
		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
		
		}
	}
}
