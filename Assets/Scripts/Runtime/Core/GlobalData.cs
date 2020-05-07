using System;
using Lesson;
using Serialization.LessonsFileSystem;
using UnityEngine;
using Util.EventBusSystem;

namespace Runtime.Core
{
    public class GlobalData
    {
        public readonly FolderAsset RootFolder;
        
        private LessonAsset m_CurrentLessonAsset;
        public LessonAsset CurrentLessonAsset => m_CurrentLessonAsset;
        
        private LessonData m_CurrentLessonData;
        public LessonData CurrentLessonData => m_CurrentLessonData;

        private int m_CurrentLessonStageNumber;
        public int CurrentLessonStageNumber => m_CurrentLessonStageNumber;

        private ApplicationMode m_CurrentApplicationMode;
        public ApplicationMode CurrentApplicationMode => m_CurrentApplicationMode;

        private Action<ApplicationMode, Action<ApplicationMode>> m_RequestChangeModeAction;

        public GlobalData(Action<ApplicationMode, Action<ApplicationMode>> requestChangeModeAction, FolderAsset rootFolder)
        {
            m_RequestChangeModeAction = requestChangeModeAction;
            RootFolder = rootFolder;
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
            
            RequestChangeApplicationMode(ApplicationMode.Session3D);
        }

        public void SetCurrentLessonStageNumber(int number)
        {
            m_CurrentLessonStageNumber = number;
            EventBus.RaiseEvent<ICurrentLessonStageNumberHandler>(h => h.HandleLessonStageNumberChanged(number));
        }

        public void RequestChangeApplicationMode(ApplicationMode mode)
        {
            m_RequestChangeModeAction.Invoke(mode, SetCurrentApplicationMode);
        }

        private void SetCurrentApplicationMode(ApplicationMode mode)
        {
            m_CurrentApplicationMode = mode;
            EventBus.RaiseEvent<IApplicationModeHandler>(h => h.HandleApplicationModeChanged(mode));

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