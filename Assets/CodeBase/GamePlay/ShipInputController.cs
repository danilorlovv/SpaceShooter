using UnityEngine;
using Common;

namespace SpaceShooter
{
    public class ShipInputController : MonoBehaviour
    {
        public enum ControlMode
        {
            Keyboard,
            Mobile
        }

        [SerializeField] private ControlMode m_ControlMode;

        private VirtualGamePad m_VirtualGamePad;

        public void Construct(VirtualGamePad virtualGamePad)
        {
            m_VirtualGamePad = virtualGamePad;
        }

        private SpaceShip m_TargetShip;
        public void SetTarget(SpaceShip ship) => m_TargetShip = ship;

        private void Start()
        {
            if (m_ControlMode == ControlMode.Mobile)
            {
                m_VirtualGamePad.VirtualJoystick.gameObject.SetActive(true);
                m_VirtualGamePad.MobileFirePrimary.gameObject.SetActive(true);
                m_VirtualGamePad.MobileFireSecondary.gameObject.SetActive(true);
            } 
            else
            {
                m_VirtualGamePad.VirtualJoystick.gameObject.SetActive(false);
                m_VirtualGamePad.MobileFirePrimary.gameObject.SetActive(false);
                m_VirtualGamePad.MobileFireSecondary.gameObject.SetActive(false);
            }          
        }

        private void Update()
        {
            if (m_TargetShip == null) return;

            if (m_ControlMode == ControlMode.Mobile) ControlMobile();
            if (m_ControlMode == ControlMode.Keyboard) ControlKeyboard();
        }

        private void ControlMobile()
        {
            Vector3 dir = m_VirtualGamePad.VirtualJoystick.Value;

            var dot = Vector2.Dot(dir, m_TargetShip.transform.up);
            var dot2 = Vector2.Dot(dir, m_TargetShip.transform.right);

            if (m_VirtualGamePad.MobileFirePrimary.IsHold) m_TargetShip.Fire(TurretMode.Primary);
            if (m_VirtualGamePad.MobileFireSecondary.IsHold) m_TargetShip.Fire(TurretMode.Secondary);

            m_TargetShip.ThrustControl = Mathf.Max(0, dot);
            m_TargetShip.TorqueControl = -dot2;
        }

        private void ControlKeyboard()
        {
            float thrust = 0.0f;
            float torque = 0.0f;

            if (Input.GetKey(KeyCode.UpArrow)) thrust = 1.0f;
            if (Input.GetKey(KeyCode.DownArrow)) thrust = -1.0f;
            if (Input.GetKey(KeyCode.LeftArrow)) torque = 1.0f;
            if (Input.GetKey(KeyCode.RightArrow)) torque = -1.0f;

            if (Input.GetKey(KeyCode.Space)) m_TargetShip.Fire(TurretMode.Primary);
            if (Input.GetKey(KeyCode.X)) m_TargetShip.Fire(TurretMode.Secondary);

            m_TargetShip.ThrustControl = thrust;
            m_TargetShip.TorqueControl = torque;
        }
    }
}