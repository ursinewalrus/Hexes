﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexes.Actors
{
    public class ActionHandler
    {
        public static Dictionary<string, Dictionary<ActionArgs, string>> ActionsList = new Dictionary<string, Dictionary<ActionArgs, string>>();

        public static ActionArgs ConvertActionArg(string arg)
        {
            switch (arg)
            {
                case "type":
                    return ActionArgs.Type;
                case "baseDamage":
                    return ActionArgs.BaseDamage;
                case "blockable":
                    return ActionArgs.Blockable;
                case "effectShape":
                    return ActionArgs.EffectShape;
                case "effectRange":
                    return ActionArgs.EffectRange;
                case "texture":
                    return ActionArgs.Texture;
                case "moduleName":
                    return ActionArgs.ModuleName;
                case "instant":
                    return ActionArgs.Instant;
                default:
                    throw new Exception("Invalid XML action parameter " + arg);
            }
        }
    }

    public enum ActorTurnState
    {
        WaitingForTurn,
        OnTurn,
        TurnDone
    }

    public enum APUseType
    {
        TotalAp,
        Movement,
        Attack,
        Defend,
        Rotation,
        Idle
    }


    public enum ActionArgs
    {
        Type,
        BaseDamage,
        Blockable,
        EffectShape,
        EffectRange,
        Texture,
        Instant,
        ModuleName,
        /// <summary>
        /// The hex cord target
        /// </summary>
        TargetHexCord
    }
}
