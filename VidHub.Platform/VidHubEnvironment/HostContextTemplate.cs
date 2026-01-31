<<<<<<<< HEAD:VidHub.Platform/Environment/HostContextTemplate.cs
﻿namespace VidHub.Platform.Environment
========
﻿namespace VidHub.Platform.VidHubEnvironment
>>>>>>>> main:VidHub.Platform/VidHubEnvironment/HostContextTemplate.cs
{
    internal class HostContextTemplate : IHostContext
    {
        public T GetService<T>() where T : class
        {
            return default;
        }
    }
}
