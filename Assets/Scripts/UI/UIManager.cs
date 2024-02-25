using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerStatBar playerStatBar;
    [Header("�¼�����")]
    public CharacterEventSO characterEvent;

    private void OnEnable()
    {
        characterEvent.OnEventRaised += OnHealthEvent;
    }

    private void OnDisable()
    {
        characterEvent.OnEventRaised -= OnHealthEvent;
    } 

    private void OnHealthEvent(Character character)
    {
        var persentage = character.CurrentHealth / character.maxHealth;
        playerStatBar.OnHealthChange(persentage);
        playerStatBar.OnPowerChange(character);
    }
}
