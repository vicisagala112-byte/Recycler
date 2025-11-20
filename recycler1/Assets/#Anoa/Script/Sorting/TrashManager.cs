using System.Collections.Generic;
using UnityEngine;


public class TrashManager : MonoBehaviour
{
    public GameObject[] arrPrefabSampahSorting; // 24 prefab

    private List<GameObject> listSampahSorting = new List<GameObject>();

    [Header("Area Spawn Acak")]
    [SerializeField] private float minX = -3f;
    [SerializeField] private float maxX = 3f;
    [SerializeField] private float minY = -2f;
    [SerializeField] private float maxY = 2f;

    private void Start()
    {
        SpawnTrashByCollectedID();
    }

    private void SpawnTrashByCollectedID()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        listSampahSorting.Clear();

        foreach (int id in CollectedTrashData.listTrashID)
        {
            if (id < 0 || id >= arrPrefabSampahSorting.Length)
            {
                Debug.LogError("ID sampah tidak valid: " + id);
                continue;
            }

            GameObject prefab = arrPrefabSampahSorting[id];

            // 🎯 POSISI RANDOM PAKAI MIN/MAX
            Vector3 posisiSpawn =
                new Vector3(
                    Random.Range(minX, maxX),
                    Random.Range(minY, maxY),
                    0f
                );

            // Jadi relatif terhadap posisi parent
            posisiSpawn += transform.position;

            GameObject obj = Instantiate(prefab, posisiSpawn, Quaternion.identity, transform);
            listSampahSorting.Add(obj);
        }
    }
    public bool IsAllCleared()
    {
        return listSampahSorting.TrueForAll(o => o == null);
    }

    public void FunctionOnTrashProcessed(GameObject sampah)
    {
        if (listSampahSorting.Contains(sampah))
            listSampahSorting.Remove(sampah);
    }
}
