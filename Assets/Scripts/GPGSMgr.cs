using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
    public int PlayerHp { get; set; }
    public int PlayerMp { get; set; }
    public int PlayerAttack { get; set; }
    public int PlayerDefence { get; set; }
    public int PlayerLevel { get; set; }
    public int PlayerGold { get; set; }
    public int Item { get; set; }

    public string SelectCharacter { get; set; }

    public List<CharacterData> m_saveInventory = new List<CharacterData>();
    
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
    public SaveData(string _playername, int _playerhp, int _playermp, int _attack, int _defence, int _level,int _gold ,int _item, string _selectCharacter, List<CharacterData> _chardata)
    {
        _chardata = new List<CharacterData>();
        PlayerName = _playername;
        PlayerHp = _playerhp;
        PlayerMp = _playermp;
        PlayerAttack = _attack;
        PlayerDefence = _defence;
        PlayerLevel = _level;
        PlayerGold = _gold;
        Item = _item;
        SelectCharacter = _selectCharacter;
        m_saveInventory = _chardata;

    }

    public void SaveInventory()
    {
        for(int i = 0; i < MainInvenUIManager.GetInstance.m_inventory.Count; i++)
        {
            m_saveInventory.Add(MainInvenUIManager.GetInstance.m_inventory[i]);
        }
    }
}



public class GPGSMgr : Singleton<GPGSMgr> {

    public bool m_bLogin { get; set; }
    
    //유저 첫 접속 초기화 스탯
    public int hp;
    public int mp;
    public int attack;
    public int defence;
    public int level;
    public int gold;
    public int item; 
    public string  selectCharacter;

    
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
        InitializeUserStatus();

    }

    void InitializeUserStatus()
    {
        gold = 2000;
        item = 1;
        hp = 100;
        mp = 100;
        attack = 5;
        defence = 5;
        level = 1;
        
        selectCharacter = "UnityChan";
        
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

        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate(LoginCallBackGPGS);
            
            SaveGame();
            LoadGame();
        }
        
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
            SaveGame();
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
            SaveData saveData = new SaveData(GetNameGPGS(), hp, mp, attack, defence ,level ,gold ,item , selectCharacter, MainInvenUIManager.GetInstance.m_inventory);
           

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

                            LoadData loadData = new LoadData();
                            GetLoadData(saveData.PlayerName, saveData.PlayerHp, saveData.PlayerMp, saveData.PlayerAttack, saveData.PlayerDefence, saveData.PlayerLevel, saveData.PlayerGold, saveData.Item, saveData.SelectCharacter);

                            hp = saveData.PlayerHp;
                            mp = saveData.PlayerMp;
                            attack = saveData.PlayerAttack;
                            defence = saveData.PlayerDefence;
                            level = saveData.PlayerLevel;
                            gold = saveData.PlayerGold;
                            item = 0;
                            for(int i = 0; i< saveData.m_saveInventory.Count; i++)
                            {
                                if(MainInvenUIManager.GetInstance.m_inventory.Contains(saveData.m_saveInventory[i]))
                                {
                                    return;
                                }
                                else
                                {
                                    MainInvenUIManager.GetInstance.m_inventory.Add(saveData.m_saveInventory[i]);
                                }                                
                            }                           

                            //loadData.LoadInventory(saveData.m_saveInventory);
                            //LoadData LoadData = new LoadData(saveData.PlayerName, saveData.Playerhealth, saveData.PlayerScore);

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
 
    void GetLoadData(string _playername , int _hp , int _mp, int _attack, int _defence , int _level, int _gold, int _item, string _selectcharacter)
    {
        LoadData.GetInstance.GetUserData(_playername,_level , _hp,  _mp,  _attack,  _defence, _gold,  _item, _selectcharacter );      
        
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
