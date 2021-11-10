using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rikottava : MonoBehaviour
{
    public int hp = 1;
    TavaranTiputus tiputus;
    public ParticleSystem ps;
	// Use this for initialization
	void Awake()
	{
		tiputus = GetComponent<TavaranTiputus>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (hp <= 0)
		{
            if (tiputus)
			{
				tiputus.Tiputa();
			}
            ps.Play();
            gameObject.SetActive(false);
        }
	}
}
