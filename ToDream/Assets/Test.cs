using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
	BoxCollider[] boxes;
    // Start is called before the first frame update
    void Start()
	{
		boxes = GetComponentsInChildren<BoxCollider>();
		foreach(var b in boxes)
		{
			GameObject.DestroyImmediate(b);
		}
		
    }
}
