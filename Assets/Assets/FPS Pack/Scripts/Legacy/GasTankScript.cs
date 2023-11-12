using UnityEngine;
using System.Collections;

public class GasTankScript : MonoBehaviour
{
	float randomRotationValue;
	float randomValue;

	bool routineStarted = false;

	public bool isHit = false;

	[Header("Prefabs")]
	public Transform explosionPrefab;
	public Transform destroyedGasTankPrefab;

	[Header("Customizable Options")]
	public float explosionTimer;
	public float rotationSpeed;
	public float maxRotationSpeed;
	public float moveSpeed;
	public float audioPitchIncrease = 0.5f;

	[Header("Explosion Options")]
	public float explosionRadius = 12.5f;
	public float explosionForce = 4000.0f;

	[Header("Light")]
	public Light lightObject;

	[Header("Particle Systems")]
	public ParticleSystem flameParticles;
	public ParticleSystem smokeParticles;

	[Header("Audio")]
	public AudioSource flameSound;
	public AudioSource impactSound;
	//Used to check if the audio has played
	bool audioHasPlayed = false;
	
	private void Start ()
	{
		lightObject.intensity = 0;
		randomValue = Random.Range (-50, 50);
	}

	private void Update ()
	{
		if (isHit == true) 
		{
			randomRotationValue += 1.0f * Time.deltaTime;

			if (randomRotationValue > maxRotationSpeed) 
			{
				randomRotationValue = maxRotationSpeed;
			}

			gameObject.GetComponent<Rigidbody>().AddRelativeForce(Vector3.down * moveSpeed * 50 *Time.deltaTime);

			transform.Rotate (randomRotationValue,0,randomValue * rotationSpeed * Time.deltaTime); 

			flameParticles.Play ();
			smokeParticles.Play ();

			flameSound.pitch += audioPitchIncrease * Time.deltaTime;

			if (!audioHasPlayed) 
			{
				flameSound.Play ();
				audioHasPlayed = true;
			}

			if (routineStarted == false) 
			{
				StartCoroutine(Explode());
				routineStarted = true;
				lightObject.intensity = 3;
			}
		}
	}
	private void OnCollisionEnter (Collision collision)
	{
		impactSound.Play ();
	}

	private IEnumerator Explode ()
	{
		yield return new WaitForSeconds(explosionTimer);
		
		Instantiate (destroyedGasTankPrefab, transform.position, 
		             transform.rotation); 
		
		Vector3 explosionPos = transform.position;
		Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);

		foreach (Collider hit in colliders) 
		{
			Rigidbody rb = hit.GetComponent<Rigidbody> ();
			
			if (rb != null)
				rb.AddExplosionForce (explosionForce * 50, explosionPos, explosionRadius);
			
			if (hit.transform.tag == "GasTank") {
				
				hit.transform.gameObject.GetComponent<GasTankScript>().isHit = true;
			}

			if (hit.transform.tag == "ExplosiveBarrel") 
			{
				hit.transform.gameObject.GetComponent<ExplosiveBarrelScript>().explode = true;
			}
		}
		
		Instantiate (explosionPrefab, transform.position, transform.rotation); 

		Destroy (gameObject);
	}
}