# Projet PRBD 2324 - Groupe c07 - Tricount

- Lambert Lucian
- Maes Quentin
  
# Tricount-Clone

Une application desktop Windows inspirée de Tricount, permettant de gérer facilement les dépenses de groupe, calculer les parts et visualiser les soldes des participants.

---

## 🚀 Table des matières

- [À propos](#à-propos)  
- [Technologies utilisées](#technologies-utilisées)  
- [Fonctionnalités](#fonctionnalités)  
- [Installation & utilisation](#installation--utilisation)  
- [Architecture du projet](#architecture-du-projet)  
- [Limitations & améliorations possibles](#limitations--améliorations-possibles)  
- [Capture d'écran](#capture-décran)  
- [Contact](#contact)  
- [Licence](#licence)

---

## À propos

Ce projet a été développé dans le cadre d’un projet scolaire avec un autre étudiant. L’objectif était de créer une application desktop Windows permettant de gérer les dépenses de groupe, inspirée de l’application Tricount.  

Le projet a permis de mettre en pratique les compétences suivantes :  
- Développement en **C# et .NET 7.0 (WPF)**  
- Gestion des données avec **Entity Framework Core 7**  
- Architecture MVVM, data-binding et interface utilisateur riche  

---

## Technologies utilisées

### Backend / Core
- **Langage** : C#  
- **Framework** : .NET 7.0 (WPF)  
- **Base de données** : SQLite (développement), SQL Server (optionnel pour production)  
- **ORM** : Entity Framework Core 7 (avec Proxies pour lazy-loading)  
- **JSON** : Newtonsoft.Json pour sérialisation des données  

### Frontend / UI
- **Interface** : WPF (Windows Presentation Foundation)  
- **Composants UI** : CalcBinding, NumericUpDownLib  
- **Icônes et visuels** : FontAwesome6  

---

## Fonctionnalités

1. **Gestion des utilisateurs**
   - Écran de connexion / création de compte  
   - Gestion des droits (owner / co-owner / participant)  

2. **Page d’accueil**
   - Liste de tous les Tricounts auxquels l’utilisateur appartient  
   - Création de nouveaux Tricounts et templates  

3. **Création d’un Tricount**
   - Titre, description  
   - Date de création (aujourd’hui ou futur)  
   - Sélection des participants  

4. **Gestion des dépenses**
   - Ajout de dépenses sous forme de cartes  
   - Attribution du montant payé à un ou plusieurs participants  
   - Édition et suppression des dépenses (selon rôle)  

5. **Visualisation des soldes**
   - Balance graphique indiquant dettes et avances de chaque participant  

6. **Édition / suppression d’un Tricount**
   - Possibilité de modifier ou supprimer un Tricount si propriétaire  

---

## Installation & utilisation

1. Cloner le dépôt :  
   ```bash
   git clone https://github.com/LucienLambert/Tricount-clone.git


## Notes de version

## Listes utilisateur

- Mail = "boverhaegen@epfc.eu", FullName = "Boris", Password = "Password1,"
- Mail = "bepenelle@epfc.eu", FullName = "Benoît", Password = "Password1,"
- Mail = "xapigeolet@epfc.eu", FullName = "Xavier", Password = "Password1,"
- Mail = "mamichel@epfc.eu", FullName = "Marc", Password = "Password1,"
- Mail = "admin@epfc.eu", FullName = "Administrator", Password = "Password1,"
