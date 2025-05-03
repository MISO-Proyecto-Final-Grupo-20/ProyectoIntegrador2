describe('Navegación del Menú', () => {
  beforeEach(() => {
    // Iniciar sesión antes de probar los elementos del menú
    cy.fixture('users').then((users) => {
      cy.login(users.admin.email, users.admin.password);
      
      // Esperar a que se complete el inicio de sesión y se cargue el panel
      cy.wait(1000);
    });
  });

  it('debería mostrar todos los elementos de menú esperados para usuario administrador', () => {
    const expectedMenuItems = [
      'Fabricantes',
      'Productos',
      'Vendedores',
      'Planes de venta',
      'Informes',
      'Compras y bodegas',
      'Análisis de tiendas',
      'Programación de rutas'
    ];
    
    expectedMenuItems.forEach(menuItem => {
      cy.get('[data-testid="menu-navegacion"]')
        .contains(menuItem)
        .should('be.visible');
    });
  });

  it('debería navegar a la página correcta al hacer clic en los elementos del menú', () => {
    // Probar la navegación para algunos elementos clave del menú
    
    // 1. Navegar a Productos
    cy.navigateToMenuOption('Productos');
    
    // el elemento app-registrar-planes-venta es visible
    cy.get('form, app-registrar-productos, .productos-form').should('be.visible');
    
    // Esperar a que el menú se cargue de nuevo
    cy.wait(500);
    
    // 2. Navegar a Fabricantes
    cy.navigateToMenuOption('Fabricantes');
    
    // el elemento app-registrar-fabricante es visible
    cy.get('form, app-registrar-fabricante, .fabricante-form').should('be.visible');
    
    // Esperar a que el menú se cargue de nuevo
    cy.wait(500);
    
    // 3. Navegar a Vendedores
    cy.navigateToMenuOption('Vendedores');
    
    // el elemento app-registrar-vendedores es visible
    cy.get('form, app-registrar-vendedores, .vendedor-form').should('be.visible');

    // Esperar a que el menú se cargue de nuevo
    cy.wait(500);

    // 4. Navegar a Planes de venta
    cy.navigateToMenuOption('Planes de venta');

    // el elemento app-planes-venta-container es visible
    cy.get('form, app-planes-venta-container, .planes-venta-form').should('be.visible');

    // Esperar a que el menú se cargue de nuevo
    cy.wait(500);

    // 5. Navegar a Informes
    cy.navigateToMenuOption('Informes');

    // el elemento app-informes-container es visible
    cy.get('form, app-informes-container, .informes-form').should('be.visible');

    // 5. Navegar a Compras y bodegas
    cy.navigateToMenuOption('Compras y bodegas');

    // el elemento app-compras-bodega-container es visible
    cy.get('form, app-compras-bodega-container, .compras-form').should('be.visible');

    // Esperar a que el menú se cargue de nuevo
    cy.wait(500);

    // 6. Navegar a Análisis de tiendas
    cy.navigateToMenuOption('Análisis de tiendas');

    // el elemento app-analisis-tiendas-container es visible
    cy.get('form, app-analisis-tiendas-container, .analisis-tiendas-form').should('be.visible');

    // Esperar a que el menú se cargue de nuevo
    cy.wait(500);

    // 7. Navegar a Programación de rutas
    cy.navigateToMenuOption('Programación de rutas');

    // el elemento app-programacion-rutas-container es visible
    cy.get('form, app-programacion-rutas-container, .programacion-rutas-form').should('be.visible');

  });

});