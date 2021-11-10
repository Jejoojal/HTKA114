using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavaranTiputus : MonoBehaviour
{
	public Transform tavaraAllas;
	public int dropChance = 100;
	Transform[] tavarat;
	
	// Use this for initialization
	void Start ()
	{
		if (tavaraAllas)
		{
			tavarat = tavaraAllas.GetComponentsInChildren<Transform>(true);
			Debug.Log(tavarat[0]);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
	
	public void Tiputa()
	{
		Transform tavara = AnnaTavara();
		if (!tavara) return;
		int satunnainen = Random.Range(0, 100);
		if (!tavara.gameObject.activeSelf && satunnainen <= dropChance)
		{
			tavara.transform.position = transform.position;
			tavara.gameObject.SetActive(true);
		}
	}
	
	Transform AnnaTavara()
	{
		for (int i = 0; i < tavarat.Length; i++)
		{
			if (!tavarat[i].gameObject.activeSelf) return tavarat[i];
		}
		Debug.Log("Ei ole tavaroita: " + tavarat.Length);
		return null;
	}
}
