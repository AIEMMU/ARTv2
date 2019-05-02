using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtLibrary.ModelInterface.Results.Behaviour.Movement;

namespace ArtLibrary.Model.Results.Behaviour.Movement
{
    internal abstract class MovementBehaviourBase : BehaviourBase, IMovementBehaviour
    {
        protected MovementBehaviourBase(string name, int id) : base(name, id)
        {
            
        }

        public override bool Equals(object obj)
        {
            IMovementBehaviour behaviour = obj as IMovementBehaviour;

            if (behaviour == null)
            {
                return false;
            }

            return Equals(behaviour);
        }

        public bool Equals(IMovementBehaviour behaviour)
        {
            return behaviour.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
