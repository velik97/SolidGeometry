using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Serialization.LessonsFileSystem
{
    [CreateAssetMenu]
    public class FolderAsset : FileSystemAsset
    {
        [SerializeField]
        private Color m_Color;
        [SerializeField] private List<FileSystemAsset> m_AssetsList;

        public List<FileSystemAsset> AssetsList => m_AssetsList;

        public override Color Color => m_Color;

        public override void ValidateNullReferences(ref bool valid)
        {
            for (int i = 0; i < m_AssetsList.Count; i++)
            {
                FileSystemAsset asset = m_AssetsList[i];
                if (asset == null)
                {
                    valid = false;
                    Debug.LogError($"Have null at folder \"{name}\" at number {i}");
                }
                else
                {
                    asset.ValidateNullReferences(ref valid);
                }
            }
        }

        public bool HaveCycles()
        {
            HashSet<FolderAsset> visitedFolders = new HashSet<FolderAsset>();
            Queue<FolderAsset> foldersToVisit = new Queue<FolderAsset>();
            
            foldersToVisit.Enqueue(this);
            visitedFolders.Add(this);

            while (foldersToVisit.Count > 0)
            {
                FolderAsset current = foldersToVisit.Dequeue();

                foreach (FolderAsset folderAsset in current.AssetsList.OfType<FolderAsset>())
                {
                    if (folderAsset == null)
                    {
                        continue;
                    }
                    if (visitedFolders.Contains(folderAsset))
                    {
                        return true;
                    }
                    visitedFolders.Add(folderAsset);
                    foldersToVisit.Enqueue(folderAsset);
                }
            }

            return false;
        }

        public void AssignParentFolders()
        {
            foreach (FileSystemAsset asset in AssetsList)
            {
                asset.SetParentFolder(this);
                if (asset is FolderAsset folderAsset)
                {
                    folderAsset.AssignParentFolders();
                }
            }
        }
    }
}