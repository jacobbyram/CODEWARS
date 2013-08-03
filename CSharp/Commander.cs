using CruiseControl.Enums;
using CruiseControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CruiseControl
{
    public class Commander
    {
        private BoardStatus _currentBoard;
        private int _maxCommandCount;
        private List<Command> _commands = new List<Command>(); 

        public Commander()
        {
            _currentBoard = new BoardStatus();
            _commands = new List<Command>(); 
        }

        // Add Commands Here.
        // You can only give as many commands as you have un-sunk vessels. Powerup commands do not count against this number. 
        // You are free to use as many powerup commands at any time. Any additional commands you give (past the number of active vessels) will be ignored.

        private List<Command> MoveSouth()
        {
            var commands = new List<Command>();
            foreach (var ship in _currentBoard.MyVesselStatuses)
            {
                commands.Add(new Command() { vesselid = ship.Id, action = "move:south", });
            }

            return commands;
        } 

        // Do not alter/remove this method signature
        public List<Command> GiveCommands()
        {
            var myShipsCoordinates = _currentBoard.MyVesselStatuses.SelectMany(a => a.Location);

            //move towards center (implement broadside move)
            foreach (var ship in _currentBoard.MyVesselStatuses)
            {
                var loc = ship.Location;
                var minX = loc.Min(a => a.X);
                var maxX = loc.Max(a => a.X);
                var minY = loc.Min(a => a.Y);
                var maxY = loc.Max(a => a.Y);

                if (minX <= 5 && minY <= 5)
                {
                    AddCommand(new Command() {vesselid = ship.Id, action = "move:south",});
                }
                else if (minX <= 9 && minY <= 5)
                {
                    AddCommand(new Command() {vesselid = ship.Id, action = "move:south",});
                }
                else if (minX <= 15 && minY <= 5)
                {
                    AddCommand(new Command() {vesselid = ship.Id, action = "move:west",});
                }
                else if (minX <= 5 && minY <= 9)
                {
                    AddCommand(new Command() {vesselid = ship.Id, action = "move:east",});
                }
                else if (minX <= 9 && minY <= 9)
                {
                    //chill out
                    //commands.Add(new Command() {vesselid = ship.Id, action = "move:east", });
                }
                else if (minX <= 15 && minY <= 9)
                {
                    AddCommand(new Command() {vesselid = ship.Id, action = "move:west",});
                }
                else if (minX <= 5 && minY <= 15)
                {
                    AddCommand(new Command() {vesselid = ship.Id, action = "move:east",});
                }
                else if (minX <= 9 && minY <= 15)
                {
                    AddCommand(new Command() {vesselid = ship.Id, action = "move:north",});
                }
                else if (minX <= 9 && minY <= 15)
                {
                    AddCommand(new Command() {vesselid = ship.Id, action = "move:north",});
                }
                else
                {
                    AddCommand(new Command() {vesselid = ship.Id, action = "move:north",});
                }
            }

            //load cms
            foreach (var ship in _currentBoard.MyVesselStatuses)
            {
                if (!ship.CounterMeasuresLoaded && ship.CounterMeasures > 0)
                {
                    AddCommand(new Command() {vesselid = ship.Id, action = "load_countermeasures",});
                }
            }

            //repair appropriately
            foreach (var ship in _currentBoard.MyVesselStatuses)
            {
                if (ship.DamagedSections.Any())
                {
                    var damagedCoordinateIndex = ship.DamagedSections.IndexOf(true);
                    var damagedCoordinate = ship.Location[damagedCoordinateIndex];
                    AddCommand(new Command() {vesselid = ship.Id, action = "repair", coordinate = damagedCoordinate});
                }
            }

            //fire on something you just hit



            //fire at something you can see
            foreach (var ship in _currentBoard.MyVesselStatuses)
            {
                var sonarPings = ship.SonarReport;
                var possibleTargets = sonarPings.Where(a => !myShipsCoordinates.Contains(a));

                foreach (var possibleTarget in possibleTargets)
                {
                    AddCommand(new Command() {action = "fire", vesselid = ship.Id, coordinate = possibleTarget});
                }
            }

            //use powerups
            if (_currentBoard.MyPowerUps.Contains(PowerUpType.ExtraCounterMeasures))
            {
                AddCommand(new Command() {action = "powerup:3"});
            }
            else if (_currentBoard.MyPowerUps.Contains(PowerUpType.BoostRadar))
            {

            }
            else if (_currentBoard.MyPowerUps.Contains(PowerUpType.ClusterMissle))
            {

            }
            else if (_currentBoard.MyPowerUps.Contains(PowerUpType.InstantRepair))
            {

            }






            return _commands;
        }

        private void AddCommand( Command command)
        {
            if (command.action.Contains("powerup"))
            {
                _commands.Add(command);
            }
            else if (_commands.Count(a => !a.action.Contains("powerup")) <= _maxCommandCount)
            {
                _commands.Add(command);
            }
        }

        

        // Do NOT modify or remove! This is where you will receive the new board status after each round.
        public void GetBoardStatus(BoardStatus board)
        {
            _currentBoard = board;
            _maxCommandCount = _currentBoard.MyVesselStatuses.Count(a => a.Health > 0) + 1;
            _commands = new List<Command>(); 
        }

        // This method runs at the start of a new game, do any initialization or resetting here 
        public void Reset()
        {
            _commands = new List<Command>(); 
        }
    }

}