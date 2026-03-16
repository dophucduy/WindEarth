using Unity.Netcode;
using UnityEngine;

public class Trampoline : NetworkBehaviour
{
    [SerializeField] private float bounceForce = 15f;
    private Animator anim;
    private AudioSource audioSource;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsServer) return;

        NetworkObject netObj = collision.GetComponent<NetworkObject>();
        if (netObj == null || !netObj.IsPlayerObject) return;

        BouncePlayerClientRpc(netObj);
        PlayBounceClientRpc();
    }

    [ClientRpc]
    void BouncePlayerClientRpc(NetworkObjectReference playerRef)
    {
        if (!playerRef.TryGet(out NetworkObject playerObj)) return;

        Rigidbody2D rb = playerObj.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        if (rb.linearVelocity.y <= 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);

        }
    }

    [ClientRpc]
    void PlayBounceClientRpc()
    {
        anim.SetTrigger("Bounce");
         audioSource?.Play();
    }
}