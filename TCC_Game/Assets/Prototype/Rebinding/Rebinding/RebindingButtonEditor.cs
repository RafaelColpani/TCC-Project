// Created by: Henrique Batista de Assis
// Date: 03/01/2023

#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;

[CustomEditor(typeof(RebindingButton))]
public class RebindingButtonEditor : UnityEditor.Editor
{
    // each serialize field of the BindingButton needs a property
    private SerializedProperty rebindActionProperty;
    private SerializedProperty bindingIdProperty;
    private SerializedProperty displayStringProperty;
    private SerializedProperty buttonTextProperty;

    private GUIContent[] bindingOptions; // the options that will be displayed on the inspector as each binding of the action
    private GUIContent bindingLabel = new GUIContent("Binding ID"); // just the label on the instector
    private GUIContent displayOptionsLabel = new GUIContent("Display Options"); // just the label on the inspector

    private string[] bindingOptionsValues; // the text of the oprions of each binding
    private int selectedBindingIndex; // the index of the binding that the user selected

    private void OnEnable()
    {
        // each property needs to find the reference on the inspaector
        rebindActionProperty = serializedObject.FindProperty("rebindAction");
        bindingIdProperty = serializedObject.FindProperty("bindingId");
        displayStringProperty = serializedObject.FindProperty("displayStringOptions");
        buttonTextProperty = serializedObject.FindProperty("buttonText");

        RefreshBindingOptions();
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        //-- BINDING
        using (new EditorGUI.IndentLevelScope())
        {
            EditorGUILayout.PropertyField(rebindActionProperty);
            var newSelectedBinding = EditorGUILayout.Popup(bindingLabel, selectedBindingIndex, bindingOptions);

            if (newSelectedBinding != selectedBindingIndex)
            {
                var bindingId = bindingOptionsValues[newSelectedBinding];
                bindingIdProperty.stringValue = bindingId;
                selectedBindingIndex = newSelectedBinding;
            }

            var optionsOld = (InputBinding.DisplayStringOptions)displayStringProperty.intValue;
            var optionsNew = (InputBinding.DisplayStringOptions)EditorGUILayout.EnumFlagsField(displayOptionsLabel, optionsOld);
            if (optionsOld != optionsNew)
                displayStringProperty.intValue = (int)optionsNew;
        }

        // SPACE
        EditorGUILayout.Space();

        //-- UI
        using (new EditorGUI.IndentLevelScope())
        {
            EditorGUILayout.PropertyField(buttonTextProperty);
        }

        //-- FINISH
        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
            RefreshBindingOptions();
        }
    }

    private void RefreshBindingOptions()
    {
        // get the actions of the InputActionReference
        var actionRef = (InputActionReference)rebindActionProperty.objectReferenceValue;
        var action = actionRef?.action;

        // if nothing is selected on the inspector
        if (action == null)
        {
            bindingOptions = new GUIContent[0];
            bindingOptionsValues = new string[0];
            selectedBindingIndex = -1;
            return;
        }

        // get all the bindings from the action
        var bindings = action.bindings;
        var bindingsCount = bindings.Count;

        bindingOptions = new GUIContent[bindingsCount]; // the options will be the same count as the amount of bindingd
        bindingOptionsValues = new string[bindingsCount]; // same for the value
        selectedBindingIndex = -1; // always start with no property selected, so start with index of -1

        var currentBindingId = bindingIdProperty.stringValue; // store the string of the selected binding

        // loop through all the binding options
        for (int i = 0; i < bindingsCount; ++i)
        {
            // get the binding and its id
            var binding = bindings[i];
            var bindingId = binding.id.ToString();

            // store if have control schemes
            bool haveBindingGroups = !string.IsNullOrEmpty(binding.groups);

            // just the eay that will be displayed the string
            var displayOptions = InputBinding.DisplayStringOptions.DontUseShortDisplayNames | InputBinding.DisplayStringOptions.IgnoreBindingOverrides;

            // show the device of the binding
            if (haveBindingGroups)
                displayOptions |= InputBinding.DisplayStringOptions.DontOmitDevice;

            // the string displayed in inspector, getting the string of the binding
            var displayString = action.GetBindingDisplayString(i, displayOptions);

            // chech if binding have composite, like a value of Vector2 for example
            if (binding.isPartOfComposite)
                displayString = $"{ObjectNames.NicifyVariableName(binding.name)}: {displayString}";

            // change the slashes to prevent bugs
            displayString = displayString.Replace('/', '\\');

            // if have cotnrol schemes
            if (haveBindingGroups)
            {
                var asset = action.actionMap?.asset;

                if (asset != null)
                {
                    var controlSchemes = string.Join(", ",
                        binding.groups.Split(InputBinding.Separator)
                            .Select(x => asset.controlSchemes.FirstOrDefault(c => c.bindingGroup == x).name));

                    displayString = $"{displayString} ({controlSchemes})";
                }
            }

            bindingOptions[i] = new GUIContent(displayString);
            bindingOptionsValues[i] = bindingId;

            if (currentBindingId == bindingId)
                selectedBindingIndex = i;
        }
    }
}
#endif