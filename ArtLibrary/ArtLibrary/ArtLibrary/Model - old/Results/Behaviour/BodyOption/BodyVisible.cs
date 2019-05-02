using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtLibrary.ModelInterface.Results.Behaviour.BodyOption;

namespace ArtLibrary.Model.Results.Behaviour.BodyOption
{
    internal class BodyVisible : BodyOptionsBase, IBodyVisible
    {
        public BodyVisible() : base("Body Visible", 3)
        {
        }
    }
}
