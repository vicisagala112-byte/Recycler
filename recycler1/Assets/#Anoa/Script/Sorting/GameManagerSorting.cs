using UnityEngine;

namespace Anoa
{
    public class GameManagerSorting : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] protected TrashManager trashManager;
        [SerializeField] protected UIManager uiManager;
        [SerializeField] protected SoundManager soundManager;
        [SerializeField] protected GameObject prefabTextKoinNaik; // 🟡 Prefab teks koin naik

        [Header("Game Settings")]
        [SerializeField] protected float floatWaktuMax = 30f;

        protected float floatWaktuSekarang;
        protected bool boolGameBerakhir = false;
        protected int totalKoin = 0;

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

        // ✅ Jika sampah benar
        public void FunctionBenarBuangSampah(TrashController _trash)
        {
            if (boolGameBerakhir) return;

            int tambahanKoin = 0;

            switch (_trash.GetTypeSampah())
            {
                case TRASH_TYPE.ORGANIK: tambahanKoin = 2; break;
                case TRASH_TYPE.ANORGANIK: tambahanKoin = 5; break;
                case TRASH_TYPE.B3: tambahanKoin = 8; break;
            }

            totalKoin += tambahanKoin;

            uiManager.FunctionTampilTeksBenar();
            uiManager.FunctionUpdateScore(totalKoin);

            // 🟡 Tampilkan teks koin naik di atas tong
            if (_trash != null)
            {
                Vector3 posTong = _trash.transform.position;
                FunctionSpawnTextKoin(posTong, tambahanKoin);
            }

            if (soundManager != null)
            {
                soundManager.FunctionPlaySampahBenar();
                soundManager.FunctionPlayKoinBertambah();
            }
        }

        // ❌ Jika sampah salah
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

        // 🔚 Game Over
        protected void FunctionGameOver()
        {
            if (boolGameBerakhir) return;
            boolGameBerakhir = true;

            uiManager.FunctionTampilPanelGameOver(totalKoin);

            if (soundManager != null)
                soundManager.FunctionPlayKalah();
        }

        // 🏁 Finish
        protected void FunctionFinish()
        {
            if (boolGameBerakhir) return;
            boolGameBerakhir = true;

            uiManager.FunctionTampilPanelFinish(totalKoin);

            if (soundManager != null)
                soundManager.FunctionPlayMenang();
        }

        // 🟨 Fungsi teks koin naik
        public void FunctionSpawnTextKoin(Vector3 posisiTong, int jumlahKoin)
        {
            if (prefabTextKoinNaik == null) return;

            GameObject obj = Instantiate(prefabTextKoinNaik, posisiTong, Quaternion.identity);

            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas != null)
                obj.transform.SetParent(canvas.transform, false);

            TMPro.TMP_Text txt = obj.GetComponent<TMPro.TMP_Text>();
            if (txt != null)
                txt.text = "+" + jumlahKoin.ToString();
        }
    }
}
