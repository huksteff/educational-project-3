using System;
using Content.Scripts.Player.Frozen;
using Content.Scripts.Utilities;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Content.Scripts.Player
{
    public class PlayerView : MonoBehaviour
    {
        public Animator Animator;
        public float MovementSpeed = 5f;
        public float RotationSpeed = 40f;
        public float MoveSmoothTime = 2f;
        public Material FrozenScreenMaterial;
        public ScriptableRendererFeature FreezingScreenRF;
        public PlayerFrozenView FrozenView;
        
        public event Action<bool> OnColdZoneEntered;

        public void OnEnable()
        {
            FrozenView.OnColdZoneEntered += HandleColdZoneEnter;
        }

        public void OnDisable()
        {
            FrozenView.OnColdZoneEntered -= HandleColdZoneEnter;
        }

        private void HandleColdZoneEnter(bool state)
        {
            OnColdZoneEntered?.Invoke(state);
        }
    }
}