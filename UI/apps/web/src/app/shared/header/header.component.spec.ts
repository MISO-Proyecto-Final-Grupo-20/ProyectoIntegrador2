import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { HeaderComponent } from './header.component';

describe('HeaderComponent', () => {
  let component: HeaderComponent;
  let fixture: ComponentFixture<HeaderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HeaderComponent],
      providers: [],
    }).compileComponents();

    fixture = TestBed.createComponent(HeaderComponent);
    component = fixture.componentInstance;

    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('Debe ocultarse la "seccion-avatar" cuando no tenga nombre de usuario ', () => {
    fixture.detectChanges();

    const seccionAvatar = fixture.debugElement.query(
      By.css(`[data-testid="seccion-avatar"]`)
    );
    expect(seccionAvatar).toBeFalsy();
  });
  it('Debe mostrarse la "seccion-avatar" cuando traiga nombre de usuario', () => {
    fixture.componentRef.setInput('nombreUsuario', 'pepito');
    fixture.detectChanges();

    const seccionAvatar = fixture.debugElement.query(
      By.css(`[data-testid="seccion-avatar"]`)
    );
    expect(seccionAvatar).toBeTruthy();
  });
});
