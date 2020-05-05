using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Serialization.LessonsFileSystem
{
    [CreateAssetMenu]
    public class FolderAsset : FileSystemAsset
    {
        [SerializeField] private List<FileSystemAsset> m_AssetsList;

        public List<FileSystemAsset> AssetsList => m_AssetsList;

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