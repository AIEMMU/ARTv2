using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtLibrary.ModelInterface.Results.Behaviour.BodyOption;

namespace ArtLibrary.Model.Results.Behaviour.BodyOption
{
    internal class HeadVisible : BodyOptionsBase, IHeadVisible
    {
        public HeadVisible() : base("Head Visible", 2)
        {

        }
    }
}
