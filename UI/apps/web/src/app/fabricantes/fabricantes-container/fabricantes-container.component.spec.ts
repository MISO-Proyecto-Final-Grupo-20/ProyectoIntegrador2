import { provideHttpClient } from '@angular/common/http';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FabricantesContainerComponent } from './fabricantes-container.component';

describe('FabricantesContainerComponent', () => {
  let component: FabricantesContainerComponent;
  let fixture: ComponentFixture<FabricantesContainerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FabricantesContainerComponent, BrowserAnimationsModule],
      providers: [provideHttpClient()],
    }).compileComponents();

    fixture = TestBed.createComponent(FabricantesContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
