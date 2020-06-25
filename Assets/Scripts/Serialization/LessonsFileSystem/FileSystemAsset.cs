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
        [SerializeField]
        private bool m_isBlocked;
        
        [SerializeField]
        private bool m_isUnlockLibraryButton;
        
        [SerializeField]
        private int m_complexityValue;

        [CanBeNull]
        private FolderAsset m_ParentFolder;

        public bool isUnlockLibraryButton => m_isUnlockLibraryButton;
        public string AssetName => m_AssetName;
        public string Description => m_Description;
        public bool isBlocked => m_isBlocked;
        public int complexityValue => m_complexityValue;

        [CanBeNull]
        public FolderAsset ParentFolder => m_ParentFolder;

        public abstract Color Color { get;}

        public void SetParentFolder(FolderAsset folderAsset)
        {
            m_ParentFolder = folderAsset;
        }

        public abstract void ValidateNullReferences(ref bool valid);
    }
}