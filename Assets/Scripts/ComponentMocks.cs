using System;
using UnityEngine;

// ReSharper disable once InvalidXmlDocComment
/// <summary>
/// Mocks commonly used components that are not serializable.
/// Stores the values of the component in a serializable format.
/// Contains methods to convert the mock to the component or apply the stored values to an existing component.
/// </summary>
namespace ComponentMocks
{
    public interface IMock<T>
    {
        T Convert();
        void ApplyTo(T other);
    }
    
    /// <summary>
    /// Mocks the ConfigurableJoint.
    /// Configurable Joints incorporate all the functionality of the other joint types,
    /// and provide greater control of character movement.
    /// </summary>
    [Serializable]
    [HelpURL("https://docs.unity3d.com/2020.3/Documentation/ScriptReference/ConfigurableJoint.html")]
    public class ConfigurableJointMock : IMock<ConfigurableJoint>
    {
        public Rigidbody connectedBody;
        public Vector3 anchor;
        public Vector3 axis;
        public bool autoConfigureConnectedAnchor;
        public Vector3 connectedAnchor;
        public Vector3 secondaryAxis;
        public ConfigurableJointMotion xMotion;
        public ConfigurableJointMotion yMotion;
        public ConfigurableJointMotion zMotion;
        public ConfigurableJointMotion angularXMotion;
        public ConfigurableJointMotion angularYMotion;
        public ConfigurableJointMotion angularZMotion;
        public SoftJointLimitSpringMock linearLimitSpring;
        public SoftJointLimitMock linearLimit;
        public SoftJointLimitSpringMock angularXLimitSpring;
        public SoftJointLimitMock lowAngularXLimit;
        public SoftJointLimitMock highAngularXLimit;
        public SoftJointLimitSpringMock angularYZLimitSpring;
        public SoftJointLimitMock angularYLimit;
        public SoftJointLimitMock angularZLimit;
        public Vector3 targetPosition;
        public Vector3 targetVelocity;
        public JointDriveMock xDrive;
        public JointDriveMock yDrive;
        public JointDriveMock zDrive;
        public Quaternion targetRotation;
        public Vector3 targetAngularVelocity;
        public RotationDriveMode rotationDriveMode;
        public JointDriveMock angularXDrive;
        public JointDriveMock angularYZDrive;
        public JointDriveMock slerpDrive;
        public JointProjectionMode projectionMode;
        public float projectionDistance;
        public float projectionAngle;
        public bool configuredInWorldSpace;
        public bool swapBodies;
        public float breakForce;
        public float breakTorque;
        public bool enableCollision;
        public bool enablePreprocessing;
        public float massScale;
        public float connectedMassScale;

        /// <summary>
        /// Applies the values of this mock to the given ConfigurableJoint
        /// </summary>
        /// <param name="other">The ConfigurableJoint to apply the values to</param>
        public void ApplyTo(ConfigurableJoint other)
        {
            // Apply all the fields to the other joint
            other.connectedBody = connectedBody;
            other.anchor = anchor;
            other.axis = axis;
            other.autoConfigureConnectedAnchor = autoConfigureConnectedAnchor;
            other.connectedAnchor = connectedAnchor;
            other.secondaryAxis = secondaryAxis;
            other.xMotion = xMotion;
            other.yMotion = yMotion;
            other.zMotion = zMotion;
            other.angularXMotion = angularXMotion;
            other.angularYMotion = angularYMotion;
            other.angularZMotion = angularZMotion;
            other.linearLimitSpring = linearLimitSpring.Convert();
            other.linearLimit = linearLimit.Convert();
            other.angularXLimitSpring = angularXLimitSpring.Convert();
            other.lowAngularXLimit = lowAngularXLimit.Convert();
            other.highAngularXLimit = highAngularXLimit.Convert();
            other.angularYZLimitSpring = angularYZLimitSpring.Convert();
            other.angularYLimit = angularYLimit.Convert();
            other.angularZLimit = angularZLimit.Convert();
            other.targetPosition = targetPosition;
            other.targetVelocity = targetVelocity;
            other.xDrive = xDrive.Convert();
            other.yDrive = yDrive.Convert();
            other.zDrive = zDrive.Convert();
            other.targetRotation = targetRotation;
            other.targetAngularVelocity = targetAngularVelocity;
            other.rotationDriveMode = rotationDriveMode;
            other.angularXDrive = angularXDrive.Convert();
            other.angularYZDrive = angularYZDrive.Convert();
            other.slerpDrive = slerpDrive.Convert();
            other.projectionMode = projectionMode;
            other.projectionDistance = projectionDistance;
            other.projectionAngle = projectionAngle;
            other.configuredInWorldSpace = configuredInWorldSpace;
            other.swapBodies = swapBodies;
            other.breakForce = breakForce;
            other.breakTorque = breakTorque;
            other.enableCollision = enableCollision;
            other.enablePreprocessing = enablePreprocessing;
            other.massScale = massScale;
            other.connectedMassScale = connectedMassScale;
        }

        public ConfigurableJoint Convert()
        {
            // TODO: Implement this
            ConfigurableJoint joint = new ConfigurableJoint();
            ApplyTo(joint);
            return joint;
        }
    }

