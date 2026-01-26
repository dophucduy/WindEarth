using Unity.Netcode;
using UnityEngine;

namespace Game.Earth
{
    public class EarthPillarSpawner : NetworkBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private NetworkObject pillarPrefab;

        [Header("Rules")]
        [SerializeField] private int maxActivePillars = 2;
        [SerializeField] private float cooldown = 1.2f;

        [Header("Placement")]
        [SerializeField] private Vector2 pillarSize = new(0.9f, 1.6f);
        [SerializeField] private LayerMask groundMask;
        [SerializeField] private LayerMask blockingMask;

        private float _cd;
        private readonly NetworkVariable<int> _activeCount = new(0);

        private void Update()
        {
            if (!IsOwner) return;
            if (_cd > 0f) _cd -= Time.deltaTime;
        }

        public void TryRequestSpawn(Vector2 desiredWorldPos)
        {
            if (!IsOwner) return;
            if (_cd > 0f) return;

            _cd = cooldown;
            RequestSpawnServerRpc(desiredWorldPos);
        }

        [ServerRpc]
        private void RequestSpawnServerRpc(Vector2 desiredWorldPos, ServerRpcParams rpcParams = default)
        {
            if (_activeCount.Value >= maxActivePillars) return;

            if (!PillarPlacementValidator.CanPlacePillar(
                    desiredWorldPos, pillarSize, groundMask, blockingMask))
                return;

            var pillar = Instantiate(pillarPrefab, desiredWorldPos, Quaternion.identity);
            pillar.Spawn(true);

            _activeCount.Value++;

            // Khi pillar despawn thì giảm count (đơn giản: hook bằng coroutine server-side)
            StartCoroutine(DecrementOnDespawn(pillar));
        }

        private System.Collections.IEnumerator DecrementOnDespawn(NetworkObject pillar)
        {
            while (pillar != null && pillar.IsSpawned)
                yield return null;

            _activeCount.Value = Mathf.Max(0, _activeCount.Value - 1);
        }
    }
}
