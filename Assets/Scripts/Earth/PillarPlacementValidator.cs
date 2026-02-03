using UnityEngine;

namespace Game.Earth
{
    public static class PillarPlacementValidator
    {
        public static bool CanPlacePillar(
            Vector2 worldPos,
            Vector2 pillarSize,
            LayerMask groundMask,
            LayerMask blockingMask,
            float groundCheckDistance = 0.1f)
        {
            // 1) ph?i có ground ngay bên d??i (tránh spawn gi?a không trung)
            var groundHit = Physics2D.Raycast(worldPos, Vector2.down, groundCheckDistance, groundMask);
            if (!groundHit.collider) return false;

            // 2) vùng pillar không ???c ch?ng lên collider khác (t??ng/ng??i/v?t)
            var overlap = Physics2D.OverlapBox(worldPos, pillarSize, 0f, blockingMask);
            if (overlap) return false;

            return true;
        }
    }
}
