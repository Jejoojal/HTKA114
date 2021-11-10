using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: iskun vaikutus vihulle

public class AttackOnCollision : MonoBehaviour
{
	public float vahvuus = 400f;
	public int minDmg = 17;
	public int maxDmg = 20;
	public float stunnausAika;
	
	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	}
	
	//Kun 'trigger' osuu 'collideriin'
	void OnTriggerEnter(Collider other)
	{
		VihuScript vihu = other.gameObject.GetComponent<VihuScript>();
		if (vihu)	//Onko vihu?
		{
			vihu.hp -= Random.Range(minDmg, maxDmg); 	//Pienentää vihun hp:ta
			vihu.stunnaus += stunnausAika;
			Rigidbody rbody = other.attachedRigidbody;
			rbody.AddForce(other.transform.forward * -vahvuus + transform.up * vahvuus, ForceMode.Impulse);
		}
		else
		{
			HahmoScript hahmo = other.gameObject.GetComponent<HahmoScript>();
			if (hahmo)	//Onko hahmo?
			{
				int dmg = Random.Range(minDmg, maxDmg);	//satunnainen vahinko
				hahmo.laskeEnkka(dmg); 	//Pienentää hahmon hp:ta
				Animator anim = other.GetComponentInChildren<Animator>();
				anim.SetTrigger("vahinko");
				FindObjectOfType<AudioManager>().Soita("Osuma");
			}
		}
		Rikottava riko = other.gameObject.GetComponent<Rikottava>();
		if (riko)	//onko rikottava?
		{
			Debug.Log("Osuuuui");
			riko.hp -= Random.Range(minDmg, maxDmg); 	//Pienentää esineen hp:ta
		}
    }
}

