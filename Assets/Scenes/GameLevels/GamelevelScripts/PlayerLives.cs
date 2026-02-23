using UnityEngine;
using UnityEngine.SceneManagement;

// Simple lives manager for a player. Attach to player prefab.
public class PlayerLives : MonoBehaviour
{
    [Header("Respawn")]
    public int maxRespawns = 2; // allow respawning this many times before game over
    private int respawnsUsed = 0;

    [Header("Refs")]
    public Transform respawnPoint; // optional: where to respawn the player

    public void HandleDeath(bool isPlayer1)
    {
        respawnsUsed++;

        if (respawnsUsed > maxRespawns)
        {
            LevelFinishManager.Instance.GameOver();
            return;
        }

        // simple respawn: move to respawnPoint if set, otherwise reload scene for simplicity for now
        if (respawnPoint != null)
        {
            var rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            transform.position = respawnPoint.position;
        }
        else
        {
            // reload current scene to reset player state
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
