import { provideHttpClient } from '@angular/common/http';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { PlanesVentaContainerComponent } from './planes-venta-container.component';

describe('PlanesVentaContainerComponent', () => {
  let component: PlanesVentaContainerComponent;
  let fixture: ComponentFixture<PlanesVentaContainerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PlanesVentaContainerComponent, BrowserAnimationsModule],
      providers: [provideHttpClient()],
    }).compileComponents();

    fixture = TestBed.createComponent(PlanesVentaContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
