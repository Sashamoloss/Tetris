// GENERATED AUTOMATICALLY FROM 'Assets/Inputs/Controles.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Controles : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controles()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controles"",
    ""maps"": [
        {
            ""name"": ""AM"",
            ""id"": ""06bde03e-66b9-41b0-8145-47c674a98ac4"",
            ""actions"": [
                {
                    ""name"": ""Deplacement"",
                    ""type"": ""Value"",
                    ""id"": ""fa1f2e79-c539-4667-b182-9be5cb8d9fba"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Rotation"",
                    ""type"": ""Button"",
                    ""id"": ""688b0299-14e4-4a40-af94-30ccf5930e0b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""7e429e86-a661-428e-9177-e9dccdef93ea"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Deplacement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""4cfa2b25-646e-456c-bc6a-abb54437c392"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Deplacement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""4ce8969f-afea-407b-842f-87ce48b7e37f"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Deplacement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""67c44f81-0d62-4ca0-bfab-43532442b776"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotation"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""980941e2-0824-4794-848d-521cc88924c2"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""faf362e9-504c-4536-a41c-a67904bf5bdd"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // AM
        m_AM = asset.FindActionMap("AM", throwIfNotFound: true);
        m_AM_Deplacement = m_AM.FindAction("Deplacement", throwIfNotFound: true);
        m_AM_Rotation = m_AM.FindAction("Rotation", throwIfNotFound: true);
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

    // AM
    private readonly InputActionMap m_AM;
    private IAMActions m_AMActionsCallbackInterface;
    private readonly InputAction m_AM_Deplacement;
    private readonly InputAction m_AM_Rotation;
    public struct AMActions
    {
        private @Controles m_Wrapper;
        public AMActions(@Controles wrapper) { m_Wrapper = wrapper; }
        public InputAction @Deplacement => m_Wrapper.m_AM_Deplacement;
        public InputAction @Rotation => m_Wrapper.m_AM_Rotation;
        public InputActionMap Get() { return m_Wrapper.m_AM; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(AMActions set) { return set.Get(); }
        public void SetCallbacks(IAMActions instance)
        {
            if (m_Wrapper.m_AMActionsCallbackInterface != null)
            {
                @Deplacement.started -= m_Wrapper.m_AMActionsCallbackInterface.OnDeplacement;
                @Deplacement.performed -= m_Wrapper.m_AMActionsCallbackInterface.OnDeplacement;
                @Deplacement.canceled -= m_Wrapper.m_AMActionsCallbackInterface.OnDeplacement;
                @Rotation.started -= m_Wrapper.m_AMActionsCallbackInterface.OnRotation;
                @Rotation.performed -= m_Wrapper.m_AMActionsCallbackInterface.OnRotation;
                @Rotation.canceled -= m_Wrapper.m_AMActionsCallbackInterface.OnRotation;
            }
            m_Wrapper.m_AMActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Deplacement.started += instance.OnDeplacement;
                @Deplacement.performed += instance.OnDeplacement;
                @Deplacement.canceled += instance.OnDeplacement;
                @Rotation.started += instance.OnRotation;
                @Rotation.performed += instance.OnRotation;
                @Rotation.canceled += instance.OnRotation;
            }
        }
    }
    public AMActions @AM => new AMActions(this);
    public interface IAMActions
    {
        void OnDeplacement(InputAction.CallbackContext context);
        void OnRotation(InputAction.CallbackContext context);
    }
}
