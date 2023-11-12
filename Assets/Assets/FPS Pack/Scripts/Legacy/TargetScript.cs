using UnityEngine;
using System.Collections;

public class TargetScript : MonoBehaviour
{
	float randomTime;
	bool routineStarted = false;

	public bool isHit = false;

	[Header("Customizable Options")]
	public float minTime;
	public float maxTime;

	[Header("Audio")]
	public AudioClip upSound;
	public AudioClip downSound;

	[Header("Animations")]
	public AnimationClip targetUp;
	public AnimationClip targetDown;

	public AudioSource audioSource;
	
	private void Update () 
	{
		randomTime = Random.Range (minTime, maxTime);

		if (isHit == true) 
		{
			if (routineStarted == false) 
			{
				gameObject.GetComponent<Animation>().clip = targetDown;
				gameObject.GetComponent<Animation>().Play();

				audioSource.GetComponent<AudioSource>().clip = downSound;
				audioSource.Play();

				StartCoroutine(DelayTimer());
				routineStarted = true;
			} 
		}
	}

	private IEnumerator DelayTimer ()
	{
		yield return new WaitForSeconds(randomTime);

		gameObject.GetComponent<Animation>().clip = targetUp;
		gameObject.GetComponent<Animation>().Play();

		audioSource.GetComponent<AudioSource>().clip = upSound;
		audioSource.Play();

		isHit = false;
		routineStarted = false;
	}
}