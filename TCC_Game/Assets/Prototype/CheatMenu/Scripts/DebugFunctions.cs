using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugFunctions : MonoBehaviour
{
    #region Debug Commands
    private void Start()
    {
        DebugController.AddCommand("toggle_belly", "Troca se calcula ou não o valor da fome.", "toggle_belly", D_ToggleBelly);
        DebugController.AddCommand("toggle_damage", "Troca se o player pode ou não receber dano.", "toggle_damage", D_ToggleDamage);
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
        var player = GameObject.FindGameObjectWithTag("Player");
        var damage = player.GetComponentInChildren<IsDamagedAndDead>();
        damage.ToggleIsDamageable();
    }
    #endregion
}