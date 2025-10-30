using System.Collections.Generic;
using UnityEngine;

namespace Anoa
{
    public class PoolModule : MonoBehaviour
    {
        [SerializeField] protected List<GameObject> listObjPrefab = new List<GameObject>();
        [SerializeField] protected int intPoolPerPrefab = 3;

        protected Dictionary<GameObject, List<GameObject>> dictPool = new Dictionary<GameObject, List<GameObject>>();

        private void Awake()
        {
            foreach (GameObject _prefab in listObjPrefab)
            {
                List<GameObject> _pool = new List<GameObject>();
                for (int i = 0; i < intPoolPerPrefab; i++)
                {
                    GameObject _obj = Instantiate(_prefab, transform);
                    _obj.SetActive(false);
                    _pool.Add(_obj);
                }
                dictPool[_prefab] = _pool;
            }
        }

        public List<GameObject> GetListObjPrefabs() => listObjPrefab;

        public GameObject FunctionGetFromPool(GameObject _prefab)
        {
            if (!dictPool.ContainsKey(_prefab)) return null;

            foreach (GameObject _obj in dictPool[_prefab])
            {
                if (!_obj.activeSelf)
                {
                    _obj.SetActive(true);
                    return _obj;
                }
            }
            return null;
        }

        public void FunctionReturnToPool(GameObject _obj)
        {
            _obj.SetActive(false);
            _obj.transform.SetParent(transform);
        }

        public bool HasAvailableObject()
        {
            foreach (var kvp in dictPool)
            {
                foreach (GameObject obj in kvp.Value)
                {
                    if (!obj.activeSelf)
                        return true;
                }
            }
            return false;
        }
    }
}
