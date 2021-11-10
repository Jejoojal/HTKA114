using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnausPaikka : MonoBehaviour
{
	public Transform kohde;				//Pelaaja
	public float huomausEtaisyys = 10f;	//kuinka läheltä vihut huomaa
	public float jahtausEtaisyys = 90f;	//kuinka kauas vihut jahtaa, kun on huomannut
	public bool onHuomannut = false;
	public float respawnAika = 400f;
    public float reaktioAika = 1f;
	
	[HideInInspector] public float etaisyys;
	[HideInInspector] public bool respawn;
	
	ScavAI[] scavit;
	float seuraavaRespawn = 0;
	
	// Use this for initialization
	void Awake()
	{
		seuraavaRespawn = Time.time + respawnAika;
	}
	
	void Start()
	{
		scavit = GetComponentsInChildren<ScavAI>(true);
		foreach(ScavAI scav in scavit)
		{
			scav.paikka = this;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		etaisyys = Vector3.Distance(transform.position, kohde.position);
		if (etaisyys <= huomausEtaisyys)
		{
			onHuomannut = true;
			respawn = false;
		}
		if (etaisyys >= jahtausEtaisyys)
		{
			onHuomannut = false;
			Respawn();
		}
	}
	
	//Ei vielä valmis...
	void Respawn()
	{
		if (Time.time > seuraavaRespawn)
		{
			Resettaa();
		}
	}
	
	public void Resettaa()
	{
		onHuomannut = false;
		foreach(ScavAI scav in scavit)
		{
			scav.gameObject.SetActive(true);
			VihuScript scavStat = scav.gameObject.GetComponent<VihuScript>();
			scavStat.hp = scavStat.maxHP;
			scav.transform.position = scavStat.aloitusPaikka;
		}
	}
}
