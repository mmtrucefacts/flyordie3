using UnityEngine;
using System.Collections;

public class GunRotator : MonoBehaviour
{
	public float maxRotation;
	public float delay;
	public float tumble;
	private float rotation;
	private float increment;

	void Start ()
	{
		rotation = 0;
		increment = ((int)Random.Range(0,2) == 0 ? 1 : -1) * tumble;
		StartCoroutine (RotateGun());
	}

	void FixedUpdate ()
	{
		GetComponent<Transform>().rotation = Quaternion.Euler (0.0f, 180+rotation, 0.0f);
	}

	IEnumerator RotateGun () {
		while (true) {
			yield return new WaitForSeconds (delay);
			if (rotation <= -maxRotation || rotation >= maxRotation) {
				increment *= -1;
			};
			rotation += increment;
		}
	}
}