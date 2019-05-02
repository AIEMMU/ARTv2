using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtLibrary.Model.Behaviours;
using ARWT.ViewModel.NewSession;

namespace ARWT.ViewModel.Behaviours
{
    public class BehaviourHolderViewModel : ViewModelBase
    {
        public BoundaryBaseViewModel Boundary
        {
            get;
            set;
        }

        public InteractionBehaviour Interaction
        {
            get;
            set;
        }

        public int FrameNumber
        {
            get;
            set;
        }

        public string BoundaryName
        {
            get
            {
                return Boundary.Name;
            }
        }

        public string Name
        {
            get
            {
                string firstComponent;

                if (Interaction == InteractionBehaviour.Started)
                {
                    firstComponent = "Begun ";
                }
                else
                {
                    firstComponent = "Ended ";
                }

                firstComponent += "interaction";

                return firstComponent;
            }
        }

        public BehaviourHolderViewModel(BoundaryBaseViewModel boundary, InteractionBehaviour interaction, int frameNumber)
        {
            Boundary = boundary;
            Interaction = interaction;
            FrameNumber = frameNumber;
        }
    }
}
