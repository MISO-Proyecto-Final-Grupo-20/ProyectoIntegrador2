/// <reference types="cypress" />
// ***********************************************
// This example commands.ts shows you how to
// create various custom commands and overwrite
// existing commands.
//
// For more comprehensive examples of custom
// commands please read more here:
// https://on.cypress.io/custom-commands
// ***********************************************

// -- This is a parent command --
Cypress.Commands.add('login', (email, password) => { 
  // Visit the base URL which loads the login page directly
  cy.visit('/');
  
  // Use the data-testid attributes from your actual HTML
  cy.get('[data-testid="input-login-correo"]').type(email);
  cy.get('[data-testid="input-login-contrasena"]').type(password);
  cy.get('[data-testid="boton-ingresar"]').click();
  
  // Verify successful login - adjust based on your app's behavior
  // After login, we should be redirected to a different page
  cy.url().should('not.equal', Cypress.config().baseUrl + '/');
});

// -- New command for navigating through menu items --
Cypress.Commands.add('navigateToMenuOption', (menuText) => {
  // Find the menu, element app-menu-navegacion
  cy.get('[data-testid="menu-navegacion"]')
    .contains(menuText)
    .click();
  
  // Wait for the page to load
  cy.wait(1000);
});

// -- API Interaction Commands --
Cypress.Commands.add('apiLogin', (email, password) => {
  cy.request({
    method: 'POST',
    url: `${Cypress.env('apiUrl')}/usuarios/login`,
    body: {
      email: email,
      password: password
    }
  }).then((response) => {
    // Store the token for future API requests
    window.localStorage.setItem('token', response.body.token);
    return response;
  });
});

Cypress.Commands.add('apiRequest', (method, endpoint, body = null) => {
  const token = window.localStorage.getItem('token');
  
  return cy.request({
    method: method,
    url: `${Cypress.env('apiUrl')}${endpoint}`,
    body: body,
    headers: {
      'Authorization': `Bearer ${token}`
    }
  });
});

// -- This is a custom command for verifying toast messages --
Cypress.Commands.add('verifyToast', (message) => {
  cy.get('.toast-message').should('be.visible').and('contain', message);
});

// -- This is a custom command for waiting for API requests to complete --
Cypress.Commands.add('waitForApi', (endpoint) => {
  cy.intercept(endpoint).as('apiRequest');
  cy.wait('@apiRequest');
});

// -- This will help with testing tables --
Cypress.Commands.add('findRowByText', (text) => {
  return cy.contains('tr', text);
});

// ***********************************************
// -- This will overwrite an existing command --
// Cypress.Commands.overwrite('visit', (originalFn, url, options) => { ... })
// ***********************************************

declare global {
  namespace Cypress {
    interface Chainable {
      login(email: string, password: string): Chainable<void>
      navigateToMenuOption(menuText: string): Chainable<void>
      apiLogin(email: string, password: string): Chainable<Cypress.Response<any>>
      apiRequest(method: string, endpoint: string, body?: any): Chainable<Cypress.Response<any>>
      verifyToast(message: string): Chainable<Element>
      waitForApi(endpoint: string): Chainable<void>
      findRowByText(text: string): Chainable<JQuery<HTMLElement>>
    }
  }
}