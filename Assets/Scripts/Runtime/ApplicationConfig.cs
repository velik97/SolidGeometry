using Serialization.LessonsFileSystem;
using UnityEngine;
using Util.SceneUtils;

namespace Runtime
{
    [CreateAssetMenu]
    public class ApplicationConfig : ScriptableObject
    {
        [Header("Scenes")]
        [SerializeField]
        private SceneReference m_MainMenuMechanicsScene;
        
        [SerializeField]
        private SceneReference m_Session3DScene;
        [SerializeField]
        private SceneReference m_SessionARScene;
        [SerializeField]
        private SceneReference m_SessionUIScene;

        [SerializeField]
        private SceneReference m_ShapeViewFactoryScene;
        [SerializeField]
        private SceneReference m_CameraScene3D;
        
        [Header("Other")]
        [SerializeField]
        private FolderAsset m_RootFolder;

        public SceneReference MainMenuMechanicsScene => m_MainMenuMechanicsScene;

        public SceneReference SessionARScene => m_SessionARScene;
        public SceneReference Session3DScene => m_Session3DScene;
        public SceneReference SessionUIScene => m_SessionUIScene;

        public SceneReference ShapeViewFactoryScene => m_ShapeViewFactoryScene;
        public SceneReference CameraScene3D => m_CameraScene3D;

        public FolderAsset RootFolder => m_RootFolder;
    }
}