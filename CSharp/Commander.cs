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

        public Commander()
        {
            _currentBoard = new BoardStatus();
        }

        // Add Commands Here.
        // You can only give as many commands as you have un-sunk vessels. Powerup commands do not count against this number. 
        // You are free to use as many powerup commands at any time. Any additional commands you give (past the number of active vessels) will be ignored.

        // Do not alter/remove this method signature
        public List<Command> GiveCommands()
        {
            var commands = new List<Command>();

            if (_currentBoard.TurnsUntilBoardShrink <= 2)
            {
                
            }

            var myShipsCoordinates = _currentBoard.MyVesselStatuses.SelectMany(a => a.Location);

            //move around
            foreach (var ship in _currentBoard.MyVesselStatuses)
            {
                var loc = ship.Location;
                var minX = loc.Min(a => a.X);
                var maxX = loc.Max(a => a.X);
                var minY = loc.Min(a => a.Y);
                var maxY = loc.Max(a => a.Y);

                if (minX <= 5 && minY <= 5)
                {
                    commands.Add(new Command() {vesselid = ship.Id, action = "move:south",});
                    break;
                }
                else if (minX <= 9 && minY <= 5)
                {
                    commands.Add(new Command() {vesselid = ship.Id, action = "move:south",});
                    break;
                }
                else if (minX <= 15 && minY <= 5)
                {
                    commands.Add(new Command() {vesselid = ship.Id, action = "move:west",});
                    break;
                }
                else if (minX <= 5 && minY <= 9)
                {
                    commands.Add(new Command() {vesselid = ship.Id, action = "move:east",});
                    break;
                }
                else if (minX <= 9 && minY <= 9)
                {
                    //chill out
                    //commands.Add(new Command() {vesselid = ship.Id, action = "move:east", });
                }
                else if (minX <= 15 && minY <= 9)
                {
                    commands.Add(new Command() {vesselid = ship.Id, action = "move:west",});
                    break;
                }
                else if (minX <= 5 && minY <= 15)
                {
                    commands.Add(new Command() {vesselid = ship.Id, action = "move:east",});
                    break;
                }
                else if (minX <= 9 && minY <= 15)
                {
                    commands.Add(new Command() {vesselid = ship.Id, action = "move:north",});
                    break;
                }
                else if (minX <= 9 && minY <= 15)
                {
                    commands.Add(new Command() {vesselid = ship.Id, action = "move:north",});
                    break;
                }
            }


            //load up some countermeasures
            foreach (var ship in _currentBoard.MyVesselStatuses)
            {
                if (!ship.CounterMeasuresLoaded && ship.CounterMeasures > 0)
                {
                    //commands.Add(new Command() { vesselid = ship.Id, action = "load_countermeasures" });
                }
            }


            foreach (var xx in _currentBoard.MyVesselStatuses.SelectMany(a => a.SonarReport))
            {
                
            }

            while (commands.Count < _maxCommandCount)
            {
                //commands.Add(new Command { vesselid = 1, action = "fire", coordinate = new Coordinate { X = 1, Y = 1 } });
            }


            //commands = commands.Take(_maxCommandCount).ToList();
            return commands;
        }

        // Do NOT modify or remove! This is where you will receive the new board status after each round.
        public void GetBoardStatus(BoardStatus board)
        {
            _currentBoard = board;
            _maxCommandCount = _currentBoard.MyVesselStatuses.Count(a => a.Health > 0) + 1;
        }

        // This method runs at the start of a new game, do any initialization or resetting here 
        public void Reset()
        {

        }
    }
}