using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtLibrary.ModelInterface.Results.Behaviour.Movement;

namespace ArtLibrary.Model.Results.Behaviour.Movement
{
    internal class Still : MovementBehaviourBase, IStill
    {
        public Still() : base("Still", 1)
        {
            
        }
    }
}
