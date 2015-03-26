using UnityEngine;
using System.Collections;

namespace KofTools{
	public static class ConstructionController {
		
		public static ArrayList boilers = new ArrayList();
		public static ArrayList pipes = new ArrayList();
		public static ArrayList rechargingStations = new ArrayList();
		
		public static ArrayList doors = new ArrayList();
		
		static Pipe[] pipeses;
		public static int pipeNum;
		public static int originalSize;
		
		public static void updatePipes(){
			
			pipeses = new Pipe[pipes.Count];
			pipeNum = 0;
			originalSize = pipes.Count;
			
			for (int i = 0; i < pipes.Count; i++){
				pipeses[i] = (Pipe) pipes[i];
			}
			
			for (int i = 0; i < pipes.Count; i++){
				
				Pipe p = pipeses[i];
				
				if (p != null){
					
					p.calculateAdjacentSteams();
					p.calculatePressure();
					p.calculateWater();
					p.updatePos();
					p.checkSurroundings();
					
					try {
					if (p.type != p.lastType & p.activated){
						p.updateGraphics();
					}} catch {}
				}
			}
		}
		
		public static void updatenextPipe(){
			
			pipeNum ++;
			
			if (pipeNum >= originalSize){
				pipeses = new Pipe[pipes.Count];
				pipeNum = 0;
				originalSize = pipes.Count;
				for (int i = 0; i < pipes.Count; i++){
					pipeses[i] = (Pipe) pipes[i];
				}
			}
			
			Pipe p = pipeses[pipeNum];
	
			if (p != null){
				p.calculateAdjacentSteams();
				p.calculatePressure();
				p.calculateWater();
				p.updatePos();
				p.checkSurroundings();
				
				try {
				if (p.type != p.lastType & p.activated){
					p.updateGraphics();
				}} catch {}
			} else {
				//updatenextPipe();
			}
			
			
			
		}
		
	}
}
