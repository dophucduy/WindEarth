using Unity.Netcode;
using UnityEngine;

namespace Game.Earth
{
    public class EarthAbilityController : NetworkBehaviour
    {
        [Header("Refs")]
        [SerializeField] private EarthPillarSpawner pillarSpawner;

        [Header("Targeting")]
        [SerializeField] private Transform castOrigin;
        [SerializeField] private float interactRange = 1.8f;
        [SerializeField] private LayerMask anchorableMask;
        [SerializeField] private LayerMask heavyMask;

        [Header("Pillar")]
        [SerializeField] private float pillarForwardOffset = 0.9f;

        // Demo input: ??i sang Input System c?a team n?u c?n
        private void Update()
        {
            if (!IsOwner) return;

            // Q: d?ng tr?
            if (Input.GetKeyDown(KeyCode.Q))
                TryCastPillar();

            // E: toggle anchor
            if (Input.GetKeyDown(KeyCode.E))
                TryToggleAnchor();

            // R: kéo v?t n?ng khi gi?
            if (Input.GetKey(KeyCode.R))
                TryPullHeavy(holding: true);
            if (Input.GetKeyUp(KeyCode.R))
                TryPullHeavy(holding: false);
        }

        private void TryCastPillar()
        {
            if (pillarSpawner == null) return;

            Vector2 dir = GetFacingDir();
            Vector2 pos = (Vector2)castOrigin.position + dir * pillarForwardOffset;

            pillarSpawner.TryRequestSpawn(pos);
        }

        private void TryToggleAnchor()
        {
            var hit = Physics2D.Raycast(castOrigin.position, GetFacingDir(), interactRange, anchorableMask);
            if (!hit.collider) return;

            var anchorable = hit.collider.GetComponentInParent<Anchorable2D>();
            if (anchorable == null) return;

            ToggleAnchorServerRpc(anchorable.NetworkObject);
        }

        private void TryPullHeavy(bool holding)
        {
            var hit = Physics2D.Raycast(castOrigin.position, GetFacingDir(), interactRange, heavyMask);
            if (!hit.collider) return;

            var heavy = hit.collider.GetComponentInParent<HeavyPullable2D>();
            if (heavy == null) return;

            if (holding)
                heavy.PullTowardsServerRpc(castOrigin.position);
            else
                heavy.StopPullServerRpc();
        }

        [ServerRpc]
        private void ToggleAnchorServerRpc(NetworkObjectReference targetRef)
        {
            if (!targetRef.TryGet(out var no)) return;

            var anchorable = no.GetComponent<Anchorable2D>();
            if (anchorable == null) return;

            anchorable.IsAnchored.Value = !anchorable.IsAnchored.Value;
        }

        private Vector2 GetFacingDir()
        {
            // Cách ??n gi?n: d?a vào localScale.x (b?n có th? thay b?ng controller chung c?a team)
            return transform.localScale.x >= 0 ? Vector2.right : Vector2.left;
        }
    }
}
