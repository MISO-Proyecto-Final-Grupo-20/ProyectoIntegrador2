describe('Página de Fabricantes', () => {
  const uniqueFabricanteName = `Test Fabricante ${Date.now()}`;

  beforeEach(() => {
    // Iniciar sesión antes de probar la funcionalidad de fabricantes
    cy.fixture('users').then((users) => {
      cy.login(users.admin.email, users.admin.password);
      
      // Esperar a que se complete el inicio de sesión y se cargue el panel
      cy.wait(1000);
    });
  });

  it('debería permitir crear un nuevo fabricante', () => {
    // Navegar a Fabricantes a través del menú
    cy.navigateToMenuOption('Fabricantes');
    
    // Comprobar que el formulario de creación de fabricante se carga, componente app-registrar-fabricante
    cy.get('form, app-registrar-fabricante, .fabricante-form').should('be.visible');
    
    // Rellenar el formulario con los detalles del fabricante
    cy.get('input[formControlName*="nombre"], [data-testid="input-registrar-fabricante-nombre"]')
      .type(uniqueFabricanteName);
      
    cy.get('input[formControlName*="email"], [data-testid="input-registrar-fabricante-correo"]')
      .type(`test-${Date.now()}@fabricante.com`);
    
    // Enviar el formulario
    cy.get('button[type="submit"], button:contains("Guardar"), button:contains("Crear"), [data-testid="boton-guardar-fabricante"], [data-testid*="crear"]')
      .click();
    
    // Verificar mensaje de éxito
    cy.get('app-alerta, .success-message, [data-testid*="alerta-exito"]').should('be.visible');
  });

  it('debería mostrar el fabricante recién creado en la página de productos', () => {
    // Navegar a Productos a través del menú
    cy.navigateToMenuOption('Productos');
    
    // Esperar a que se cargue la página de productos
    cy.wait(1000);
    
    // Buscar el componente app-registrar-productos
    cy.get('app-registrar-productos, .productos-form').should('be.visible');
    
    // Esperar a que se cargue el formulario
    cy.wait(500);
    
    // Abrir el desplegable de fabricantes
    cy.get('mat-select[formControlName*="fabricante"], [data-testid="select-fabricante-asociado"]')
      .click();
    
    // Comprobar que nuestro nuevo fabricante existe en las opciones
    cy.get('mat-option').contains(uniqueFabricanteName).should('exist');

  });
});