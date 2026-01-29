using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Vector3 spawnPoint;

    void Start()
    {
        spawnPoint = transform.position;
    }

    public void Respawn()
    {
        transform.position = spawnPoint;
    }
}
