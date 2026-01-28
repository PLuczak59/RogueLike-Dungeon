using UnityEngine;

public class GameSceneManager : MonoBehaviour
{

    [Header("Scene GameObjects")]
    public GameObject FightRoom;
    public GameObject CampFireRoom;
    public GameObject FountainReviveRoom;
    public GameObject BossRoom;

    [Header("Managers")]
    public DungeonManager dungeonManager;
    public CombatManager combatManager;
    public PartyManager partyManager;
    public string mainMenuSceneName = "MainMenu";

    private void Awake()
    {
        DeactivateAllScenes();

        if (combatManager != null)
            combatManager.CombatEnded += OnCombatEnded;
    }



    private void DeactivateAllScenes()
    {
        if (FightRoom)
        {
            Debug.Log("FightRoom off");
            FightRoom.SetActive(false);
        }
        if (CampFireRoom)
        {
            Debug.Log("CampFireRoom off");
            CampFireRoom.SetActive(false);
        }
        if (FountainReviveRoom) 
        {
            Debug.Log("FountainReviveRoom off");
            FountainReviveRoom.SetActive(false);
        }
        if (BossRoom)
        {
            Debug.Log("BossRoom off");
            BossRoom.SetActive(false);
        }   
    }

    public void StartDungeonExploration()
    {
        
    }
    public void ActivateSceneForRoomType(RoomType roomType)
    {
        DeactivateAllScenes();

        switch (roomType)
        {
            case RoomType.Combat:
                if (FightRoom)
                {
                    Debug.Log("FountainReviveRoom on");
                    FightRoom.SetActive(true);
                    if (combatManager)
                    {
                        combatManager.enabled = true;
                        EnsurePartyInitialized();
                        combatManager.StartCombat(partyManager);
                    }
                }
                break;

            case RoomType.Event:
                if (CampFireRoom)
                {
                    Debug.Log("CampFireRoom on");
                    CampFireRoom.SetActive(true);
                }
                break;

            case RoomType.Rest:
                if (FountainReviveRoom)
                {
                    Debug.Log("FountainReviveRoom on");
                    FountainReviveRoom.SetActive(true);
                }
                break;

            case RoomType.Boss:
                if (BossRoom)
                {
                    Debug.Log("BossRoom off");
                    BossRoom.SetActive(true);
                    if (combatManager)
                    {
                        combatManager.enabled = true;
                        EnsurePartyInitialized();
                        combatManager.StartCombat(partyManager);
                    }
                }
                break;
            
        }
    }

    private void EnsurePartyInitialized()
    {
        if (partyManager == null || combatManager == null)
            return;
        partyManager.InitializeFromPrefabs(combatManager.heroPrefabs);
    }

    private void OnCombatEnded(bool victory)
    {
        if (victory)
        {
            if (dungeonManager != null)
                dungeonManager.ContinueExploration();
            return;
        }

        if (UnityEngine.Application.CanStreamedLevelBeLoaded(mainMenuSceneName))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(mainMenuSceneName);
        }
        else
        {
            Debug.LogWarning($"[GameSceneManager] Scene d'accueil introuvable: {mainMenuSceneName}");
        }
    }
}