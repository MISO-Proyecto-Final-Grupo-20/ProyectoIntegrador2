describe('Página de Inicio de Sesión', () => {
  beforeEach(() => {
    // Visitar la URL base que automáticamente carga la página de inicio de sesión
    cy.visit('/');
  });

  it('debería mostrar el formulario de inicio de sesión', () => {
    // Comprobar el formulario usando el data-testid
    cy.get('[data-testid="formulario-login"]').should('be.visible');
    cy.get('[data-testid="input-login-correo"]').should('exist');
    cy.get('[data-testid="input-login-contrasena"]').should('exist');
    cy.get('[data-testid="boton-ingresar"]').should('exist');
  });

  it('debería iniciar sesión correctamente con credenciales de administrador válidas', () => {
    cy.fixture('users').then((users) => {
      cy.get('[data-testid="input-login-correo"]').type(users.admin.email);
      cy.get('[data-testid="input-login-contrasena"]').type(users.admin.password);
      cy.get('[data-testid="boton-ingresar"]').click();
      
      // Verificar inicio de sesión exitoso - deberíamos ser redirigidos al panel o a la página principal
      cy.url().should('not.include', '/login');
    });
  });

  it('debería mostrar errores de validación con credenciales inválidas', () => {
    cy.get('[data-testid="input-login-correo"]').type('invalid@example.com');
    cy.get('[data-testid="input-login-contrasena"]').type('wrongpassword');
    cy.get('[data-testid="boton-ingresar"]').click();
    
    // Comprobar mensaje de error usando los selectores exactos de la inspección del DOM
    cy.get('app-alerta').should('be.visible');
    // Alternativamente, usar el selector de clase más específico
    cy.get('.ng-star-inserted app-alerta').should('exist');
    // O buscar el contenido de texto específico
    cy.contains('Http failure response for').should('be.visible');
  });
});