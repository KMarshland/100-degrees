using UnityEngine;
using System.Collections;

public class DirtBlock : IBlock
{
	public static byte type = 1;
	
	#region IBlock implementation
	public bool IsSolid ()
	{
		return true;
	}
	#endregion
}

