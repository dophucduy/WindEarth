using Unity.Netcode;
using UnityEngine;

namespace Game.Earth
{
    [RequireComponent(typeof(Rigidbody2D), typeof(NetworkObject))]
    public class Anchorable2D : NetworkBehaviour
    {
        private Rigidbody2D _rb;

        public NetworkVariable<bool> IsAnchored = new(false);

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        public override void OnNetworkSpawn()
        {
            ApplyAnchor(IsAnchored.Value);
            IsAnchored.OnValueChanged += (_, newVal) => ApplyAnchor(newVal);
        }

        private void OnDestroy()
        {
            IsAnchored.OnValueChanged -= (_, __) => { };
        }

        private void ApplyAnchor(bool anchored)
        {
            if (_rb == null) return;

            // Neo: freeze position (ho?c t?ng mass/constraints tu? style puzzle)
            _rb.constraints = anchored
                ? RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation
                : RigidbodyConstraints2D.FreezeRotation;
        }
    }
}
