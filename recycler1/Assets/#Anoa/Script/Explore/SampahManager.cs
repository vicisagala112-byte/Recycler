using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anoa.Explore
{
    public class SampahManager : MonoBehaviour
    {
        public static SampahManager instance;

        [Header("Pengaturan Sampah")]
        [SerializeField] protected GameObject[] arrObjSampahPrefabs;
        [SerializeField] protected int intJumlahMaksimalSampah = 20;
        [SerializeField] protected int intJumlahAktifAwal = 10;

        [Header("Respawn Settings")]
        [SerializeField] protected float floatRespawnDelay = 2f;
        [SerializeField] protected int intMinSpawnBaru = 1;
        [SerializeField] protected int intMaxSpawnBaru = 3;

        protected List<GameObject> listObjSampahPool = new List<GameObject>();
        protected List<Collider2D> listColSungai = new List<Collider2D>();

        private void Awake()
        {
            instance = this;
        }

        protected void Start()
        {
            // ✅ Reset UI tong sampah setiap kali scene dimulai
            if (SampahUIManager.instance != null)
                SampahUIManager.instance.FunctionResetTong();

            // 🔹 Ambil collider sungai
            GameObject[] _arrObjSungai = GameObject.FindGameObjectsWithTag("Sungai");
            foreach (GameObject _objSungai in _arrObjSungai)
            {
                Collider2D _col = _objSungai.GetComponent<Collider2D>();
                if (_col != null)
                    listColSungai.Add(_col);
            }

            if (listColSungai.Count == 0)
            {
                Debug.LogWarning("❗ Tidak ada objek bertag 'Sungai' yang punya Collider2D!");
                return;
            }

            // 🔹 Buat pool sampah
            for (int i = 0; i < intJumlahMaksimalSampah; i++)
            {
                GameObject _prefab = arrObjSampahPrefabs[Random.Range(0, arrObjSampahPrefabs.Length)];
                GameObject _objSampah = Instantiate(_prefab);
                _objSampah.SetActive(false);

                if (_objSampah.GetComponent<SampahBehavior>() == null)
                    _objSampah.AddComponent<SampahBehavior>();

                listObjSampahPool.Add(_objSampah);
            }

            // 🔹 Aktifkan sampah awal
            for (int i = 0; i < intJumlahAktifAwal; i++)
                FunctionActivateRandomTrash();
        }

        // 🔹 Aktifkan sampah secara acak di sungai
        protected void FunctionActivateRandomTrash()
        {
            List<GameObject> _listNonActive = listObjSampahPool.FindAll(t => !t.activeSelf);
            if (_listNonActive.Count == 0) return;

            GameObject _objRandomTrash = _listNonActive[Random.Range(0, _listNonActive.Count)];
            Vector3 _vecSpawnPos = FunctionGetRandomRiverPosition();

            _objRandomTrash.transform.position = _vecSpawnPos;
            _objRandomTrash.SetActive(true);
        }

        // 🔹 Saat sampah diambil
        public void FunctionOnTrashCollected(GameObject _objTrash)
        {
            _objTrash.SetActive(false);
            StartCoroutine(FunctionRespawnRandomTrash());
        }

        // 🔹 Respawn beberapa sampah baru setelah delay
        protected IEnumerator FunctionRespawnRandomTrash()
        {
            yield return new WaitForSeconds(floatRespawnDelay);
            int _intJumlahBaru = Random.Range(intMinSpawnBaru, intMaxSpawnBaru + 1);

            for (int i = 0; i < _intJumlahBaru; i++)
                FunctionActivateRandomTrash();

            Debug.Log($"🔁 {_intJumlahBaru} sampah baru diaktifkan setelah ambil sampah!");
        }

        // 🔹 Tentukan posisi acak di area sungai
        protected Vector3 FunctionGetRandomRiverPosition()
        {
            if (listColSungai.Count == 0) return Vector3.zero;

            Collider2D _col = listColSungai[Random.Range(0, listColSungai.Count)];
            Bounds _bounds = _col.bounds;

            for (int i = 0; i < 50; i++)
            {
                float _x = Random.Range(_bounds.min.x, _bounds.max.x);
                float _y = Random.Range(_bounds.min.y, _bounds.max.y);
                Vector2 _vecRandomPoint = new Vector2(_x, _y);
                if (_col.OverlapPoint(_vecRandomPoint))
                    return _vecRandomPoint;
            }

            return _col.bounds.center;
        }
    }
}
