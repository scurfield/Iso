// GENERATED AUTOMATICALLY FROM 'Assets/InputS/InputControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputControls"",
    ""maps"": [
        {
            ""name"": ""PlayerControls"",
            ""id"": ""783600aa-77da-4702-bc51-97e6c375e17f"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""fd6ca5d5-0050-40bd-95b1-6119506e8744"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""InteractAbility"",
                    ""type"": ""Button"",
                    ""id"": ""0174d7f8-9e81-4ff4-9d5c-5fd0535cfd5b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""AbilityLeft"",
                    ""type"": ""Button"",
                    ""id"": ""244bdc94-1a29-4608-9c4a-06094bbdabe9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""AbilityTop"",
                    ""type"": ""Button"",
                    ""id"": ""2e33a0ee-4628-47ec-a13f-40dd44e5e288"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""AbilityRight"",
                    ""type"": ""Button"",
                    ""id"": ""0454218a-0106-40b8-9a84-45c2e11e7746"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""OpenAbilityPanel"",
                    ""type"": ""Button"",
                    ""id"": ""e4712f23-885f-456f-8dce-8e9b000c95c0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""OpenMenuPanel"",
                    ""type"": ""Button"",
                    ""id"": ""2e974aa1-eb69-487b-95a0-09e15e3341a7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""d419f734-be2d-4f1e-8722-8deddedd90f3"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""95b6d192-44ba-43ab-adae-2bfa938bc25b"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""d41e21c7-e445-4aad-912f-974fc177f91d"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""8c2c2758-732b-4f99-a21e-245640ec1977"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""98f8ea67-a275-4e2a-8b2e-8e8bda280d58"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""460517ab-f948-43f4-9e05-8ea2c800f19a"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""11a3c91e-5837-4b7a-8c59-74b267f07e05"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""88552c9b-e450-44ca-bba4-8cde06b6555b"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""10bafeac-b30e-4410-8277-061b65b8f093"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""6f30ebce-0c84-4aef-9b65-69d4b02cff12"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""f9777b2d-962b-4a8f-88d0-200971d01067"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""455e4a66-8613-4030-b819-b07578a4f187"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""InteractAbility"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3cd710ec-dfa9-48bc-b162-b358b13075b9"",
                    ""path"": ""<Keyboard>/k"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""InteractAbility"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""884b80af-ae59-48f6-94cc-3c8345ad53ba"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""InteractAbility"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""14cff287-f964-4175-8bfa-a0135663b554"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""AbilityLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d2a94fb9-6d2c-4391-b911-4736adb92cf7"",
                    ""path"": ""<Keyboard>/j"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""AbilityLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""748d0966-c572-470c-af1b-7626ca506a03"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""AbilityLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a2607250-45a2-4952-8f87-049e54c54452"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""AbilityTop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d3b638e8-5251-45ef-999d-da24b6f202cd"",
                    ""path"": ""<Keyboard>/i"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""AbilityTop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ba7842ae-3bd9-44be-9df0-a92f8b1f4b66"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""AbilityTop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0fa88fad-a1ad-43a9-a1b7-a3b6defe3425"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""AbilityRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""123bd9e6-b4ce-4205-a3f3-02db940b5951"",
                    ""path"": ""<Keyboard>/l"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""AbilityRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""99cadc23-e407-4766-b6db-bb5e16f15fdd"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""AbilityRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d97bd317-2838-4edb-8d6e-4696b9dcd89e"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""OpenAbilityPanel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1671440f-60a6-4471-b233-80bac8d52652"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""OpenAbilityPanel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""33e73d99-bb68-4ce8-b5a6-a7d353b83ab2"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""OpenMenuPanel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // PlayerControls
        m_PlayerControls = asset.FindActionMap("PlayerControls", throwIfNotFound: true);
        m_PlayerControls_Move = m_PlayerControls.FindAction("Move", throwIfNotFound: true);
        m_PlayerControls_InteractAbility = m_PlayerControls.FindAction("InteractAbility", throwIfNotFound: true);
        m_PlayerControls_AbilityLeft = m_PlayerControls.FindAction("AbilityLeft", throwIfNotFound: true);
        m_PlayerControls_AbilityTop = m_PlayerControls.FindAction("AbilityTop", throwIfNotFound: true);
        m_PlayerControls_AbilityRight = m_PlayerControls.FindAction("AbilityRight", throwIfNotFound: true);
        m_PlayerControls_OpenAbilityPanel = m_PlayerControls.FindAction("OpenAbilityPanel", throwIfNotFound: true);
        m_PlayerControls_OpenMenuPanel = m_PlayerControls.FindAction("OpenMenuPanel", throwIfNotFound: true);
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

    // PlayerControls
    private readonly InputActionMap m_PlayerControls;
    private IPlayerControlsActions m_PlayerControlsActionsCallbackInterface;
    private readonly InputAction m_PlayerControls_Move;
    private readonly InputAction m_PlayerControls_InteractAbility;
    private readonly InputAction m_PlayerControls_AbilityLeft;
    private readonly InputAction m_PlayerControls_AbilityTop;
    private readonly InputAction m_PlayerControls_AbilityRight;
    private readonly InputAction m_PlayerControls_OpenAbilityPanel;
    private readonly InputAction m_PlayerControls_OpenMenuPanel;
    public struct PlayerControlsActions
    {
        private @InputControls m_Wrapper;
        public PlayerControlsActions(@InputControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_PlayerControls_Move;
        public InputAction @InteractAbility => m_Wrapper.m_PlayerControls_InteractAbility;
        public InputAction @AbilityLeft => m_Wrapper.m_PlayerControls_AbilityLeft;
        public InputAction @AbilityTop => m_Wrapper.m_PlayerControls_AbilityTop;
        public InputAction @AbilityRight => m_Wrapper.m_PlayerControls_AbilityRight;
        public InputAction @OpenAbilityPanel => m_Wrapper.m_PlayerControls_OpenAbilityPanel;
        public InputAction @OpenMenuPanel => m_Wrapper.m_PlayerControls_OpenMenuPanel;
        public InputActionMap Get() { return m_Wrapper.m_PlayerControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerControlsActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerControlsActions instance)
        {
            if (m_Wrapper.m_PlayerControlsActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMove;
                @InteractAbility.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnInteractAbility;
                @InteractAbility.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnInteractAbility;
                @InteractAbility.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnInteractAbility;
                @AbilityLeft.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnAbilityLeft;
                @AbilityLeft.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnAbilityLeft;
                @AbilityLeft.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnAbilityLeft;
                @AbilityTop.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnAbilityTop;
                @AbilityTop.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnAbilityTop;
                @AbilityTop.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnAbilityTop;
                @AbilityRight.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnAbilityRight;
                @AbilityRight.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnAbilityRight;
                @AbilityRight.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnAbilityRight;
                @OpenAbilityPanel.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnOpenAbilityPanel;
                @OpenAbilityPanel.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnOpenAbilityPanel;
                @OpenAbilityPanel.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnOpenAbilityPanel;
                @OpenMenuPanel.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnOpenMenuPanel;
                @OpenMenuPanel.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnOpenMenuPanel;
                @OpenMenuPanel.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnOpenMenuPanel;
            }
            m_Wrapper.m_PlayerControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @InteractAbility.started += instance.OnInteractAbility;
                @InteractAbility.performed += instance.OnInteractAbility;
                @InteractAbility.canceled += instance.OnInteractAbility;
                @AbilityLeft.started += instance.OnAbilityLeft;
                @AbilityLeft.performed += instance.OnAbilityLeft;
                @AbilityLeft.canceled += instance.OnAbilityLeft;
                @AbilityTop.started += instance.OnAbilityTop;
                @AbilityTop.performed += instance.OnAbilityTop;
                @AbilityTop.canceled += instance.OnAbilityTop;
                @AbilityRight.started += instance.OnAbilityRight;
                @AbilityRight.performed += instance.OnAbilityRight;
                @AbilityRight.canceled += instance.OnAbilityRight;
                @OpenAbilityPanel.started += instance.OnOpenAbilityPanel;
                @OpenAbilityPanel.performed += instance.OnOpenAbilityPanel;
                @OpenAbilityPanel.canceled += instance.OnOpenAbilityPanel;
                @OpenMenuPanel.started += instance.OnOpenMenuPanel;
                @OpenMenuPanel.performed += instance.OnOpenMenuPanel;
                @OpenMenuPanel.canceled += instance.OnOpenMenuPanel;
            }
        }
    }
    public PlayerControlsActions @PlayerControls => new PlayerControlsActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    public interface IPlayerControlsActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnInteractAbility(InputAction.CallbackContext context);
        void OnAbilityLeft(InputAction.CallbackContext context);
        void OnAbilityTop(InputAction.CallbackContext context);
        void OnAbilityRight(InputAction.CallbackContext context);
        void OnOpenAbilityPanel(InputAction.CallbackContext context);
        void OnOpenMenuPanel(InputAction.CallbackContext context);
    }
}
