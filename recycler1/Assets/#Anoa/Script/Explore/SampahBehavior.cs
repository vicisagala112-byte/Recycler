using UnityEngine;

namespace Anoa.Explore
{
    public class SampahBehavior : MonoBehaviour
    {
        [SerializeField] protected float floatDetectRange = 1.5f;

        protected Transform transPlayer;
        protected SampahManager classSampahManager;
        protected SampahUIManager classSampahUI;

        protected void Start()
        {
            transPlayer = GameObject.FindGameObjectWithTag("Player")?.transform;
            classSampahManager = FindObjectOfType<SampahManager>();
            classSampahUI = FindObjectOfType<SampahUIManager>();
        }

        protected void Update()
        {
            if (transPlayer == null || classSampahUI == null)
                return;

            float _floatDistance = Vector2.Distance(transform.position, transPlayer.position);
            if (_floatDistance <= floatDetectRange)
            {
                if (classSampahUI.FunctionTongPenuh())
                    return;

                bool _boolBerhasil = classSampahUI.FunctionTambahSampah();
                if (_boolBerhasil)
                {
                    gameObject.SetActive(false);
                    classSampahManager?.FunctionOnTrashCollected(gameObject);
                }
            }
        }
    }
}
