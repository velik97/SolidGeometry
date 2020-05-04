using UnityEngine;

namespace FileSystem
{
    public abstract class BaseFileSystemAsset : ScriptableObject
    {
        [SerializeField]
        private string m_AssetName;
        
        [SerializeField]
        private string m_Description;

        public string AssetName => m_AssetName;

        public string Description => m_Description;
    }
}