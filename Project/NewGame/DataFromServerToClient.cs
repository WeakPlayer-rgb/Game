using System;
using System.Collections.Generic;
using System.Drawing;
using NewGame;

namespace NewGame
{
    public class FirstConnection
    {
        public Point Player;
        public Dictionary<Point,IGameObject> Map;
        public List<Bullet> Bullets;
        public List<Point> OtherPlayers;
    }

    public class DataFromServerToClient
    {
        public List<Bullet> Bullets;
        public List<Point> OtherPlayers;
    }

    public class DataFromClientToServer
    {
        public DataFromClientToServer()
        {
            NewPlayerPosition = Point.Empty;
            NewBullets = new List<Bullet>();
        }

        public Point NewPlayerPosition;
        public List<Bullet> NewBullets;
    }
}