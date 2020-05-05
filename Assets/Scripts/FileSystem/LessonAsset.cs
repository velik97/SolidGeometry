using UnityEngine;

namespace FileSystem
{
    [CreateAssetMenu]
    public class LessonAsset : BaseFileSystemAsset
    {
        [SerializeField] private TextAsset m_LessonFile;

        public TextAsset LessonFile => m_LessonFile;
    }
}