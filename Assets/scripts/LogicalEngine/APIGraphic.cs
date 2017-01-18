﻿using UnityEngine;
using System.Collections;

public class APIGraphic{

    LogicalEngine logicalengine;
    GraphicalEngine graphicalengine;

    public APIGraphic(LogicalEngine logicalengine)
    {
        this.logicalengine = logicalengine;
        graphicalengine = GameObject.Find("Graphical").GetComponent<GraphicalEngine>();
    }

    public void MovePlayer(Player player, Vector2 position, bool isonramp)
    {
        player.gameObject.GetComponent<PlayerGraphics>().Player_Move(player.gameObject, position);
    }

    public void MovePlayerFinished(GameObject player_obj)
    {
        logicalengine.graphic_MoveAnimationFinished(player_obj.GetComponent<Player>());
    }
    
    public void MovePlayerToBranch(Player player, Vector2 position, bool isonramp)
    {

    }

    public void MovePlayerOnRamp(Player player, Vector2 position, bool isonramp)
    {

    }

    public void Jump(Player player, Vector2 position)
    {

    }


    public void MoveObject(Unit u)
    {

    }

    public void Lean(Player player)
    {

    }

    public void LeanFinished(Player player)
    {

    }

}
