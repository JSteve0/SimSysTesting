//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/InputMaster.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @InputMaster : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputMaster()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputMaster"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""55e339c4-2276-4121-b02d-489124571bc0"",
            ""actions"": [
                {
                    ""name"": ""LeftStick"",
                    ""type"": ""Value"",
                    ""id"": ""f5e69f8b-5afc-4362-a4ab-6af2625d9516"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""RightStick"",
                    ""type"": ""Value"",
                    ""id"": ""d46e0a6e-45f9-4121-a985-344392a480ac"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""MoveUp"",
                    ""type"": ""Value"",
                    ""id"": ""965747dc-778b-4ce8-b228-e8aa5c53d861"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""MoveDown"",
                    ""type"": ""Value"",
                    ""id"": ""b7663485-237b-4800-80ee-4ddcd0c96109"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""ToggleActiveForcep"",
                    ""type"": ""Button"",
                    ""id"": ""f6c15867-462e-4040-858f-d02bf6bb7fe5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Rotate"",
                    ""type"": ""Value"",
                    ""id"": ""44002c0d-d5ec-4a81-beb5-6164c5ae0407"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""CloseLeftForcep"",
                    ""type"": ""Value"",
                    ""id"": ""e9452be9-f718-4853-97bb-d8c6023080c5"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""CloseRightForcep"",
                    ""type"": ""Value"",
                    ""id"": ""6aae169f-d264-4806-9d58-2946190e9cf7"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""c2b72a91-28fd-404b-9aa1-737fcf011a40"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Xbox Control Scheme"",
                    ""action"": ""LeftStick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""900d20ff-e8ea-441d-ace0-c1bc988a8908"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Xbox Control Scheme"",
                    ""action"": ""RightStick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c43c0c63-ae79-4201-bf2f-c16a546257a9"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7a26f5ed-b780-4282-a91c-a4b29c2deb77"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""997d4dbf-4aff-4901-ae53-854201f2fb32"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ToggleActiveForcep"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""bdb00f90-4df3-4027-b604-8e9870dda11c"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""82e756a7-8f32-4bab-9e2f-67a56ec1c800"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Xbox Control Scheme"",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""b3fa646b-4f74-48d1-9160-303aab964442"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""edadfe5f-9cf4-4f18-a03f-9b2365cdcb09"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CloseLeftForcep"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dd0cc7b3-b2aa-4b09-8000-a9ff302e139c"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Xbox Control Scheme"",
                    ""action"": ""CloseRightForcep"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Xbox Control Scheme"",
            ""bindingGroup"": ""Xbox Control Scheme"",
            ""devices"": [
                {
                    ""devicePath"": ""<XInputController>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_LeftStick = m_Player.FindAction("LeftStick", throwIfNotFound: true);
        m_Player_RightStick = m_Player.FindAction("RightStick", throwIfNotFound: true);
        m_Player_MoveUp = m_Player.FindAction("MoveUp", throwIfNotFound: true);
        m_Player_MoveDown = m_Player.FindAction("MoveDown", throwIfNotFound: true);
        m_Player_ToggleActiveForcep = m_Player.FindAction("ToggleActiveForcep", throwIfNotFound: true);
        m_Player_Rotate = m_Player.FindAction("Rotate", throwIfNotFound: true);
        m_Player_CloseLeftForcep = m_Player.FindAction("CloseLeftForcep", throwIfNotFound: true);
        m_Player_CloseRightForcep = m_Player.FindAction("CloseRightForcep", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_LeftStick;
    private readonly InputAction m_Player_RightStick;
    private readonly InputAction m_Player_MoveUp;
    private readonly InputAction m_Player_MoveDown;
    private readonly InputAction m_Player_ToggleActiveForcep;
    private readonly InputAction m_Player_Rotate;
    private readonly InputAction m_Player_CloseLeftForcep;
    private readonly InputAction m_Player_CloseRightForcep;
    public struct PlayerActions
    {
        private @InputMaster m_Wrapper;
        public PlayerActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @LeftStick => m_Wrapper.m_Player_LeftStick;
        public InputAction @RightStick => m_Wrapper.m_Player_RightStick;
        public InputAction @MoveUp => m_Wrapper.m_Player_MoveUp;
        public InputAction @MoveDown => m_Wrapper.m_Player_MoveDown;
        public InputAction @ToggleActiveForcep => m_Wrapper.m_Player_ToggleActiveForcep;
        public InputAction @Rotate => m_Wrapper.m_Player_Rotate;
        public InputAction @CloseLeftForcep => m_Wrapper.m_Player_CloseLeftForcep;
        public InputAction @CloseRightForcep => m_Wrapper.m_Player_CloseRightForcep;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @LeftStick.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeftStick;
                @LeftStick.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeftStick;
                @LeftStick.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeftStick;
                @RightStick.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRightStick;
                @RightStick.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRightStick;
                @RightStick.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRightStick;
                @MoveUp.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveUp;
                @MoveUp.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveUp;
                @MoveUp.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveUp;
                @MoveDown.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveDown;
                @MoveDown.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveDown;
                @MoveDown.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveDown;
                @ToggleActiveForcep.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnToggleActiveForcep;
                @ToggleActiveForcep.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnToggleActiveForcep;
                @ToggleActiveForcep.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnToggleActiveForcep;
                @Rotate.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRotate;
                @Rotate.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRotate;
                @Rotate.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRotate;
                @CloseLeftForcep.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCloseLeftForcep;
                @CloseLeftForcep.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCloseLeftForcep;
                @CloseLeftForcep.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCloseLeftForcep;
                @CloseRightForcep.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCloseRightForcep;
                @CloseRightForcep.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCloseRightForcep;
                @CloseRightForcep.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCloseRightForcep;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @LeftStick.started += instance.OnLeftStick;
                @LeftStick.performed += instance.OnLeftStick;
                @LeftStick.canceled += instance.OnLeftStick;
                @RightStick.started += instance.OnRightStick;
                @RightStick.performed += instance.OnRightStick;
                @RightStick.canceled += instance.OnRightStick;
                @MoveUp.started += instance.OnMoveUp;
                @MoveUp.performed += instance.OnMoveUp;
                @MoveUp.canceled += instance.OnMoveUp;
                @MoveDown.started += instance.OnMoveDown;
                @MoveDown.performed += instance.OnMoveDown;
                @MoveDown.canceled += instance.OnMoveDown;
                @ToggleActiveForcep.started += instance.OnToggleActiveForcep;
                @ToggleActiveForcep.performed += instance.OnToggleActiveForcep;
                @ToggleActiveForcep.canceled += instance.OnToggleActiveForcep;
                @Rotate.started += instance.OnRotate;
                @Rotate.performed += instance.OnRotate;
                @Rotate.canceled += instance.OnRotate;
                @CloseLeftForcep.started += instance.OnCloseLeftForcep;
                @CloseLeftForcep.performed += instance.OnCloseLeftForcep;
                @CloseLeftForcep.canceled += instance.OnCloseLeftForcep;
                @CloseRightForcep.started += instance.OnCloseRightForcep;
                @CloseRightForcep.performed += instance.OnCloseRightForcep;
                @CloseRightForcep.canceled += instance.OnCloseRightForcep;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    private int m_XboxControlSchemeSchemeIndex = -1;
    public InputControlScheme XboxControlSchemeScheme
    {
        get
        {
            if (m_XboxControlSchemeSchemeIndex == -1) m_XboxControlSchemeSchemeIndex = asset.FindControlSchemeIndex("Xbox Control Scheme");
            return asset.controlSchemes[m_XboxControlSchemeSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnLeftStick(InputAction.CallbackContext context);
        void OnRightStick(InputAction.CallbackContext context);
        void OnMoveUp(InputAction.CallbackContext context);
        void OnMoveDown(InputAction.CallbackContext context);
        void OnToggleActiveForcep(InputAction.CallbackContext context);
        void OnRotate(InputAction.CallbackContext context);
        void OnCloseLeftForcep(InputAction.CallbackContext context);
        void OnCloseRightForcep(InputAction.CallbackContext context);
    }
}
