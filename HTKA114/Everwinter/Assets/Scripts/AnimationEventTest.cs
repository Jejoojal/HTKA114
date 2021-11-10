using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventTest : MonoBehaviour
{
	/*
	Tässä scriptissä erinäisiä animationevent aliohjelmia,
	jotka liitetään muualla scriptissä animaatio-clippiin
	*/
	
	public Transform isku;
	public LipasScript lipasPistooli;
	public LayerMask mihinVoiOsua;
	HahmoScript hahmo;
	
	// Use this for initialization
	void Start ()
	{
		hahmo = transform.parent.GetComponent<HahmoScript>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
	
	public void StartAttack(int i)
    {
		isku.gameObject.SetActive(true);
    }
	
	public void EndAttack(int i)
	{
		isku.gameObject.SetActive(false);
	}
	
	public void Ammu(int i)
	{
		RaycastHit osuma;
		Transform cam = Camera.main.transform;
		Vector3 kohta = cam.position;
		kohta.y += 0.5f;
		
		if (Physics.Raycast(kohta, cam.forward, out osuma, Mathf.Infinity, mihinVoiOsua))
		{
			FindObjectOfType<AudioManager>().Soita("Ammus");
			Vector3 suunta = osuma.point;
			LuotiScript ammus = lipasPistooli.AnnaAmmus();
			if (ammus)
			{
				ammus.transform.position = transform.parent.position + Vector3.up * 1.25f;
				ammus.gameObject.SetActive(true);
				ammus.Ammu(suunta);
                hahmo.laskePistooli(1);
			}
		}
	}
}
