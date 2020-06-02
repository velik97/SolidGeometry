using System;
using Lesson;
using Runtime.Access.ApplicationModeManagement;
using Runtime.Access.ARLesson;
using Serialization.LessonsFileSystem;
using UniRx;
using Util.EventBusSystem;
using Util.UniRxExtensions;

namespace Runtime.Access.Lesson
{
    public class LessonAccess : MultipleDisposable, IApplicationModeHandler, IARLessonStateHandler
    {
        public static LessonAccess Instance => RootAccess.Instance.LessonAccess;
        
        public readonly FolderAsset RootFolder;
        
        private LessonAsset m_CurrentLessonAsset;
        public LessonAsset CurrentLessonAsset => m_CurrentLessonAsset;
        
        private LessonData m_CurrentLessonData;
        public LessonData CurrentLessonData => m_CurrentLessonData;

        private LessonState m_LessonState;
        public LessonState LessonState => m_LessonState;

        private int m_CurrentLessonStageNumber;
        public int CurrentLessonStageNumber => m_CurrentLessonStageNumber;
        
        public LessonAccess(FolderAsset rootFolder)
        {
            RootFolder = rootFolder;
            AddDisposable(EventBus.Subscribe(this));
        }
        
        public void RequestStartLesson(LessonAsset lessonAsset)
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
            
            ApplicationModeAccess.Instance.GoToLessonMode();
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

            UpdateLessonState();
        }

        public void HandleARLessonStateChanged(ARLessonState state)
        {
            UpdateLessonState();
        }

        private LessonState GetLessonState()
        {
            if (ApplicationModeAccess.Instance.CurrentApplicationMode != ApplicationMode.Session3D &&
                ApplicationModeAccess.Instance.CurrentApplicationMode != ApplicationMode.SessionAR)
            {
                return LessonState.NotRunning;
            }

            if (ApplicationModeAccess.Instance.CurrentApplicationMode == ApplicationMode.SessionAR)
            {
                if (ARLessonAccess.Instance.ARLessonState == ARLessonState.ExtractingFeaturePoints ||
                    ARLessonAccess.Instance.ARLessonState == ARLessonState.PlacingLesson)
                {
                    return LessonState.Interrupted;
                }
            }

            return LessonState.Running;
        }

        private void UpdateLessonState()
        {
            LessonState state = GetLessonState();
            if (m_LessonState == state)
            {
                return;
            }

            m_LessonState = state;
            EventBus.RaiseEvent<ILessonStateHandler>(h => h.HandlerLessonStateChanged(state));
        }
    }

    public enum LessonState
    {
        NotRunning,
        Running,
        Interrupted
    }
}