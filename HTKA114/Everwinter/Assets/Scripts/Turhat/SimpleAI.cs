using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAI : MonoBehaviour
{
	public Transform player;
	public float detectRadius;
	public float speed;
	public int HP;
	
	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		float distance = Vector3.Distance(transform.position, player.position);
		if (distance <= detectRadius)
		{
			float step = speed * Time.deltaTime;
			if (distance > detectRadius/10)
				transform.position = Vector3.MoveTowards(transform.position, player.position, step);
		}
		
		if (HP <= 0)
		{
			gameObject.SetActive(false);
		}
	}
}