    /// <summary>
    /// Mocks the SoftJointLimitSpring.
    /// The SoftJointLimitSpring is the configuration of the spring attached to the joint's limits: linear and angular.
    /// Used by CharacterJoint and ConfigurableJoint.
    /// </summary>
    [Serializable]
    [HelpURL("https://docs.unity3d.com/ScriptReference/SoftJointLimitSpring.html")]
    public class SoftJointLimitSpringMock : IMock<SoftJointLimitSpring>
    {
        /// <summary>
        /// The stiffness of the spring limit. When stiffness is zero the limit is hard, otherwise soft.
        /// </summary>
        [SerializeField] 
        [Min(0)]
        [Tooltip("The stiffness of the spring limit. When stiffness is zero the limit is hard, otherwise soft.")]
        public float spring;
        
        /// <summary>
        /// The damping of the spring limit. In effect when the stiffness of the sprint limit is not zero.
        /// </summary>
        [SerializeField] 
        [Min(0)]
        [Tooltip("The damping of the spring limit. In effect when the stiffness of the sprint limit is not zero.")]
        public float damper;

        /// <summary>
        /// Converts this mock to a SoftJointLimitSpring
        /// </summary>
        /// <returns>A new SoftJointLimitSpring with the same values as this mock</returns>
        public SoftJointLimitSpring Convert()
        {
            return new SoftJointLimitSpring
            {
                spring = spring,
                damper = damper
            };
        }
        
        /// <summary>
        /// Applies the values of this mock to the given SoftJointLimitSpring
        /// </summary>
        /// <param name="other">The SoftJointLimitSpring to apply the values to</param>
        public void ApplyTo(SoftJointLimitSpring other)
        {
            other.spring = spring;
            other.damper = damper;
        }
    }

    /// <summary>
    /// Mocks the SoftJointLimit class.
    /// A SoftJointLimit defines the rotational or translational limits of joints,
    /// specifying the maximum allowed movement angle or position offset,
    /// bounciness, and the contact distance where the limit starts taking effect.
    /// </summary>
    [Serializable]
    [HelpURL("https://docs.unity3d.com/ScriptReference/SoftJointLimit.html")]
    public class SoftJointLimitMock : IMock<SoftJointLimit>
    {
        /// <summary>
        /// When the joint hits the limit, it can be made to bounce off it.
        /// Bounciness determines how much to bounce off an limit.
        /// </summary>
        [SerializeField]
        [Range(0f, 1f)]
        [Tooltip("Bounciness determines how much to bounce off an limit.")]
        public float bounciness;
        
        /// <summary>
        /// Determines how far ahead in space the solver can "see" the joint limit.

        /// Distance inside the limit value at which the limit will be considered to be active by the solver.
        /// For translational joints the unit is meters.
        /// For rotational joints the unit is degrees.
        /// Setting this low can cause jittering, but might run faster.
        /// Setting this high can reduce jittering, but might run slower. Jointed objects will still fall asleep correctly.
        /// 0 = use defaults
        /// </summary>
        /// <see href="https://docs.unity3d.com/ScriptReference/SoftJointLimit-contactDistance.html"></see>
        [SerializeField]
        [Tooltip("Determines how far ahead in space the solver can \"see\" the joint limit. 0 = use defaults")]
        public float contactDistance;
        
        /// <summary>
        /// The limit position/angle of the joint (in degrees)
        /// </summary>
        [SerializeField]
        [Tooltip("The limit position/angle of the joint (in degrees)")]
        public float limit;

        /// <summary>
        /// Converts this mock to a SoftJointLimit
        /// </summary>
        /// <returns>A new SoftJointLimit with the same values as this mock</returns>
        public SoftJointLimit Convert()
        {
            return new SoftJointLimit {
                bounciness = bounciness,
                contactDistance = contactDistance,
                limit = limit
            };
        }
        
        /// <summary>
        /// Applies the values of this mock to the given SoftJointLimit
        /// </summary>
        /// <param name="other">The SoftJointLimit to apply the values to</param>
        public void ApplyTo(SoftJointLimit other)
        {
            other.bounciness = bounciness;
            other.contactDistance = contactDistance;
            other.limit = limit;
        }
    }

    /// <summary>
    /// Mocks the JointDrive.
    /// Joint Drive applies a constant force to a joint to reach a desired target velocity, target angular velocity, target position, or target rotation.
    /// </summary>
    [Serializable]
    [HelpURL("https://docs.unity3d.com/ScriptReference/JointDrive.html")]
    public class JointDriveMock : IMock<JointDrive>
    {
        [SerializeField] public float maximumForce;
        [SerializeField] public float positionDamper;
        [SerializeField] public float positionSpring;

        public JointDrive Convert()
        {
            return new JointDrive
            {
                maximumForce = maximumForce,
                positionDamper = positionDamper,
                positionSpring = positionSpring,
            };
        }

        public void ApplyTo(JointDrive other)
        {
            other.maximumForce = maximumForce;
            other.positionDamper = positionDamper;
            other.positionSpring = positionSpring;
        }
    }

}
