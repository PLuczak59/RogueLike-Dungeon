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
    public HealManager healManager;
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
        Debug.Log($"[GameSceneManager] Activation de la scène pour le type: {roomType}");
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
                    
                    // Activation du HealManager pour soigner les membres vivants
                    if (healManager != null)
                    {
                        Debug.Log("[GameSceneManager] Appel du HealManager pour Event");
                        healManager.ActivateHealing(RoomType.Event);
                    }
                    else
                    {
                        Debug.LogError("[GameSceneManager] HealManager est NULL pour Event !");
                    }
                    
                    // Afficher immédiatement l'UI de choix après les soins
                    ShowRoomChoiceUI(RoomType.Event);
                }
                break;

            case RoomType.Rest:
                if (FountainReviveRoom)
                {
                    Debug.Log("FountainReviveRoom on");
                    FountainReviveRoom.SetActive(true);
                    
                    // Activation du HealManager pour ressusciter les membres morts
                    if (healManager != null)
                    {
                        healManager.ActivateHealing(RoomType.Rest);
                    }
                    
                    // Afficher immédiatement l'UI de choix après la résurrection
                    ShowRoomChoiceUI(RoomType.Rest);
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

    /// <summary>
    /// Affiche l'UI de choix après une action de salle
    /// </summary>
    /// <param name="roomType">Type de salle pour adapter le message</param>
    private void ShowRoomChoiceUI(RoomType roomType)
    {
        if (dungeonManager != null && dungeonManager.roomUI != null)
        {
            string title = "";
            string description = "";
            
            switch (roomType)
            {
                case RoomType.Event:
                    title = "Feu de camp";
                    description = "Vos héros ont été soignés ! Que souhaitez-vous faire maintenant ?";
                    break;
                case RoomType.Rest:
                    title = "Fontaine de résurrection";
                    description = "Vos héros morts ont été ressuscités ! Que souhaitez-vous faire maintenant ?";
                    break;
                default:
                    title = "Salle terminée";
                    description = "Que souhaitez-vous faire maintenant ?";
                    break;
            }
            
            Debug.Log($"[GameSceneManager] Affichage de l'UI de choix pour salle {roomType}");
            dungeonManager.roomUI.ShowChoiceUI(title, description, dungeonManager);
        }
        else
        {
            Debug.LogWarning("[GameSceneManager] DungeonManager ou RoomUI non assigné pour ShowRoomChoiceUI");
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
            // Afficher le RoomUI pour permettre au joueur de choisir
            if (dungeonManager != null && dungeonManager.roomUI != null)
            {
                Debug.Log("[GameSceneManager] Affichage de l'UI de choix post-combat");
                
                // S'assurer que le RoomUI est visible avant d'afficher les choix
                dungeonManager.roomUI.gameObject.SetActive(true);
                
                dungeonManager.roomUI.ShowChoiceUI(
                    "Victoire !", 
                    "Que souhaitez-vous faire maintenant ?", 
                    dungeonManager
                );
            }
            else
            {
                Debug.LogWarning("[GameSceneManager] DungeonManager ou RoomUI non assigné, fallback vers ContinueExploration automatique");
                if (dungeonManager != null)
                    dungeonManager.ContinueExploration();
            }
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