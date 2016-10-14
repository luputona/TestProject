using UnityEngine;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine.UI;
using System;

public class CloudManager : MonoBehaviour
{
    public Text m_dataText;
    public Text m_stateText;

    GameData slot0;
    bool isSaving;
    
    //ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

    void Start()
    {

        // 슬롯을 초기화한다
        slot0 = new GameData("New game");
        // 구글플레이를 초기화하고 로그인한다
        //PlayGamesClientConfiguration _config = new PlayGamesClientConfiguration.Builder()
        //    .EnableSavedGames()
        //    .Build();
        //PlayGamesPlatform.InitializeInstance(_config);
        //PlayGamesPlatform.Activate();
        PlayGamesPlatform.DebugLogEnabled = false;
        //PlayGamesPlatform.Instance.Authenticate((bool _success) =>
        //{
        //    if (_success)
        //    {
        //        slot0.State = "Authenticated";
        //    }
        //    else
        //    {
        //        Debug.LogWarning("Authentication failed!");
        //    }
        //});

        
    }


    //protected virtual void OnGUI()
    //{
    //    // 슬롯을 선택해 게임을 불러온다
    //    if (GUI.Button(new Rect(0, 0, 100, 50), "Load"))
    //    {
    //        isSaving = false;
    //        ((PlayGamesPlatform)Social.Active).SavedGame.ShowSelectSavedGameUI(
    //            "Select Slot to Load", 2, false, false, SavedGameSelected);
    //    }

    //    // 슬롯을 선택해 게임을 저장한다
    //    if (GUI.Button(new Rect(100, 0, 100, 50), "Save"))
    //    {
    //        slot0.Data = UnityEngine.Random.Range(0, 100).ToString();
    //        isSaving = true;
    //        ((PlayGamesPlatform)Social.Active).SavedGame.ShowSelectSavedGameUI(
    //            "Select Slot to Save", 2, true, true, SavedGameSelected);
    //    }

    //    // 세이브 데이터의 상태와 값을 보여준다
    //    GUI.Box(new Rect(0, 100, 400, 30), slot0.State);
    //    GUI.Box(new Rect(0, 150, 400, 600), slot0.Data);
    //}

    void Update()
    {
        DisplayDataSlot();
    }

    public void SaveGameBtn()
    {
        slot0.Data = UnityEngine.Random.Range(0, 100).ToString();

        isSaving = true;
        ((PlayGamesPlatform)Social.Active).SavedGame.ShowSelectSavedGameUI("Select Slot to Save", 5, true, true, SavedGameSelected1);
        //ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        //savedGameClient.ShowSelectSavedGameUI("Select Slot to Save", 5, true, false, SavedGameSelected1);
        Debug.Log(slot0.Data);
        
    }

    public void LoadGameBtn()
    {
        isSaving = false;
        ((PlayGamesPlatform)Social.Active).SavedGame.ShowSelectSavedGameUI("Select Slot to Load", 5, false, false, SavedGameSelected1);
        //ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        //savedGameClient.ShowSelectSavedGameUI("Select Slot to Load", 5, false, false, SavedGameSelected1);
    }

    void DisplayDataSlot()
    {
        m_dataText.text = string.Format("{0}", slot0.Data);
        m_stateText.text = string.Format("{0}", slot0.State);
    }

    public void SavedGameSelected1(SelectUIStatus _status, ISavedGameMetadata _game)
    {
        if (_status == SelectUIStatus.SavedGameSelected)
        {

            string _filename = _game.Filename;
            // 파일을 읽고 쓰기 전에 열어야만 한다
            ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(_filename, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, SavedGameOpened);
            if (isSaving && (_filename == null || _filename.Length == 0))
            {
                // 새로 저장하기
                _filename = "save" + DateTime.Now.ToBinary();
            }
            if (isSaving)
            {
                // 저장하기
                slot0.State = "Saving to " + _filename;
                Debug.Log("true");
            }
            else
            {
                // 불러오기
                slot0.State = "Loading from " + _filename;
            }
            
        }
        else
        {
            Debug.LogWarning("Error selecting save game: " + _status);
        }

    }

