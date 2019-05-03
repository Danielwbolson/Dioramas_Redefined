using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdCalls : MonoBehaviour
{
	
	public AudioSource[] birb;
	private AudioClip clip;
	public float callRate = 3.0f;
	private float nextCall = 0.0f;
	
	//public AudioClip Cerulean;
	//public AudioClip Chestnut;
	//public AudioClip Heron;
	//public AudioClip Mourning;
	//public AudioClip Ovenbird;
	
    // Start is called before the first frame update
    void Start()
    {
		birb = GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
		if (Time.time > nextCall) {
			int index = Random.Range(0, 4);
			birb[index].Play();
			nextCall = Time.time + callRate;
		}
    }
}
