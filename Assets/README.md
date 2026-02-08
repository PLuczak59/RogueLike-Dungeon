# Rogue-Like Dungeon

Un jeu de type roguelike dungeon crawler développé avec Unity, mélangeant exploration de donjon et combats tactiques au tour par tour.

## Concept du Jeu

Dans ce jeu, vous contrôlez un groupe de 4 héros qui explorent un donjon composé de plusieurs étages. Chaque étage contient plusieurs salles à découvrir, et vous devrez faire des choix stratégiques pour survivre jusqu'au boss final.

## Gameplay

### Exploration du Donjon

Le jeu se déroule étage par étage. À chaque nouvelle salle, vous devrez explorer différents types de pièces :

- **Salles de Combat** : Affrontez des groupes d'ennemis dans des combats tactiques
- **Salles de Repos** : Récupérez vos points de vie
- **Salles d'Événement** : Découvrez des événements aléatoires
- **Salles de Boss** : Affrontez des ennemis puissants
- **Salles de Résurrection** : Ramenez vos héros tombés au combat

Une fois toutes les salles d'un étage explorées, vous passez à l'étage suivant, jusqu'à la fin du donjon.

### Système de Combat

Les combats se déroulent au tour par tour et sont basés sur l'initiative :

- **Ordre des tours** : Les personnages agissent selon leur statistique d'initiative (du plus rapide au plus lent)
- **Phase de combat** : 
  - Pendant le tour d'un héros, sélectionnez un ennemi pour l'attaquer
  - Les ennemis choisissent automatiquement leur cible
  - Le combat continue jusqu'à ce qu'un camp soit entièrement vaincu

- **Calcul des dégâts** : Basé sur l'attaque de l'attaquant et la défense du défenseur

## Les Héros

Votre groupe est composé de 4 classes de héros, chacune avec ses propres statistiques :

- **Tank** : Grande résistance, protège l'équipe
- **Healer** : Soigne les alliés (fonctionnalité en développement)
- **Mage** : Attaques magiques puissantes
- **Assassin** : Vitesse et attaque physique puissante

### Statistiques

Chaque personnage possède 4 statistiques principales :

- **HP (Points de Vie)** : Détermine la survie du personnage
- **Attaque** : Influence les dégâts infligés
- **Défense** : Réduit les dégâts reçus (0-100)
- **Initiative** : Détermine l'ordre d'action dans les combats

## Interface

- **Icônes des personnages** : Affichage des héros (vert) et des ennemis (rouge) avec leurs barres de vie
- **Ordre des tours** : Une barre en haut de l'écran montre l'ordre d'action des combattants
- **Sélection de salle** : Interface pour choisir les salles à explorer

## Progression

Les statistiques de vos héros persistent d'un combat à l'autre :
- Les HP restants sont conservés entre les combats
- Utilisez les salles de repos pour récupérer
- La résurrection est possible dans les salles dédiées

Si tous vos héros tombent au combat, c'est la défaite !

## Inspiration

Ce jeu s'inspire des roguelikes classiques comme **Slay the Spire** et **Darkest Dungeon**, en mélangeant la prise de décision stratégique avec des combats tactiques au tour par tour.

---

## Développement

### Pierre-Henri LUCZAK
- Conception et intégration des assets Unity
- Développement du système de combat tactique au tour par tour
- Gestion de l'interface utilisateur et des interactions

### Louis MAJCHRZAK  
- Développement du système de génération de donjons
- Implémentation de la boucle de jeu et de la progression
- Gestion des différents types de salles et événements

---

## Technologies

- **Moteur** : Unity
- **Langage** : C#
