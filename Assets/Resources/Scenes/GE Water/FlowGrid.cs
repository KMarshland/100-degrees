using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FlowGrid
{
	Vector3[,] grid;
	public int gridX, gridY;
	
	public int numUpdates = 0;
	
	public Vector3 this[int a,int b]{
		get{
			return grid[a,b];
		}
		set{
			grid[a,b] = value;
		}
	}
	
	public FlowGrid(int sizeX, int sizeY, bool seed = true){
		grid = new Vector3[sizeX, sizeY];
		
		gridX = sizeX;
		gridY = sizeY;
		
		for (int i = 0; i < gridX; i++){
			for (int j = 0; j < gridY; j++){
				grid[i,j] = new Vector3(i,0,j);
			}
		}
		
		if (seed){
			seedGrid();
		}
	}
	private void seedGrid(){
		System.Random gen = new System.Random();
		
		for (int i = 0; i < 10000; i++){
			int x = gen.Next(gridX);
			int y = gen.Next(gridY);
			
			grid[x,y].y += 1;
		}
	}
	public void update(){
		float decay = 0.75f * (1 - 0.001f * ++numUpdates);
		
		if (decay < 0.1){ decay = 0.1f;}
		
		FlowGrid temp = new FlowGrid(gridX, gridY, false);
		
		for (int i = 0; i < gridY; i++){
			for (int j = 0; j < gridX; j++){
				HashSet<Vector3> neighbors = new HashSet<Vector3>();
				
				for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        if (dx != 0 || dy!=0){
                            try
                            {
                                neighbors.Add(grid[j + dx, i + dy]);
                            }
                            catch { }
                        }
                    }
                }
				
				int indexNeedsTheMost = -1;
				float changeInHeight = 0.0f;
				
				Vector3[] neighborSet = neighbors.ToArray();
				for (int h = 0; h < neighborSet.Length; h++){
					float dH = neighborSet[h].y - grid[j,i].y;
					
					if (dH > changeInHeight){
						indexNeedsTheMost = h;
						changeInHeight = dH;
					}
				}
				
				if (indexNeedsTheMost != -1){
					float moveAmount = changeInHeight / 2f * decay;
					
					Vector3 t1 = temp[j,i];
					t1.y += moveAmount;
					temp[j,i] = t1;
					
					Vector3 t2 = temp[(int)neighborSet[indexNeedsTheMost].x, (int)neighborSet[indexNeedsTheMost].z];
					t2.y -= moveAmount;
					
					temp[(int)neighborSet[indexNeedsTheMost].x, (int)neighborSet[indexNeedsTheMost].z] = t2;
				}
			}
		}
		
		for (int i = 0; i < gridY; i++){
			for (int j = 0; j < gridX; j++){
				grid[j,i].y += temp[j,i].y;
			}
		}
	}
	
}

