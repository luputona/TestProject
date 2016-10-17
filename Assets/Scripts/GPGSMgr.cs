using UnityEngine;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine.UI;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;



//외부에서 불러오는 데이타들을 SaveData로 넣는다
//함수 형태로 만들어서 인자로 받아오게 만든다.
//SaveData에 필요한것 
//1.플레이어 정보
//2.캐릭터 정보  - 기본스펙, 캐릭+기본 합친 종합스펙
//3.인벤토리 정보
//4.
[Serializable]
public class SaveData
{
    public string PlayerName;// { get; set; }
    public float Playerhealth { get; set; }
    public int PlayerScore { get; set; }
    
    
    // Convert class instance to byte array
    public static byte[] ToBytes (SaveData data)
    { 
        BinaryFormatter formatter = new BinaryFormatter();
        using(MemoryStream stream = new MemoryStream())
        {
            formatter.Serialize(stream, data);
            return stream.ToArray();
        }
    }

    // Convert byte array to class instance
    public static SaveData FromBytes (byte [] data)
    {
        using(MemoryStream stream = new MemoryStream())
        {
            BinaryFormatter formatter = new BinaryFormatter();
            stream.Write(data, 0, data.Length);
            stream.Seek(0, SeekOrigin.Begin);

            SaveData block = (SaveData)formatter.Deserialize(stream);
            return block;
        }
    }
    public SaveData()
    {

    }
    public SaveData(string _playername, float _playerhealth, int _playerscore)
    {
        PlayerName = _playername;
        Playerhealth = _playerhealth;
        PlayerScore = _playerscore;
    }
}



public class GPGSMgr : Singleton<GPGSMgr> {

    public bool m_bLogin { get; set; }

    public Text[] m_statText;    
    public int count;
    public float health;


    public string   m_playername;
    public float    m_playerhealth;
    public int      m_playerscore;


    SaveData[] savedata;// = new SaveData[10];

    private ISavedGameMetadata m_currentGame = null;

    void Awake()
    {
        if (m_instance != null)
        {
            GameObject.Destroy(gameObject);
        }
        else
        {
            GameObject.DontDestroyOnLoad(gameObject);
            m_instance = this;
        }
        
    }

    void Start()
    {
        InitializeGPGS();
        
        count = 0;
        health = 100;
    }

    public void InitializeGPGS()
    {
        m_bLogin = false;
        
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        //if(!Social.localUser.authenticated)
        //{
        //    Social.localUser.Authenticate((bool success) =>
        //    {
        //        if(success)
        //        {
        //            LoadCloud();
        //        }
        //    });
        //}

        //if (!Social.localUser.authenticated)
        //{
        //    Social.localUser.Authenticate(LoginCallBackGPGS);
        //    LoadGame();
        //}
        
    }

