using UnityEngine;
using Random = UnityEngine.Random;

namespace Content.Scripts.SnowSimulation
{
    public class NewBehaviourScript : MonoBehaviour
    {
        public GameObject CloudPrefab;
        public Vector3 InitialPosition = new Vector3(0, _maxHeight, 0);
        public float CloudMoveSpeed;
        public float MinReachDistance = 2f;
        [SerializeField] private int _maxHorizontalDistance = 100;
        [SerializeField] private int _maxForwardDistance = 100;
        private const int _maxHeight = 16;
    
        private Vector3 _flyArea;

        private float _timer = 0;
        private Vector3 _targetPosition;
        private const float _maxTime = 20f;
        private bool _isReachedTarget;
    
        private void Start()
        {
            _flyArea = new Vector3(_maxHorizontalDistance, _maxHeight, _maxForwardDistance);
            CloudPrefab.transform.position = InitialPosition;
            _targetPosition = Vector3.zero;
            SelectNewTargetPosition();
        }

        void Update()
        {
            _timer += Time.deltaTime;
            if (_timer > _maxTime)
            {
                SelectNewTargetPosition();
                _timer = 0;
            }
            else
            {
                if (_targetPosition != Vector3.zero && !_isReachedTarget)
                {
                    MoveToTargetPosition();
                }
                else
                {
                    SelectNewTargetPosition();
                    _timer = 0;
                    _isReachedTarget = false;
                }
            }
        }

        private void SelectNewTargetPosition()
        {
            var targetX = Random.Range(-_flyArea.x, _flyArea.x);
            var targetZ = Random.Range(-_flyArea.x, _flyArea.z);
            _targetPosition = new Vector3(targetX, _maxHeight, targetZ);
        }

        private void MoveToTargetPosition()
        {
            CloudPrefab.transform.position = Vector3.Lerp(CloudPrefab.transform.position, _targetPosition, Time.deltaTime * CloudMoveSpeed);
            if (Vector3.Distance(CloudPrefab.transform.position, _targetPosition) < MinReachDistance)
            {
                _isReachedTarget = true;
            }
        }
    }
}
