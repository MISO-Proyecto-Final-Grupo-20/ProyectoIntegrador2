describe('Página de Productos', () => {
  beforeEach(() => {
    // Iniciar sesión antes de probar la funcionalidad de productos
    cy.fixture('users').then((users) => {
      cy.login(users.admin.email, users.admin.password);
      
      // Esperar a que se complete el inicio de sesión y se cargue el panel
      cy.wait(1000);
      
      // Navegar a Productos a través del menú
      cy.navigateToMenuOption('Productos');
    });
  });

  it('debería permitir crear productos', () => {
    // Comprobar que el formulario de creación de producto se carga, componente app-registrar-productos
    cy.get('form, app-registrar-productos, .productos-form').should('be.visible');
    
    // Rellenar el formulario con los detalles del producto
    cy.get('input[formControlName*="nombre"], [data-testid="input-registrar-producto-nombre"]')
      .type(`Test Producto ${Date.now()}`);
      
    cy.get('input[formControlName*="precio"], [data-testid="input-registrar-producto-precio"]')
      .type('100.00');

    // Seleccionar el primer fabricante disponible
    cy.get('mat-select[formControlName*="fabricante"], [data-testid="select-fabricante-asociado"]')
      .click()
      .get('mat-option')
      .first()
      .click();

    // Establecer el código del producto (data-testid="input-registrar-producto-codigo")
    cy.get('input[formControlName*="codigo"], [data-testid="input-registrar-producto-codigo"]')
      .type(`CODE-${Date.now()}`);

    // Establecer la imagen del producto (data-testid="input-registrar-producto-imagen")
    cy.get('input[formControlName*="imagen"], [data-testid="input-registrar-producto-imagen"]')
      .type('https://example.com/image.jpg');
    
    // Enviar el formulario
    cy.get('button[type="submit"], button:contains("Guardar"), button:contains("Crear"), [data-testid="boton-guardar-producto"], [data-testid*="crear"]')
      .click();
    
    // Verificar mensaje de éxito
    cy.get('app-alerta, .success-message, [data-testid*="alerta-exito"]').should('be.visible');
  });
});