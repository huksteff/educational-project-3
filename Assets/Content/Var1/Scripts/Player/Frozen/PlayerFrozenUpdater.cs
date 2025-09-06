using Content.Scripts.Utilities;

namespace Content.Scripts.Player.Frozen
{
    public class PlayerFrozenUpdater : IUpdater
    {
        private readonly PlayerModel _model;
        private readonly PlayerView _view;

        public PlayerFrozenUpdater(PlayerModel model, PlayerView view)
        {
            _view = view;
            _model = model;
        }
        
        public void Update(float deltaTime)
        {
            var frozenStep = deltaTime * 10;
            
            if (!_model.IsInColdZone)
            {
                if (_model.FrozenStrength > 0)
                {
                    _model.DecreaseFrozenStrength(frozenStep);
                    SetupView(_model.FrozenStrength/100);
                }
                else
                {
                    if (_view.FreezingScreenRF.isActive)
                    {
                        _view.FreezingScreenRF.SetActive(false);
                    }
                }
            }
            else
            {
                _view.FreezingScreenRF.SetActive(true);
                _model.IncreaseFrozenStrength(frozenStep);
                SetupView(_model.FrozenStrength/100);
            }
        }

        public void SetupView(float frozenStep)
        {
            _view.FrozenScreenMaterial.SetFloat("_VignetteScale", frozenStep);
        }
    }
}