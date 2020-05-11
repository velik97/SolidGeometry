using System;
using Lesson;
using Runtime.Global.ApplicationModeManagement;
using Serialization.LessonsFileSystem;
using UniRx;
using Util.EventBusSystem;

namespace Runtime.Global.LessonManagement
{
    public class LessonAccess : CompositeDisposable, IApplicationModeHandler
    {
        public static LessonAccess Instance => GlobalAccess.Instance.LessonAccess;
        
        public readonly FolderAsset RootFolder;
        
        private LessonAsset m_CurrentLessonAsset;
        public LessonAsset CurrentLessonAsset => m_CurrentLessonAsset;
        
        private LessonData m_CurrentLessonData;
        public LessonData CurrentLessonData => m_CurrentLessonData;

        private int m_CurrentLessonStageNumber;
        public int CurrentLessonStageNumber => m_CurrentLessonStageNumber;

        private Action m_RequestApplicationModeForLessonAction;

        public LessonAccess(FolderAsset rootFolder, Action requestApplicationModeForLessonAction)
        {
            RootFolder = rootFolder;
            m_RequestApplicationModeForLessonAction = requestApplicationModeForLessonAction;
        }
        
        public void StartLesson(LessonAsset lessonAsset)
        {
            LessonData lessonData = lessonAsset.GetLessonDataCashed();
            if (lessonData == null)
            {
                // TODO dialog window
                return;
            }

            m_CurrentLessonAsset = lessonAsset;
            m_CurrentLessonData = lessonData;
            EventBus.RaiseEvent<ICurrentLessonDataChangedHandler>(h => h.HandleCurrentLessonDataChanged(lessonData));
            
            m_RequestApplicationModeForLessonAction.Invoke();
        }

        public void SetCurrentLessonStageNumber(int number)
        {
            m_CurrentLessonStageNumber = number;
            EventBus.RaiseEvent<ICurrentLessonStageNumberHandler>(h => h.HandleLessonStageNumberChanged(number));
        }

        public void HandleApplicationModeChanged(ApplicationMode mode)
        {
            if (mode == ApplicationMode.MainMenu)
            {
                SetCurrentLessonStageNumber(-1);
            }
            else if (m_CurrentLessonStageNumber < 0)
            {
                SetCurrentLessonStageNumber(1);
            }
        }
    }
}