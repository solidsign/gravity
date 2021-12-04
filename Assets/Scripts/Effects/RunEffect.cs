using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class RunEffect : Effect
    {
        [SerializeField] private float newFieldOfView;
        [SerializeField] private float toggleOnTime;
        [SerializeField] private float toggleOffTime;
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
            _cam.DOFieldOfView(newFieldOfView, toggleOnTime)
                .SetAutoKill()
                .SetEase(Ease.InOutSine);
        }

        public override void ToggleOff()
        {
            _cam.DOKill();
            _cam.DOFieldOfView(_defaultFieldOfView, toggleOffTime)
                .SetAutoKill()
                .SetEase(Ease.InSine);
        }
    }
}