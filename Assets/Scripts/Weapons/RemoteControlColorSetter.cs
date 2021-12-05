﻿using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public sealed class RemoteControlColorSetter : WeaponColorSetter
    {
        [SerializeField] private MeshRenderer button;
        [SerializeField] private float changeTime;

        private Material _mat;
        private int i;
        private void Awake()
        {
            _mat = button.sharedMaterial;
        }

        public override void Set(GravityState gravityState)
        {
            _mat.DOKill();
            _mat.DOColor(ToColor(gravityState), changeTime);
        }
    }
}