using DG.Tweening;
using UnityEngine;

namespace Game
{
    public sealed class RemoteControlColorSetter : WeaponColorSetter
    {
        [SerializeField] private MeshRenderer button;
        [SerializeField] private float changeTime;

        private Material _mat;
        private void Awake()
        {
            _mat = button.sharedMaterial;
        }

        public void Set(GravityState gravityState)
        {
            _mat.DOKill();
            _mat.DOColor(ToColor(gravityState), changeTime);
        }
    }
}