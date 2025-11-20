using UnityEngine;

namespace Anoa
{
    public class GameManagerSorting : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] protected TrashManager trashManager;
        [SerializeField] protected UIManager uiManager;
        [SerializeField] protected SoundManager soundManager;

        [Header("Prefab Text Koin Berdasarkan Jenis")]
        [SerializeField] private GameObject prefabTextKoinOrganik;
        [SerializeField] private GameObject prefabTextKoinAnorganik;
        [SerializeField] private GameObject prefabTextKoinB3;

        [Header("Posisi Teks Koin (di atas tong)")]
        [SerializeField] private Transform posTeksOrganik;
        [SerializeField] private Transform posTeksAnorganik;
        [SerializeField] private Transform posTeksB3;

        [Header("Game Settings")]
        [SerializeField] protected float floatWaktuMax = 30f;

        protected float floatWaktuSekarang;
        protected bool boolGameBerakhir = false;

        protected int totalKoin = 0;
        protected int totalSampahDikumpulkan = 0;

        private void Start()
        {
            floatWaktuSekarang = floatWaktuMax;
            uiManager.FunctionUpdateWaktu(floatWaktuSekarang);
            uiManager.FunctionUpdateScore(totalKoin);
        }

        private void Update()
        {
            if (boolGameBerakhir) return;

            floatWaktuSekarang -= Time.deltaTime;
            uiManager.FunctionUpdateWaktu(floatWaktuSekarang);

            if (floatWaktuSekarang <= 0f)
            {
                floatWaktuSekarang = 0f;
                FunctionGameOver();
            }

            if (trashManager != null && trashManager.IsAllCleared() && !boolGameBerakhir)
            {
                FunctionFinish();
            }
        }


        // ============================
        // SAMPAH SALAH
        // ============================
        public void FunctionSalahBuangSampah()
        {
            if (boolGameBerakhir) return;

            floatWaktuSekarang -= 2f;
            if (floatWaktuSekarang < 0f) floatWaktuSekarang = 0f;

            uiManager.FunctionTampilTeksSalah();
            uiManager.FunctionUpdateWaktu(floatWaktuSekarang);

            if (soundManager != null)
                soundManager.FunctionPlaySampahSalah();

            if (floatWaktuSekarang <= 0f)
                FunctionGameOver();
        }

        // ============================
        // SAMPAH BENAR
        // ============================
        public void FunctionBenarBuangSampah(TrashController _trash)
        {
            if (boolGameBerakhir) return;

            int tambahanKoin = 0;
            Transform targetPos = null;
            GameObject prefabTeks = null;

            switch (_trash.GetTypeSampah())
            {
                case TRASH_TYPE.ORGANIK:
                    tambahanKoin = 2;
                    targetPos = posTeksOrganik;
                    prefabTeks = prefabTextKoinOrganik;
                    break;

                case TRASH_TYPE.ANORGANIK:
                    tambahanKoin = 5;
                    targetPos = posTeksAnorganik;
                    prefabTeks = prefabTextKoinAnorganik;
                    break;

                case TRASH_TYPE.B3:
                    tambahanKoin = 8;
                    targetPos = posTeksB3;
                    prefabTeks = prefabTextKoinB3;
                    break;
            }

            totalKoin += tambahanKoin;
            totalSampahDikumpulkan++;

            uiManager.FunctionTampilTeksBenar();
            uiManager.FunctionUpdateScore(totalKoin);

            // Spawn teks di atas tong
            if (prefabTeks != null && targetPos != null)
                FunctionSpawnTextKoin(prefabTeks, targetPos.position, tambahanKoin);

            // Suara
            soundManager?.FunctionPlaySampahBenar();
            soundManager?.FunctionPlayKoinBertambah();
        }

        // ============================
        // SPWAN TEXT KOIN KHUSUS
        // ============================
        public void FunctionSpawnTextKoin(GameObject prefab, Vector3 worldPos, int jumlahKoin)
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas == null) return;

            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

            GameObject obj = Instantiate(prefab, screenPos, Quaternion.identity);
            obj.transform.SetParent(canvas.transform, false);

            RectTransform rect = obj.GetComponent<RectTransform>();
            rect.position = screenPos;

            TMPro.TMP_Text txt = obj.GetComponent<TMPro.TMP_Text>();
            if (txt != null)
                txt.text = "+" + jumlahKoin;
        }

        // ============================
        // GAME OVER / FINISH
        // ============================
        protected void FunctionGameOver()
        {
            if (boolGameBerakhir) return;
            boolGameBerakhir = true;
            uiManager.FunctionTampilPanelGameOver(totalSampahDikumpulkan, totalKoin);
            soundManager?.FunctionPlayKalah();
        }

        protected void FunctionFinish()
        {
            if (boolGameBerakhir) return;
            boolGameBerakhir = true;
            uiManager.FunctionTampilPanelFinish(totalSampahDikumpulkan, totalKoin);
            soundManager?.FunctionPlayMenang();
        }
    }
}
