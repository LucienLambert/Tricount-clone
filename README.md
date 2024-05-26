# Projet PRBD 2324 - Groupe c07 - Tricount

## Notes de version

### Liste des bugs connus

  - Bouton Reset 

Ne fonctionne pas (retire bien les Tricounts supplémentaires mais ne modifie pas les participants, de plus chaque nouvelle fenêtre consomme un peu plus de ram)


- Login

Bouton Login apparent, doit être désactivé jusqu'à ce que les credentials bons


- Signup

Bouton Signup apparent, faut-il faire comme dans Login ? Il y a un flou


- Tricounts List

Legendes non responsive


- Add tricount

Lorsque l'on rentre un titre de tricount déjà existant, on obtient "tricount already exists", il faut check si le Tricount est déjà existant pour l'utilisateur en question, pas si il est deja existant tout court

Les dates du futur passent par la validation alors qu'elles doivent être désactivées dans le calendrier

Il manque le (creator) sur le créateur du Tricount dans la liste des participants, et le bouton supprimer doit disparaitre pour lui

Le bouton Save doit fermer l'onglet et ouvrir le Tricount (TricountCardDetailView)

### Liste des fonctionnalités supplémentaires

### Divers
