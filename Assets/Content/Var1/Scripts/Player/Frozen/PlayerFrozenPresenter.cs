using Content.Scripts.Utilities;
using UnityEngine;

namespace Content.Scripts.Player.Frozen
{
    public class PlayerFrozenPresenter : IPresenter
    {
        private readonly GameModel _gameModel;
        private readonly PlayerModel _model;
        private readonly PlayerView _view;
        private IUpdater _updater;

        public PlayerFrozenPresenter(GameModel gameModel, PlayerModel model, PlayerView view)
        {
            _gameModel = gameModel;
            _model = model;
            _view = view;
        }
        
        public void Init()
        {
            _view.OnColdZoneEntered += HandleColdZoneEnter;
            _model.OnFrozen += HandleFrozen;
            
            _updater = new PlayerFrozenUpdater(_model, _view);
            _gameModel.UpdatersList.Add(_updater);
        }

        public void Dispose()
        {
            _gameModel.UpdatersList.Remove(_updater);
            
            _view.OnColdZoneEntered -= HandleColdZoneEnter;
            _model.OnFrozen -= HandleFrozen;
        }

        private void HandleFrozen()
        {
            
        }

        private void HandleColdZoneEnter(bool state)
        {
            _model.IsInColdZone = state;
            Debug.Log(state);
        }
    }
}