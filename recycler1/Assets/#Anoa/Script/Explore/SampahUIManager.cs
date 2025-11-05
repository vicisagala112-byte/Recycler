using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Anoa.Player;

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

        protected void Start()
        {
            FunctionUpdateUI();
        }

        private void Awake()
        {
            instance = this;
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

            // Cek apakah tong penuh dan ubah animasi player
            if (PlayerController.instance != null && PlayerController.instance.Animator != null)
            {
                bool isFull = intJumlahSampah >= intKapasitasMaksimal;
                PlayerController.instance.Animator.SetBool("isCarryingTrash", isFull);
            }
        }

    }
}
