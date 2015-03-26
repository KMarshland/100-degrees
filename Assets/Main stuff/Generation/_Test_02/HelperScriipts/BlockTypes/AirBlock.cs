using UnityEngine;
using System.Collections;

public class AirBlock : IBlock
{
	public static byte type = 0;
	
	#region IBlock implementation
	public bool IsSolid ()
	{
		return false;
	}
	#endregion
	
}

