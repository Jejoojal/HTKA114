using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour {

	public Aani[] aanet;
	internal static bool isPlaying;
	CharacterController controller;

	void Awake () {
		foreach (Aani s in aanet)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
			s.source.loop = s.loop;
		}
	}

	 void Start()
	{
		Soita("Taustamusiikki");
		Soita("Ambienssi");
	}

	public void Soita(string nimi)
	{
		Aani s = Array.Find(aanet, sound => sound.nimi == nimi);
		if (s == null)
		{
			return;
		}
		s.source.Play();
	} 
}

