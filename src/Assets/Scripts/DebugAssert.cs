using UnityEngine;
using System;

public class Dbg
{ 
	public static void Assert(bool condition, string description) 
	{ 
#if DEBUG 
		if (!condition) {
			Debug.LogError(description);
			throw new Exception(); 
		}
#endif 
	} 
}

