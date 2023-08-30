using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugFunctions : MonoBehaviour
{
    #region Debug Commands
    private void Start()
    {
        DebugController.AddCommand("toggle_belly", "Troca se calcula ou não o valor da fome.", "toggle_belly", D_ToggleBelly);
        DebugController.AddCommand("toggle_damage", "Troca se o player pode ou não receber dano.", "toggle_damage", D_ToggleDamage);

        DebugController.AddCommand("artifact", "Da spawn em todos os artefatos na base.", "artifact", D_ArtefatosSpawn);

        DebugController.AddCommand("reset_game", "Reseta todo o jogo para o seu estado inicial.", "reset_game", D_ResetGame);

        DebugController.AddCommand("god_mode", "Da o modo Deus para o player.", "god_mode", D_GodMode);

        //DebugController.AddCommand("teleport_base", "Teleporta o player para a base.", "teleport_base", D_teleportBase);

    }
    #endregion

    #region Debug Functions
    public void D_ToggleBelly()
    {
        var playerBelly = FindObjectOfType<Belly>();
        playerBelly.ToggleCalculateBelly();
    }

    public void D_ToggleDamage()
    {
        //var player = GameObject.FindGameObjectWithTag("Player");
        var player = GameObject.Find("pfb_playerEntregaFinal");
        var damage = player.GetComponentInChildren<IsDamagedAndDead>();
        damage.ToggleIsDamageable();
    }

    public void D_ArtefatosSpawn()
    {
        var summerArtifact = GameObject.Find("spr_artifact_summer");
        var autumnArtifact = GameObject.Find("spr_artifact_autumn");
        var winterArtifact = GameObject.Find("spr_artifact_winter");

        summerArtifact.gameObject.transform.position = new Vector3(-26.48f, -3.2f, 0f);

        autumnArtifact.gameObject.transform.position = new Vector3(-47.99999f, -3.07f, 0f);

        winterArtifact.gameObject.transform.position = new Vector3(-73.03f, -3.27f, 0f);
    }

    public void D_ResetGame()
    {
        DialogueConditions.hasSummer = false;
        DialogueConditions.hasAutumn = false;
        DialogueConditions.hasWinter = false;

        NpcInteractable.timesTalked = 0;
        
        SceneManager.LoadScene("LoadRoom");
    }

    public void D_GodMode()
    {
        var player = GameObject.Find("pfb_playerEntregaFinal");
        var damage = player.GetComponentInChildren<IsDamagedAndDead>();
        var playerBelly = FindObjectOfType<Belly>();

        damage.ToggleIsDamageable();
        playerBelly.ToggleCalculateBelly();
    }

    /*
    public void D_teleportBase()
    {
        //var player = GameObject.Find("/pfb_playerEntregaFinal/spr_player2/Body");
        var player = GameObject.Find("pfb_playerEntregaFinal");
        var playerSPR = GameObject.Find("pfb_playerEntregaFinal/spr_player2");
        var playerBody = GameObject.Find("pfb_playerEntregaFinal/spr_player2/Body");

        player.gameObject.transform.position = new Vector3(0f, 0f, 0f);
        playerSPR.gameObject.transform.position = new Vector3(0f, 0f, 0f);
        playerBody.gameObject.transform.position = new Vector3(0f, 0f, 0f);
    }
    */

    #endregion
}