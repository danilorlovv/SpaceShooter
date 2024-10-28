using UnityEngine;
using Common;

namespace SpaceShooter
{
    [RequireComponent(typeof(SpaceShip))]
    public class AIController : MonoBehaviour
    {
        public enum AIBehaviour
        {
            Null,
            Patrol
        }

        public enum PatrolBehaviour
        {
            Random,
            Trail
        }

        [SerializeField] private AIBehaviour m_AIBehaviour;

        [SerializeField] private PatrolBehaviour m_PatrolBehaviour;

        [SerializeField] private Transform[] m_TrailDots;

        [SerializeField] private float m_DotsOffset;

        [SerializeField] private AIPointPatrol m_PointPatrol;

        [Range(0.0f, 1.0f)]
        [SerializeField] private float m_NavigationLinear;

        [Range(0.0f, 1.0f)]
        [SerializeField] private float m_NavigationAngular;

        [SerializeField] private float m_RandomSelectMovePointTime;

        [SerializeField] private float m_FindNewTargetTime;

        [SerializeField] private float m_ShootDelay;

        [SerializeField] private float m_EvadeRayLength;

        private SpaceShip m_SpaceShip;

        private Vector3 m_MovePosition;

        private Destructible m_SelectedTarget;

        private static int CurrentDot = 0;

        private Timer m_RandomizeDirectionTimer;
        private Timer m_FindNewTargetTimer;
        private Timer m_FireTimer;


        private void Start()
        {
            m_SpaceShip = GetComponent<SpaceShip>();

            InitTimers();
        }

        private void Update()
        {
            UpdateTimers();

            UpdateAI();
        }

        #region Timers

        private void InitTimers()
        {
            m_RandomizeDirectionTimer = new Timer(m_RandomSelectMovePointTime);
            m_FindNewTargetTimer = new Timer(m_FindNewTargetTime);
            m_FireTimer = new Timer(m_ShootDelay);
        }

        private void UpdateTimers()
        {
            m_RandomizeDirectionTimer.RemoveTime(Time.deltaTime);
            m_FindNewTargetTimer.RemoveTime(Time.deltaTime);
            m_FireTimer.RemoveTime(Time.deltaTime);
        }

        #endregion

        private void UpdateAI()
        {
            if (m_AIBehaviour == AIBehaviour.Null)
            {
                return;
            }

            if (m_AIBehaviour == AIBehaviour.Patrol)
            {
                UpdateBehaviourPatrol();
            }
        }

        private void UpdateBehaviourPatrol()
        {
            ActionFindNewMovePosition();
            ActionEvadeCollision();
            ActionControlShip();
            ActionFindNewAttackTarget();
            ActionFire();
        }

        private void ActionFindNewMovePosition()
        {
            if (m_SelectedTarget != null)
            {
                if (m_SelectedTarget.GetComponent<SpaceShip>().ThrustControl == 0)
                    m_MovePosition = m_SelectedTarget.transform.position;
                else
                    m_MovePosition = m_SelectedTarget.transform.position + (m_SelectedTarget.transform.up * 10.0f *
                        m_SelectedTarget.GetComponent<SpaceShip>().ThrustControl);
            }
            else
            {
                if (m_PointPatrol != null)
                {
                    bool isInsidePatrolZone = (m_PointPatrol.transform.position - transform.position).sqrMagnitude < m_PointPatrol.Radius * m_PointPatrol.Radius;

                    if (isInsidePatrolZone)
                    {
                        if (m_PatrolBehaviour == PatrolBehaviour.Random)
                        {
                            if (m_RandomizeDirectionTimer.IsFinished)
                            {
                                Vector2 newPoint = Random.onUnitSphere * m_PointPatrol.Radius + m_PointPatrol.transform.position;

                                m_MovePosition = newPoint;

                                m_RandomizeDirectionTimer.Start(m_RandomSelectMovePointTime);
                            }
                        }
                        else
                        {
                            m_MovePosition = m_TrailDots[CurrentDot].transform.position;

                            if (transform.position.x < m_TrailDots[CurrentDot].transform.position.x + m_DotsOffset &&
                                transform.position.x > m_TrailDots[CurrentDot].transform.position.x - m_DotsOffset &&
                                transform.position.y < m_TrailDots[CurrentDot].transform.position.y + m_DotsOffset &&
                                transform.position.y > m_TrailDots[CurrentDot].transform.position.y - m_DotsOffset)
                            {
                                CurrentDot++;

                                if (CurrentDot == m_TrailDots.Length) CurrentDot = 0;
                            }
                        }
                    }
                    else
                    {
                        m_MovePosition = m_PointPatrol.transform.position;
                    }
                }
            }
        }

        private void ActionEvadeCollision()
        {
            if (Physics2D.Raycast(transform.position, transform.up, m_EvadeRayLength))
            {
                m_MovePosition = transform.position + transform.right * 100.0f;
            }
        }

        private void ActionControlShip()
        {
            m_SpaceShip.ThrustControl = m_NavigationLinear;

            m_SpaceShip.TorqueControl = ComputeAliginTorqueNormalized(m_MovePosition, m_SpaceShip.transform) * m_NavigationAngular;
        }

        private static float ComputeAliginTorqueNormalized(Vector3 targetPosition, Transform ship)
        {
            Vector2 localTargetPosition = ship.InverseTransformPoint(targetPosition);

            float angle = Vector3.SignedAngle(localTargetPosition, Vector3.up, Vector3.forward);

            angle = Mathf.Clamp(angle, -MAX_ANGLE, MAX_ANGLE) / MAX_ANGLE;

            return -angle;
        }

        private static float MAX_ANGLE = 45.0f;

        private void ActionFindNewAttackTarget()
        {
            if (m_FindNewTargetTimer.IsFinished)
            {
                m_SelectedTarget = FindNearestDestructibleTarget();
                
                m_FindNewTargetTimer.Start(m_FindNewTargetTime);
            }
        }

        private Destructible FindNearestDestructibleTarget()
        {
            float minDist = float.MaxValue;

            Destructible potentialTarget = null;

            foreach (var v in Destructible.AllDestructibles)
            {
                if (v.GetComponent<SpaceShip>() == m_SpaceShip) continue;
                if (v.TeamId == Destructible.TeamIdNeutral) continue;
                if (v.TeamId == m_SpaceShip.TeamId) continue;

                float dist = Vector2.Distance(m_SpaceShip.transform.position, v.transform.position);

                if (dist < minDist)
                {
                    minDist = dist;
                    potentialTarget = v;
                }
            }

            return potentialTarget;
        }

        private void ActionFire()
        {
            if (m_SelectedTarget != null)
            {
                if (m_FireTimer.IsFinished)
                {
                    m_SpaceShip.Fire(TurretMode.Primary);

                    m_FireTimer.Start(m_ShootDelay);
                }
            }
        }

        private void SetPatrolBehaviour(AIPointPatrol point)
        {
            m_AIBehaviour = AIBehaviour.Patrol;
            m_PointPatrol = point;
        }
    }
}