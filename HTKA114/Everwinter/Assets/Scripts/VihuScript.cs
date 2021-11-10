using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VihuScript : MonoBehaviour
{
	public int hp = 1;	//Vihun HP:t
	public int maxHP = 1;	//Vihun maksimi HP:t
	public float stunnaus = 0;
	public Vector3 aloitusPaikka;
	public float palautumisAika = 0.01f;
	
	
	// Use this for initialization
	void Awake()
	{
		aloitusPaikka = transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (stunnaus > 0) stunnaus -= palautumisAika;
		if (hp <= 0)
		{
			hp = hp - 1;
			if (hp <= -500)
			{
				transform.position = aloitusPaikka;
				hp = maxHP;
				gameObject.SetActive(false);
			}
		}
	}
}
