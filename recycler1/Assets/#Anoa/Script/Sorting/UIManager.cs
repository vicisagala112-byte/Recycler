using UnityEngine;
using TMPro;
using UnityEngine.UI; // <--- penting untuk pakai Slider
using System.Collections;

namespace Anoa
{
    public class UIManager : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] protected GameObject panelGameOver;
        [SerializeField] protected GameObject panelFinish;

        [Header("Texts")]
        [SerializeField] protected TMP_Text textInfo;
        [SerializeField] protected TMP_Text textWaktu;
        [SerializeField] protected TMP_Text textKoin;
        [SerializeField] protected TMP_Text textScoreGameOver;
        [SerializeField] protected TMP_Text textScoreFinish;

        [Header("UI Tambahan")]
        [SerializeField] protected Slider sliderWaktu; // <--- slider waktu baru

        [Header("Info Durations")]
        [SerializeField] protected float floatInfoDisplayTime = 1f;

        protected Coroutine corInfo;
        protected float waktuAwal; // <--- buat patokan total waktu

        private void Start()
        {
            panelGameOver.SetActive(false);
            panelFinish.SetActive(false);
            textInfo.text = "";

            // inisialisasi slider (kalau kamu punya waktu total, bisa ganti manual)
            waktuAwal = 60f; // misal durasi permainan 60 detik
            if (sliderWaktu != null)
            {
                sliderWaktu.maxValue = waktuAwal;
                sliderWaktu.value = waktuAwal;
            }
        }

        public void FunctionTampilTeksBenar() => FunctionShowInfo("Benar!");
        public void FunctionTampilTeksSalah() => FunctionShowInfo("Salah!");

        protected void FunctionShowInfo(string _message)
        {
            if (corInfo != null) StopCoroutine(corInfo);
            corInfo = StartCoroutine(CorRoutineShowInfo(_message));
        }

        protected IEnumerator CorRoutineShowInfo(string _message)
        {
            textInfo.text = _message;
            yield return new WaitForSeconds(floatInfoDisplayTime);
            textInfo.text = "";
            corInfo = null;
        }

        public void FunctionUpdateWaktu(float _waktu)
        {
            int _intWaktu = Mathf.CeilToInt(_waktu);
            textWaktu.text = _intWaktu.ToString();

            // update slider waktu juga
            if (sliderWaktu != null)
            {
                sliderWaktu.value = _waktu;
            }
        }

        public void FunctionUpdateKoin(int _koin)
        {
            textKoin.text = _koin.ToString();
        }

        public void FunctionTampilPanelGameOver(int _score)
        {
            panelGameOver.SetActive(true);
            textScoreGameOver.text = _score.ToString();
        }

        public void FunctionTampilPanelFinish(int _score)
        {
            panelFinish.SetActive(true);
            textScoreFinish.text = _score.ToString();
        }
    }
}
