using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Aani
{
	public string nimi;
	public AudioClip clip;

	[Range(0f, 1)]
	public float volume;
	[Range(.1f, 3)]
	public float pitch;

	public bool loop;

	[HideInInspector]
	public AudioSource source;
}
