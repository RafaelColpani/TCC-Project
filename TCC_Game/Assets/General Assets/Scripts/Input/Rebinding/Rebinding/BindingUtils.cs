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
                text = "<SPRITE=11>";
                break;

            case "D-Pad/Right":
                text = "<SPRITE=12>";
                break;

            case "D-Pad/Up":
                text = "<SPRITE=13>";
                break;

            case "D-Pad/Down":
                text = "<SPRITE=10>";
                break;

            // ACTION BUTTONS -----
            case "Button South":
                text = "<SPRITE=24>"; // 28 x, 27 n
                break;

            case "Button West":
                text = "<SPRITE=25>"; // 29 x, 26 n
                break;

            case "Button North":
                text = "<SPRITE=22>"; // 26 x, 29 n
                break;

            case "Button East":
                text = "<SPRITE=23>"; // 27 x, 28 n
                break;

            // SHOULDERS & TRIGGERS -----
            case "Left Shoulder":
                text = "<SPRITE=0>"; // 30 x
                break;

            case "Left Trigger":
                text = "<SPRITE=0>";
                break;

            case "Right Shoulder":
                text = "<SPRITE=1>"; // 31 x
                break;

            case "Right Trigger":
                text = "<SPRITE=1>";
                break;

            // LEFT STICK -----
            case "Left Stick/Left":
                text = "<SPRITE=3>";
                break;

            case "Left Stick/Up":
                text = "<SPRITE=5>";
                break;

            case "Left Stick/Right":
                text = "<SPRITE=4>";
                break;

            case "Left Stick/Down":
                text = "<SPRITE=2>";
                break;

            case "Left Stick Press":
                text = "<SPRITE=15>";
                break;

            // RIGHT STICK -----
            case "Right Stick/Left":
                text = "<SPRITE=7>";
                break;

            case "Right Stick/Up":
                text = "<SPRITE=9>";
                break;

            case "Right Stick/Right":
                text = "<SPRITE=8>";
                break;

            case "Right Stick/Down":
                text = "<SPRITE=6>";
                break;

            case "Right Stick Press":
                text = "<SPRITE=18>";
                break;

            // OPTIONS -----
            case "Select":
                text = "<SPRITE=19>"; // 20 x
                break;

            case "Start":
                text = "<SPRITE=16>"; // 21 x
                break;

            default:
                break;
        }

        return text;
    }

    
}
