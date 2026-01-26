using Unity.Netcode;
using UnityEngine;

namespace Game.Earth
{
    [RequireComponent(typeof(NetworkObject))]
    public class EarthPillar : NetworkBehaviour
    {
        [SerializeField] private float lifeTime = 8f;

        private float _t;

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                _t = lifeTime;
            }
        }

        private void Update()
        {
            if (!IsServer) return;

            _t -= Time.deltaTime;
            if (_t <= 0f)
            {
                NetworkObject.Despawn(true);
            }
        }
    }
}
