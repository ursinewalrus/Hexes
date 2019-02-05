using Hexes.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hexes.UI;
using Hexes.Utilities;

namespace Hexes.Actors
{
    public class BasicActor : Drawable, IMovable
    {
        #region properites and setup
        public string Name;
        public HexPoint Location { get; set; }
        public Boolean Controllable;
        public int MoveDistance;
        public int MoveDistanceRemaining;
        public int SightRange;
        public Texture2D Texture;
        public int HP { get; set; }
        public string ModuleName;
        public static float SizeX;
        public static float SizeY;
        public int Rotation;
        public int Speed;
        public List<HexPoint> HexesCanSee = new List<HexPoint>();
        public AIController AIController = null;
        public ActorTurnState TurnState = ActorTurnState.WaitingForTurn;
        public ActorFactions Faction;
        //maybe this should somehow be made global this is getting silly
        public HexGrid.HexGrid ActorHexGrid;

        public List<string> DefaultActions = new List<string>();
        public List<Dictionary<ActionArgs, string>> QueuedActions = new List<Dictionary<ActionArgs, string>>();

        //make dif for attack and def?
        public int ActionPoints;
        public int MaxMoveAp;
        //:TODO maybe enum the values, but where
        public Dictionary<CurrentTurn, int> ActiveTurnState = new Dictionary<CurrentTurn, int>()
        {
            {CurrentTurn.RemainingMoves, 0},
            {CurrentTurn.RemainingActions, 0 }
        };

        public static Dictionary<int, List<Vector2>> FoVArms = new Dictionary<int, List<Vector2>>()
        {
            //rotation, <left arm, right arm>
            {0, new List<Vector2>(){new Vector2(-1,0),new Vector2(0,1)}},
            {1, new List<Vector2>(){new Vector2(-1,1),new Vector2(1,0)}}, //rotation 1
            {2, new List<Vector2>(){new Vector2(0,1),new Vector2(1,-1)}}, // 3 o clock
            {3, new List<Vector2>(){new Vector2(1,0),new Vector2(0,-1)}},
            {4, new List<Vector2>(){new Vector2(1,-1),new Vector2(-1,0)}},
            {5, new List<Vector2>(){new Vector2(0,-1),new Vector2(-1,1)}}
        };

        public BasicActor(HexPoint location, string name, Dictionary<string, string> actorData, int rotation, bool PC, string moduleName, HexGrid.HexGrid hexGrid)
        {
            Name = name;
            Location = location;
            Controllable = PC;
            Rotation = rotation;
            ModuleName = moduleName;
            ActorHexGrid = hexGrid;
            AsignActorData(actorData);
        }
        public void AsignActorData(Dictionary<string, string> actorData)
        {
            MoveDistance = Convert.ToInt32(actorData["moveDistance"]);
            HP = Convert.ToInt32(actorData["defaultHP"]);
            Speed = Convert.ToInt32(actorData["speed"]);
            DefaultActions = actorData["defaultActions"].Split(',').ToList();
            //need better faction logic
            if (Controllable)
                Faction = ActorFactions.Player;
            else
                Faction = ActorFactions.Enemy1;

            string assetPath = @"Modules\" + ModuleName + @"\" + actorData["texture"];
            FileStream fs = new FileStream(assetPath, FileMode.Open);
            Texture = Texture2D.FromStream(GraphicsDevice, fs);
            fs.Dispose();
            SizeX = Convert.ToInt32(actorData["bottomrightX"]);
            SizeY = Convert.ToInt32(actorData["bottomrightY"]);
            MaxMoveAp = Convert.ToInt32(actorData["maxMoveAp"]);
            ActionPoints = Convert.ToInt32(actorData["ap"]);

            SightRange = 9;
        }
        #endregion

        public void StartTurn()
        {
            ActiveTurnState[CurrentTurn.RemainingMoves] = MaxMoveAp;
            ActiveTurnState[CurrentTurn.RemainingActions] = ActionPoints - MaxMoveAp;
            TurnState = ActorTurnState.OnTurn;

        }

