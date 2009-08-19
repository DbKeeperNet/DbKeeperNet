using System;
using System.Collections.Generic;
using System.Text;

namespace DbKeeperNet.Engine
{
    public interface IExtension
    {
        void Initialize(IUpdateContext context);
    }
}
