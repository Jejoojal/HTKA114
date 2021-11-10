using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActiveState
{
	public Transform olio;
	[HideInInspector] public bool tila;
	[HideInInspector] public int hp = 1;
}

public class CheckPointScript : MonoBehaviour
{
	public HahmoScript hahmo;
	public SpawnausPaikka[] paikat;
	public Transform[] altaat;
	public ActiveState[] tilat;			//Olioiden tilat tallentaa
	[HideInInspector] public int maxHp, maxEnergia, ammoPistooli;
	[HideInInspector] public float maxStamina;
	
	// Use this for initialization
	void Awake()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject == hahmo.gameObject)
		{
			if (hahmo.checkpoint)
			{
				tilat = hahmo.checkpoint.tilat;
				altaat = hahmo.checkpoint.altaat;
			}
			hahmo.checkpoint = this;
			maxHp = hahmo.maxHp;
			maxStamina = hahmo.maxStamina;
			maxEnergia = hahmo.maxEnergia;
			ammoPistooli = hahmo.ammoPistooli;
			hahmo.energia = hahmo.maxEnergia;
			UpdateTilat();
		}
	}
	
	public void UpdateTilat()
	{
		foreach(ActiveState tila in tilat)
		{
			tila.tila = tila.olio.gameObject.activeSelf;
			Rikottava riko = tila.olio.gameObject.GetComponent<Rikottava>();
			if (riko)
			{
				tila.hp = riko.hp;
			}
		}
	}
	
	public void DeaktivoiAltaanOliot()
	{
		for(int i = 0; i < altaat.Length; i++)
		{
			Transform[] oliot = altaat[i].GetComponentsInChildren<Transform>();
			if (oliot.Length > 1)
			{
				for (int j = 1; j < oliot.Length; j++)
				{
					oliot[j].gameObject.SetActive(false);
				}
			}
		}
	}
	
	public void JatkaTasta()
	{
		foreach(SpawnausPaikka paikka in paikat)
		{
			paikka.Resettaa();
		}
		foreach(ActiveState tila in tilat)
		{
			Rikottava riko = tila.olio.gameObject.GetComponent<Rikottava>();
			if (riko)
			{
				riko.hp = tila.hp;
			}
			tila.olio.gameObject.SetActive(tila.tila);
		}
		DeaktivoiAltaanOliot();
		hahmo.maxHp = maxHp;
		hahmo.maxStamina = maxStamina;
		hahmo.maxEnergia = maxEnergia;
		hahmo.ammoPistooli = ammoPistooli;
		hahmo.transform.position = transform.position;
		hahmo.hp = hahmo.maxHp;
	}
}
