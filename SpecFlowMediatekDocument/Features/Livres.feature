Feature: SpecFlowMediatekDocuments
	Recherche d'un livre

@rechercheLivreNumero
Scenario: Rechercher un livre avec son numero 
	Given Je saisis la valeur 00017 dans txbLivresNumRecherche
	When Je clic sur le bouton Rechercher
	Then Les informations détaillées affichent le titre 'Catastrophes au Brésil'

Scenario: Rechercher un livre selon son genre
	Given Je sélectionne la valeur 'Bande dessinée' dans cbxLivresGenres
	Then Le résultat est de 5 livres dans dgvLivresListe

Scenario: rechercher un livre selon le public
	Given Je sélectionne la valeur 'Jeunesse' dans cbxLivresPublics
	Then 1 livre retourné 

Scenario: Rechercher un livre selon le rayon
	Given Je sélectionne la valeur 'BD Adultes' dans cbxLivresRayons
	Then 4 livres obtenus

Scenario: rechercher un livre avec son titre 
	Given Je saisis la valeur 'Catastrophe au Brésil' dans txbLivresTitreRecherche
	When Je clique sur le bouton Rechercher
	Then 1 livre obtenu : Philippe Masson : Catastrophe au Brésil