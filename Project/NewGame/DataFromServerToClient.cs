using System;
using System.Collections.Generic;
using System.Drawing;
using NewGame;

namespace NewGame
{
    public class FirstConnection
    {
        public Player Player;
        public Dictionary<Point,Tree> Map;
        public List<Bullet> Bullets;
        public List<Player> OtherPlayers;
    }

    public class DataFromServerToClient
    {
        public List<Bullet> Bullets;
        public List<Player> OtherPlayers;
        public int ChangeHp;
    }

    public class DataFromClientToServer
    {
        public Player NewPlayerPosition;
        public List<Bullet> NewBullets;
    }
}