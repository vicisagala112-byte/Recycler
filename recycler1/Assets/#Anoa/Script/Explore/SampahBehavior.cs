using Anoa.Player;
using UnityEngine;

namespace Anoa.Explore
{
    public class SampahBehavior : MonoBehaviour
    {
        [Header("ID Sampah (0–23)")]
        public int idSampah; // isi ID berbeda untuk tiap prefab sampah

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

            if (classSampahUI.FunctionTongPenuh())
                return;

            float _floatDistance = Vector2.Distance(transform.position, transPlayer.position);

            // 🔹 Tarik ke player jika dalam jangkauan magnet
            if (_floatDistance <= PlayerController.instance.MagnetRange)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    transPlayer.position,
                    Time.deltaTime * 10
                );
            }

            // 🔹 Jika cukup dekat → ambil
            if (_floatDistance <= floatDetectRange)
            {
                bool _boolBerhasil = classSampahUI.FunctionTambahSampah();
                if (_boolBerhasil)
                {
                    CollectedTrashData.listTrashID.Add(idSampah);
                    PlayerController.instance?.FunctionTriggerBawaSampah(idSampah);

                    gameObject.SetActive(false);
                    classSampahManager?.FunctionOnTrashCollected(gameObject);
                }
            }
        }
    }
}
