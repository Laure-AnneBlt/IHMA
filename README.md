# Projet Collecticiel 

Ce projet a été réalisé dans le cadre du cours de **Interaction Humain-Machine Avancée** à Polytech Grenoble, promo INFO5 2024-2025 (option Multimédia).

Réalisé par Laure-Anne BLUTEAU, Asmae EL KANBI, Achraf KHRIBECH, Abderahmane MESBAH, Aurélie AVENTURIER, Sidd VANDER-ELST.

### Description du projet

L’objectif de ce projet est de permettre à un groupe de deux joueurs sur place de construire une structure précise à l’aide de blocs virtuels, en suivant les instructions d’un expert situé à distance. Les participants jouent sur smartphone et casque VR, tandis que l'expert est sur PC.

Notre choix a été de simplifier et d’optimiser l’expérience utilisateur en superposant une grille virtuelle sur la table réelle et en implémentant un système de **guidage visuel basé sur la coloration** : la case cible et l’objet correspondant sont colorés pour orienter les joueurs de manière intuitive.

### Rôles et droits des participants

1.  **Expert (Admin)** :
    
    -   Situé à distance, l’expert peut colorier les cases de la grille et les blocs virtuels pour guider visuellement les joueurs.
    -   Ce rôle est essentiel pour fournir des instructions claires et intuitives.
2.  **Joueurs** :
    
    -   Chaque joueur a une couleur assignée qui lui est propre.
    -   Les joueurs n'ont pas le droit de modifier les couleurs, mais uniquement de déplacer les blocs virtuels sur la table réelle pour construire la structure demandée.

### Installation et lancement

#### Prérequis

-   Unity version 2021.3 ou supérieure.
-  Configurer Unity et MRTK à l'aide de ce tutoriel : https://learn.microsoft.com/fr-fr/training/modules/learn-mrtk-tutorials/1-5-exercise-configure-resources

#### Étapes pour le build

1.  **Cloner le dépôt** :
    
    `git clone https://github.com/Laure-AnneBlt/IHMA.git` 
    
2.  **Ouvrir le projet dans Unity**.
3.  **Configurer la plateforme cible** (Android, VR ou PC).  
    Consultez les tutoriels spécifiques pour les étapes détaillées :
    -   Guide pour le build sur smartphone  (Android) : https://localjoost.github.io/Running-an-MRTK3-app-on-an-Android-Phone/
    -   Guide pour le build sur casque VR (Hololens) : https://learn.microsoft.com/en-us/windows/mixed-reality/develop/unity/build-and-deploy-to-hololens
4.  **Lancer le build et déployer l’application** :
    -   Dans Unity, cliquez sur **File > Build Settings**.
    -   Sélectionnez votre plateforme cible et configurez les options nécessaires.
    -   Cliquez sur **Build & Run** pour générer et déployer l’application sur l’appareil connecté.

