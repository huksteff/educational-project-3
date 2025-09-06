using UnityEngine;
using UnityEngine.VFX;

namespace Content.Scripts.SnowSimulation
{
    public class PlayerSnowDeform : MonoBehaviour
    {
        public GameObject PlayerCenter;
        public VisualEffect PlayerSnowEffect;
        private float _playerSnowHeight;
        [SerializeField] private float _rayMaxDistance = 1f;
        [SerializeField] private bool _lineRenderer = false;
        public LineRenderer LineRenderer;
        private float _lineWidth = 0.1f;
        private float _lineMaxLength = 5f;
    
        void Start()
        {
            Vector3[] initLinePosition = new Vector3[2] { Vector3.zero, Vector3.zero };
            LineRenderer.SetPositions( initLinePosition );
            LineRenderer.SetWidth( _lineWidth, _lineWidth );   
        }

        void Update()
        {
            var getWorldPos = PlayerCenter.transform.position;
            var endPosition = Vector3.zero;
            var distance = Vector3.Distance(getWorldPos, endPosition);
            CheckDistance(getWorldPos, endPosition, distance);
        }

        private void CheckDistance(Vector3 worldPos, Vector3 endPosition, float distance)
        {
            RaycastHit hit;
        
            if (Physics.Raycast(worldPos, transform.TransformDirection(Vector3.down), out hit, distance))
            {
                endPosition = hit.point;
                _lineRenderer = true;
                LineRenderer.SetPosition(0, worldPos);
                LineRenderer.SetPosition(1, endPosition);
            }
            else
            {
                _lineRenderer = false;
                LineRenderer.SetPosition(0, worldPos);
                LineRenderer.SetPosition(1, worldPos);
            }
            PlayerSnowEffect.SetFloat("PlayerHeight", distance);

        } 
    }
}
