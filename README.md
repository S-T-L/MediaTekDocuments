# MediatekDocuments
## Lien vers le dépôt d'origine
Le dépôt d'origine de l'application MediaTekDocuments est disponible ici : https://github.com/CNED-SLAM/MediaTekDocuments <br>
Celui-ci contient la présentation complète de l'application de base.
# Fonctionnalités ajoutées :
## Création de 3 nouveaux onglets pour la gestion des commandes : 
![Capture d'écran 2025-04-01 085728](https://github.com/user-attachments/assets/ee346ded-7276-4eab-81f7-73039ac76321)
### Onglets Commande de livre et Dvd : 
L'interface permet à l'utilisateur de saisir le numéro d'un document afin d'afficher ses informations ainsi que la liste des commandes associées.
Il est possible d'ajouter une nouvelle commande en respectant les champs obligatoires :  Date de la commande, Numéro de commande, Nombre d'exemplaires et montant de la commande.
Plusieurs règles doivent être respectées concernant la gestion du suivi des commandes : une nouvelle commande doit obligatoirement être enregistrée avec un suivui "en cours", une commande "livrée ou réglée" ne peut pas revenir à une étape précédente, une commande ne peut pas être réglée si elle n'est pas encore livrée.
### Onglet Commande de revue :
![Capture d'écran 2025-04-01 090941](https://github.com/user-attachments/assets/7d13d02c-3287-4c7e-80a1-d25c4305c96e)
Le fonctionnement est similaire à celui des onglets dédiés aux livres et DVD, avec quelques spécificités propres aux revues : :Lors de l'ajout d'une nouvelle commande (nouvel abonnement), la saisie d'une date de début et d'une date de fin d'abonnement est obligatoire.Une commande ne peut être supprimée si au moins un exemplaire a été acquis durant la période d'abonnement.



## Mise en place d'une fenêtre d'authentification
![Capture d'écran 2025-03-31 113938](https://github.com/user-attachments/assets/336fa2de-df15-4f5e-8471-e1616ccb4cd6)<br>
Pour se connecter, l'utilisateur doit saisir son identifiant et son mot de passe. L'accès aux fonctionnalités de l'application dépends de son service de rattachement : <br>
- Les utilisateurs du service Culture n'ont pas les droits d'accès à l'application. Lorsqu'ils tentent de se connecter, un message d'information s'affiche :<br>
![Capture d'écran 2025-03-31 140222](https://github.com/user-attachments/assets/5739a20e-e484-4c21-850f-d44246529170)<br>
- Les utilisateurs du service "Prêt" ont uniquement accès aux onglets de consultation des documents.<br>
- Le service administratif et les administrateurs disposent d'un accès complet à toutes les fonctionnalités de l'application

## Fenêtre d'alerte pour les abonnements expirants dans moins de 30 jours : 
Lorsqu'un utilisateur disposant de tous les droits accède à l'application, une fenêtre s'affiche pour lui signaler les abonnements arrivant à expiration.<br>
![Capture d'écran 2025-03-31 105229](https://github.com/user-attachments/assets/ded4444d-a083-4fe9-b476-3c446a44881f)


## La base de données
La base de données 'mediatek86 ' est au format MySQL.<br>
Voici sa structure :<br>
![bdd](https://github.com/user-attachments/assets/843b3553-3513-4f74-9367-a73b82e6e7a1)<br>
Une nouvelle table, Suivi, a été ajoutée à la base de données d'origine. Elle contient l'ID du suivi ainsi que son libellé et est reliée à la table CommandeDocument afin de permettre la gestion du suivi des commandes.
<br>
![Capture d'écran 2025-03-12 101041](https://github.com/user-attachments/assets/7a55a7d8-4bd1-4533-9812-3cc99fb9085e)<br>
Deux nouvelles tables indépendantes ont également été ajoutées à la base de données : la table utilisateur et la table service. Chaque utilisateur est associé à un seul service.
La base de données est remplie de quelques exemples pour pouvoir tester son application. Dans les champs image (de Document) et photo (de Exemplaire) doit normalement se trouver le chemin complet vers l'image correspondante. Pour les tests, vous devrez créer un dossier, le remplir de quelques images et mettre directement les chemins dans certains tuples de la base de données qui, pour le moment, ne contient aucune image.<br>
Lorsque l'application sera opérationnelle, c'est le personnel de la médiathèque qui sera en charge de saisir les informations des documents.
## L'API REST
L'accès à la BDD se fait à travers une API REST protégée par une authentification basique.<br>
Le code de l'API se trouve ici :<br>
https://github.com/S-T-L/rest_mediatekdocuments
avec toutes les explications pour l'utiliser (dans le readme).
## Installation de l'application 
### A l'aide de l'installateur
Pour installer l'application sur votre PC il faut telecharger le fichier MediatekDocument86.ms présent dans le dossier Fichiers d'installation du dépôt.<br>
https://github.com/S-T-L/MediaTekDocuments/tree/master/Fichiers%20d'installation<br>

### En local
Ce mode opératoire permet d'installer l'application pour pouvoir travailler dessus.<br>
- Installer Visual Studio 2019 entreprise et newtonsoft.json (pour ce dernier, voir l'article "Accéder à une API REST à partir d'une application C#" dans le wiki de ce dépôt : consulter juste le début pour la configuration, car la suite permet de comprendre le code existant).<br>
- Télécharger le code et le dézipper puis renommer le dossier en "mediatekdocuments".<br>
- Récupérer et installer l'API REST nécessaire (https://github.com/S-T-L/rest_mediatekdocuments) ainsi que la base de données (les explications sont données dans le readme correspondant).