        #region move related
        public List<HexPoint> MoveableInMoveRange()
        {
            //https://www.redblobgames.com/grids/hexagons/#range
            //var possibleMoves = new List<HexPoint>();
            var visited = new List<HexPoint>();
            var toVisit = new List<HexPoint>[MoveDistance + 1];
            for (var i=0 ; i<toVisit.Count();i++)
            {
                toVisit[i] = new List<HexPoint>();
            }
            toVisit[0].Add(Location);

            for (var i = 0; i < MoveDistance; i++)
            {
                foreach (var h in toVisit[i])
                {
                    var neighbors = ActorHexGrid.GetNeighbors(h);
                    foreach (var n in neighbors)
                    {
                        if (!n.Value.BlocksMovment && !visited.Contains(n.Key) && !ActorHexGrid.ActorStorage.Any(a => a.Location.Equals(n.Key)))
                        {
                            visited.Add(n.Key);
                            toVisit[i + 1].Add(n.Key);
                        }
                    }
                }
            }
            return visited;
        }

        public void MoveTo(object sender, ActorMoveActionEvent eventArgs)
        {
            MoveTo(eventArgs.Location);
        }
        public void MoveTo(HexPoint moveTo)
        {
            if (CanMoveTo(moveTo) && ActiveTurnState[CurrentTurn.RemainingMoves] > 0)//should be checked elsewhere for UI reasons
            {
                Location = moveTo;
                ActorHexGrid.UnHighlightAll();
                ActorHexGrid.DebugLines = new List<DebugLine>();
                UseAp(APUseType.Movement);
            }
        }

        //make another type for def
        public void UseAp(APUseType actionType)
        {
            if (actionType == APUseType.Movement)
            {
                ActiveTurnState[CurrentTurn.RemainingMoves]--;
            }
            else if(actionType == APUseType.Attack)
            {
                ActiveTurnState[CurrentTurn.RemainingActions]--;
            }
            if (ActiveTurnState[CurrentTurn.RemainingMoves] + ActiveTurnState[CurrentTurn.RemainingActions] == 0)
            {
                TurnState = ActorTurnState.TurnDone;
            }
        }

        public Boolean CanMoveTo(HexPoint moveTo)
        {
            //split for maybe sep messages
            //also cant be straight line, need to pathfind to it
            if (MoveableInMoveRange().Contains(moveTo))
            {
                return true;
            }
            return false;
        }

        public void Rotate(object sender, ActorRotateActionEvent eventArgs)
        {
            Rotate(eventArgs.ClockWise);
        }

        public void Rotate(bool clockwise)
        {
            var dir = clockwise ? 1 : -1;
            Rotation = (Rotation + dir + 6) % 6;
        }

        public void Rotate(int dir)
        {
            //cap on this if rotation costs
            Rotation = dir;
        }
        #endregion

        /// <summary>
        /// If no param for coneArmLength given, what can be seen given sightrange, otherwise what in cone of given arm length
        /// </summary>
        /// <param name="hexGrid"></param>
        /// <param name="coneArmLength"></param>
        /// <returns></returns>
        public List<HexPoint> CanSee(int coneArmLength = -1)
        {
            if (coneArmLength == -1)
            {
                coneArmLength = SightRange;
            }
            HexesCanSee = new List<HexPoint>();
            //invent hexes for ones that dont exist for the purpose of our calculations
            var lookDirArms = FoVArms[Rotation];

            var lArmR = Location.R + (int)lookDirArms[0].X * coneArmLength;
            var lArmQ = Location.Q + (int)lookDirArms[0].Y * coneArmLength;

            var rArmR = Location.R + (int)lookDirArms[1].X * coneArmLength;
            var rArmQ = Location.Q + (int)lookDirArms[1].Y * coneArmLength;

            var leftArmCord = new HexPoint(lArmR, lArmQ);
            var rightArmCord = new HexPoint(rArmR, rArmQ);

            var shiftRight = (Rotation > 0 && Rotation < 4) ? true : false;
            var furthestEdge = HexGrid.HexGrid.LineBetweenTwoPoints(leftArmCord, rightArmCord, shiftRight);
            foreach (var hexPoint in furthestEdge)
            {
                var ray = HexGrid.HexGrid.LineBetweenTwoPoints(Location, hexPoint);
                //:TODO REMOVE after testing
                var startLinePointHex = ActorHexGrid.HexStorage.First(h => h.Key.Equals(ray[0])).Value;
                var endLinePoint = ActorHexGrid.HexStorage.First(h => h.Key.Equals(ray[0])).Value;
                //
                var debugLine = new DebugLine();

                var blocked = false;

                //should be drawing away from the start point
                foreach (var rayPoint in ray)
                {
                    if (!blocked)
                    {
                        if (ActorHexGrid.HexStorage.ContainsKey((rayPoint)))
                        {
                            var hex = ActorHexGrid.HexStorage.First(h => h.Key.Equals(rayPoint));
                            if (!hex.Value.BlocksVision)
                            {
                                debugLine.DebugStrings[hex.Value.Center] = hex.Key.R + " " + hex.Key.Q;
                                HexesCanSee.Add(hex.Key);
                                endLinePoint = hex.Value;
                            }
                            else
                            {
                                blocked = true;
                            }
                        }
                        else
                        {
                            blocked = true;
                        }
                    }
                }
                var debugLineLine = new Line(startLinePointHex.Center.X, startLinePointHex.Center.Y,
                    endLinePoint.Center.X, endLinePoint.Center.Y, 3f, Color.Black);
                debugLine.Line = debugLineLine;
                ActorHexGrid.DebugLines.Add(debugLine);
            }
            return HexesCanSee;
        }

