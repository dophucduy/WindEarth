using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FanTrap2D : MonoBehaviour
{
    [Header("Fan settings")]
    public float blowForce = 200f;
    public float maxDistance = 3f;
    public Vector2 direction = Vector2.right;
    public bool isActive = true;

    [Header("Filter")]
    public LayerMask affectedLayers;

    private readonly List<Rigidbody2D> bodies = new();
    private Collider2D triggerCollider;

    void Awake()
    {
        triggerCollider = GetComponent<Collider2D>();
        //triggerCollider.isTrigger = true;
    }

    //void FixedUpdate()
    //{
    //    if (!isActive) return;

    //    Vector2 worldDir = transform.TransformDirection(direction.normalized);

    //    for (int i = bodies.Count - 1; i >= 0; i--)
    //    {
    //        var rb = bodies[i];
    //        if (rb == null)
    //        {
    //            bodies.RemoveAt(i);
    //            continue;
    //        }

    //        float dist = Vector2.Distance(rb.position, transform.position);
    //        float t = Mathf.Clamp01(1f - dist / maxDistance);
    //        if (t <= 0f) continue;

    //        // If this rigidbody belongs to the player, blow it backwards (opposite of fan direction)
    //        bool isPlayer = rb.GetComponentInParent<SmashMovement>() != null ||
    //                        rb.GetComponentInParent<PlayerNetworkController>() != null;

    //        Vector2 appliedDir = isPlayer ? -worldDir : worldDir;

    //        rb.AddForce(appliedDir * blowForce * t, ForceMode2D.Force);
    //    }
    //}
    void FixedUpdate()
    {
        if (!isActive) return;

        Vector2 worldDir = transform.TransformDirection(direction.normalized);

        for (int i = bodies.Count - 1; i >= 0; i--)
        {
            Rigidbody2D rb = bodies[i];

            if (rb == null)
            {
                bodies.RemoveAt(i);
                continue;
            }

            float dist = Vector2.Distance(rb.position, transform.position);

            if (dist > maxDistance) continue;

            float forceFactor = 1f - (dist / maxDistance);

            bool isPlayer = rb.GetComponentInParent<SmashMovement>() != null;

            Vector2 appliedDir = isPlayer ? -worldDir : worldDir;
            Debug.Log("Applying force: " + appliedDir * blowForce * forceFactor);

            rb.AddForce(appliedDir * blowForce * forceFactor, ForceMode2D.Impulse);
            float maxWindSpeed = blowForce * 0.1f;

            Vector2 vel = rb.linearVelocity;

            // nếu gió thổi sang trái
            if (appliedDir.x < 0)
            {
                vel.x = Mathf.Min(vel.x, -maxWindSpeed);
            }
            // nếu gió thổi sang phải
            else if (appliedDir.x > 0)
            {
                vel.x = Mathf.Max(vel.x, maxWindSpeed);
            }

            rb.linearVelocity = vel;
            // chặn player bay lên trên fan
            if (rb.linearVelocity.y > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            }

        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //ElementIdentity identity = other.GetComponent<ElementIdentity>();

        //if (identity != null && identity.elementType == ElementType.Wind)
        //{
        //    return; // Wind is immune to fan
        //}

        //if (((1 << other.gameObject.layer) & affectedLayers) == 0) return;

        //var rb = other.attachedRigidbody;
        //if (rb != null && !bodies.Contains(rb))
        //    bodies.Add(rb);
        Debug.Log("Fan triggered by: " + other.name);

        if (((1 << other.gameObject.layer) & affectedLayers) == 0)
        {
            Debug.Log("Layer not allowed");
            return;
        }

        Rigidbody2D rb = other.attachedRigidbody;

        if (rb != null && !bodies.Contains(rb))
        {
            Debug.Log("Added body: " + rb.name);
            bodies.Add(rb);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        var rb = other.attachedRigidbody;
        if (rb != null)
            bodies.Remove(rb);
    }
    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("Inside fan: " + other.name);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        var col = GetComponent<Collider2D>();

        if (col is BoxCollider2D b)
            Gizmos.DrawWireCube(transform.position + (Vector3)b.offset, b.size);
        else if (col is CircleCollider2D c)
            Gizmos.DrawWireSphere(transform.position + (Vector3)c.offset, c.radius);

        Gizmos.DrawLine(transform.position,
            transform.position + (Vector3)transform.TransformDirection(direction.normalized) * 1.5f);
    }
}
