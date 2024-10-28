namespace Common
{
    public class Timer
    {
        private float m_CurrentTime;

        public bool IsFinished => m_CurrentTime <= 0;

        public Timer(float startTime)
        {
            Start(startTime);
        }

        public void Start(float startTime)
        {
            m_CurrentTime = startTime;
        }

        public void RemoveTime(float deltaTime)
        {
            if (IsFinished) return;

            m_CurrentTime -= deltaTime;
        }
    }
}