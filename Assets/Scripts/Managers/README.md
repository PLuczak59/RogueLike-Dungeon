# Système de Gestion des Scènes par Type de Salle

## Vue d'ensemble

Le système a été modifié pour permettre l'activation/désactivation automatique des scènes selon le type de salle du donjon.

## Components principaux

### GameSceneManager

Le `GameSceneManager` est le composant central qui gère l'activation des différentes scènes selon le type de salle :
- **Combat Scene** : Activée pour les salles de type `Combat`
- **Event Scene** : Activée pour les salles de type `Event` 
- **Rest Scene** : Activée pour les salles de type `Rest`
- **Boss Scene** : Activée pour les salles de type `Boss`

### Configuration requise dans Unity

1. **Dans la scène DungeonRoom**, créer un GameObject vide nommé `GameSceneManager`
2. Attacher le script `GameSceneManager` à ce GameObject
3. Configurer les références dans l'inspecteur :
   - `Combat Scene` : GameObject contenant tous les éléments de combat
   - `Event Scene` : GameObject contenant tous les éléments d'événement
   - `Rest Scene` : GameObject contenant tous les éléments de repos
   - `Boss Scene` : GameObject contenant tous les éléments de boss
   - `Dungeon Manager` : Référence vers le DungeonManager
   - `Combat Manager` : Référence vers le CombatManager

## Flux de fonctionnement

1. **Menu Principal** : Le joueur clique sur "Start Game"
2. **Chargement** : La scène `DungeonRoom` est chargée
3. **Initialisation** : Le `GameSceneManager` démarre et lance le `DungeonManager`
4. **Sélection de salle** : Le `DungeonManager` choisit une salle aléatoire
5. **Activation de scène** : Le `GameSceneManager` active la scène correspondant au type de salle
6. **Démarrage spécifique** : Si c'est du combat/boss, le `CombatManager` est initialisé

## Types de salles

- `Combat` : Salle de combat normale
- `Event` : Salle d'événement/interaction
- `Rest` : Salle de repos pour se soigner
- `Boss` : Salle de boss (utilise le système de combat)

## Modifications apportées

### MainMenuUI.cs
- Conservé le chargement de scène existant
- Le `GameSceneManager` prend le relais automatiquement

### DungeonManager.cs  
- Ajout de `StartDungeonExploration()` pour démarrage contrôlé
- Appel à `GameSceneManager.Instance.ActivateSceneForRoomType()` lors de l'entrée dans une salle

### CombatManager.cs
- Ajout de `StartCombat()` pour démarrage contrôlé
- Le combat ne démarre que quand explicitement activé

## Installation

1. Copier `GameSceneManager.cs` dans `Assets/Scripts/Managers/`
2. Configurer les GameObjects dans la scène selon les indications ci-dessus
3. Assigner les références dans l'inspecteur du `GameSceneManager`

## Notes importantes

- Tous les GameObjects de scène sont désactivés au démarrage
- Seule la scène correspondant au type de salle courante est active
- Le système préserve la logique existante du DungeonManager