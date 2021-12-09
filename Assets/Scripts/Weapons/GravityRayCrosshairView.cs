using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class GravityRayCrosshairView : WeaponColorSetter
    {
        [SerializeField] private RectTransform rTransform;
        [SerializeField] private Image image;
        [Range(0f,1f)] [SerializeField] private float toZeroTime;
        [SerializeField] private float showHideTime;

        private Tween _showHide;
        private bool _shown = true;
        private Camera _cam;
        private Vector2 _defaultSize;

        private void Awake()
        {
            _cam = Camera.main;
            _defaultSize = rTransform.sizeDelta;
        }

        public void Reload(float reloadTime, GravityState newState)
        {
            image.DOKill();
            var zeroTime = reloadTime * toZeroTime;
            image.DOColor(ToColor(GravityState.None), zeroTime);
            image.DOFillAmount(0f, zeroTime)
                .OnComplete(() =>
                {
                    var duration = reloadTime - zeroTime;
                    image.DOFillAmount(1f, duration);
                    image.DOColor(ToColor(newState), duration);
                });
        }

        public void Hide()
        {
            if (!_shown) return;
            _showHide?.Kill();
            _showHide =  image.DOFade(0f, showHideTime);
            _shown = false;
        }

        public void Show()
        {
            if (_shown) return;
            _showHide?.Kill();
            _showHide =  image.DOFade(1f, showHideTime);
            _shown = true;
        }

        public void SetPosition(Vector3 worldPosition)
        {
            var distance = Vector3.Distance(worldPosition, _cam.transform.position);
            rTransform.position = Vector3.Lerp(rTransform.position, _cam.WorldToScreenPoint(worldPosition), 0.5f);
            rTransform.sizeDelta = Vector2.Lerp(rTransform.sizeDelta, (10f / distance) * _defaultSize, 0.5f);
        }
    }
}