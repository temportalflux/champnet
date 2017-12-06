﻿// Author: Jake Ruth
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// \addtogroup client
/// @{

public enum MenuState
{
    MAIN_MENU,
    ATTACK_MENU,
    ITEM_MENU,
    SWITCH_MENU,
    FORFEIT_MENU,
    WAITING
}

public class BattleUIController : MonoBehaviour
{
    private MenuState _menuState;

    public MenuState menuState
    {
        get { return _menuState; }
        set
        {
            _menuState = value;

            mainMenuGameObject.SetActive(_menuState == MenuState.MAIN_MENU);
            attackMenuGameObject.SetActive(_menuState == MenuState.ATTACK_MENU);
            itemsMenuGameObject.SetActive(_menuState == MenuState.ITEM_MENU);
            switchMenuGameObject.SetActive(_menuState == MenuState.SWITCH_MENU);
            forfeitMenuGameObject.SetActive(_menuState == MenuState.FORFEIT_MENU);
            waitingGameObject.SetActive(_menuState == MenuState.WAITING);

            switch (_menuState)
            {
                case MenuState.MAIN_MENU:
                    break;
                case MenuState.ATTACK_MENU:
                    List<AttackObject> availableAttacks = battleHandler.participant1.currentCretin.GetAvailableAttacks;
                    for (int i = 0; i < attackButtons.Length; i++)
                    {
                        bool hasMatchingAttack = i < availableAttacks.Count;
                        attackButtons[i].interactable = hasMatchingAttack;
                        if (hasMatchingAttack)
                        {
                            Text attackTextBox = attackButtons[i].GetComponentInChildren<Text>();
                            attackTextBox.text = availableAttacks[i].attackName;
                        }
                    }
                    break;
                case MenuState.ITEM_MENU:
                    break;
                case MenuState.SWITCH_MENU:
                    //Debug.Assert(battleHandler.participant1.isPlayer());

                    IList<MonsterDataObject> availableMonsters = battleHandler.participant1.playerController.monsters;
                    for (int i = 0; i < cretinButtons.Length; i++)
                    {
                        bool hasMatchingMonster = i < availableMonsters.Count;
                        cretinButtons[i].interactable = hasMatchingMonster || battleHandler.participant1.currentCretinIndex == i;

                        cretinButtons[i].GetComponentInChildren<Text>().text = hasMatchingMonster ? availableMonsters[i].GetMonsterName : "N/A";
                    }
                    break;
                case MenuState.FORFEIT_MENU:
                    break;
                case MenuState.WAITING:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    [Header("Transform Dependencies")]
    public BattleHandler battleHandler;

    [Header("Base menu holders")]
    public GameObject mainMenuGameObject;
    public GameObject attackMenuGameObject;
    public GameObject itemsMenuGameObject;
    public GameObject switchMenuGameObject;
    public GameObject forfeitMenuGameObject;
    public GameObject waitingGameObject;

    [Header("Attack Variables")]
    public Button[] attackButtons;

    [Header("Switch Variables")]
    public Button[] cretinButtons;

    //[Header("Items Variables")]
    // coming soon. HA....

    //[Header("Forfeit Variables")]

    [Header("Waiting Variables")]
    public Text waitingText;

    void Start()
    {
        menuState = MenuState.MAIN_MENU;
    }

    public void ButtonClicked(uint buttonIndex)
    {
        switch (menuState)
        {
            case MenuState.MAIN_MENU:

                switch (buttonIndex)
                {
                    case 1:
                        menuState = MenuState.ATTACK_MENU;
                        break;
                    case 2:
                        menuState = MenuState.SWITCH_MENU;
                        break;
                    case 3:
                        menuState = MenuState.ITEM_MENU;
                        break;
                    case 4:
                        menuState = MenuState.FORFEIT_MENU;
                        break;
                    default:
                        break;
                }

                break;
            case MenuState.ATTACK_MENU:

                battleHandler.SendBattleOption(true, GameState.Player.EnumBattleSelection.ATTACK, buttonIndex);
                menuState = MenuState.WAITING;

                break;
            case MenuState.ITEM_MENU:

                // Eventually do something... maybe.
                menuState = MenuState.MAIN_MENU;

                break;
            case MenuState.SWITCH_MENU:

                battleHandler.SendBattleOption(true, GameState.Player.EnumBattleSelection.SWAP, buttonIndex);
                menuState = MenuState.WAITING;

                break;
            case MenuState.FORFEIT_MENU:
                switch (buttonIndex)
                {
                    case 1:
                        battleHandler.SendBattleOption(true, GameState.Player.EnumBattleSelection.FLEE, 0);
                        break;
                    default:
                        BackButtonClicked();
                        break;
                }
                break;
            case MenuState.WAITING:
                SetFlavorText("Waiting for opponnet's response");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void BackButtonClicked()
    {
        if (menuState != MenuState.WAITING)
            menuState = MenuState.MAIN_MENU;
    }

    public void SetFlavorText(string text)
    {
        waitingText.text = text;
    }
}
/// @}
