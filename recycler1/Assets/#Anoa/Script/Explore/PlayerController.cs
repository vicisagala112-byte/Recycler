using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Anoa.Player
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController instance;

        [Header("Movement Settings")]
        [SerializeField] protected float floatMoveSpeed = 5f;
        [SerializeField] protected Joystick joystick;
        protected Rigidbody2D rb;
        protected Animator anim;
        protected Vector2 vecMovement;

        [Header("Boat Settings")]
        [SerializeField] protected bool boolIsOnBoat = false;
        [SerializeField] protected GameObject objBoat;

        [Header("Magnet Sampah")]
        public float MagnetRange = 10f;

        [Header("Bawa Sampah Settings (Non-Prefab)")]
        public Transform sampahHolder;              // Posisi tangan player
        public SpriteRenderer spriteSampahTangan;   // SpriteRenderer di tangan
        public List<Sprite> listSpriteSampah;       // Daftar sprite sampah (apel, plastik, tulang, dll)
        private Coroutine coroutineBawaSampah;

        private void Awake()
        {
            instance = this;
        }

        protected void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();

            if (objBoat != null)
                objBoat.SetActive(false);

            // Awalnya sembunyikan sprite di tangan
            if (spriteSampahTangan != null)
                spriteSampahTangan.enabled = false;
        }

        protected void Update()
        {
            // 🔹 Input pergerakan
            float _x = joystick ? joystick.Horizontal : Input.GetAxisRaw("Horizontal");
            float _y = joystick ? joystick.Vertical : Input.GetAxisRaw("Vertical");

            vecMovement = new Vector2(_x, _y).normalized;
            bool _isMoving = vecMovement.magnitude > 0.1f;

            anim.SetBool("isMoving", _isMoving);

            if (_isMoving)
            {
                anim.SetFloat("moveX", vecMovement.x);
                anim.SetFloat("moveY", vecMovement.y);
            }

            // 🔹 Balik arah sprite sampah di tangan (biar sesuai arah player)
            if (spriteSampahTangan != null)
            {
                bool isFacingLeft = anim.GetFloat("moveX") < 0;
                spriteSampahTangan.flipX = isFacingLeft;
            }
        }

        protected void FixedUpdate()
        {
            rb.MovePosition(rb.position + vecMovement * floatMoveSpeed * Time.fixedDeltaTime);
        }

        // 🔹 Dipanggil saat player mengambil sampah
        public void FunctionTriggerBawaSampah(int idSampah)
        {
            if (coroutineBawaSampah != null)
                StopCoroutine(coroutineBawaSampah);

            coroutineBawaSampah = StartCoroutine(CoBawaSampah(idSampah));
        }

        // 🔹 Coroutine untuk animasi dan menampilkan sprite sampah
        private IEnumerator CoBawaSampah(int idSampah)
        {
            anim.SetBool("BawaSampah", true);

            // 🔹 Tampilkan sprite sampah sesuai ID
            if (spriteSampahTangan != null && idSampah >= 0 && idSampah < listSpriteSampah.Count)
            {
                spriteSampahTangan.sprite = listSpriteSampah[idSampah];
                spriteSampahTangan.enabled = true;
            }

            // ⏳ Tunggu durasi animasi (2 detik, bisa kamu sesuaikan)
            yield return new WaitForSeconds(2f);

            anim.SetBool("BawaSampah", false);

            // 🔹 Sembunyikan sprite setelah animasi selesai
            if (spriteSampahTangan != null)
            {
                spriteSampahTangan.sprite = null;
                spriteSampahTangan.enabled = false;
            }
        }

        protected void OnTriggerEnter2D(Collider2D _col)
        {
            if (_col.CompareTag("Sungai"))
            {
                boolIsOnBoat = true;
                if (objBoat != null)
                    objBoat.SetActive(true);
            }
        }

        protected void OnTriggerExit2D(Collider2D _col)
        {
            if (_col.CompareTag("Sungai"))
            {
                boolIsOnBoat = false;
                if (objBoat != null)
                    objBoat.SetActive(false);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, MagnetRange);
        }
    }
}
