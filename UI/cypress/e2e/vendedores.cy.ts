describe('Página de Vendedores', () => {
  beforeEach(() => {
    // Iniciar sesión antes de probar la funcionalidad de vendedores
    cy.fixture('users').then((users) => {
      cy.login(users.admin.email, users.admin.password);
      
      // Esperar a que se complete el inicio de sesión y se cargue el panel
      cy.wait(1000);
    });
  });

  it('debería crear un nuevo vendedor', () => {
    // Navegar a Vendedores a través del menú
    cy.navigateToMenuOption('Vendedores');
    
    // Comprobar que el formulario de creación de vendedor se carga, componente app-registrar-vendedores
    cy.get('form, app-registrar-vendedores, .vendedor-form').should('be.visible');
    
    // Rellenar el formulario con los detalles del vendedor
    cy.get('input[formControlName*="nombre"], [data-testid="input-registrar-vendedor-nombre"]')
      .type(`Test Vendedor ${Date.now()}`);
      
    cy.get('input[formControlName*="email"], [data-testid="input-registrar-vendedor-correo"]')
      .type(`test-${Date.now()}@vendedor.com`);

    // Establecer la contraseña (data-testid="input-registrar-vendedor-contrasena")
    cy.get('input[formControlName*="contrasena"], [data-testid="input-registrar-vendedor-contrasena"]')
      .type('TestPassword123!');
    
    // Enviar el formulario
    cy.get('button[type="submit"], button:contains("Guardar"), button:contains("Crear"), [data-testid="boton-guardar-vendedor"], [data-testid*="crear"]')
      .click();
    
    // Verificar mensaje de éxito
    cy.get('app-alerta, .success-message, [data-testid*="alerta-exito"]').should('be.visible');
  });
});