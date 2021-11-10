using UnityEngine;
using System.Collections;
 
[RequireComponent (typeof (CharacterController))]
public class HahmonLiike : MonoBehaviour
{
    public float nopeus = 6;
	public float vaistoNopeus = 20;
	public float hyppyKerroin = 2f;
	public Transform hahmoMalli;
	bool onKuollut = false;
	int valittuAse = 0;
	bool onkoVaistamassa = false;
	int vaistoAika = 0;
    Vector3 liike;
    CharacterController controller;
	Animator anim;
	HahmoScript hahmo;
	Transform pistooli;
	Transform keppi;
	Transform iskuLaatikko;
	public AudioSource Audio;
    UIScript ui;


    float seuraavaIsku = 0;
	public float iskuNopeus = 0.5f;
	ShooterGameCamera cam;
	public Vector3[] kameraKulmat;
    


	// Use this for initialization
	void Awake()
	{
		Cursor.visible = false;
        controller = GetComponent<CharacterController> ();
		hahmo = GetComponent<HahmoScript>();
		anim = hahmoMalli.GetComponent<Animator>();
        ui = GameObject.Find("UI").GetComponent<UIScript>();
        AddEvent(3, 0.3f, "StartAttack", 0);
		AddEvent(3, 0.4f, "EndAttack", 0);
		AddEvent(18, 0.0f, "Ammu", 0);
		Audio = GetComponent<AudioSource>();
    }
	
	void Start()
	{
		pistooli = transform.Find("Suit_Animated/Pistooli");
		keppi = transform.Find("Suit_Animated/Keppi");
		iskuLaatikko = transform.Find("isku");
		cam = Camera.main.gameObject.GetComponent<ShooterGameCamera>();
	}
   
    // Update is called once per frame
    void Update ()
	{
        liike = controller.velocity;
        if(hahmo.hp > 0 && !onkoVaistamassa)UpdateSmoothControls();
        UpdateGravity();
		if (!onKuollut) UpdateKuolema();
		if (!onKuollut) UpdateAttack();
		if (!onKuollut) UpdateVaisto();
        if (!onKuollut) controller.Move (liike * Time.deltaTime);
		if (!onKuollut) UpdateAika();
		UpdateStamina();
		anim.SetInteger("hp",hahmo.hp);
		
		if (Input.GetKeyDown(KeyCode.R))
		{
			hahmo.checkpoint.JatkaTasta();
			onKuollut = false;
		}
		
		if (controller.isGrounded == true && controller.velocity.magnitude > 2f && Audio.isPlaying == false)
		{
			Audio.volume = Random.Range(0.6f, 1);
			Audio.pitch = Random.Range(0.7f, 1.1f);
			Audio.Play();
		}
	}
 
    void UpdateSmoothControls()
	{
		float ver = Input.GetAxis("Vertical");
		float hor = Input.GetAxis("Horizontal");
		
		if (Mathf.Abs(ver) + Mathf.Abs(hor) > 0) UpdateRotation(); 
		
        Vector3 input = new Vector3(hor, 0, ver / (1 - ver + Mathf.Abs(ver)) );
        input = transform.TransformDirection(input);
		anim.SetFloat("vertical",ver);
		anim.SetFloat("horizontal",hor);
		if (hahmo.stamina > 0) anim.SetBool("sprint", Input.GetKey(KeyCode.LeftShift));
 
        float loppuNopeus = nopeus;
        if (Input.GetKey (KeyCode.LeftShift) && hahmo.stamina > 0) loppuNopeus *= 2;
        input *= loppuNopeus;
 
        if (!controller.isGrounded)
		{
            input *= Time.deltaTime * 2;
            liike += input;
        }
		else
		{
            input.y = liike.y;
            liike = input;
        }
 
        float y = liike.y;
        liike.y = 0;
        if (liike.magnitude > loppuNopeus) liike = liike.normalized * loppuNopeus;
        liike.y = y;
 
    }
 
    void UpdateGravity()
	{
		anim.SetBool("isGrounded", controller.isGrounded);
        liike += Physics.gravity * 2 * Time.deltaTime;
        if (controller.isGrounded && Input.GetButtonDown ("Jump") && hahmo.hp > 0)
		{
            liike -= Physics.gravity * hyppyKerroin;
			anim.SetTrigger("hyppy");
		}
    }
	
