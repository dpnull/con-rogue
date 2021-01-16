using System;
using System.Collections.Generic;
using System.Text;

namespace con_rogue.Interfaces
{
    public interface IAction
    {
        void Execute(Entity actor, Entity target);
    }
}
