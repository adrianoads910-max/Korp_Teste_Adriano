import { TestBed } from '@angular/core/testing';

import { Notas } from './notas';

describe('Notas', () => {
  let service: Notas;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Notas);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
