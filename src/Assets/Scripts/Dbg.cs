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

	public static void Box (Vector3 p)
	{
#if true 
//		Debug.Log ("DebugBox:" + p);

		Color color = Color.white;
		float duration = 0.6f;

		// bottom 
		Debug.DrawLine (new Vector3 (p.x,		p.y,		p.z),		new Vector3 (p.x + 1f,	p.y,		p.z), 		color, 	duration);
		Debug.DrawLine (new Vector3 (p.x + 1f,	p.y,		p.z),		new Vector3 (p.x + 1f,	p.y,		p.z + 1f),	color, 	duration);
		Debug.DrawLine (new Vector3 (p.x + 1f,	p.y,		p.z + 1f),	new Vector3 (p.x,		p.y,		p.z + 1f),	color, 	duration);
		Debug.DrawLine (new Vector3 (p.x,		p.y,		p.z),		new Vector3 (p.x,		p.y,		p.z + 1f),	color, 	duration);
		
		// top 
		Debug.DrawLine (new Vector3 (p.x,		p.y + 1f,	p.z),		new Vector3 (p.x + 1f,	p.y + 1f,	p.z),		color, 	duration);
		Debug.DrawLine (new Vector3 (p.x + 1f,	p.y + 1f,	p.z),		new Vector3 (p.x + 1f,	p.y + 1f,	p.z + 1f),	color, 	duration);
		Debug.DrawLine (new Vector3 (p.x + 1f,	p.y + 1f,	p.z + 1f),	new Vector3 (p.x,		p.y + 1f,	p.z + 1f),	color, 	duration);
		Debug.DrawLine (new Vector3 (p.x,		p.y + 1f,	p.z),		new Vector3 (p.x,		p.y + 1f,	p.z + 1f),	color, 	duration);
		
		//Column
		Debug.DrawLine (new Vector3 (p.x,		p.y,		p.z),		new Vector3 (p.x,		p.y + 1f,	p.z),		color, 	duration);
		Debug.DrawLine (new Vector3 (p.x + 1f,	p.y,		p.z),		new Vector3 (p.x + 1f,	p.y + 1f,	p.z),		color, 	duration);
		Debug.DrawLine (new Vector3 (p.x + 1f, 	p.y,		p.z + 1f),	new Vector3 (p.x + 1f,	p.y + 1f,	p.z + 1f),	color, 	duration);
		Debug.DrawLine (new Vector3 (p.x,		p.y,		p.z + 1f),	new Vector3 (p.x,		p.y + 1f,	p.z + 1f),	color, 	duration);

//		Debug.Break ();
#endif
	}
}

