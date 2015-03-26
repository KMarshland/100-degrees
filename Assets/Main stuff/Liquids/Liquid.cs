using UnityEngine;
using System.Collections;

namespace KofTools{
	public class Liquid {
		
		public static readonly int placesKept = 1000; // 1/1000 of a meter
		
		protected int x;
		protected int z;
		
		public float depth;
		public float surfaceLevel;
		public float bottom;
		
		GameObject surface;
		
		bool showing;
		bool hasObject;
		
		public Liquid(int nx, int nz){
			x = nx;
			z = nz;
			showing = false;
			hasObject = false;
		}
		
		//calculates how high the surface is based on depth and ground
		public void checkSurface(){
			try {
				Ray ray = new Ray(new Vector3(x, surfaceLevel, z), Vector3.up * -1f);
				RaycastHit[] hits = Physics.RaycastAll(ray);
				RaycastHit h = hits[0];
				bottom = (h.point.y);
			} catch {
				surfaceLevel = 0f;
			}
			surfaceLevel = bottom + depth;
		}
		
		public void normalizeFlows(Liquid l){
			
			float avgSurfaceLevel = (surfaceLevel + l.surfaceLevel) / 2;
			float surfaceDif = surfaceLevel + l.surfaceLevel;
			float totalVolume = (depth + l.depth);
			float avgDepth = totalVolume / 2;
			
			Debug.Log(totalVolume);
			
			if (bottom > l.bottom){
				float filled = bottom - l.bottom;
				totalVolume -= filled;
				if (totalVolume < 0){
					filled -= totalVolume;
					totalVolume = 0f;
					l.depth = filled;
					depth = 0;
				} else {
					float halfVol = totalVolume / 2;
					depth = halfVol;
					l.depth = halfVol + filled;
				}
			} else if (l.bottom > bottom){
				float filled = l.bottom - bottom;
				totalVolume -= filled;
				if (totalVolume < 0){
					filled -= totalVolume;
					totalVolume = 0f;
					depth = filled;
					l.depth = 0;
				} else {
					float halfVol = totalVolume / 2;
					l.depth = halfVol;
					depth = halfVol + filled;
				}
			} else {
				depth = avgDepth + (bottom - l.bottom);
				l.depth = avgDepth + (l.bottom - bottom);
			}
			
			//depth = avgSurfaceLevel - bottom;
			//l.depth = avgSurfaceLevel - l.bottom;
			
			//changeDepth(avgSurfaceLevel - this.surfaceLevel);
			//l.changeDepth(avgSurfaceLevel - l.surfaceLevel);
	
		}
		
		public void normalizeFlows(Liquid l, float inverseViscosity){
			
			float avgSurfaceLevel = (surfaceLevel + l.surfaceLevel) / 2;
			float surfaceDif = surfaceLevel + l.surfaceLevel;
			float totalVolume = (depth + l.depth);
			float avgDepth = totalVolume / 2;
			
			//Debug.Log(totalVolume);
			
			float pd = depth;
			float lpd = l.depth;
			
			if (bottom > l.bottom){
				/*float filled = bottom - l.bottom;
				totalVolume -= filled;
				if (totalVolume < 0){
					filled -= totalVolume;
					totalVolume = 0f;
					l.depth = filled;
					depth = 0;
				} else {
					float halfVol = totalVolume / 2;
					depth = halfVol;
					l.depth = (halfVol) + filled;
				}*/
				
				depth = avgDepth - (0.5f * (bottom - l.bottom));
				l.depth = avgDepth + (0.5f * (bottom - l.bottom));
				
			} else if (l.bottom > bottom){
				/*float filled = l.bottom - bottom;
				totalVolume -= filled;
				if (totalVolume < 0){
					filled -= totalVolume;
					totalVolume = 0f;
					depth = filled;
					l.depth = 0;
				} else {
					float halfVol = totalVolume / 2;
					l.depth = halfVol;
					depth = halfVol + filled;
				}*/
				
				depth = avgDepth + (0.5f * (l.bottom - bottom));
				l.depth = avgDepth - (0.5f * (l.bottom - bottom));
				
			} else {
				depth = avgDepth;
				l.depth = avgDepth;
			}
			
			depth += (inverseViscosity * (depth - pd));
			l.depth += (inverseViscosity * (l.depth - lpd));
			
		}
		
		//hides it if it is too shallow
		public void checkShowing(){
			if (depth < 0.03f){
				hide();
			} else if (!showing){
				show();
			}
		}
		
		protected void show(){
			if (hasObject){
				showing = true;
				surface.GetComponent<Renderer>().enabled = true;
			}
		}
		
		protected void hide(){
			if (hasObject){
				showing = false;
				surface.GetComponent<Renderer>().enabled = false;
			}
		}
		
		// updates the graphics of the water
		public void updateSurface(){
			if (hasObject){
				surface.transform.position = new Vector3(x, surfaceLevel - (0.5f * depth), z);
				surface.transform.localScale = new Vector3(1f, depth, 1f);
			}
		}
		
		//creates a new graphics thing
		public void newObject(string str){
			hasObject = true;
			surface = (GameObject)(GameObject.Instantiate(Resources.Load(str)));
		}
		
		public static float roundTo1Over(int over, float original){
			
			float places = (float) (over);
			
			float rounded = Mathf.Round(original * places)/ places;
			
			return rounded;
		}
		
		//allows you to change the depth by change
		public void changeDepth(float change){
			depth += change;
			if (depth < 0){
				//depth = 0f;
			}
			depth = Liquid.roundTo1Over(Liquid.placesKept, depth);
			
			checkShowing();
			updateSurface();
		}
		
	}
}
