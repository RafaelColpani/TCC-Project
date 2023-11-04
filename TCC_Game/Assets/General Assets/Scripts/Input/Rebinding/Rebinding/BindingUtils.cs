using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.XInput;
using UnityEngine.InputSystem.Switch;

public enum GamepadDevices
{
    playstation, xbox, nintendo, none
}

public class BindingUtils
{
    public static Gamepad GetConnectedGamepad()
    {
        var gamepad = Gamepad.current;
        if (gamepad == null)
            return null;

        return gamepad;
    }

    public static GamepadDevices GetCurrentGamepadDevice()
    {
        switch (GetConnectedGamepad())
        {
            case DualShockGamepad:
                return GamepadDevices.playstation;

            case XInputController:
                return GamepadDevices.xbox;

            case SwitchProControllerHID:
                return GamepadDevices.nintendo;

            default:
                return GamepadDevices.none;
        }
    }

    public static string GetButtonImageTextByInputAction(InputAction action, int bindingIndex)
    {
        var inputText = InputControlPath.ToHumanReadableString(
            action.bindings[bindingIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);

        return GetButtonImageTextByText(inputText);
    }

    public static string GetButtonImageTextByText(string inputText)
    {
        var text = inputText;

        switch (inputText)
        {
            // D-PADS -----
            case "D-Pad/Left":
                text = "<SPRITE=0>";
                break;

            case "D-Pad/Right":
                text = "<SPRITE=1>";
                break;

            case "D-Pad/Up":
                text = "<SPRITE=2>";
                break;

            case "D-Pad/Down":
                text = "<SPRITE=3>";
                break;

            // ACTION BUTTONS -----
            case "Button South":
                text = "<SPRITE=4>";
                break;

            case "Button West":
                text = "<SPRITE=5>";
                break;

            case "Button North":
                text = "<SPRITE=6>";
                break;

            case "Button East":
                text = "<SPRITE=7>";
                break;

            // SHOULDERS & TRIGGERS -----
            case "Left Shoulder":
                text = "<SPRITE=8>";
                break;

            case "Left Trigger":
                text = "<SPRITE=9>";
                break;

            case "Right Shoulder":
                text = "<SPRITE=10>";
                break;

            case "Right Trigger":
                text = "<SPRITE=11>";
                break;

            // LEFT STICK -----
            case "Left Stick/Left":
                text = "<SPRITE=12>";
                break;

            case "Left Stick/Up":
                text = "<SPRITE=13>";
                break;

            case "Left Stick/Right":
                text = "<SPRITE=14>";
                break;

            case "Left Stick/Down":
                text = "<SPRITE=15>";
                break;

            case "Left Stick Press":
                text = "<SPRITE=16>";
                break;

            // RIGHT STICK -----
            case "Right Stick/Left":
                text = "<SPRITE=17>";
                break;

            case "Right Stick/Up":
                text = "<SPRITE=18>";
                break;

            case "Right Stick/Right":
                text = "<SPRITE=19>";
                break;

            case "Right Stick/Down":
                text = "<SPRITE=20>";
                break;

            case "Right Stick Press":
                text = "<SPRITE=21>";
                break;

            // OPTIONS -----
            case "Select":
                text = "<SPRITE=22>";
                break;

            case "Start":
                text = "<SPRITE=23>";
                break;

            default:
                break;
        }

        return text;
    }

    
}
