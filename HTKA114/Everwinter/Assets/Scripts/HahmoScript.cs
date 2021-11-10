using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HahmoScript : MonoBehaviour
{
    public int hp = 100;
    public int maxHp = 100;
	public float stamina = 100;
	public float maxStamina = 100;
    public int energia = 100;
    public int maxEnergia = 100;
    public int maxAmmo = 60;
	public float regNopeus = 0.5f;	//Staminan palautumisnopeus
    public Text pistooliUI;
	float staminaReg = 0;
	public int ammoPistooli = 0;
	public CheckPointScript checkpoint;
    UIScript ui;

    // Use this for initialization
    void Start()
    {
        ui = GameObject.Find("UI").GetComponent<UIScript>();
        laskePistooli(0);
    }

    public void laskeEnkka(int damage)
    {
        hp = hp - damage;
        ui.enkkaPalkki.value = hp;
    }

    public void laskePistooli(int vahennys)
    {
        ammoPistooli = ammoPistooli - vahennys;
        if (ammoPistooli > 60) ammoPistooli = 60;
        pistooliUI.text = ammoPistooli+"/"+maxAmmo;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
		if (Time.time > staminaReg)
		{
			staminaReg = Time.time + regNopeus * (2 - Time.timeScale);
			if (stamina < maxStamina && !Input.GetKey(KeyCode.LeftShift)) stamina++;
			if (energia > 0) energia = energia - 1 - BoolToInt(Input.GetKey(KeyCode.F),2);
			else if (hp > 0)
			{
				laskeEnkka(1);
				FindObjectOfType<AudioManager>().Soita("Hengastyminen");
			}
		}
        ui.staminaPalkki.value = (int)stamina;
        ui.batteryArvo(energia);

        if (stamina <= 0) FindObjectOfType<AudioManager>().Soita("Hengastyminen");
    }
	
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 14)
		{
			switch(other.gameObject.tag)
			{
				case "ammo":
					Ammo(other);
					break;
				case "paristo":
					Paristo(other);
					break;
				case "parannus":
					Parannus(other);
					break;
				default:
					break;
			}
		}
	}
	
	void Ammo(Collider other)
	{
		if (ammoPistooli >= 60) return;
		ammoPistooli = ammoPistooli + 30;
		laskePistooli(0);
		FindObjectOfType<AudioManager>().Soita("Poiminta");
		other.gameObject.SetActive(false);
	}
	
	void Paristo(Collider other)
	{
		if (energia >= maxEnergia) return;
		energia += 50;
		if (energia > maxEnergia) energia = maxEnergia;
		FindObjectOfType<AudioManager>().Soita("Poiminta");
		other.gameObject.SetActive(false);
	}
	
	void Parannus(Collider other)
	{
		if (hp >= maxHp) return;
		hp += 25;
		if (hp > maxHp) hp = maxHp;
		laskeEnkka(0);
		FindObjectOfType<AudioManager>().Soita("Poiminta");
		other.gameObject.SetActive(false);
	}
	
	int BoolToInt(bool b, int multiplier = 1)
	{
		if (b) return multiplier;
		return 0;
	}
}
