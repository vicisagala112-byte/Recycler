using System.Collections.Generic;
using UnityEngine;

namespace Anoa
{
    public class TrashManager : MonoBehaviour
    {
        [Header("Spawn Prefab Pool")]
        [SerializeField] protected PoolModule poolModule;

        [Header("Spawn Points")]
        [SerializeField] protected Transform[] arrTransformLokasiSpawn;

        [Header("Pengaturan")]
        [SerializeField] protected int intMaxSampahAktif = 10;

        protected List<GameObject> listObjAktif = new List<GameObject>();
        protected List<Transform> listSpawnTerpakai = new List<Transform>();

        private void Start()
        {
            FunctionFillToMax();
        }

        public void FunctionFillToMax()
        {
            int jumlahKurang = intMaxSampahAktif - listObjAktif.Count;
            for (int i = 0; i < jumlahKurang; i++)
            {
                FunctionSpawnOne();
            }
        }

        public bool IsAllCleared()
        {
            return listObjAktif == null || listObjAktif.Count == 0;
        }

        protected void FunctionSpawnOne()
        {
            if (poolModule == null || arrTransformLokasiSpawn.Length == 0) return;

            List<GameObject> listPrefab = poolModule.GetListObjPrefabs();
            if (listPrefab.Count == 0) return;

            GameObject _prefab = listPrefab[Random.Range(0, listPrefab.Count)];
            GameObject _obj = poolModule.FunctionGetFromPool(_prefab);
            if (_obj == null) return;

            Transform _spawn = FunctionGetSpawnKosong();
            if (_spawn == null)
            {
                poolModule.FunctionReturnToPool(_obj);
                return;
            }

            _obj.transform.position = _spawn.position;
            _obj.transform.rotation = Quaternion.identity;
            _obj.transform.SetParent(null);

            listObjAktif.Add(_obj);
            listSpawnTerpakai.Add(_spawn);
        }

        protected Transform FunctionGetSpawnKosong()
        {
            List<Transform> _tersedia = new List<Transform>();
            foreach (Transform _t in arrTransformLokasiSpawn)
            {
                if (!listSpawnTerpakai.Contains(_t))
                    _tersedia.Add(_t);
            }

            if (_tersedia.Count == 0) return null;
            return _tersedia[Random.Range(0, _tersedia.Count)];
        }

        public void FunctionOnTrashProcessed(GameObject _obj)
        {
            if (listObjAktif.Contains(_obj))
                listObjAktif.Remove(_obj);

            poolModule.FunctionReturnToPool(_obj);

            // hapus spawn point yang dipakai
            foreach (Transform _t in arrTransformLokasiSpawn)
            {
                if (Vector2.Distance(_t.position, _obj.transform.position) < 0.1f)
                {
                    listSpawnTerpakai.Remove(_t);
                    break;
                }
            }

            // Spawn ulang hanya jika belum capai batas total pool
            if (listObjAktif.Count < intMaxSampahAktif && poolModule.HasAvailableObject())
                FunctionFillToMax();
        }
    }
}
