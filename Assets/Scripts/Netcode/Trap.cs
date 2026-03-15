using Unity.Netcode;
using UnityEngine;
using System.Collections;

public class Trap : NetworkBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsServer) return;

        SmashMovement player = other.GetComponent<SmashMovement>();

        if (player != null)
        {
            player.Die();
            StartCoroutine(GameOverDelay());
        }
    }

    IEnumerator GameOverDelay()
    {
        yield return new WaitForSeconds(1.5f); // length of death animation
        GameOverManager.Singleton.TriggerGameOverServerRpc();
    }
}