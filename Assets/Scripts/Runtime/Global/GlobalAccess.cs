using System;
using Runtime.Core;
using Runtime.Global.ApplicationModeManagement;
using Runtime.Global.DeviceARRequirements;
using Runtime.Global.LessonManagement;
using UniRx;

namespace Runtime.Global
{
    public class GlobalAccess : CompositeDisposable
    {
        private static GlobalAccess s_Instance;
        public static GlobalAccess Instance => s_Instance;

        public readonly LessonAccess LessonAccess;
        public readonly ApplicationModeAccess ApplicationModeAccess;
        public readonly DeviceARRequirementsAccess DeviceARRequirementsAccess;

        public static IDisposable Create(ApplicationConfig applicationConfig)
        {
            s_Instance = new GlobalAccess(applicationConfig);
            return s_Instance;
        }

        private GlobalAccess(ApplicationConfig applicationConfig)
        {
            Add(ApplicationModeAccess = new ApplicationModeAccess(applicationConfig));
            Add(LessonAccess = new LessonAccess(applicationConfig.RootFolder, ApplicationModeAccess.GoToLessonMode));
            Add(DeviceARRequirementsAccess = new DeviceARRequirementsAccess());

            s_Instance = this;
        }
    }
}