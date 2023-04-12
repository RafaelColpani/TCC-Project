// Created by: Henrique Assis
// Date: 28/02/2023

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviour
{
    #region VARS
    private static List<object> commandList;

    private Vector2 scroll = Vector2.zero;
    private Vector2 scrollPosition = Vector2.zero;
    private bool showConsole;
    private bool showHelp;
    private string debugInput;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        commandList = new List<object>();

        AddCommand("help", "Mostra todos os comandos dispon�veis para debug", "help", () => { showHelp = !showHelp; });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))  OnToggleDebug();
        if (Input.GetKeyDown(KeyCode.Return)) OnReturn();
    }
    #endregion

    #region Static Methods
    public static void AddCommand<T1>(string commandId, string commandDescription, string commandFormat, Action<T1> command)
    {
        commandList.Add(new DebugCommand<T1>(commandId, commandDescription, commandFormat, command));
    }

    public static void AddCommand(string commandId, string commandDescription, string commandFormat, Action command)
    {
        commandList.Add(new DebugCommand(commandId, commandDescription, commandFormat, command));
    }
    #endregion

    #region Methods
    private void HandleInput()
    {
        string[] properties = debugInput.Split(' ');

        foreach(DebugCommandBase command in commandList)
        {
            DebugCommandBase commandBase = command as DebugCommandBase;

            if (!debugInput.Contains(commandBase.CommandId)) continue;

            if (command as DebugCommand != null)
            {
                (command as DebugCommand).Invoke();
                return;
            }

            else if (command as DebugCommand<int> != null)
            {
                (command as DebugCommand<int>).Invoke(int.Parse(properties[1]));
                return;
            }
        }
    }
    #endregion

    #region GUI
    public void OnToggleDebug() => showConsole = !showConsole;

    public void OnReturn()
    {
        if (!showConsole) return;

        HandleInput();
        debugInput = string.Empty;
    }

    private void OnGUI()
    {
        if (!showConsole) return;

        float yPos = Screen.height - 70f;
        float xPos = 10f;
        float boxWidth = (Screen.width / 2) - 200f;
        Color bgColor = new Color(0, 0, 0, 0);

        GUIStyle labelStyle = GUIStyle.none;
        labelStyle.fontSize = 25;
        labelStyle.wordWrap = true;
        labelStyle.normal.textColor = Color.white;

        GUI.skin.textField.fontSize = 25;
        if (showHelp)
        {
            // help window
            GUI.Box(new Rect(xPos, yPos - 400, boxWidth, 400), "");
            Rect viewport = new Rect(xPos, yPos - 400, boxWidth - 30, 0);

            float labelHeight = 0;
            const int lineBreak = 30;

            // calculate viewport height
            foreach (DebugCommandBase command in commandList)
            {
                string label = $"{command.CommandFormat} -> {command.CommandDescription}";

                var lineCount = (int)Math.Ceiling(labelStyle.CalcHeight(new GUIContent(label), viewport.width - 30) / labelStyle.lineHeight);
                labelHeight += (lineBreak * lineCount) + lineBreak;
            }

            viewport.height = labelHeight;
            labelHeight = 0;

            // calculate labels and scrollbar
            this.scroll = GUI.BeginScrollView(new Rect(0, yPos - 385, boxWidth, 365), scroll, viewport);
            foreach (DebugCommandBase command in commandList)
            {
                string label = $"{command.CommandFormat} -> {command.CommandDescription}";
                Rect labelRect = new Rect(xPos + 30, viewport.y + labelHeight, viewport.width - 30, 365);

                GUI.Label(labelRect, label, labelStyle);
                var lineCount = (int)Math.Ceiling(labelStyle.CalcHeight(new GUIContent(label), viewport.width - 30) / labelStyle.lineHeight);
                labelHeight += (lineBreak * lineCount) + lineBreak;
            }
            GUI.EndScrollView();
        }

        // text input
        GUI.Box(new Rect(xPos, yPos, boxWidth, 60), "");
        GUI.backgroundColor = bgColor;
        debugInput = GUI.TextField(new Rect(xPos + 10f, yPos + 10, boxWidth - 20f, 40f), debugInput);
    }
    #endregion
}
