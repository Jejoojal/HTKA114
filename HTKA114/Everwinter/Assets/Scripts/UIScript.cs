using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIScript : MonoBehaviour {
    public Slider enkkaPalkki;
    public Slider staminaPalkki;
    public Slider batteryPalkki;
    public Text batteryJaljella;
    public Text pistooliUI;
    public Image crosshair;

    // Use this for initialization
    void Start () {
        crosshair.enabled = false;
	}


	
	// Update is called once per frame
	void Update () {
    }

    public void batteryArvo(int arvo)
    {
        batteryJaljella.text = arvo+"%";
            batteryPalkki.value = arvo;
    }
}
