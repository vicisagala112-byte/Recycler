using UnityEngine;
using TMPro;
using UnityEngine.UI;
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
        [SerializeField] protected TMP_Text textScore; // tampil selama bermain
        [SerializeField] protected TMP_Text textScoreGameOver;
        [SerializeField] protected TMP_Text textScoreFinish;

        [Header("UI Tambahan")]
        [SerializeField] protected Slider sliderWaktu;

        [Header("Info Durations")]
        [SerializeField] protected float floatInfoDisplayTime = 1f;

        protected Coroutine corInfo;
        protected float waktuAwal;

        private void Start()
        {
            panelGameOver.SetActive(false);
            panelFinish.SetActive(false);
            textInfo.text = "";

            waktuAwal = sliderWaktu != null ? sliderWaktu.maxValue : 60f;

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

            if (sliderWaktu != null)
                sliderWaktu.value = _waktu;
        }

        public void FunctionUpdateScore(int _score)
        {
            textScore.text = _score.ToString(); // tampilkan koin saat bermain
        }

        public void FunctionTampilPanelGameOver(int _totalKoin)
        {
            panelGameOver.SetActive(true);
            textScoreGameOver.text = _totalKoin.ToString();
        }

        public void FunctionTampilPanelFinish(int _totalKoin)
        {
            panelFinish.SetActive(true);
            textScoreFinish.text = _totalKoin.ToString();
        }
    }
}
