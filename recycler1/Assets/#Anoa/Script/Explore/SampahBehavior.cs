using Anoa.Player;
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
            classSampahManager = SampahManager.instance; 
            classSampahUI = SampahUIManager.instance;
        }

        protected void Update()
        {
            if (transPlayer == null || classSampahUI == null)
                return;

            float _floatDistance = Vector2.Distance(transform.position, transPlayer.position);

            if (_floatDistance <= PlayerController.instance.MagnetRange)
            {
                transform.position = Vector3.MoveTowards(transform.position, transPlayer.position, Time.deltaTime * 10);
            }

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
