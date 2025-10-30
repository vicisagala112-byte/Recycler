using UnityEngine;
using UnityEngine.EventSystems;

namespace Anoa
{
    public class DragDropModule : MonoBehaviour
    {
        [Header("Drag Settings")]
        [SerializeField] protected bool boolIsDraggable = true;

        private bool boolIsDragging = false;
        private Vector3 offset;
        private Camera mainCamera;
        private int originalSortingOrder;
        private SpriteRenderer spriteRenderer;

        private Vector3 startPosition; // posisi awal sebelum di-drag
        private TrashController trashController; // referensi ke script sampah

        private void Start()
        {
            mainCamera = Camera.main;
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
                originalSortingOrder = spriteRenderer.sortingOrder;

            startPosition = transform.position; // simpan posisi awal
            trashController = GetComponent<TrashController>();
        }

        private void OnMouseDown()
        {
            if (!boolIsDraggable) return;

            // jangan drag kalau sedang di atas UI
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            boolIsDragging = true;
            offset = transform.position - GetMouseWorldPos();

            // bawa ke depan agar tidak ketimpa objek lain
            if (spriteRenderer != null)
                spriteRenderer.sortingOrder = 1000;
        }

        private void OnMouseDrag()
        {
            if (!boolIsDragging || !boolIsDraggable) return;

            transform.position = GetMouseWorldPos() + offset;
        }

        private void OnMouseUp()
        {
            if (!boolIsDragging) return;

            boolIsDragging = false;

            // kembalikan urutan layer sprite
            if (spriteRenderer != null)
                spriteRenderer.sortingOrder = originalSortingOrder;

            // cek apakah sudah dibuang ke tempat yang benar
            if (trashController != null)
            {
                if (!trashController.SudahDibuang)
                {
                    // jika belum dibuang atau salah tempat -> kembali ke posisi awal
                    transform.position = startPosition;
                }
            }
            else
            {
                // kalau tidak punya TrashController, tetap kembalikan saja
                transform.position = startPosition;
            }
        }

        private Vector3 GetMouseWorldPos()
        {
            Vector3 mousePoint = Input.mousePosition;
            mousePoint.z = 10f; // jarak kamera ke object
            return mainCamera.ScreenToWorldPoint(mousePoint);
        }

        public void SetDraggable(bool _state)
        {
            boolIsDraggable = _state;
        }
    }
}
