# Custom Tourism

Un mod pour **Cities: Skylines II** qui permet de controler le nombre de touristes dans votre ville et de modifier l'attractivite des batiments.

## Fonctionnalites

### Controle du tourisme
- Definissez un **nombre cible de touristes** pour votre ville depuis les parametres
- Le mod cree et gere les menages touristiques pour atteindre votre cible
- **0 = desactive** (comportement vanilla)
- Fonctionne independamment de la formule de tourisme du jeu
- Assurez-vous d'avoir assez d'hotels pour accueillir vos touristes

### Distribution des arrivees
- Repondre la part des touristes arrivant par **Route / Train / Avion / Bateau** depuis les parametres
- Les poids sont normalises — laissez les quatre a 0 pour utiliser la repartition vanilla

### Editeur d'attractivite
- Selectionnez un batiment avec de l'attractivite (monuments, parcs, attractions touristiques) et un panneau d'edition apparait
- **Curseur** pour regler l'attractivite de **0 a 500**
- Bouton **Restaurer par defaut** pour revenir aux valeurs d'origine
- Les changements s'appliquent a **tous les batiments du meme type**
- Les modifications sont **sauvegardees automatiquement** entre les parties
- Pour fermer le panneau, cliquez n'importe ou ailleurs

## Langues
- Anglais, Francais

## Comment ca marche

### Controle du tourisme
- Allez dans **Options → Custom Tourism → Tourisme**
- Reglez le curseur **Nombre cible de touristes** (0–20000)
- Le mod va progressivement creer des touristes jusqu'a atteindre la cible
- Baisser le curseur va progressivement retirer les touristes en surplus

### Editeur d'attractivite
- Cliquez sur un batiment avec de l'attractivite en jeu
- Le panneau Custom Tourism s'affiche a droite de l'ecran
- Deplacez le curseur ou entrez une valeur, puis cliquez sur **Appliquer**
- La modification est sauvegardee automatiquement et rechargee a la prochaine partie

### Arrivees par train / avion / bateau
Le jeu de base lance chaque touriste avec un pathfind pieton depuis sa connexion exterieure. Les connexions routieres sont sur une route, donc la marche fonctionne, mais les connexions train/avion/bateau n'ont pas de voie pieton de depart — le pathfind ne renvoie rien et le jeu tue le touriste avec `TouristNoTarget`. C'est pour cela que la grande majorite des arrivees non routieres n'apparaissent jamais en ville dans le jeu de base.

Pour que la Distribution des arrivees ait reellement un effet, le mod assigne lui-meme une destination a ces touristes : il choisit un hotel (ou, si aucun n'a de chambres libres, une attraction) avec **la meme formule de score que le jeu utilise en interne** — chambres libres, prix de la chambre, et l'attractivite de l'arete routiere du quartier de destination. Les ecritures de reservation (decrement des chambres libres, ajout au buffer Renter, composant `Target`) correspondent a ce que fait `HotelReserveJob` vanilla, donc le reste du cycle de vie touristique se comporte normalement.

## Note
- Les changements d'attractivite prennent environ **5 a 20 secondes** avant d'apparaitre dans le menu tourisme. La modification est appliquee immediatement, mais les statistiques de tourisme du jeu prennent un moment a se recalculer.

## Compatibilite
- Peut etre retire a tout moment. Restaurez toutes les modifications par defaut avant de desinstaller pour retrouver les valeurs d'origine.
- N'utilise pas Harmony

## Fichier de configuration
`ModsSettings/CustomizeIt/CustomizeIt.coc`

## Fonctionnalites en detail

| Fonctionnalite | Description |
|----------------|-------------|
| Nombre cible de touristes | Definissez un nombre cible de touristes pour votre ville (0–20000). |
| Distribution des arrivees | Repondre les parts d'arrivee Route / Train / Avion / Bateau. |
| Curseur d'attractivite | Reglez l'attractivite de n'importe quel batiment entre **0 et 500**. |
| Restaurer par defaut | Remet le batiment a sa valeur d'attractivite d'origine. |
| Sauvegarde persistante | Vos modifications sont sauvegardees et automatiquement appliquees au chargement. |
| Modification par prefab | Modifier un batiment affecte tous les batiments places du meme type. |

## Credits

Merci a **Honu / River-Mochi** de la communaute Discord de modding Cities: Skylines pour son aide!

## Licence

[Licence MIT](LICENSE)
