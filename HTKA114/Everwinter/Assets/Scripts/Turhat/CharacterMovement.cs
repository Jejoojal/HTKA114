using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: korvaa mappingit
//TODO: hienostuneempi hyppy ja maantunnistus


public class CharacterMovement : MonoBehaviour
{
	float loppuNopeus = 2; 
	public float hyppy = 10, vaistoNopeus = 50, juoksuNopeus = 2f, perusNopeus = 2;
	public LayerMask maaLayerit;	//Layerit jotka lasketaan maaksi(eli voi hypätä sen päältä)
	public Transform iskuOlio;
	public Transform hahmoMalli;	//Hahmon malli, jossa on animaattori
	bool onkoMaassa = true;
	bool onVaistamassa = false;
	float saakoVaistaa = 1f;
	Rigidbody rbody;	//Hahmon rigidbody(eli perusfysiikat)
	HahmoScript hahmo;	//Hahmon statistiikat
	Animator anim; //Hahmon animaattorikontrolleri
	
	// Use this for initialization
	void Awake ()
	{
		Cursor.visible = false;
		rbody = GetComponent<Rigidbody>();		//Alustetaan rbody
		hahmo = GetComponent<HahmoScript>();	//Alustetaan hahmon scripti
		anim = hahmoMalli.GetComponent<Animator>();		//Alustetaan animaattori
	}
	
	// Update is called once per frame
	void Update ()
	{
		float painoVoima = rbody.velocity.y * Time.deltaTime;
		//Tarkistaa onko hahmon alapuolella maata
		onkoMaassa = Physics.Raycast(transform.position, -Vector3.up, 1f, maaLayerit);
		if (Input.GetKeyDown(KeyCode.Space) && onkoMaassa)
		{
			rbody.AddForce(Vector3.up*hyppy); //"Puskee" hahmon ylöspäin jos on maata alla ja on painanut välilyöntiä
		}
		
		//Säätää liikkumisnopeuden shiftin perusteella
		if (Input.GetKey(KeyCode.LeftShift)) loppuNopeus = perusNopeus + juoksuNopeus;
		else loppuNopeus = perusNopeus;
		
		//Lähettää animaattorille onko shift pohjassa
		anim.SetBool("sprint", Input.GetKey(KeyCode.LeftShift));
		
		//Perusliikkuminen
		float ver = Input.GetAxis("Vertical");
		anim.SetFloat("vertical",Mathf.Abs(ver));	//Lähettää animaattorille onko vertikaalit pohjassa
		float hor = Input.GetAxis("Horizontal");
		Quaternion rotation = Camera.main.transform.rotation;
		rotation.x = 0;
		rotation.z = 0;
		transform.rotation = rotation;
		
		//Laskee liikkeen
		Vector3 nopeus =
			transform.forward * ver * loppuNopeus + transform.right * hor * loppuNopeus + transform.up * painoVoima;
		nopeus *= Time.deltaTime;
		if (!onVaistamassa) rbody.velocity = nopeus;
		
		saakoVaistaa += 0.1f;
		
		//Väistöliike
		if (Input.GetKeyDown(KeyCode.LeftControl) && saakoVaistaa >= 2)
		{
			saakoVaistaa = 0;
			onVaistamassa = true;
			rbody.AddForce(transform.forward * vaistoNopeus * ver +
				transform.right * vaistoNopeus * hor + transform.up * 5f, ForceMode.Impulse);
		}
		
		if (rbody.velocity == Vector3.zero) onVaistamassa = false;
		
		//Hyökkäys
		if (Input.GetMouseButtonDown(0))
		{
			anim.SetTrigger("attack");
			iskuOlio.gameObject.SetActive(true);
			hahmo.energia -= 10;
		}
		else iskuOlio.gameObject.SetActive(false);
	}
}