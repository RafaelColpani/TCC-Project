using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region COMMANDS
/// <summary>Perform the interaction with environment, given the PlayerInteract script.</summary>
public class InteractionCommand : ICommand
{
    private PlayerInteract playerInteract;

    public InteractionCommand(PlayerInteract playerInteract)
    {
        this.playerInteract = playerInteract;
    }

    public void Execute(Transform actor, float value = 1, CharacterController characterController = null)
    {
        this.playerInteract.ExecuteInteractionCommand();
    }
}

/// <summary>Perform the skip dialogue interaction, given the DialogueReader and the NPCInteractable scripts.</summary>
public class SkipDialogueCommand : ICommand
{
    private DialogueReader dialogueReader;
    private NpcInteractable npcInteractable;

    public void Execute(Transform actor, float value = 1, CharacterController characterController = null)
    {
        if (dialogueReader == null)
            dialogueReader = GameObject.FindGameObjectWithTag("dialogueManager").GetComponent<DialogueReader>();

        if (npcInteractable == null)
            npcInteractable = GameObject.Find("NPC").GetComponent<NpcInteractable>();

        if (dialogueReader != null)
        {
            if (!npcInteractable.canTalk)
                this.dialogueReader.ExecuteCommmand();
        }
    }
}

/// <summary>Perform the drop item from inventory command, given the InventoryManager script.</summary>
public class DropItemCommand : ICommand
{
    InventoryManager inventoryManager;

    public DropItemCommand(InventoryManager inventoryManager)
    {
        this.inventoryManager = inventoryManager;
    }

    public void Execute(Transform actor, float value = 1, CharacterController characterController = null)
    {
        this.inventoryManager.ExecuteDropCommand();
    }
}
#endregion
