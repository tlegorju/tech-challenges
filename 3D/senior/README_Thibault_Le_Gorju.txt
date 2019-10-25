Le projet se limite à deux classes
- CameraViewAllObjectController
	Qui gère le déplacement de la caméra et qui fournit les informations du champs de vision
- ObjectBoundsManager
	Qui gère la liste des objets à intégrer dans le cadre et détermine si un objet est visible ou non

L'algorithme suis la logique suivante :
- charger tous les objets concernés dans une liste
- calculer la position moyenne de ces objets
- déterminer quels objets sont visibles et lesquels ne le sont pas
- déplacer la caméra jusqu'à la position moyenne des objets (pour la centrer)
- faire reculer la caméra jusqu'à ce que tous les objets soient visibles.


Dans la scène d'exemple, l'objet Cameras à plusieurs cameras en enfants.
Celles-ci servent à tester différentes positions et orientations.
