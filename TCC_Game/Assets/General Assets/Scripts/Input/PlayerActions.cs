//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/General Assets/Scripts/Input/PlayerActions.inputactions
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

public partial class @PlayerActions : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerActions"",
    ""maps"": [
        {
            ""name"": ""Movement"",
            ""id"": ""d777bf78-3ce8-4864-9254-74aeb6070f11"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Button"",
                    ""id"": ""942e6c19-087c-4524-acac-3a986a0e3909"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""5916ffc5-de81-41d0-9e71-c89e2b09a3eb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""fcecd834-f0d9-4939-afe8-265ba466c268"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Interaction"",
                    ""type"": ""Button"",
                    ""id"": ""728e2858-5e43-4e66-aef3-7a82fb83dc43"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SkipDialogue"",
                    ""type"": ""Button"",
                    ""id"": ""cf6920b7-c372-49f5-a181-8b749277cf2f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Drop"",
                    ""type"": ""Button"",
                    ""id"": ""9d133e0a-26d9-4887-a56c-7ad5bb212099"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""InventorySlot_1"",
                    ""type"": ""Button"",
                    ""id"": ""d4bb142b-72e3-49b1-83ad-378a4ae920f8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""InventorySlot_2"",
                    ""type"": ""Button"",
                    ""id"": ""1e2f3585-2df6-4a81-8d85-b0ccb78c7a2e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""InventorySlot_3"",
                    ""type"": ""Button"",
                    ""id"": ""0dd4e8b0-e092-4540-9028-9b8a52d5c331"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""7b1c5045-6e63-4b34-86ce-939881a3f70e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SecondFinal"",
                    ""type"": ""Button"",
                    ""id"": ""523d0e4f-1f58-47f9-9f8c-9257a547d3b3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Reset"",
                    ""type"": ""Button"",
                    ""id"": ""6702a1c3-ad08-4728-9edb-4ec260822b6c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""AD"",
                    ""id"": ""8da64533-713f-4fc3-a5ec-c1f1a7ab0e91"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""b636d1e0-d525-4fe1-9599-d98e08e517e5"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""55fc4fe4-c336-42b5-8f26-5c628d09138b"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Arrows"",
                    ""id"": ""08d78fac-c59a-489e-89d8-3c21d240c269"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""7b751f92-476f-4ddb-ab7c-8e010691dd05"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""aab4ecb4-4f74-437c-a95c-8d13d9f12949"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""LeftStick"",
                    ""id"": ""3f22cbfd-c724-45da-a293-fd8833bd94d0"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""52cbcf4a-ec01-48af-8508-8e907e65a974"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""96fb763f-b002-4da6-a0cf-075b39de9544"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""DPad"",
                    ""id"": ""ed0fa853-8800-4743-a857-a35ca5243aa5"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""c646c3c4-ed0e-45b3-820a-92463303a6af"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""bdba0cc3-007e-4dcd-82f4-b366d65e6003"",
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
                    ""id"": ""df9b2228-3f7c-4b4f-9c95-8e0a59f31f92"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8d6850cc-dcbc-4678-a5cd-45ca1afc5850"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6333e346-0f32-4a21-9c2d-474ce4490c18"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""42e590e5-c2ae-43ff-ad6b-39d1afaf3d8f"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8157f3af-8e06-46c6-b693-9659868c920e"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Interaction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""aa5d3d8d-2948-45f2-aff7-31b1bb21aa27"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Interaction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a42ca7bb-1253-4db6-96b5-3fcd03527c08"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""SkipDialogue"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7a81cf23-bad4-4b74-a425-c04e970bacb6"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""SkipDialogue"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f75417b4-6013-4045-a600-0d9621e4c1e7"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Drop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""84f8d67c-2b5f-494d-afee-cf20c2272d42"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Drop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9e0a7b01-d3cf-441f-aa71-59aa8c2ae61b"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""InventorySlot_1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dd2a25fe-3d47-4c37-9b9c-8c6a9f947aa9"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""InventorySlot_2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7c57fd30-9fe0-4fbf-916d-d26132bf5b41"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""InventorySlot_3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4933a93d-cb8c-4ccf-9995-b7fa217e96db"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f61038b4-f907-4621-881c-060b41cc7485"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a6560160-e4eb-457f-b0b8-7324ca42bcc1"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""SecondFinal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d9ec23ba-0063-49d7-89d0-e47f2b7eabf8"",
                    ""path"": ""<Gamepad>/rightStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""SecondFinal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""267949d2-efc3-4aa8-93b7-b33e001c6058"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Reset"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""37d74255-e78e-4636-8321-6b99d300ee65"",
                    ""path"": ""<Gamepad>/leftStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Reset"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""41f1a20d-4414-48b3-a147-02d33444c329"",
            ""actions"": [
                {
                    ""name"": ""TabChange"",
                    ""type"": ""Button"",
                    ""id"": ""a0407a57-fa20-4fc9-9158-0dafe30287b1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""QE"",
                    ""id"": ""db7527bc-70bf-46d2-9f05-a8466c68162a"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TabChange"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""dd12f454-41e6-42cf-b47d-94eefcc63879"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""TabChange"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""414a4892-f119-4313-8212-1c62e20fb276"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""TabChange"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""PadShoulders"",
                    ""id"": ""8d52872e-c58f-4a81-a34d-7d1d04060405"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TabChange"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""3a74da89-72cf-4d59-a9cd-a9eea1090d44"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""TabChange"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""5a9729af-df26-4791-92b7-97b5732dccfb"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""TabChange"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard&Mouse"",
            ""bindingGroup"": ""Keyboard&Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
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
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Movement
        m_Movement = asset.FindActionMap("Movement", throwIfNotFound: true);
        m_Movement_Move = m_Movement.FindAction("Move", throwIfNotFound: true);
        m_Movement_Jump = m_Movement.FindAction("Jump", throwIfNotFound: true);
        m_Movement_Shoot = m_Movement.FindAction("Shoot", throwIfNotFound: true);
        m_Movement_Interaction = m_Movement.FindAction("Interaction", throwIfNotFound: true);
        m_Movement_SkipDialogue = m_Movement.FindAction("SkipDialogue", throwIfNotFound: true);
        m_Movement_Drop = m_Movement.FindAction("Drop", throwIfNotFound: true);
        m_Movement_InventorySlot_1 = m_Movement.FindAction("InventorySlot_1", throwIfNotFound: true);
        m_Movement_InventorySlot_2 = m_Movement.FindAction("InventorySlot_2", throwIfNotFound: true);
        m_Movement_InventorySlot_3 = m_Movement.FindAction("InventorySlot_3", throwIfNotFound: true);
        m_Movement_Pause = m_Movement.FindAction("Pause", throwIfNotFound: true);
        m_Movement_SecondFinal = m_Movement.FindAction("SecondFinal", throwIfNotFound: true);
        m_Movement_Reset = m_Movement.FindAction("Reset", throwIfNotFound: true);
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_TabChange = m_UI.FindAction("TabChange", throwIfNotFound: true);
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

    // Movement
    private readonly InputActionMap m_Movement;
    private IMovementActions m_MovementActionsCallbackInterface;
    private readonly InputAction m_Movement_Move;
    private readonly InputAction m_Movement_Jump;
    private readonly InputAction m_Movement_Shoot;
    private readonly InputAction m_Movement_Interaction;
    private readonly InputAction m_Movement_SkipDialogue;
    private readonly InputAction m_Movement_Drop;
    private readonly InputAction m_Movement_InventorySlot_1;
    private readonly InputAction m_Movement_InventorySlot_2;
    private readonly InputAction m_Movement_InventorySlot_3;
    private readonly InputAction m_Movement_Pause;
    private readonly InputAction m_Movement_SecondFinal;
    private readonly InputAction m_Movement_Reset;
    public struct MovementActions
    {
        private @PlayerActions m_Wrapper;
        public MovementActions(@PlayerActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Movement_Move;
        public InputAction @Jump => m_Wrapper.m_Movement_Jump;
        public InputAction @Shoot => m_Wrapper.m_Movement_Shoot;
        public InputAction @Interaction => m_Wrapper.m_Movement_Interaction;
        public InputAction @SkipDialogue => m_Wrapper.m_Movement_SkipDialogue;
        public InputAction @Drop => m_Wrapper.m_Movement_Drop;
        public InputAction @InventorySlot_1 => m_Wrapper.m_Movement_InventorySlot_1;
        public InputAction @InventorySlot_2 => m_Wrapper.m_Movement_InventorySlot_2;
        public InputAction @InventorySlot_3 => m_Wrapper.m_Movement_InventorySlot_3;
        public InputAction @Pause => m_Wrapper.m_Movement_Pause;
        public InputAction @SecondFinal => m_Wrapper.m_Movement_SecondFinal;
        public InputAction @Reset => m_Wrapper.m_Movement_Reset;
        public InputActionMap Get() { return m_Wrapper.m_Movement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MovementActions set) { return set.Get(); }
        public void SetCallbacks(IMovementActions instance)
        {
            if (m_Wrapper.m_MovementActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnMove;
                @Jump.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnJump;
                @Shoot.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnShoot;
                @Shoot.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnShoot;
                @Shoot.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnShoot;
                @Interaction.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnInteraction;
                @Interaction.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnInteraction;
                @Interaction.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnInteraction;
                @SkipDialogue.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnSkipDialogue;
                @SkipDialogue.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnSkipDialogue;
                @SkipDialogue.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnSkipDialogue;
                @Drop.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnDrop;
                @Drop.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnDrop;
                @Drop.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnDrop;
                @InventorySlot_1.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnInventorySlot_1;
                @InventorySlot_1.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnInventorySlot_1;
                @InventorySlot_1.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnInventorySlot_1;
                @InventorySlot_2.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnInventorySlot_2;
                @InventorySlot_2.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnInventorySlot_2;
                @InventorySlot_2.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnInventorySlot_2;
                @InventorySlot_3.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnInventorySlot_3;
                @InventorySlot_3.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnInventorySlot_3;
                @InventorySlot_3.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnInventorySlot_3;
                @Pause.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnPause;
                @SecondFinal.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnSecondFinal;
                @SecondFinal.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnSecondFinal;
                @SecondFinal.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnSecondFinal;
                @Reset.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnReset;
                @Reset.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnReset;
                @Reset.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnReset;
            }
            m_Wrapper.m_MovementActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Shoot.started += instance.OnShoot;
                @Shoot.performed += instance.OnShoot;
                @Shoot.canceled += instance.OnShoot;
                @Interaction.started += instance.OnInteraction;
                @Interaction.performed += instance.OnInteraction;
                @Interaction.canceled += instance.OnInteraction;
                @SkipDialogue.started += instance.OnSkipDialogue;
                @SkipDialogue.performed += instance.OnSkipDialogue;
                @SkipDialogue.canceled += instance.OnSkipDialogue;
                @Drop.started += instance.OnDrop;
                @Drop.performed += instance.OnDrop;
                @Drop.canceled += instance.OnDrop;
                @InventorySlot_1.started += instance.OnInventorySlot_1;
                @InventorySlot_1.performed += instance.OnInventorySlot_1;
                @InventorySlot_1.canceled += instance.OnInventorySlot_1;
                @InventorySlot_2.started += instance.OnInventorySlot_2;
                @InventorySlot_2.performed += instance.OnInventorySlot_2;
                @InventorySlot_2.canceled += instance.OnInventorySlot_2;
                @InventorySlot_3.started += instance.OnInventorySlot_3;
                @InventorySlot_3.performed += instance.OnInventorySlot_3;
                @InventorySlot_3.canceled += instance.OnInventorySlot_3;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
                @SecondFinal.started += instance.OnSecondFinal;
                @SecondFinal.performed += instance.OnSecondFinal;
                @SecondFinal.canceled += instance.OnSecondFinal;
                @Reset.started += instance.OnReset;
                @Reset.performed += instance.OnReset;
                @Reset.canceled += instance.OnReset;
            }
        }
    }
    public MovementActions @Movement => new MovementActions(this);

    // UI
    private readonly InputActionMap m_UI;
    private IUIActions m_UIActionsCallbackInterface;
    private readonly InputAction m_UI_TabChange;
    public struct UIActions
    {
        private @PlayerActions m_Wrapper;
        public UIActions(@PlayerActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @TabChange => m_Wrapper.m_UI_TabChange;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void SetCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterface != null)
            {
                @TabChange.started -= m_Wrapper.m_UIActionsCallbackInterface.OnTabChange;
                @TabChange.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnTabChange;
                @TabChange.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnTabChange;
            }
            m_Wrapper.m_UIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @TabChange.started += instance.OnTabChange;
                @TabChange.performed += instance.OnTabChange;
                @TabChange.canceled += instance.OnTabChange;
            }
        }
    }
    public UIActions @UI => new UIActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard&Mouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
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
    public interface IMovementActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnShoot(InputAction.CallbackContext context);
        void OnInteraction(InputAction.CallbackContext context);
        void OnSkipDialogue(InputAction.CallbackContext context);
        void OnDrop(InputAction.CallbackContext context);
        void OnInventorySlot_1(InputAction.CallbackContext context);
        void OnInventorySlot_2(InputAction.CallbackContext context);
        void OnInventorySlot_3(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
        void OnSecondFinal(InputAction.CallbackContext context);
        void OnReset(InputAction.CallbackContext context);
    }
    public interface IUIActions
    {
        void OnTabChange(InputAction.CallbackContext context);
    }
}
