using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace Anoa.Explore
{
    public class SampahUIManager : MonoBehaviour
    {
        public static SampahUIManager instance;

        [Header("Referensi UI")]
        [SerializeField] protected Image imgIconTongSampah;
        [SerializeField] protected TMP_Text textJumlah;

        [Header("Kapasitas Tong")]
        [SerializeField] protected int intKapasitasMaksimal = 10;
        protected int intJumlahSampah = 0;
        public bool IsMembawaSampah => intJumlahSampah > 0;

        private void Awake()
        {
            instance = this;
            // ✅ Pastikan reset sejak awal scene dimulai
            intJumlahSampah = 0;
        }

        protected void Start()
        {
            // ✅ Pastikan UI menampilkan nol saat awal main
            FunctionUpdateUI();
        }

        // 🔹 Tambah sampah ke tong (kembalikan true kalau berhasil)
        public bool FunctionTambahSampah()
        {
            if (intJumlahSampah >= intKapasitasMaksimal)
                return false;

            intJumlahSampah++;
            FunctionUpdateUI();
            return true;
        }

        // 🔹 Mengecek apakah tong penuh
        public bool FunctionTongPenuh()
        {
            return intJumlahSampah >= intKapasitasMaksimal;
        }

        // 🔹 Update teks UI jumlah sampah
        protected void FunctionUpdateUI()
        {
            if (textJumlah != null)
                textJumlah.text = $"{intJumlahSampah}/{intKapasitasMaksimal}";
        }

        // 🔹 Reset jumlah sampah (misal setelah dibuang)
        public void FunctionResetTong()
        {
            intJumlahSampah = 0;
            FunctionUpdateUI();
        }
        public void FunctionButtonSampahClicked()
        {
            if (FunctionTongPenuh())
            {
                SceneManager.LoadScene("Sorting");
            }
            else
            {
                Debug.Log("❗Sampah belum cukup untuk pindah ke scene sorting!");
            }
        }

    }

}
