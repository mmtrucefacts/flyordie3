using UnityEngine;
using System.Collections;

public class DestroyByContactB : MonoBehaviour
{
	public GameObject playerExplosion;
	private GameController gameController;

	void Start(){
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent<GameController> ();
		}
		if (gameController == null) {
			Debug.Log ("Can't find GameController");
		}
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.tag != "Player")
		{
			return;
		}

		Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
		gameController.GameOver ();
		Destroy(other.gameObject);
		Destroy(gameObject);
	}
}
