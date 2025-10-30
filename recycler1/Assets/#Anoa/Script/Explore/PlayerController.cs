using UnityEngine;
using UnityEngine.InputSystem;

namespace Anoa.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] protected float floatMoveSpeed = 5f;
        [SerializeField] protected Joystick joystick;

        protected Rigidbody2D rb;
        protected Animator anim;
        protected Vector2 vecMovement;

        [Header("Boat Settings")]
        [SerializeField] protected bool boolIsOnBoat = false;
        [SerializeField] protected GameObject objBoat;

        protected void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();

            if (objBoat != null)
                objBoat.SetActive(false);
        }

        protected void Update()
        {
            float _floatMoveX = joystick != null ? joystick.Horizontal : Input.GetAxisRaw("Horizontal");
            float _floatMoveY = joystick != null ? joystick.Vertical : Input.GetAxisRaw("Vertical");

            vecMovement = new Vector2(_floatMoveX, _floatMoveY).normalized;

            bool _boolIsMoving = vecMovement.magnitude > 0;
            anim.SetBool("isMoving", _boolIsMoving);

            if (_boolIsMoving)
            {
                anim.SetFloat("moveX", vecMovement.x);
                anim.SetFloat("moveY", vecMovement.y);
            }
        }

        protected void FixedUpdate()
        {
            rb.MovePosition(rb.position + vecMovement * floatMoveSpeed * Time.fixedDeltaTime);
        }

        protected void OnTriggerEnter2D(Collider2D _col)
        {
            if (_col.CompareTag("Sungai"))
            {
                boolIsOnBoat = true;
                if (objBoat != null)
                    objBoat.SetActive(true);

                Debug.Log("Player naik kapal!");
            }
        }

        protected void OnTriggerExit2D(Collider2D _col)
        {
            if (_col.CompareTag("Sungai"))
            {
                boolIsOnBoat = false;
                if (objBoat != null)
                    objBoat.SetActive(false);

                Debug.Log("Player turun kapal!");
            }
        }
    }
}
