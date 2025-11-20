using UnityEngine;
using System.Collections;

namespace Anoa
{
    public class TrashController : MonoBehaviour
    {
        [Header("Data Sampah")]
        [SerializeField] protected TRASH_TYPE typeSampah;
        [SerializeField] protected bool boolSudahDibuang = false;
        public bool SudahDibuang => boolSudahDibuang;

        [Header("Komponen Animator")]
        [SerializeField] protected Animator animator;

        protected GameManagerSorting gameManagerSorting;
        protected TrashManager trashManager;
        protected Vector3 vec3PosisiAwal;

        private void Awake()
        {
            gameManagerSorting = FindObjectOfType<GameManagerSorting>();
            trashManager = FindObjectOfType<TrashManager>();

            if (animator == null)
                animator = GetComponent<Animator>();
        }

      

        private void Start()
        {
            gameManagerSorting = FindObjectOfType<GameManagerSorting>();
            trashManager = FindObjectOfType<TrashManager>();
            vec3PosisiAwal = transform.position;

            if (animator == null)
                animator = GetComponent<Animator>();
        }

        public TRASH_TYPE GetTypeSampah() => typeSampah;

        private void OnEnable()
        {
            boolSudahDibuang = false;
        }

        private void OnTriggerEnter2D(Collider2D _coll)
        {
            if (boolSudahDibuang) return;

            if (!_coll.CompareTag("TongOrganik") &&
                !_coll.CompareTag("TongAnorganik") &&
                !_coll.CompareTag("TongB3"))
                return;

            bool _benar = false;

            if (_coll.CompareTag("TongOrganik") && typeSampah == TRASH_TYPE.ORGANIK)
                _benar = true;
            else if (_coll.CompareTag("TongAnorganik") && typeSampah == TRASH_TYPE.ANORGANIK)
                _benar = true;
            else if (_coll.CompareTag("TongB3") && typeSampah == TRASH_TYPE.B3)
                _benar = true;

            Animator tongAnimator = _coll.GetComponent<Animator>();

            if (_benar)
            {
                boolSudahDibuang = true;

                // 🔹 Animasi sampah masuk
                if (animator != null)
                    animator.SetTrigger("masuk");

                // 🔹 Animasi tong benar
                if (tongAnimator != null)
                    tongAnimator.SetTrigger("benar");

                gameManagerSorting.FunctionBenarBuangSampah(this);

                StartCoroutine(DisableAfterAnimation());
            }
            else
            {
                // 🔹 Animasi tong salah
                if (tongAnimator != null)
                    tongAnimator.SetTrigger("salah");

                // 🔹 Warna merah sementara
                SpriteRenderer sr = _coll.GetComponent<SpriteRenderer>();
                if (sr != null)
                    StartCoroutine(FlashRed(sr));

                gameManagerSorting.FunctionSalahBuangSampah();

                // 🔹 Balik ke posisi awal
                transform.position = vec3PosisiAwal;
            }
        }

        private IEnumerator FlashRed(SpriteRenderer sr)
        {
            Color originalColor = sr.color;
            sr.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            sr.color = originalColor;
        }

        private IEnumerator DisableAfterAnimation()
        {
            float animDuration = 0.5f;
            if (animator != null)
            {
                AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
                animDuration = state.length;
            }

            yield return new WaitForSeconds(animDuration);

            if (trashManager != null)
                trashManager.FunctionOnTrashProcessed(gameObject);
        }

    }
}
