using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Content.New.Scripts
{
    public class SpawnSpheres : MonoBehaviour
    {
        public GameObject SnowPrefab;
        public float SpawnHeight = 1;
        public bool SpawnEnable;
        
        void Start()
        {
        
        }
        
        void Update()
        {
            if (SpawnEnable)
            {
                var pos = new Vector3(0, SpawnHeight, 0);
                GameObject.Instantiate(SnowPrefab, pos, Quaternion.AngleAxis(0,new Vector3(0, 0, 0)));
            }
        }
    }
}
