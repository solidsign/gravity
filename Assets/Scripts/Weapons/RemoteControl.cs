using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class RemoteControl : Weapon{
        [Header("How is it viewed")] 
        [SerializeField] private RemoteControlColorSetter colorSetter;
        [SerializeField] private PlayableEffect shootEffect;
        [Header("Target")]
        [SerializeField] private GravityController gravityController;
        [Header("Allowed Gravity states cycle")]
        [SerializeField] private List<GravityState> statesOrder;

        [SerializeField] private float reloadTime;
        private bool _loaded = true;
        private int _currentState = 0;

        private void Awake()
        {
            colorSetter.Set(statesOrder[0]);
        }

        public override void Shoot()
        {
            if (!_loaded) return;
            gravityController.SetGravity(statesOrder[_currentState]);
            shootEffect?.Play();
            ++_currentState;
            _currentState %= statesOrder.Count;
            colorSetter.Set(GravityState.None);
            StartCoroutine(Reload());
        }

        private IEnumerator Reload()
        {
            _loaded = false;
            yield return new WaitForSeconds(reloadTime);
            colorSetter.Set(statesOrder[_currentState]);
            _loaded = true;
        }
    }
}