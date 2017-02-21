using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Sfs2X;
using Sfs2X.Logging;
using Sfs2X.Util;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;



public class LoginPanel : MonoBehaviour {

    public string HostIp = "192.168.0.69";
    public int TcpPort = 9933;
    public int WsPort = 8888;
    public string ZoneName = "BasicExamples";

    public Button _connectButton;
    public InputField _userNameField;
    public InputField _passwordField;

    SmartFox _smartFoxServer;

    void Awake()
    {
        Application.runInBackground = true;

#if UNITY_WEBPLAYER
		if (!Security.PrefetchSocketPolicy(Host, TcpPort, 500)) {
			Debug.LogError("Security Exception. Policy file loading failed!");
		}
#endif

    }

    // Use this for initialization
    void Start () {

        _connectButton.onClick.AddListener(ClickConnect);
	
	}

    void ClickConnect()
    {
        _connectButton.interactable = true;

        ConfigData cfg = new ConfigData();
        cfg.Host = HostIp;
#if !UNITY_WEBGL
        cfg.Port = TcpPort;
#else
        cfg.Port = WsPort;
#endif
        cfg.Zone = ZoneName;

#if !UNITY_WEBGL
        _smartFoxServer = new SmartFox();
#else
        _smartFoxServer = new SmartFox(UseWebSocket.WS);
#endif

        _smartFoxServer.ThreadSafeMode = true;

        _smartFoxServer.AddEventListener(SFSEvent.CONNECTION, OnConnection);
        _smartFoxServer.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
        _smartFoxServer.AddEventListener(SFSEvent.LOGIN, OnLogin);
        _smartFoxServer.AddEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);

        _smartFoxServer.Connect(cfg);

        DebugConsole.Log("Connecting...");
    }



    // Update is called once per frame
    void Update () {

        if (_smartFoxServer != null)
            _smartFoxServer.ProcessEvents();
	}

    void ResetLogin()
    {
        if (_smartFoxServer != null)
            _smartFoxServer.RemoveAllEventListeners();

        _connectButton.interactable = true;
    }



    void OnConnection(BaseEvent evt)
    {
        bool succeed = (bool)evt.Params["success"];

        if(succeed)
        {
            DebugConsole.Log("Connected.");

            SmartFoxConnection.Connection = _smartFoxServer;

            // Login
            var data = new SFSObject();
            data.PutUtfString("ClearPass", _passwordField.text);
            _smartFoxServer.Send(new Sfs2X.Requests.LoginRequest(_userNameField.text, _passwordField.text, ZoneName, data));
        }
        else
        {
            DebugConsole.Log("Failed", "error");

            ResetLogin();
        }
    }

    void OnConnectionLost(BaseEvent evt)
    {
        ResetLogin();

        string reason = (string)evt.Params["reason"];

        if (reason != ClientDisconnectionReason.MANUAL)
        {
            // Show error message
            string text = "Connection was lost; reason is: " + reason;
            DebugConsole.Log(text, "error");
        }
    }

    void OnLogin(BaseEvent evt)
    {
        // Remove SFS2X listeners and re-enable interface
        ResetLogin();

        //evt.Params["userName"]
        //TODO

        print(evt.ToString());

        var data = (SFSObject)evt.Params["data"];

        int coin = (int)data.GetInt("Coin");        

        print("Coin: " + coin.ToString());
        print("USer: " + ((SFSUser)evt.Params["user"]).Name);

        DebugConsole.Log("Coin: " + coin.ToString());

        // Load lobby scene
        SceneManager.LoadScene("LobbyScene");
    }

    void OnLoginError(BaseEvent evt)
    {
        _smartFoxServer.Disconnect();

        ResetLogin();

        string errorMessage = "Login failed: " + (string)evt.Params["errorMessage"];
        DebugConsole.Log(errorMessage, "error");
    }
}
