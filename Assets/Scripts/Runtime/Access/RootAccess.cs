using System;
using Runtime.Access.ApplicationModeManagement;
using Runtime.Access.ARLesson;
using Runtime.Access.Camera;
using Runtime.Access.DeviceARRequirements;
using Runtime.Access.Lesson;
using UniRx;
using Util.UniRxExtensions;

namespace Runtime.Access
{
    public class RootAccess : MultipleDisposable
    {
        private static RootAccess s_Instance;
        public static RootAccess Instance => s_Instance;

        public readonly LessonAccess LessonAccess;
        public readonly ApplicationModeAccess ApplicationModeAccess;
        public readonly DeviceARRequirementsAccess DeviceARRequirementsAccess;
        public readonly CameraAccess CameraAccess;
        public readonly ARLessonAccess ARLessonAccess;

        public static IDisposable Create(ApplicationConfig applicationConfig)
        {
            s_Instance = new RootAccess(applicationConfig);
            return s_Instance;
        }

        private RootAccess(ApplicationConfig applicationConfig)
        {
            AddDisposable(ApplicationModeAccess = new ApplicationModeAccess(applicationConfig));
            AddDisposable(LessonAccess = new LessonAccess(applicationConfig.RootFolder));
            AddDisposable(ARLessonAccess = new ARLessonAccess());
            AddDisposable(DeviceARRequirementsAccess = new DeviceARRequirementsAccess());
            AddDisposable(CameraAccess = new CameraAccess());

            s_Instance = this;
        }
    }
}