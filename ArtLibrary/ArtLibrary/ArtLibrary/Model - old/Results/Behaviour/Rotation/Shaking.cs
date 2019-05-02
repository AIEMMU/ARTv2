using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtLibrary.ModelInterface.Results.Behaviour.Rotation;

namespace ArtLibrary.Model.Results.Behaviour.Rotation
{
    internal class Shaking : RotationBehaviourBase, IShaking
    {
        public Shaking() : base("Shaking", 4)
        {

        }
    }
}
