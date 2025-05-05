import { TestBed } from '@angular/core/testing';
import { VendedoresService } from './vendedores.service';
import { provideHttpClient } from '@angular/common/http';

describe('VendedoresService', () => {
  let service: VendedoresService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [VendedoresService, provideHttpClient()],
    });
    service = TestBed.inject(VendedoresService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
