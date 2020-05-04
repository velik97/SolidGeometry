﻿using Util.EventBusSystem;

namespace Runtime.Core
{
    public interface IApplicationModeHandler : IGlobalSubscriber
    {
        void HandleApplicationModeChanged(ApplicationMode mode);
    }
}