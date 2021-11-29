using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(RotationHandler))]
    public class PlayerMouseLook : GravityObserver
    {
        [SerializeField] private float sensitivityX;
        [SerializeField] private float sensitivityY;
        private RotationHandler _rotationHandler;

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _rotationHandler = GetComponent<RotationHandler>();
        }

        private void Update()
        {
            _rotationHandler.RotateHorizontally(Input.GetAxis("Mouse X") * sensitivityX);
            _rotationHandler.RotateVertically(Input.GetAxis("Mouse Y") * sensitivityY);
        }

        public override void GravityChangeStarted(GravityState prevState, GravityState newState, float gravityChangeTime)
        {
            enabled = false;
        }

        public override void GravityChangeFinished()
        {
            enabled = true;
        }

        public override void GravityInit(GravityState gravityState) { }
    }
}