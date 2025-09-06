using Content.Scripts.Input;
using Content.Scripts.Utilities;
using UnityEngine;

namespace Content.Scripts.Player
{
    public class PlayerMovementUpdater : IUpdater
    {
        private readonly PlayerModel _model;
        private readonly PlayerView _view;
        private readonly InputModel _inputModel;
        private Camera _camera;
        private float _currentVelocity;

        public PlayerMovementUpdater(PlayerModel model, PlayerView view, InputModel inputModel)
        {
            _model = model;
            _view = view;
            _inputModel = inputModel;
            _camera = Camera.main;
        }

        public void Update(float deltaTime)
        {
            var direction = new Vector3(_inputModel.Direction.x, 0, _inputModel.Direction.y);
            var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            
            if (direction != Vector3.zero)
            {
                var newPosition = _view.transform.position + (direction * _view.MovementSpeed);
                var angle = Mathf.SmoothDampAngle(_view.transform.eulerAngles.y, targetAngle, ref _currentVelocity,
                    _view.RotationSpeed * deltaTime);

                _view.Animator.SetFloat("Speed", Mathf.Lerp(0, 1, 1f));
                _view.transform.position = Vector3.Lerp(_view.transform.position, newPosition, _view.MoveSmoothTime * deltaTime);
                _view.transform.rotation = Quaternion.Euler(0, angle, 0);
            }
            else
            {
                _view.Animator.SetFloat("Speed", Mathf.Lerp(1, 0, 1f));
            }
        }
    }
}