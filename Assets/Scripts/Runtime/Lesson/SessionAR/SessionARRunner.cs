using Runtime.Lesson.Session3D;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace Runtime.Lesson.SessionAR
{
    public class SessionARRunner : Session3DRunner
    {
        /// <summary>
        /// Contains m_LessonOrigin
        /// Used to be translated while replacing lesson in AR
        /// </summary>
        [SerializeField]
        private Transform m_ARLessonOrigin;

        [SerializeField]
        private ARRaycastManager m_ARRaycastManager;

        [SerializeField]
        private GameObject m_PlacementIndicator;

        private ARLessonPlacing m_ARLessonPlacing;
        
        public override void Initialize()
        {
            base.Initialize();
            InitializeLessonReplace();
        }

        private void InitializeLessonReplace()
        {
            AddDisposable(m_ARLessonPlacing = new ARLessonPlacing(m_ARLessonOrigin, m_PlacementIndicator, m_ARRaycastManager));
        }
    }
}