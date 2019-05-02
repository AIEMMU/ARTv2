using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtLibrary.ModelInterface.Results.Behaviour.Rotation;

namespace ArtLibrary.Model.Results.Behaviour.Rotation
{
    internal class FastTurning : RotationBehaviourBase, IFastTurning
    {
        public FastTurning() : base("Fast Turning", 3)
        {
            
        }
    }
}
