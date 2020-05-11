using System;
using Runtime.Core;
using Runtime.Global;

namespace Runtime
{
    public interface ISceneRunner
    {
        void Initialize();
        void Unload(Action callback);
    }
}