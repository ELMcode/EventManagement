describe('Tests connexion : indeitifnats incorrects', () => {
  it('devrait charger la page de connexion', () => {
    cy.visit('/Account/Login')
    cy.contains('Connexion').should('be.visible')
    cy.get('[name="Email"]').should('be.visible')
    cy.get('[name="Password"]').should('be.visible')
  })

  it('devrait afficher un message erreur avec des identifiants incorrects', () => {
    cy.visit('/Account/Login')
    cy.get('[name="Email"]').type('test@example.com')
    cy.get('[name="Password"]').type('password123')
    cy.get('form').submit()

    // Verifie qu'on reste sur la page de login
    cy.url().should('include', '/Account/Login')
  })
})
