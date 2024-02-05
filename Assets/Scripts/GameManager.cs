using Packages.Rider.Editor.UnitTesting;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public Player player;
    public ParticleSystem explosion;
    public float respawnTime = 3.0f;
    public float respawnInvulnerabilityTime = 3.0f;
    public int lives = 3;
    public int score = 0;

    public Text scoreText;
    public Text livesText;

    public void AsteroidDestroyed(Asteroid asteroid) {
        scoreText.text = "Score: " + score;
        livesText.text = "Lives: " + lives;


        this.explosion.transform.position = asteroid.transform.position;
        this.explosion.Play();

        if (asteroid.size < 0.75f) {
            score += 100;
        }
        else if (asteroid.size < 1.2f) {
            score += 50;
        }
        else {
            score += 25;
        }

        scoreText.text = "Score: " + score;
    }

    public void PlayerDied() {
        this.explosion.transform.position = this.player.transform.position;
        this.explosion.Play();
        this.lives--;
        if (this.lives <= 0) {
            GameOver();
        }
        Invoke(nameof(Respawn), this.respawnTime);
        livesText.text = "Lives: " + lives;
    }

    private void Respawn() {
        this.player.transform.position = Vector3.zero;
        this.player.gameObject.layer = LayerMask.NameToLayer("Ignore Collisions");
        this.player.gameObject.SetActive(true);
        
        Invoke(nameof(TurnOnCollisions), respawnInvulnerabilityTime);
    }

    private void TurnOnCollisions() {
        this.player.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private void GameOver() {
        this.lives = 0;
        this.score = 0;

        Invoke(nameof(Respawn), this.respawnTime);
    }
}
