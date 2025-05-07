describe('Connexion', () => {
  it('devrait se connecter avec succès', () => {
    cy.visit('/Account/Login')

    // Verifie si on est sur la page de connexion
    cy.contains('Connexion').should('be.visible')

    // Rempli le formulaire de connexion
    cy.get('[name="Email"]').type('amine_admin@eventmanagement.com')
    cy.get('[name="Password"]').type('Admin123!')

    // Envoyer le formulaire
    cy.get('form').submit()

    // Verifie que la connexion a réussi
    cy.url().should('not.include', '/Account/Login')
  })
})
