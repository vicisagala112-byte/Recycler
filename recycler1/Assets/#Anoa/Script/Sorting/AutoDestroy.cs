using UnityEngine;

namespace Anoa
{
    public class AutoDestroy : MonoBehaviour
    {
        [SerializeField] private float lifetime = 0.6f;

        private void OnEnable()
        {
            Destroy(gameObject, lifetime);
        }
    }
}
