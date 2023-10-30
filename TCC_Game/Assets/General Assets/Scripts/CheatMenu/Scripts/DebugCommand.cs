// Created by: Henrique Assis
// Date: 28/02/2023

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCommandBase
{
    private string _commandId;
    private string _commandDescription;
    private string _commandFormat;

    public string CommandId { get { return _commandId;} }
    public string CommandDescription { get { return _commandDescription;} }
    public string CommandFormat { get { return _commandFormat;} }

    public DebugCommandBase(string commandId, string commandDescription, string commandFormat)
    {
        _commandId = commandId;
        _commandDescription = commandDescription;
        _commandFormat = commandFormat;
    }
}

public class DebugCommand : DebugCommandBase
{
    private Action command;

    public DebugCommand(string commandId, string commandDescription, string commandFormat, Action command) : base(commandId, commandDescription, commandFormat)
    {
        this.command = command;
    }

    public void Invoke()
    {
        if (command != null) 
            this.command.Invoke(); 
    }
}

public class DebugCommand<T1> : DebugCommandBase
{
    private Action<T1> command;

    public DebugCommand(string commandId, string commandDescription, string commandFormat, Action<T1> command) : base(commandId, commandDescription, commandFormat)
    {
        this.command = command;
    }

    public void Invoke(T1 value)
    {
        if (command != null)
            this.command.Invoke(value);
    }
}
