using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KuusiScript : MonoBehaviour {

    public Animator anim;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	}

    private void OnTriggerEnter(Collider other)
    {
        anim.Play("On_Touch");
		FindObjectOfType<AudioManager>().Soita("Kuusi");
    }
}
