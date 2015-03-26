using UnityEngine;
using System;
using System.Collections;

public struct Vector3I{
	
	public int x, y, z;
	
	// Accessors
	public static Vector3I one{
		get{
			return new Vector3I(1,1,1);
		}
	}
	public static Vector3I zero{
		get{
			return new Vector3I(0,0,0);
		}
	}
	
	// Directionals
	public static Vector3I forward{
		get{
			return new Vector3I(0,0,1);
		}
	}
	public static Vector3I back{
		get{
			return new Vector3I(0,0,-1);
		}
	}
	public static Vector3I up{
		get{
			return new Vector3I(0,1,0);
		}
	}
	public static Vector3I down{
		get{
			return new Vector3I(0,-1,0);
		}
	}
	public static Vector3I left{
		get{
			return new Vector3I(-1,0,0);
		}
	}
	public static Vector3I right{
		get{
			return new Vector3I(1,0,0);
		}
	}
	
	// This[Index] [0,2] (returns x,y, or z)
	public int this [int index]
	{
		get
		{
			switch (index)
			{
			case 0:

				{
					return this.x;
				}
			case 1:

				{
					return this.y;
				}
			case 2:

				{
					return this.z;
				}
			default:

				{
					throw new IndexOutOfRangeException ("Invalid Vector3 index!");
				}
			}
		}
		set
		{
			switch (index)
			{
			case 0:

				{
					this.x = value;
					break;
				}
			case 1:

				{
					this.y = value;
					break;
				}
			case 2:

				{
					this.z = value;
					break;
				}
			default:

				{
					throw new IndexOutOfRangeException ("Invalid Vector3 index!");
				}
			}
		}
	}
	
	
	// Constructor
	public Vector3I(int x, int y, int z){
		this.x = x;
		this.y = y;
		this.z = z;
	}
	public Vector3I(int x, int y){
		this.x = x;
		this.y = y;
		this.z = 0;
	}
	
	// Helper Functions - Distance
	public static int EDistance (Vector3 a, Vector3 b)
	{
		Vector3 vector = new Vector3 (a.x - b.x, a.y - b.y, a.z - b.z);
		return (int)Mathf.Sqrt (vector.x * vector.x + vector.y * vector.y + vector.z * vector.z);
	}
	public static int EDistance (Vector3I a, Vector3I b){
		Vector3I vector = new Vector3I(a.x - b.x, a.y - b.y, a.z - b.z);
		return (int)Mathf.Sqrt(vector.x * vector.x + vector.y * vector.y + vector.z * vector.z);
	}
	
	public static int MDistance (Vector3I a, Vector3I b){
		Vector3I vector = new Vector3I(a.x - b.x, a.y - b.y, a.z - b.z);
		//return (vector.x*vector.x) + (vector.y * vector.y) + (vector.z * vector.z);
		return (int)(Math.Abs(vector.x) + Math.Abs(vector.y) + Math.Abs(vector.z));
	}
	
	// Bit-Shift Operators
	public static Vector3I LShift(int a, Vector3I v){
		return new Vector3I(a<<v[0], a<<v[1], a<<v[2]);
	}
	public static Vector3I LShift(Vector3I a, Vector3I v){
		return new Vector3I(a[0]<<v[0], a[1]<<v[1], a[2]<<v[2]);
	}
	
	public static Vector3I RShift(int a, Vector3I v){
		return new Vector3I(a>>v[0], a>>v[1], a>>v[2]);
	}
	public static Vector3I RShift(Vector3I a, Vector3I v){
		return new Vector3I(a[0]>>v[0], a[1]>>v[1], a[2]>>v[2]);
	}
	
	public static Vector3I AShift(Vector3I a, Vector3I v){
		return new Vector3I(a[0] & v[0], a[1] & v[1], a[2] & v[2]);
	}
	
	// Casting functions
	public static Vector3 CastToVector3(Vector3I a){
		return new Vector3(a.x, a.y, a.z);
	}
	public static Vector3I CastToVector3I(Vector3 a){
		return new Vector3I((int)a.x, (int)a.y, (int)a.z);
	}
	
	// Operators
	public override bool Equals(System.Object obj){
		// If parameter is null return false.
		if (obj == null){
			return false;
		}
		
		// If parameter cannot be cast to Point return false.
		Vector3I p = (Vector3I)obj;
		if ((System.Object)p == null){
			return false;
		}
		
		// Return true if the fields match:
		return this == p;
	}
	public override int GetHashCode()
	{
		return x ^ y;
	}
	public static bool operator == (Vector3I a, Vector3I b){
		return a.x == b.x && a.y == b.y && a.z == b.z;
	}
	public static bool operator != (Vector3I a, Vector3I b){
		return a.x != b.x || a.y != b.y || a.z != b.z;
	}
	public static Vector3I operator + (Vector3I a, Vector3I b){
		return new Vector3I(a.x + b.x, a.y + b.y, a.z + b.z);
	}
	public static Vector3I operator - (Vector3I a, Vector3I b){
		return new Vector3I(a.x - b.x, a.y - b.y, a.z - b.z);
	}
	public static Vector3 operator * (Vector3I a, float d){
		return new Vector3(a.x * d, a.y * d, a.z * d);
	}
	public static Vector3I operator * (Vector3I a, int d){
		return new Vector3I(a.x * d, a.y * d, a.z * d);
	}
	public static Vector3I operator * (Vector3I a, Vector3I b){
		return new Vector3I(a.x * b.x, a.y * b.y, a.z * b.z);
	}
}
