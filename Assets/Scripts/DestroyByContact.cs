﻿using UnityEngine;
using System.Collections;

/**
 * Fly Or Die 3 by Philippe Bousquet <darken33@free.fr>
 * DestroyByContact - Destroy the objects when they enter in collision (In Game)
 * 
 * GNU General Public License
 */
public class DestroyByContact : MonoBehaviour {

	// Explosion for the object
	public GameObject explosion;

	// Explosion for the player
	public GameObject playerExplosion;

	// Explosion for the object
	public GameObject playerLooseLife;

	// Animation for shield Activation
	public GameObject shieldActivation;

	// Score earned when the object is destroyed
	public int scoreValue;

	// Life decrement when the player is touched
	public int lifeValue;

	// The Game Controller
	private GameController gameController;

	// The Player Controller
	private PlayerController playerController;

	/**
	 * Start() - Called on intialisation
	 */
	void Start() {
		// Attach the GameController
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent<GameController> ();
		}
		if (gameController == null) {
			Debug.Log ("Can't find GameController");
		}
		// Attach the PlayerController
		GameObject playerControllerObject = GameObject.FindWithTag ("Player");
		if (playerControllerObject != null) {
			playerController = playerControllerObject.GetComponent<PlayerController> ();
		}
		if (playerController == null) {
			Debug.Log ("Can't find PlayerController");
		}
	}

	/**
	 * OnTriggerEnter() - Called when object enter in collision with other collider
	 */
	void OnTriggerEnter(Collider other) {
	
		// Do nothing with the Boundary 
		if (other.tag == "Boundary") {
			return;
		}
		// If object is an Asteroid or an Enemy
		else if (this.tag == "Asteroid" || this.tag == "Enemy") {
			// Do nothing if the other is an Enemy, an Asteroid or a Bonus
			if (other.tag == "Enemy" || other.tag == "Asteroid" || other.tag == "Shield_bonus" || other.tag == "Life_bonus" || other.tag == "Laser_bonus") {
				return;
			}
			// Else destroy it
			else {
				Instantiate (explosion, transform.position, transform.rotation);
				// If it's destroyed by the player laser (add score and incraise Bonus chance)
				if (other.tag == "Bolt_p") {
					gameController.AddScore (scoreValue);
					gameController.DecBonusRandom ();
				}
				// If other is the Player
				else if (other.tag == "Player") {
					// Reset Lasers Spawns
					playerController.ResetLazers ();
					// Decrease the player lives
					if (gameController.DecLives (lifeValue) <= 0) {
						// If player life is 0 it's the end of the game
						Instantiate (playerExplosion, other.transform.position, other.transform.rotation);
						if (gameController != null) {
							gameController.GameOver ();
						}
						// Destroy player
						Destroy (other.gameObject);
					} else {
						// Play a laser impact
						Instantiate (playerLooseLife, other.transform.position, other.transform.rotation);
					}
				}
				// If it's destroyed by the Shield (desactive it)
				else if (other.tag == "Shield") {
					Instantiate (shieldActivation, playerController.gameObject.transform.position, playerController.gameObject.transform.rotation);
					int hitpoints = gameController.DecShields (lifeValue); 
					if (hitpoints <= 0) {
						Debug.Log ("DEGATS deg : " + lifeValue + ", hp : " + hitpoints); 
						other.gameObject.SetActive (false);
						if (hitpoints < 0) {
							// Reset Lasers Spawns
							playerController.ResetLazers ();
							// Decrease the player lives
							if (gameController.DecLives (-hitpoints) <= 0) {
								// If player life is 0 it's the end of the game
								Instantiate (playerExplosion, other.transform.position, other.transform.rotation);
								if (gameController != null) {
									gameController.GameOver ();
								}
								// Destroy player
								Destroy (playerController.gameObject);
							} else {
								// Play a laser impact
								Instantiate (playerLooseLife, playerController.gameObject.transform.position, playerController.gameObject.transform.rotation);
							}
						}
					}
				} 
				// Else destroy the other object
				else {
					Destroy(other.gameObject);
				}
				// Destroy the object
				Destroy(gameObject);
				return;
			}
		}
		// If object is a Shield bonus item
		else if (this.tag == "Shield_bonus") {
			// If other is player or player's laser activate the Shield
			if (other.tag == "Bolt_p" || other.tag == "Player") {
				Instantiate (explosion, this.transform.position, this.transform.rotation);
				Instantiate (shieldActivation, playerController.gameObject.transform.position, playerController.gameObject.transform.rotation);
				playerController.SetActiveShield ();
				gameController.IncShields();
				gameController.AddScore (scoreValue);
			} 
			// Destroy the object;
			Destroy(gameObject);
			return;
		}
		// If object is a Laser bonus item
		else if (this.tag == "Laser_bonus") {
			// If other is player or player's laser increase lasers spawns
			if (other.tag == "Bolt_p" || other.tag == "Player") {
				Instantiate (explosion, this.transform.position, this.transform.rotation);
				playerController.IncLazers();
				gameController.AddScore (scoreValue);
			} 
			// Destroy the object;
			Destroy(gameObject);
			return;
		}
		// If object is a Life bonus item
		else if (this.tag == "Life_bonus") {
			// If other is player or player's laser increase player lives
			if (other.tag == "Bolt_p" || other.tag == "Player") {
				Instantiate (explosion, this.transform.position, this.transform.rotation);
				gameController.IncLives();
				gameController.AddScore (scoreValue);
			} 
			// Destroy the object;
			Destroy(gameObject);
			return;
		}
		// If object is an Enemy's laser
		else if (this.tag == "Bolt_e") {
			// If other is the Shield desactivate it
			if (other.tag == "Shield") {
				Instantiate (shieldActivation, playerController.gameObject.transform.position, playerController.gameObject.transform.rotation);
				int hitpoints = gameController.DecShields (lifeValue); 
				if (hitpoints <= 0) {
					Debug.Log ("DEGATS deg : " + lifeValue + ", hp : " + hitpoints); 
					other.gameObject.SetActive (false);
					if (hitpoints < 0) {
						// Reset Lasers Spawns
						playerController.ResetLazers ();
						// Decrease the player lives
						if (gameController.DecLives (-hitpoints) <= 0) {
							// If player life is 0 it's the end of the game
							Instantiate (playerExplosion, other.transform.position, other.transform.rotation);
							if (gameController != null) {
								gameController.GameOver ();
							}
							// Destroy player
							Destroy (playerController.gameObject);
						} else {
							// Play a laser impact
							Instantiate (playerLooseLife, playerController.gameObject.transform.position, playerController.gameObject.transform.rotation);
						}
					}
				}
			} 
			// If other is the Player
			else if (other.tag == "Player") {
				// Reset Lasers Spawns
				playerController.ResetLazers ();
				// Decrease the player lives
				if (gameController.DecLives (lifeValue) <= 0) {
					// If player life is 0 it's the end of the game
					Instantiate (playerExplosion, other.transform.position, other.transform.rotation);
					if (gameController != null) {
						gameController.GameOver ();
					}
					// Destroy player
					Destroy (other.gameObject);
				} else {
					// Play a laser impact
					Instantiate (playerLooseLife, other.transform.position, other.transform.rotation);
				}
			}
			// Destroy the object
			Destroy(gameObject);
			return;
		}
	}
}
