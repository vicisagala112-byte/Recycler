using System.Collections.Generic;
using UnityEngine;

public class NpcMovement : MonoBehaviour
{
    [Header("Waypoint Parent")]
    [SerializeField] Transform parentWaypoints;

    [Header("Pengaturan Gerak")]
    public float kecepatanGerak = 2f;
    public float waktuDiam = 1f;

    private List<Transform> waypoints = new List<Transform>();

    private int index = 0;     // waypoint sekarang
    private int arah = 1;      // 1 = maju, -1 = mundur
    private float timer = 0;

    private bool menunggu = false;

    Animator anim;

    void Start()
    {
        foreach (Transform t in parentWaypoints)
            waypoints.Add(t);

        transform.position = waypoints[0].position;

        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (menunggu)
        {
            timer += Time.deltaTime;
            if (timer >= waktuDiam)
            {
                menunggu = false;
                timer = 0;
            }
            else
            {
                anim.SetBool("isMoving", false);
                return;
            }
        }

        Bergerak();
    }

    void Bergerak()
    {
        Vector2 target = waypoints[index].position;
        Vector2 selisih = target - (Vector2)transform.position;

        // sudah sampai?
        if (selisih.magnitude < 0.05f)
        {
            menunggu = true;
            timer = 0;

            index += arah;

            if (index >= waypoints.Count)
            {
                index = waypoints.Count - 1;
                arah = -1;
            }
            else if (index < 0)
            {
                index = 0;
                arah = 1;
            }

            return;
        }

        // gerakan lurus ke waypoint (tidak zigzag)
        Vector2 move = selisih.normalized;

        transform.position += (Vector3)move * kecepatanGerak * Time.deltaTime;

        // --- Animasi arah ---
        Vector2 arahAnim = new Vector2(Mathf.Round(move.x), Mathf.Round(move.y));

        anim.SetFloat("moveX", arahAnim.x);
        anim.SetFloat("moveY", arahAnim.y);
        anim.SetBool("isMoving", true);
    }
}