	//Hyökkää käyttäen valittua asetta
	void UpdateAttack()
	{
		if (Input.GetKey(KeyCode.Alpha1))
		{
            ui.crosshair.enabled = false;
			valittuAse = 0;	//Keppi
			keppi.gameObject.SetActive(true);
			pistooli.gameObject.SetActive(false);
		}
		if (Input.GetKey(KeyCode.Alpha2))
		{
            ui.crosshair.enabled = true;
            valittuAse = 1;	//Normipistooli
			keppi.gameObject.SetActive(false);
			pistooli.gameObject.SetActive(true);		}
		cam.pivotOffset = kameraKulmat[valittuAse];	//vaihtaa kuvakulmaa aseesta riippuen
		if (Input.GetMouseButton(0))	//Itse hyökkäys
		{
			if (Time.time > seuraavaIsku)
			{
				seuraavaIsku = iskuNopeus + Time.time;
				switch(valittuAse)
				{
					case 0:
						UpdateKeppi();
						break;
					case 1:
						UpdateAmmu();
						break;
					default:
						UpdateKeppi();
						break;
				}
			}
		}
		// TODO, alt-firet yms, toistaiseksi vain zoomataan 
        if (Input.GetMouseButton(1))
        {
            Camera.main.fieldOfView=30; // Unity yritti optimoida aseet pois tällä asetuksella joten laitoin editorissa "Update when off screen"
        }
        if (Input.GetMouseButtonUp(1)) Camera.main.fieldOfView = 60; // default FoV:n voisi laittaa muuttujaksi?
	}
	
	//Lyö kepillä >:(
	void UpdateKeppi()
	{
        UpdateRotation();
		anim.SetTrigger("attack");
		FindObjectOfType<AudioManager>().Soita("Heilautus");
	}
	
	//Ampuminen
	void UpdateAmmu()
	{
		UpdateRotation();
		if (hahmo.ammoPistooli <= 0)
		{
			FindObjectOfType<AudioManager>().Soita("TyhjaLipas"); //panokset loppu
			return;
		}
		anim.SetTrigger("ammu");
	}
	
	//Väistää
	void UpdateVaisto()
	{
		Vector3 suunta = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");
		anim.SetBool("vaistamassa",onkoVaistamassa);
		bool verhor = (Mathf.Abs(Input.GetAxis("Vertical")) + Mathf.Abs(Input.GetAxis("Horizontal"))) > 0;
		
		if (Input.GetKeyDown(KeyCode.LeftControl) && controller.isGrounded && vaistoAika <= 0 && hahmo.stamina >= 10 && verhor)
		{
			anim.SetTrigger("vaisto");
			onkoVaistamassa = true;
			vaistoAika = 10;
			hahmo.stamina -= 10;
		}
		if (vaistoAika > 0)
		{
			vaistoAika--;
			liike = suunta * vaistoNopeus;
			return;
		}
		onkoVaistamassa = false;
	}
	
	//Päivittää hahmon rotaation
	void UpdateRotation()
	{
		Quaternion rotaatio = Camera.main.transform.rotation;
		rotaatio.x = 0;
		rotaatio.z = 0;
		transform.rotation = rotaatio;
	}
	
	//Hahmon kuolema
	void UpdateKuolema()
	{
		if (hahmo.hp <= 0)
		{
			onKuollut = true;
			iskuLaatikko.gameObject.SetActive(false);
			anim.SetTrigger("kuole");
			FindObjectOfType<AudioManager>().Soita("PelaajanKuolema");
		}
	}
	
	void UpdateAika()
	{
		if (!PauseScript.GameIsPaused)
		{
			if (Input.GetKey(KeyCode.F) && hahmo.energia > 0)
			{
				Time.timeScale = 0.45f;
			}
			else Time.timeScale = 1;
		}
	}
	
	void UpdateStamina()
	{
		if (Input.GetKey(KeyCode.LeftShift) && hahmo.stamina > 0) hahmo.stamina -= 0.1f;
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