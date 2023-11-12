using UnityEngine;
using System.Collections;

public class ExplosiveBarrelScript : MonoBehaviour
{
    private float randomTime;
    private bool routineStarted = false;

	public bool explode = false;

	[Header("Prefabs")]
	public Transform explosionPrefab;
	public Transform destroyedBarrelPrefab;

	[Header("Customizable Options")]
	public float minTime = 0.05f;
	public float maxTime = 0.25f;

	[Header("Explosion Options")]
	public float explosionRadius = 12.5f;
	public float explosionForce = 4000.0f;

	private void Update () 
	{
		randomTime = Random.Range (minTime, maxTime);

		if (explode == true) 
		{
			if (routineStarted == false) 
			{
				StartCoroutine(Explode());

				routineStarted = true;
            } 
		}
	}
	
	private IEnumerator Explode () 
	{
		yield return new WaitForSeconds(randomTime);
        
        Instantiate (destroyedBarrelPrefab, transform.position, transform.rotation);

		Vector3 explosionPos = transform.position;
		Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);

		foreach (Collider hit in colliders)
		{
			Rigidbody rb = hit.GetComponent<Rigidbody> ();
			
			if (rb != null)
				rb.AddExplosionForce (explosionForce * 50, explosionPos, explosionRadius);

			if (hit.transform.tag == "ExplosiveBarrel") 
			{
				hit.transform.gameObject.GetComponent<ExplosiveBarrelScript>().explode = true;
			}
				
			if (hit.transform.tag == "Target") 
			{
				hit.transform.gameObject.GetComponent<TargetScript>().isHit = true;
			}

			if (hit.GetComponent<Collider>().tag == "GasTank") 
			{
				hit.gameObject.GetComponent<GasTankScript> ().isHit = true;
				hit.gameObject.GetComponent<GasTankScript> ().explosionTimer = 0.05f;
			}
		}

		RaycastHit checkGround;

		if (Physics.Raycast(transform.position, Vector3.down, out checkGround, 50))
		{
			Instantiate (explosionPrefab, checkGround.point, Quaternion.FromToRotation (Vector3.forward, checkGround.normal)); 
		}

		explode = false;
        routineStarted = false;
    }
}