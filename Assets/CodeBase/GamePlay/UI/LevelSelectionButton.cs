using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace SpaceShooter
{
    public class LevelSelectionButton : MonoBehaviour
    {
        [SerializeField] private Text m_LevelTitle;
        [SerializeField] private Image m_LevelPreview;

        private LevelProperties m_LevelProperties;

        public void SetLevelProperties(LevelProperties levelProperties)
        {
            m_LevelProperties = levelProperties;

            if (m_LevelProperties == null) return;

            m_LevelPreview.sprite = m_LevelProperties.PreviewImage;
            m_LevelTitle.text = m_LevelProperties.Title;
        }

        public void LoadLevel()
        {
            SceneManager.LoadScene(m_LevelProperties.SceneName);
        }
    }
}