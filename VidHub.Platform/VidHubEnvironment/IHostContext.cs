<<<<<<<< HEAD:VidHub.Platform/Environment/IHostContext.cs
﻿namespace VidHub.Platform.Environment
========
﻿namespace VidHub.Platform.VidHubEnvironment
>>>>>>>> main:VidHub.Platform/VidHubEnvironment/IHostContext.cs
{
    public interface IHostContext
    {
        T GetService<T>() where T : class;
    }
}
