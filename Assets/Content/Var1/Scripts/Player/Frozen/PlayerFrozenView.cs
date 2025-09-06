using System;
using Content.Scripts.Utilities;
using UnityEngine;

namespace Content.Scripts.Player.Frozen
{
    public class PlayerFrozenView : MonoBehaviour
    {
        public event Action<bool> OnColdZoneEntered;

        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(ZoneTags.Cold))
            {
                OnColdZoneEntered?.Invoke(true);
            }
        
        }
    
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(ZoneTags.Cold))
            {
                OnColdZoneEntered?.Invoke(false);
            
            }
        
            Debug.Log("Exit");
        }
    }
}