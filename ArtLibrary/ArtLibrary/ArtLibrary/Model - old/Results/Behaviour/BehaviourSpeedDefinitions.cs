using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ArtLibrary.Model.Resolver;
using ArtLibrary.ModelInterface.Results.Behaviour;
using ArtLibrary.ModelInterface.Results.Behaviour.Movement;
using ArtLibrary.ModelInterface.Results.Behaviour.Rotation;

namespace ArtLibrary.Model.Results.Behaviour
{
    internal class BehaviourSpeedDefinitions : ModelObjectBase, IBehaviourSpeedDefinitions
    {
        private const double DefaultStillCutOff = 10;
        private const double DefaultWalkingCutOff = 40;
        private const double DefaultNoRotationCutOff = 15;
        private const double DefaultSlowRotationCutOff = 40;

        private double m_ModifiedStillCutOff;
        private double m_ModifiedWalkingCutOff;
        private double m_ModifiedNoRotationCutOff;
        private double m_ModifiedSlowRotationCutOff;

        public double StillCutOff
        {
            get
            {
                return m_ModifiedStillCutOff;
            }
        }

        public double WalkingCutOff
        {
            get
            {
                return m_ModifiedWalkingCutOff;
            }
        }

        public double NoRotationCutOff
        {
            get
            {
                return m_ModifiedNoRotationCutOff;
            }
        }

        public double SlowRotationCutOff
        {
            get
            {
                return m_ModifiedSlowRotationCutOff;
            }
        }

        public BehaviourSpeedDefinitions()
        {
            m_ModifiedStillCutOff = DefaultStillCutOff;
            m_ModifiedWalkingCutOff = DefaultWalkingCutOff;
            m_ModifiedNoRotationCutOff = DefaultNoRotationCutOff;
            m_ModifiedSlowRotationCutOff = DefaultSlowRotationCutOff;
        }

        /// <summary>
        /// Get the movement model for the velocity
        /// </summary>
        /// <param name="velocity">The vlocity over 1 second</param>
        /// <returns></returns>
        public IMovementBehaviour GetMovementBehaviour(double velocity)
        {
            if (velocity < StillCutOff)
            {
                return ModelResolver.Resolve<IStill>();
            }

            if (velocity < WalkingCutOff)
            {
                return ModelResolver.Resolve<IWalking>();
            }

            return ModelResolver.Resolve<IRunning>();
        }

        /// <summary>
        /// Get the rotational model for the velocity
        /// </summary>
        /// <param name="angularVelocity">The angular velocity over 1 second</param>
        /// <returns></returns>
        public IRotationBehaviour GetRotationBehaviour(double angularVelocity)
        {
            if (angularVelocity < NoRotationCutOff)
            {
                return ModelResolver.Resolve<INoRotation>();
            }

            if (angularVelocity < SlowRotationCutOff)
            {
                return ModelResolver.Resolve<ISlowTurning>();
            }

            //Check if shaking
            return ModelResolver.Resolve<IFastTurning>();
        }

        //public IRotationBehaviour[] GetRotations(Vector[] orientationTrack)
        //{
            
        //}

        /// <summary>
        /// Update the cut offs with a calibration metric
        /// </summary>
        /// <param name="pixelCalibration">Units per pixel</param>
        public void UpdateCutOffs(double pixelCalibration)
        {
            if (pixelCalibration <= 0)
            {
                throw new Exception("Pixel Calibration can not be less than 0");
            }

            m_ModifiedStillCutOff = DefaultStillCutOff * pixelCalibration;
            m_ModifiedWalkingCutOff = DefaultWalkingCutOff * pixelCalibration;
            m_ModifiedNoRotationCutOff = DefaultNoRotationCutOff * pixelCalibration;
            m_ModifiedSlowRotationCutOff = DefaultSlowRotationCutOff * pixelCalibration;
        }
    }
}