    public void LoginGPGS()
    {
        //Social.localUser.Authenticate((bool success) =>
        //{
        //    if (success)
        //    {
        //        m_bLogin = true;
        //    }
        //    else
        //    {
        //        m_bLogin = false;
        //    }
        //});
        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate(LoginCallBackGPGS);
            LoadGame();
        }
            
    }

    public void LoginCallBackGPGS(bool result)
    {
        m_bLogin = result;
    }
    public void LogoutGPGS()
    {
        // 로그인이 되어 있으면
        if (m_bLogin)
        {            
            ((GooglePlayGames.PlayGamesPlatform)Social.Active).SignOut();
            m_bLogin = false;
        }
    }

    public void ReadSaveGame(string filename, Action<SavedGameRequestStatus, ISavedGameMetadata> callback) 
    {
        if(m_bLogin)
        {
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

            savedGameClient.OpenWithAutomaticConflictResolution(filename, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, callback);
        }

    }

    public void WriteSaveGame(ISavedGameMetadata game, byte[] saveData, Action<SavedGameRequestStatus,ISavedGameMetadata> callback)
    {
        if(m_bLogin)
        {
            SavedGameMetadataUpdate updateMetData = new SavedGameMetadataUpdate.Builder().WithUpdatedPlayedTime(TimeSpan.FromMinutes(game.TotalTimePlayed.Minutes + 1)).WithUpdatedDescription("Save at : " + DateTime.Now).Build();

            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            savedGameClient.CommitUpdate(game, updateMetData, saveData, callback);
        }
    }

    public void SaveGame()
    {
        if(m_bLogin)
        {
            Action<SavedGameRequestStatus, ISavedGameMetadata> writeCallback = (SavedGameRequestStatus status, ISavedGameMetadata game) =>
                {
                    if(status == SavedGameRequestStatus.Success)
                    {                        
                    }
                };
            //read binary callback
            Action<SavedGameRequestStatus, byte[]> readBinaryCallback = (SavedGameRequestStatus status, byte[] data) =>
                {                    
                };
            //read game callback
            Action<SavedGameRequestStatus, ISavedGameMetadata> readCallback = (SavedGameRequestStatus status, ISavedGameMetadata game) =>
                {
                    if (status == SavedGameRequestStatus.Success)
                    {
                        m_currentGame = game;
                        PlayGamesPlatform.Instance.SavedGame.ReadBinaryData(game, readBinaryCallback);
                    }
                };

            // Create new save data
            //SaveData saveData = new SaveData(GetNameGPGS(), health , count);
            SaveData saveData = new SaveData();
            saveData.PlayerName = GetNameGPGS();
            saveData.Playerhealth = health;
            saveData.PlayerScore = count;

            //savedata[1] = new SaveData(GetNameGPGS(), health+10, count+10);
            //{
                // These values are hard coded for the purpose of this tutorial.
                // Normally, you would replace these values with whatever you want to save.
                
            //};

            // Replace "MySaveGame" with whatever you would like to save file to be called
            ReadSaveGame("MySaveGame", readCallback);
            WriteSaveGame(m_currentGame, SaveData.ToBytes(saveData), writeCallback);
            //ReadSaveGame("arMySaveGame", readCallback);
            //WriteSaveGame(m_currentGame, SaveData.ToBytes(savedata[1]), writeCallback);
            
        }
    }

    public void LoadGame()
    {
        if(m_bLogin)
        {
            //Read Binary callback
            Action<SavedGameRequestStatus, byte[]> readBinaryCallback = (SavedGameRequestStatus status, byte[] data) =>
                {
                    if(status == SavedGameRequestStatus.Success)
                    {
                        //Load Game Data
                        try
                        {
                            //SaveData[] saveData = new SaveData[10]; // 배열은 나중에 리스트 count 길이만큼 선언
                            
                            SaveData saveData = SaveData.FromBytes(data);
                            // We are displaying these values for the purpose of the tutorial.
                            // Normally you would set the values here.

                            //m_playername = saveData.PlayerName;
                            //m_playerhealth = saveData.Playerhealth;

                            GetLoadData(saveData.PlayerName, saveData.Playerhealth, saveData.PlayerScore);
                         
                            //LoadData LoadData = new LoadData(saveData.PlayerName, saveData.Playerhealth, saveData.PlayerScore);
                          

                            health = saveData.Playerhealth;
                            count = saveData.PlayerScore;
 

                        }
                        catch (Exception e)
                        {
                            Debug.LogError("Failed to read binary data: " + e.ToString());
                        }
                    }
                };
            //Read Game callback
            Action<SavedGameRequestStatus, ISavedGameMetadata> readCallback = (SavedGameRequestStatus status, ISavedGameMetadata game) =>
                {
                    //Check if read was successful
                    if(status == SavedGameRequestStatus.Success)
                    {
                        m_currentGame = game;
                        PlayGamesPlatform.Instance.SavedGame.ReadBinaryData(game, readBinaryCallback);
                    }
                };
            // Replace "MySaveGame" with whatever you would like to save file to be called
            ReadSaveGame("MySaveGame", readCallback);           
        }
    }
 
    void GetLoadData(string _playername , float _playerhealth , int _playerscore )
    {
        //LoadData LoadData = new LoadData(_playername, _playerhealth, _playerscore);
        LoadData.GetInstance.m_playername = _playername;
        LoadData.GetInstance.m_playerhealth = _playerhealth;
        LoadData.GetInstance.m_playerscore = _playerscore;
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveGame();
        }
        else
        {
            LoadGame();
        }
    }

    //void OnApplicationQuit()
    //{
    //    SaveGame();
    //}


    /// <summary>
    /// GPGS에서 자신의 프로필 이미지를 가져옵니다.
    /// </summary>
    /// <returns> Texture 2D 이미지 </returns>
    public Texture2D GetImageGPGS()
    {
        if (Social.localUser.authenticated)
            return Social.localUser.image;
        else
            return null;
    }

    /// <summary>
    /// GPGS 에서 사용자 이름을 가져옵니다.
    /// </summary>
    /// <returns> 이름 </returns>
    public string GetNameGPGS()
    {
        if (Social.localUser.authenticated)
            return Social.localUser.userName;
        else
            return null;
    }

    public string GetMailGPGS()
    {
        if (Social.localUser.authenticated)
            return ((GooglePlayGames.PlayGamesLocalUser)Social.localUser).Email;
        else
            return null;
    }

    public string GetIDGPGS()
    {        
        if (Social.localUser.authenticated)
            return Social.localUser.id;
        else
            return null;
    }  

   
}
