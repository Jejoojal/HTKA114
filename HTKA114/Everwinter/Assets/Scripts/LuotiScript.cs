using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuotiScript : MonoBehaviour
{
	public LayerMask mihinVoiOsua;
	public float elinika = 5f;
	public float nopeus = 2f;
	public int maxDmg, minDmg;
	float tuhoaAika;
	BoxCollider laatikko;
	
	void Awake()
	{
		tuhoaAika = Time.time + elinika;
		laatikko = GetComponent<BoxCollider>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		UpdateOsuma();
		transform.Translate(transform.forward * nopeus * Time.deltaTime, Space.World);
		if (Time.time > tuhoaAika)
		{
			Debug.Log("Aikaloppui");
			Tuhoa();
		}
	}
	
	//Laukaisee luodin
	public void Ammu(Vector3 suunta)
	{
		tuhoaAika = Time.time + elinika;
		transform.LookAt(suunta);
	}
	
	//luodin tuhoaminen
	void Tuhoa()
	{
		if (transform.parent) transform.position = transform.parent.position;
		gameObject.SetActive(false);
	}
	
	//Kun osuu johonkin
	void UpdateOsuma()
	{
		Collider[] colliders = Physics.OverlapBox(transform.position, laatikko.size, transform.rotation, mihinVoiOsua);
		if (colliders.Length > 0)
		{
			VihuScript vihu = colliders[0].GetComponent<VihuScript>();
			if (vihu)
			{
				OsuVihuun(vihu);
			}
			else
			{
				Rikottava riko = colliders[0].GetComponent<Rikottava>();
				if (riko) riko.hp -= Random.Range(minDmg, maxDmg);
			}
			Debug.Log("Osuma: " + colliders[0].gameObject.name);
			Tuhoa();
		}
	}
	
	void OsuVihuun(VihuScript vihu)
	{
		vihu.hp -= Random.Range(minDmg, maxDmg);
		vihu.stunnaus = 1;
		FindObjectOfType<AudioManager>().Soita("VihuunOsuu");
	}
}
