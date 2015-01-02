using UnityEngine;
using System;

public class Dbg
{ 
	public static void Assert(bool condition, string description) 
	{ 
#if true 
		if (!condition) {
			Debug.LogError(description);
			throw new Exception(); 
		}
#endif 
	}

	public static void Box (Vector3 center)
	{
		Box (new Vector3 (center.x, center.y, center.z), 1f, Color.white, 1f);
	}

	public static void Box (Vector3 center, Color color)
	{
		Box (new Vector3 (center.x, center.y, center.z), 1f, color, 1f);
	}

	public static void Box (Vector3 center, float width, Color color, float duration)
	{
		// bottom 
		Debug.DrawLine (new Vector3 (center.x - (width * .5f),	center.y - (width * .5f), 	center.z - (width * .5f)),
		                new Vector3 (center.x + (width * .5f),	center.y - (width * .5f),	center.z - (width * .5f)),	color,	duration);
		Debug.DrawLine (new Vector3 (center.x + (width * .5f),	center.y - (width * .5f),	center.z - (width * .5f)),
		                new Vector3 (center.x + (width * .5f),	center.y - (width * .5f),	center.z + (width * .5f)),	color, 	duration);
		Debug.DrawLine (new Vector3 (center.x + (width * .5f),	center.y - (width * .5f),	center.z + (width * .5f)),
		                new Vector3 (center.x - (width * .5f),	center.y - (width * .5f),	center.z + (width * .5f)),	color, 	duration);
		Debug.DrawLine (new Vector3 (center.x - (width * .5f),	center.y - (width * .5f),	center.z - (width * .5f)),
		                new Vector3 (center.x - (width * .5f),	center.y - (width * .5f),	center.z + (width * .5f)),	color, 	duration);
		
		// top 
		Debug.DrawLine (new Vector3 (center.x - (width * .5f),	center.y + (width * .5f),	center.z - (width * .5f)),
		                new Vector3 (center.x + (width * .5f),	center.y + (width * .5f),	center.z - (width * .5f)),	color, 	duration);
		Debug.DrawLine (new Vector3 (center.x + (width * .5f),	center.y + (width * .5f),	center.z - (width * .5f)),
		                new Vector3 (center.x + (width * .5f),	center.y + (width * .5f),	center.z + (width * .5f)),	color, 	duration);
		Debug.DrawLine (new Vector3 (center.x + (width * .5f),	center.y + (width * .5f),	center.z + (width * .5f)),
		                new Vector3 (center.x - (width * .5f),	center.y + (width * .5f),	center.z + (width * .5f)),	color, 	duration);
		Debug.DrawLine (new Vector3 (center.x - (width * .5f),	center.y + (width * .5f),	center.z - (width * .5f)),
		                new Vector3 (center.x - (width * .5f),	center.y + (width * .5f),	center.z + (width * .5f)),	color, 	duration);
		
		//Column
		Debug.DrawLine (new Vector3 (center.x - (width * .5f),	center.y - (width * .5f),	center.z - (width * .5f)),
		                new Vector3 (center.x - (width * .5f),	center.y + (width * .5f),	center.z - (width * .5f)),	color, 	duration);
		Debug.DrawLine (new Vector3 (center.x + (width * .5f),	center.y - (width * .5f),	center.z - (width * .5f)),
		                new Vector3 (center.x + (width * .5f),	center.y + (width * .5f),	center.z - (width * .5f)),	color, 	duration);
		Debug.DrawLine (new Vector3 (center.x + (width * .5f), 	center.y - (width * .5f),	center.z + (width * .5f)),
		                new Vector3 (center.x + (width * .5f),	center.y + (width * .5f),	center.z + (width * .5f)),	color, 	duration);
		Debug.DrawLine (new Vector3 (center.x - (width * .5f),	center.y - (width * .5f),	center.z + (width * .5f)),
		                new Vector3 (center.x - (width * .5f),	center.y + (width * .5f),	center.z + (width * .5f)),	color, 	duration);
	}
}

