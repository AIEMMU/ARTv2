using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtLibrary.ModelInterface.Results.Behaviour.Movement;

namespace ArtLibrary.Model.Results.Behaviour.Movement
{
    internal class Walking : MovementBehaviourBase, IWalking
    {
        public Walking() : base("Walking", 2)
        {
            
        }
    }
}
