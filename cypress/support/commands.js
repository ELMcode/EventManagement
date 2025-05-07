// ***********************************************
// This example commands.js shows you how to
// create various custom commands and overwrite
// existing commands.
//
// For more comprehensive examples of custom
// commands please read more here:
// https://on.cypress.io/custom-commands
// ***********************************************

// Commande personnalisée pour se connecter
Cypress.Commands.add('login', (email = 'amine_admin@eventmanagement.com', password = 'Admin123!') => {
  cy.visit('/Account/Login')
  cy.get('[name="Email"]').type(email)
  cy.get('[name="Password"]').type(password)
  cy.contains('Se Connecter').click()
  cy.url().should('not.include', '/Account/Login')
})

//
// -- This is a parent command --
// Cypress.Commands.add('login', (email, password) => { ... })
//
//
// -- This is a child command --
// Cypress.Commands.add('drag', { prevSubject: 'element'}, (subject, options) => { ... })
//
//
// -- This is a dual command --
// Cypress.Commands.add('dismiss', { prevSubject: 'optional'}, (subject, options) => { ... })
//
//
// -- This will overwrite an existing command --
// Cypress.Commands.overwrite('visit', (originalFn, url, options) => { ... })
