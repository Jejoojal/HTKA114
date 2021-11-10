using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: tutoriaalin pohjalta tehty, muuta nimi tai luo uusi scripti

public class SampleAgentScript : MonoBehaviour
{
	public Transform kohde;
	public Transform isku;
	public Transform hahmomalli;
	public SpawnausPaikka paikka;
	bool onkoKuollut = false;
	VihuScript vihu;	//Itse tehty komponentti VihuScript, sisältää vihujen perus statistiikat
	Rigidbody rbody;
	Animator anim;
	BoxCollider iskuLaatikko;
	UnityEngine.AI.NavMeshAgent agentti;	//NavMeshAgent olio, pystyy liikkumaan navmeshillä(Unityn pathfinding)
	
	
	// Use this for initialization
	void Awake()
	{
		//Alustetaan komponentit
		agentti = GetComponent<UnityEngine.AI.NavMeshAgent>();
		vihu = GetComponent<VihuScript>();
		rbody = GetComponent<Rigidbody>();
		isku = transform.Find("vihu_isku");
		anim = hahmomalli.GetComponent<Animator>();
		AddEvent(1, 0.5f, "StartAttack", 0);
		AddEvent(1, 0.6f, "EndAttack", 0);
	}
	
	void Start()
	{
		iskuLaatikko = isku.GetComponent<BoxCollider>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		anim.SetFloat("stunnaus", vihu.stunnaus);
		anim.SetFloat("speed", agentti.velocity.magnitude);
		anim.SetInteger("hp", vihu.hp);
		
		if (paikka.onHuomannut && paikka.reaktioAika > 0)
		{
			agentti.isStopped = true;
			anim.SetTrigger("alert");
			UpdateRotation();
		}
		else if (paikka.onHuomannut && paikka.reaktioAika <= 0 && paikka.etaisyys < paikka.jahtausEtaisyys)
		{
			Collider[] colliders = Physics.OverlapBox(isku.transform.position, iskuLaatikko.size/2);
			if (!onkoKuollut) UpdateRotation();
			//lähtee kulkemaan navmeshia pitkin kohti kohdetta, jos ei ole stunnattuna
			if (vihu.stunnaus <= 0 && !onkoKuollut)
			{
				if (colliders[0])
				{
					if (colliders[0].gameObject.layer == 10)
					{
						agentti.SetDestination(transform.position);
						agentti.isStopped = true;
						anim.SetTrigger("isku");
					}
					else
					{
						agentti.isStopped = false;
						agentti.SetDestination(kohde.position);
					}
				}
				else
				{
					agentti.isStopped = false;
					agentti.SetDestination(kohde.position);
				}
			}
			else if (vihu.stunnaus > 0 && !onkoKuollut)
			{
				//Yrittää pysäyttää navmeshin fysiikat väliaikaisesti
				agentti.velocity = rbody.velocity;
				agentti.nextPosition = transform.position;
				isku.gameObject.SetActive(false);
			}
			else if (onkoKuollut)
			{
				agentti.isStopped = true;
				isku.gameObject.SetActive(false);
			}
		}
		
		//Jos pelaaja juoksee jahtaus etäisyyden ulkopuolelle, vihu lakkaa jahtaamasta
		if (!paikka.onHuomannut)
		{
			agentti.SetDestination(vihu.aloitusPaikka);
			if (vihu.stunnaus > 0)
			{
				paikka.onHuomannut = true;
			}
		}
		
		//Kuolema animaatio
		if (vihu.hp <= 0 && !onkoKuollut)
		{
			onkoKuollut = true;
			anim.SetTrigger("kuole");
		}
	}
	
	void UpdateRotation()
	{
        Vector3 targetDir = kohde.position - transform.position;
        float step = 10f * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
        Debug.DrawRay(transform.position, newDir, Color.red);
        transform.rotation = Quaternion.LookRotation(newDir);
	}
	
	//Lisää animaatioon tapahtumia
	void AddEvent(int Clip, float time, string functionName, float floatParameter)
	{
		AnimationEvent animationEvent = new AnimationEvent();
		animationEvent.functionName = functionName;
		animationEvent.floatParameter = floatParameter;
		animationEvent.time = time;
		AnimationClip clip = anim.runtimeAnimatorController.animationClips[Clip];
		clip.AddEvent(animationEvent);
	}
}