        public void DoAttack(Dictionary<ActionArgs,string> attackArgs)
        {
            //
            //do validitiy check, if invalid just exit
            var targetable = CanSee(Int32.Parse(attackArgs[ActionArgs.EffectRange]));
            if (!targetable.Any(h => h.Equals(HexPoint.StringToHexPoint(attackArgs[ActionArgs.TargetHexCord]))) 
                || !CanSee().Any(h => h.Equals(HexPoint.StringToHexPoint(attackArgs[ActionArgs.TargetHexCord]))))
            {
                return;
            }
            //
            //attackArgs[ActionArgs.TargetHexCord] = hexPoint.ToString();
            if (Convert.ToBoolean(attackArgs[ActionArgs.Instant]) == false)
            {
                QueuedActions.Add(attackArgs);
            }
            else
            {
                ResolveAction(attackArgs);
            }
        }

        public void ResolveAction(Dictionary<ActionArgs, string> attackArgs)
        {
            //assume any checks are done by now for if something is a valid target
            var hexPointTarget = HexPoint.StringToHexPoint(attackArgs[ActionArgs.TargetHexCord]);

        }

        #region action handling
        public void DoAction(object sender, ActorDoActionActionEvent eventArgs)
        {
            switch (eventArgs.ActionArgs[ActionArgs.Type])
            {
                case "attack":
                    DoAttack(eventArgs.ActionArgs);
                    break;
                case "move":
                    MoveTo(ActorHexGrid.ActiveHex.Key);
                    break;
                case "rotateC":
                    Rotate(true);
                    break;
                case "rotateCC":
                    Rotate(false);
                    break;

            }
        }

        #endregion
       
        #region AIControll related

        public void SetAIController(HexGrid.HexGrid grid)
        {
            AIController = new AIController(grid);
        }

        public void UseAIMoveAction()
        {
            AIController.Wander(this);
        }

        #endregion

        #region drawstuff
        //maybe also a draw that just takes the R/Q cords?
        public void Draw(Vector2 center)
        {
            //special draw for factions :TODO
            if (Controllable)
            {

                Sb.DrawString(Font, "A", center - new Vector2(50,50) , Color.Black );
            }
            Sb.Draw(Texture,
                    destinationRectangle: new Rectangle((int)center.X, (int)center.Y, (int)SizeX, (int)SizeY),
                    sourceRectangle: new Rectangle(0, 0, (int)SizeX, (int)SizeY),
                    color: Color.White,
                    rotation: (MathHelper.PiOver2 / 3.0f) + (MathHelper.PiOver2 / 1.5f ) * Rotation,
                    origin: new Vector2(SizeX/2,SizeY/2)
                );
        }
        public void Draw(FloatPoint center, Vector2 size)
        {
            Sb.Draw(Texture,
                    destinationRectangle: new Rectangle((int)center.X, (int)center.Y, (int)size.X, (int)size.Y),
                    sourceRectangle: new Rectangle(0, 0, (int)SizeX, (int)SizeY),
                    color: Color.White,
                    rotation: (MathHelper.PiOver2 / 3.0f) + (MathHelper.PiOver2 / 1.5f) * Rotation,
                    origin: new Vector2(size.X / 2, size.Y / 2)
                );
        }
        #endregion
    }
}
