using UnityEngine;

namespace Anoa
{
    public class GameManagerSorting : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] protected TrashManager trashManager;
        [SerializeField] protected UIManager uiManager;
        [SerializeField] protected SoundManager soundManager;

        [Header("Game Settings")]
        [SerializeField] protected float floatWaktuMax = 30f;
        [SerializeField] protected int intKoin = 0;

        protected float floatWaktuSekarang;
        protected bool boolGameBerakhir = false;

        private void Start()
        {
            floatWaktuSekarang = floatWaktuMax;
            uiManager.FunctionUpdateWaktu(floatWaktuSekarang);
            uiManager.FunctionUpdateKoin(intKoin);
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

            // jika semua sampah habis
            if (trashManager != null && trashManager.IsAllCleared() && !boolGameBerakhir)
            {
                FunctionFinish();
            }
        }

        public void FunctionBenarBuangSampah(TrashController _trash)
        {
            if (boolGameBerakhir) return;

            intKoin += 10;
            uiManager.FunctionTampilTeksBenar();
            uiManager.FunctionUpdateKoin(intKoin);

            // 🔊 Mainkan efek suara benar dan koin bertambah
            if (soundManager != null)
            {
                soundManager.FunctionPlaySampahBenar();
                soundManager.FunctionPlayKoinBertambah();
            }
        }

        public void FunctionSalahBuangSampah()
        {
            if (boolGameBerakhir) return;

            floatWaktuSekarang -= 2f;
            if (floatWaktuSekarang < 0f) floatWaktuSekarang = 0f;
            uiManager.FunctionTampilTeksSalah();
            uiManager.FunctionUpdateWaktu(floatWaktuSekarang);

            // 🔊 Mainkan efek suara salah
            if (soundManager != null)
            {
                soundManager.FunctionPlaySampahSalah();
            }

            if (floatWaktuSekarang <= 0f)
            {
                FunctionGameOver();
            }
        }

        protected void FunctionGameOver()
        {
            if (boolGameBerakhir) return;
            boolGameBerakhir = true;
            uiManager.FunctionTampilPanelGameOver(intKoin);

            // 🔊 Mainkan efek suara kalah
            if (soundManager != null)
            {
                soundManager.FunctionPlayKalah();
            }
        }
        protected void FunctionFinish()
        {
            if (boolGameBerakhir) return;
            boolGameBerakhir = true;
            uiManager.FunctionTampilPanelFinish(intKoin);

            // 🔊 Mainkan efek suara menang
            if (soundManager != null)
            {
                soundManager.FunctionPlayMenang();
            }
        }
    }
}
