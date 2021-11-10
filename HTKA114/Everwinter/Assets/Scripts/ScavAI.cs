using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScavAI : MonoBehaviour
{
	public Transform kohde;
	public Transform isku;
	public Transform hahmomalli;
	public SpawnausPaikka paikka;
	public float iskuRatio = 0.5f;
	bool onKuollut = false;
	float seuraavaIsku = 0;
	VihuScript vihu;	//Itse tehty komponentti VihuScript, sisältää vihujen perus statistiikat
	Rigidbody rbody;
	Animator anim;
	//BoxCollider iskuLaatikko;
	HahmoScript hahmo;
	TavaranTiputus tiputus;
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
		tiputus = GetComponent<TavaranTiputus>();
		AddEvent(1, 0.5f, "StartAttack", 0);
		AddEvent(1, 0.6f, "EndAttack", 0);
	}
	
	void Start()
	{
		//iskuLaatikko = isku.GetComponent<BoxCollider>();
		hahmo = kohde.GetComponent<HahmoScript>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		anim.SetFloat("stunnaus", vihu.stunnaus);
		anim.SetFloat("speed", agentti.velocity.magnitude);
		anim.SetInteger("hp", vihu.hp);
		anim.SetInteger("pelaajanHP", hahmo.hp);
		
		if (paikka.onHuomannut && paikka.etaisyys < paikka.jahtausEtaisyys)
		{
			//Collider[] colliders = Physics.OverlapBox(isku.transform.position, iskuLaatikko.size/2);
			float eta = Vector3.Distance(transform.position, kohde.position);
			if (!onKuollut) UpdateRotation();
			//lähtee kulkemaan navmeshia pitkin kohti kohdetta, jos ei ole stunnattuna
			if (vihu.stunnaus <= 0 && !onKuollut)
			{
				if (eta <= 2)
				{
					if (Time.time > seuraavaIsku)
					{
						seuraavaIsku = Time.time + iskuRatio;
						agentti.SetDestination(transform.position);
						agentti.isStopped = true;
						anim.SetTrigger("isku");
					}
					else
					{
						agentti.nextPosition = transform.position;
						agentti.SetDestination(transform.position);
					}
				}
				else
				{
					agentti.isStopped = false;
					agentti.SetDestination(kohde.position);
				}
			}
			else if (vihu.stunnaus > 0 && !onKuollut)
			{
				//Yrittää pysäyttää navmeshin fysiikat väliaikaisesti
				agentti.velocity = rbody.velocity;
				agentti.SetDestination(transform.position);
				agentti.isStopped = true;
				isku.gameObject.SetActive(false);
			}
			else if (onKuollut)
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
		if (vihu.hp <= 0 && !onKuollut)
		{
			onKuollut = true;
			tiputus.Tiputa();
			anim.SetTrigger("kuole");
			FindObjectOfType<AudioManager>().Soita("VihunKuolema");
		}
		
		if (vihu.hp > 0 && onKuollut)
		{
			onKuollut = false;
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
