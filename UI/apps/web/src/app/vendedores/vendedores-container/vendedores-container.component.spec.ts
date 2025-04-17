import { provideHttpClient } from '@angular/common/http';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { VendedoresContainerComponent } from './vendedores-container.component';

describe('VendedoresContainerComponent', () => {
  let component: VendedoresContainerComponent;
  let fixture: ComponentFixture<VendedoresContainerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VendedoresContainerComponent, BrowserAnimationsModule],
      providers: [provideHttpClient()],
    }).compileComponents();

    fixture = TestBed.createComponent(VendedoresContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
