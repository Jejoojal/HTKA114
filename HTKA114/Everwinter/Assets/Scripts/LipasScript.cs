using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LipasScript : MonoBehaviour
{
	[HideInInspector] public LuotiScript[] ammukset;
	
	// Use this for initialization
	void Start ()
	{
		ammukset = GetComponentsInChildren<LuotiScript>(true);
	}
	
	public LuotiScript AnnaAmmus()
	{
		for(int i = 0; i < ammukset.Length; i++)
		{
			if (!ammukset[i].gameObject.activeSelf) return ammukset[i];
		}
		return null;
	}
}