    public void SavedGameOpened(SavedGameRequestStatus _status, ISavedGameMetadata _game)
    {
        if (_status == SavedGameRequestStatus.Success)
        {
            if (isSaving)
            {
                // 스트링 데이터를 바이트로 바꿔서 메타 정보와 함꼐 저장한다
                slot0.State = "Opened, now writing";
                byte[] data = slot0.ToBytes();
                TimeSpan playedTime = slot0.TotalPlayingTime;
                SavedGameMetadataUpdate.Builder builder =
                    new SavedGameMetadataUpdate.Builder()
                        .WithUpdatedPlayedTime(playedTime)
                        .WithUpdatedDescription("Saved Game at " +
                        DateTime.Now);
                SavedGameMetadataUpdate updatedMetadata = builder.Build();
                ((PlayGamesPlatform)Social.Active).SavedGame.CommitUpdate(
                    _game, updatedMetadata, data, SavedGameWritten);
            }
            else
            {
                // 우선 파일을 읽어온다
                slot0.State = "Opened, reading...";
                ((PlayGamesPlatform)Social.Active).SavedGame
                    .ReadBinaryData(_game, SavedGameLoaded);
            }
        }
        else
        {
            Debug.LogWarning("Error opening game: " + _status);
        }
    }

    public void SavedGameLoaded(SavedGameRequestStatus _status, byte[] _data)
    {
        if (_status == SavedGameRequestStatus.Success)
        {
            // 불러온 바이트 데이터를 게임데이터로 바꾼다
            slot0 = GameData.FromBytes(_data);
        }
        else
        {
            Debug.LogWarning("Error reading game: " + _status);
        }
    }

    public void SavedGameWritten(SavedGameRequestStatus _status, ISavedGameMetadata _game)
    {
        if (_status == SavedGameRequestStatus.Success)
        {
            // 성공적으로 저장되었다
            slot0.State = "Saved!";
        }
        else
        {
            Debug.LogWarning("Error saving game: " + _status);
        }
    }

    // 게임 데이터 클래스 - 스트링 데이터를 가지며 바이트로 변환 가능하다
    [Serializable]
    public class GameData
    {

        TimeSpan playingTime;
        DateTime loadedTime;
        string data;
        string state;

        static readonly string HEADER = "GDv1";

        public GameData(string _initData)
        {

            data = _initData;
            state = "Initialized, modified";
            playingTime = new TimeSpan();
            loadedTime = DateTime.Now;

        }

        public TimeSpan TotalPlayingTime
        {
            get
            {
                TimeSpan delta = DateTime.Now.Subtract(loadedTime);
                return playingTime.Add(delta);
            }
        }

        public override string ToString()
        {
            string s = HEADER + ":" + data;
            s += ":" + TotalPlayingTime.TotalMilliseconds;
            return s;
        }

        public byte[] ToBytes()
        {
            return System.Text.ASCIIEncoding.Default.GetBytes(ToString());
        }

        public static GameData FromBytes(byte[] bytes)
        {
            return FromString(System.Text.ASCIIEncoding.Default.GetString(bytes));
        }

        public static GameData FromString(string _s)
        {
            GameData _gd = new GameData("initializing from string");
            string[] p = _s.Split(new char[] { ':' });
            if (!p[0].StartsWith(HEADER))
            {
                Debug.LogError("Failed to parse game data from: " + _s);
                return _gd;
            }
            _gd.data = p[1];
            double val = Double.Parse(p[2]);
            _gd.playingTime = TimeSpan.FromMilliseconds(val > 0f ? val : 0f);

            _gd.loadedTime = DateTime.Now;
            _gd.state = "Loaded successfully";
            return _gd;
        }

        public string Data
        {
            get
            {
                return data;
            }

            set
            {
                data = value;
                state += ", modified";
            }

        }

        public string State
        {
            get
            {
                return state;
            }

            set
            {
                state = value;
            }
        }
    }
}