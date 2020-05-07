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
        private SceneReference m_Session3DMechanicsScene;
        [SerializeField]
        private SceneReference m_SessionARMechanicsScene;
        [SerializeField]
        private SceneReference m_SessionUIScene;

        [SerializeField]
        private SceneReference m_ShapeViewFactoryScene;
        [SerializeField]
        private SceneReference m_CameraScene3D;
        [SerializeField]
        private SceneReference m_CameraSceneAR;
        
        [Header("Other")]
        [SerializeField]
        private FolderAsset m_RootFolder;

        public SceneReference MainMenuMechanicsScene => m_MainMenuMechanicsScene;

        public SceneReference SessionARMechanicsScene => m_SessionARMechanicsScene;
        public SceneReference Session3DMechanicsScene => m_Session3DMechanicsScene;
        public SceneReference SessionUIScene => m_SessionUIScene;

        public SceneReference ShapeViewFactoryScene => m_ShapeViewFactoryScene;
        public SceneReference CameraScene3D => m_CameraScene3D;
        public SceneReference CameraSceneAR => m_CameraSceneAR;

        public FolderAsset RootFolder => m_RootFolder;
    }
}