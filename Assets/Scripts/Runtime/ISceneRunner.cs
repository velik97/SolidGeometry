using System;

namespace Runtime
{
    public interface ISceneRunner
    {
        void Initialize();
        void Unload(Action callback);
    }
}