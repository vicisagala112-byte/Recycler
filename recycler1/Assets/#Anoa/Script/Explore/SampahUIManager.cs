using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Anoa.Explore
{
    public class SampahUIManager : MonoBehaviour
    {
        [Header("Referensi UI")]
        [SerializeField] protected Image imgIconTongSampah;
        [SerializeField] protected TMP_Text textJumlah;

        [Header("Kapasitas Tong")]
        [SerializeField] protected int intKapasitasMaksimal = 10;

        protected int intJumlahSampah = 0;

        protected void Start()
        {
            FunctionUpdateUI();
        }

        public bool FunctionTambahSampah()
        {
            if (intJumlahSampah >= intKapasitasMaksimal)
                return false;

            intJumlahSampah++;
            FunctionUpdateUI();
            return true;
        }

        public bool FunctionTongPenuh()
        {
            return intJumlahSampah >= intKapasitasMaksimal;
        }

        protected void FunctionUpdateUI()
        {
            if (textJumlah != null)
                textJumlah.text = $"{intJumlahSampah}/{intKapasitasMaksimal}";
        }
    }
}
