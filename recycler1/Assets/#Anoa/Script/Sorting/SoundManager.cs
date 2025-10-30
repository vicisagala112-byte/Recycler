using UnityEngine;

namespace Anoa
{
    public class SoundManager : MonoBehaviour
    {
        [Header("Audio Source")]
        [SerializeField] protected AudioSource sourceEffect;

        [Header("Audio Clips")]
        [SerializeField] protected AudioClip clipSampahBenar;
        [SerializeField] protected AudioClip clipSampahSalah;
        [SerializeField] protected AudioClip clipMenang;
        [SerializeField] protected AudioClip clipKalah;
        [SerializeField] protected AudioClip clipKoinBertambah;

        protected void Start()
        {
            // Kalau lupa isi di inspector, otomatis ambil dari GameObject ini
            if (sourceEffect == null)
                sourceEffect = GetComponent<AudioSource>();
        }

        // ===== Fungsi Pemanggil Sound Effect =====
        public void FunctionPlaySampahBenar()
        {
            FunctionPlayClip(clipSampahBenar);
        }

        public void FunctionPlaySampahSalah()
        {
            FunctionPlayClip(clipSampahSalah);
        }

        public void FunctionPlayMenang()
        {
            FunctionPlayClip(clipMenang);
        }

        public void FunctionPlayKalah()
        {
            FunctionPlayClip(clipKalah);
        }

        public void FunctionPlayKoinBertambah()
        {
            FunctionPlayClip(clipKoinBertambah);
        }

        // ===== Fungsi Internal =====
        protected void FunctionPlayClip(AudioClip _clip)
        {
            if (_clip != null && sourceEffect != null)
            {
                sourceEffect.PlayOneShot(_clip);
            }
            else
            {
                Debug.LogWarning("⚠ SoundManager: AudioClip atau AudioSource belum diatur di Inspector!");
            }
        }
    }
}
