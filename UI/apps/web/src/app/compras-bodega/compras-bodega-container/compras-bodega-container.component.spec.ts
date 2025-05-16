import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ComprasBodegaContainerComponent } from './compras-bodega-container.component';
import { ComprasBodegaStore } from '../state';
import { provideHttpClient } from '@angular/common/http';
import { ComprasBodegaService } from '../compras-bodega.service';
import { AppService } from '../../app.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe('ComprasBodegaContainerComponent', () => {
  let component: ComprasBodegaContainerComponent;
  let fixture: ComponentFixture<ComprasBodegaContainerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ComprasBodegaContainerComponent, BrowserAnimationsModule],
      providers: [
        ComprasBodegaStore,
        provideHttpClient(),
        ComprasBodegaService,
        AppService,
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(ComprasBodegaContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
