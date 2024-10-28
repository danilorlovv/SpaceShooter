using UnityEngine;

namespace SpaceShooter
{
    [CreateAssetMenu(fileName = "LevelSequences", menuName = "Properties/Level")]
    public class LevelSequences : ScriptableObject
    {
        public LevelProperties[] LevelsProperties;
    }
}