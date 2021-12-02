using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class RunEffect : Effect
    {
        [SerializeField] private float newFieldOfView;
        [SerializeField] private float toggleTime;
        private Camera _cam;
        private float _defaultFieldOfView;

        private void Awake()
        {
            _cam = Camera.main;
            _defaultFieldOfView = _cam.fieldOfView;
        }

        public override void ToggleOn()
        {
            _cam.DOKill();
            _cam.DOFieldOfView(newFieldOfView, toggleTime)
                .SetAutoKill()
                .SetEase(Ease.InOutSine);
        }

        public override void ToggleOff()
        {
            _cam.DOKill();
            _cam.DOFieldOfView(_defaultFieldOfView, toggleTime)
                .SetAutoKill()
                .SetEase(Ease.InOutSine);
        }
    }
}