using UnityEngine;

namespace Common
{
    public class SyncTransform : MonoBehaviour
    {
        [SerializeField] private Transform m_Target;

        private void FixedUpdate()
        {
            transform.position = new Vector3(m_Target.position.x, m_Target.position.y, transform.position.z);
        }

        public void SetTarget(Transform target)
        {
            m_Target = target;
        }
    }
}