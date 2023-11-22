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
                text = "<SPRITE=20>";
                break;

            case "D-Pad/Right":
                text = "<SPRITE=22>";
                break;

            case "D-Pad/Up":
                text = "<SPRITE=21>";
                break;

            case "D-Pad/Down":
                text = "<SPRITE=23>";
                break;

            // ACTION BUTTONS -----
            case "Button South":
                if (GetCurrentGamepadDevice() == GamepadDevices.playstation)
                    text = "<SPRITE=12>"; // 16 x, 15 n
                else if (GetCurrentGamepadDevice() == GamepadDevices.nintendo)
                    text = "<SPRITE=15>"; // 12 p, 16 x
                else
                    text = "<SPRITE=16>"; // 12 p, 15 n
                break;

            case "Button West":
                if (GetCurrentGamepadDevice() == GamepadDevices.playstation)
                    text = "<SPRITE=13>"; // 17 x, 14 n
                else if (GetCurrentGamepadDevice() == GamepadDevices.nintendo)
                    text = "<SPRITE=14>"; // 13 p, 17 x
                else
                    text = "<SPRITE=17>"; // 13 p, 14 n
                break;

            case "Button North":
                if (GetCurrentGamepadDevice() == GamepadDevices.playstation)
                    text = "<SPRITE=10>"; // 14 x, 17 n
                else if (GetCurrentGamepadDevice() == GamepadDevices.nintendo)
                    text = "<SPRITE=17>"; // 10 p, 14 x
                else
                    text = "<SPRITE=14>"; // 10 p, 17 n
                break;

            case "Button East":
                if (GetCurrentGamepadDevice() == GamepadDevices.playstation)
                    text = "<SPRITE=11>"; // 15 x, 16 n
                else if (GetCurrentGamepadDevice() == GamepadDevices.nintendo)
                    text = "<SPRITE=16>"; // 11 p, 15 x
                else
                    text = "<SPRITE=15>"; // 11 p, 16 n
                break;

            // SHOULDERS & TRIGGERS -----
            case "Left Shoulder":
                if (GetCurrentGamepadDevice() == GamepadDevices.playstation)
                    text = "<SPRITE=0>"; // 4 x
                else
                    text = "<SPRITE=4>"; // 0 p
                break;

            case "Left Trigger":
                if (GetCurrentGamepadDevice() == GamepadDevices.playstation)
                    text = "<SPRITE=2>"; // 6 x
                else
                    text = "<SPRITE=6>"; // 2 p
                break;

            case "Right Shoulder":
                if (GetCurrentGamepadDevice() == GamepadDevices.playstation)
                    text = "<SPRITE=1>"; // 5 x
                else
                    text = "<SPRITE=5>"; // 1 p
                break;

            case "Right Trigger":
                if (GetCurrentGamepadDevice() == GamepadDevices.playstation)
                    text = "<SPRITE=3>"; // 7 x
                else
                    text = "<SPRITE=7>"; // 3 p
                break;

            // LEFT STICK -----
            case "Left Stick/Left":
                text = "<SPRITE=28>";
                break;

            case "Left Stick/Up":
                text = "<SPRITE=29>";
                break;

            case "Left Stick/Right":
                text = "<SPRITE=30>";
                break;

            case "Left Stick/Down":
                text = "<SPRITE=31>";
                break;

            case "Left Stick Press":
                text = "<SPRITE=24>";
                break;

            case "Left Stick/X":
                text = "<SPRITE=26>";
                break;

            case "Left Stick/Y":
                text = "<SPRITE=26>";
                break;

            // RIGHT STICK -----
            case "Right Stick/Left":
                text = "<SPRITE=32>";
                break;

            case "Right Stick/Up":
                text = "<SPRITE=33>";
                break;

            case "Right Stick/Right":
                text = "<SPRITE=34>";
                break;

            case "Right Stick/Down":
                text = "<SPRITE=35>";
                break;

            case "Right Stick Press":
                text = "<SPRITE=25>";
                break;

            case "Right Stick/X":
                text = "<SPRITE=27>";
                break;

            case "Right Stick/Y":
                text = "<SPRITE=27>";
                break;

            // OPTIONS -----
            case "Select":
                if (GetCurrentGamepadDevice() == GamepadDevices.playstation)
                    text = "<SPRITE=18>"; // 8 x
                else
                    text = "<SPRITE=8>"; // 18 p
                break;

            case "Start":
                if (GetCurrentGamepadDevice() == GamepadDevices.playstation)
                    text = "<SPRITE=19>"; // 9 x
                else
                    text = "<SPRITE=9>"; // 19 p
                break;

            case "touchpadButton":
                text = "<SPRITE=18>";
                break;

            default:
                break;
        }

        return text;
    }

    
}
