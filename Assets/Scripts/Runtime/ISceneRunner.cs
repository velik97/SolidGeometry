using System;
using Runtime.Core;

namespace Runtime
{
    public interface ISceneRunner
    {
        void Initialize(GlobalData globalData);
        void Unload(Action callback);
    }
}