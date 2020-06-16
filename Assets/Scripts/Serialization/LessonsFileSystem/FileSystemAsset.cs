using JetBrains.Annotations;
using UnityEngine;

namespace Serialization.LessonsFileSystem
{
    public abstract class FileSystemAsset : ScriptableObject
    {
        [SerializeField]
        private string m_AssetName;
        [SerializeField]
        private string m_Description;

        [CanBeNull]
        private FolderAsset m_ParentFolder;

        public string AssetName => m_AssetName;
        public string Description => m_Description;

        [CanBeNull]
        public FolderAsset ParentFolder => m_ParentFolder;
        
        public abstract Color Color { get; }

        public void SetParentFolder(FolderAsset folderAsset)
        {
            m_ParentFolder = folderAsset;
        }

        public abstract void ValidateNullReferences(ref bool valid);
    }
}