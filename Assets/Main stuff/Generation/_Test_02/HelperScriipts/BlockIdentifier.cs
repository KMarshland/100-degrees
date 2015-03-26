using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class BlockIdentifier
{
	
	#region Spherical Block Finder
	
	// if radius is an integer value use this. Should only be used with Vector3I locations
	public static Vector3I[] GetBlockPositionsWithinSphericalRadius(Vector3I location, int radius){
		int nX = location.x - radius;
		int pX = location.x + radius;
		
		int nZ = location.z - radius;
		int pZ = location.z + radius;
		
		int nY = location.y - radius;
		int pY = location.y + radius;
		
		List<Vector3I> blocks = new List<Vector3I>();
		Vector3I checkBlock;
		
		for (int x = nX; x <= pX; x++){
			for (int y = nY; y <= pY; y++){
				for (int z = nZ; z <= pZ; z++){
					checkBlock = new Vector3I(x,y,z);
					
					int distance = Vector3I.EDistance(checkBlock, location);
					
					if (distance <= radius){
						blocks.Add(checkBlock);
					}
				}
			}
		}
		
		return blocks.ToArray();
	}
	// if radius is a floating point value use this. Should only be used with Vector3 locations
	public static Vector3I[] GetBlockPositionsWithinSphericalRadius(Vector3 location, float radius){
		return null;
	}
	#endregion
	
	#region Lateral Block Finder
	// - Only checks horizontally along xz-plane
	// : Includes location in the returned values.
	// Note: Does NOT check to see if the positions returned should or should not exist
	public static Vector3I[] GetBlockPositionsWithinLateralRadius(Vector3I location, int radius){
		// since the areas directly outward from location by radius are guaranteed we need to check from -radius to +radius in both x and z dimensions
		int nX = location.x - radius;
		int pX = location.x + radius;
		
		int nZ = location.z - radius;
		int pZ = location.z + radius;
		
		List<Vector3I> blocks = new List<Vector3I>();
		Vector3I checkBlock;
		
		for (int x = nX; x <= pX; x++){
			for (int z = nZ; z <= pZ; z++){
				checkBlock = new Vector3I(x,location.y, z);
				
				int distance = Vector3I.EDistance(checkBlock, location);
				
				//Debug.Log(string.Format("P<{0},{1},{2}>: Distance = {3}", checkBlock.x, checkBlock.y, checkBlock.z, distance));
				
				if (distance <= radius){
					blocks.Add(checkBlock);
				}
			}
		}
		
		return blocks.ToArray();
	}
	public static Vector3I[] GetBlockPositionsWithinLateralRadius(Vector3 location, float radius){
		return null;
	}
	#endregion
}

