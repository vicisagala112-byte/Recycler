using UnityEngine;

namespace Anoa
{
    public class TrashController : MonoBehaviour
    {
        [Header("Data Sampah")]
        [SerializeField] protected TRASH_TYPE typeSampah;
        [SerializeField] protected bool boolSudahDibuang = false;
        public bool SudahDibuang => boolSudahDibuang; // 🔹 properti agar bisa dibaca dari luar

        protected GameManagerSorting gameManagerSorting;
        protected TrashManager trashManager;
        protected Vector3 vec3PosisiAwal;

        private void Start()
        {
            gameManagerSorting = FindObjectOfType<GameManagerSorting>();
            trashManager = FindObjectOfType<TrashManager>();
            vec3PosisiAwal = transform.position;
        }

        public TRASH_TYPE GetTypeSampah() => typeSampah;

        private void OnEnable()
        {
            // reset setiap kali keluar dari pool
            boolSudahDibuang = false;
        }

        private void OnTriggerEnter2D(Collider2D _coll)
        {
            if (boolSudahDibuang) return;

            // cek apakah yang disentuh adalah tong
            if (!_coll.CompareTag("TongOrganik") &&
                !_coll.CompareTag("TongAnorganik") &&
                !_coll.CompareTag("TongB3"))
                return;

            bool _benar = false;

            // cek kecocokan jenis sampah
            if (_coll.CompareTag("TongOrganik") && typeSampah == TRASH_TYPE.ORGANIK)
                _benar = true;
            else if (_coll.CompareTag("TongAnorganik") && typeSampah == TRASH_TYPE.ANORGANIK)
                _benar = true;
            else if (_coll.CompareTag("TongB3") && typeSampah == TRASH_TYPE.B3)
                _benar = true;

            if (_benar)
            {
                boolSudahDibuang = true;

                // kasih tahu game manager
                gameManagerSorting.FunctionBenarBuangSampah(this);

                // kasih tahu TrashManager untuk hapus dari daftar aktif
                if (trashManager != null)
                    trashManager.FunctionOnTrashProcessed(gameObject);

                // langsung nonaktifkan agar hilang dari layar
                gameObject.SetActive(false);
            }
            else
            {
                // salah tong → langsung balik ke posisi awal
                gameManagerSorting.FunctionSalahBuangSampah();
                transform.position = vec3PosisiAwal;
            }
        }
    }
}
