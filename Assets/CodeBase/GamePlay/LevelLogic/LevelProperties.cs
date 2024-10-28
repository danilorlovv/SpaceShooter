using UnityEngine;

namespace SpaceShooter
{
    [System.Serializable]
    public class LevelProperties
    {
        [SerializeField] private string m_Title;
        [SerializeField] private string m_SceneName;
        [SerializeField] private Sprite m_PreviewImage;
        [SerializeField] private LevelProperties m_NextLevel;

        public string Title => m_Title;
        public string SceneName => m_SceneName;
        public Sprite PreviewImage => m_PreviewImage;
    }
}