using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Serialization.LessonsFileSystem
{
    [CreateAssetMenu]
    public class FolderAsset : FileSystemAsset
    {
        [SerializeField]
        private Color m_Color = Color.magenta;
        [SerializeField] private List<FileSystemAsset> m_AssetsList;

        public List<FileSystemAsset> AssetsList => m_AssetsList;

        private Color customGreen = new Color(0.184f, 0.490f, 0.263f, 1.000f);
        private Color customRed = new Color(0.890f, 0.286f, 0.137f, 1.000f);
        private Color customBlue = new Color(0.027f, 0.349f, 0.816f, 1.000f);

        public override Color Color => (this.name == "Folder 1" ? customBlue : (this.name == "Folder 2" ? customGreen : (this.name == "Folder 3" ? customRed : Color.grey)));

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