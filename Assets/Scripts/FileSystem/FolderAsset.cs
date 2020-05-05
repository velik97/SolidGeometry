using System.Collections.Generic;
using UnityEngine;

namespace FileSystem
{
    [CreateAssetMenu]
    public class FolderAsset : BaseFileSystemAsset
    {
        [SerializeField] private List<BaseFileSystemAsset> m_LessonsList;

        public List<BaseFileSystemAsset> LessonsList => m_LessonsList;
    }
}