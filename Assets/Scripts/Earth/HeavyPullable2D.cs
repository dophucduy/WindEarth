using Unity.Netcode;
using UnityEngine;

namespace Game.Earth
{
    [RequireComponent(typeof(Rigidbody2D), typeof(NetworkObject))]
    public class HeavyPullable2D : NetworkBehaviour
    {
        [SerializeField] private float pullSpeed = 5f;

        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        [ServerRpc(RequireOwnership = false)]
        public void PullTowardsServerRpc(Vector2 targetPos)
        {
            // Server m?i tác ??ng v?t lý ?? tránh desync
            Vector2 dir = (targetPos - _rb.position);
            Vector2 vel = dir.normalized * pullSpeed;
            _rb.linearVelocity = new Vector2(vel.x, _rb.linearVelocity.y);
        }

        [ServerRpc(RequireOwnership = false)]
        public void StopPullServerRpc()
        {
            _rb.linearVelocity = new Vector2(0f, _rb.linearVelocity.y);
        }
    }
}
