using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtLibrary.ModelInterface.Results.Behaviour.Rotation;

namespace ArtLibrary.Model.Results.Behaviour.Rotation
{
    internal class SlowTurning : RotationBehaviourBase, ISlowTurning
    {
        public SlowTurning() : base("Slow Turning", 2)
        {
            
        }
    }
}
