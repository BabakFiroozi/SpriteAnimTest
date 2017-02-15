using System;
using System.Collections.Generic;


public class GameUser
{
    static GameUser s_instance;

    int _userName;
    int _gold;

    public int UserName
    {
        get { return _userName; }
        set { _userName = value; }
    }

    public int Gold
    {
        get { return _gold; }
        set { _gold = value; }
    }

    public GameUser Instance
    {
        get
        {
            if (s_instance == null)
                s_instance = new GameUser();
            return s_instance;
        }
    }



}
