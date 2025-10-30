using UnityEngine;

namespace Anoa.Utility
{
    public class CameraFollow : MonoBehaviour
    {
        [Header("Target yang diikuti")]
        [SerializeField] protected Transform transTarget;

        [Header("Kecepatan kamera mengikuti")]
        [SerializeField] protected float floatSmoothSpeed = 0.125f;

        [Header("Offset kamera")]
        [SerializeField] protected Vector3 vecOffset;

        [Header("Batas kamera")]
        [SerializeField] protected Vector2 vecMinBounds;
        [SerializeField] protected Vector2 vecMaxBounds;

        protected void LateUpdate()
        {
            if (transTarget == null)
                return;

            Vector3 _vecDesired = transTarget.position + vecOffset;
            Vector3 _vecSmooth = Vector3.Lerp(transform.position, _vecDesired, floatSmoothSpeed);

            float _floatClampX = Mathf.Clamp(_vecSmooth.x, vecMinBounds.x, vecMaxBounds.x);
            float _floatClampY = Mathf.Clamp(_vecSmooth.y, vecMinBounds.y, vecMaxBounds.y);

            transform.position = new Vector3(_floatClampX, _floatClampY, transform.position.z);
        }
    }
}
