using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KerattavaScript : MonoBehaviour {

	// toteutetaan vähä paremmin tai jotain...
	public GameObject kerattava;
    public float spinningSpeed = 150f;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Kerattava")
		{
			other.gameObject.SetActive(false);
			FindObjectOfType<AudioManager>().Soita("Poiminta");
		}
	}

    void Update()
    {
        transform.Rotate(Vector3.up, spinningSpeed * Time.deltaTime, Space.World);
    }
}
