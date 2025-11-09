import { TestBed } from '@angular/core/testing';

import { Produtos } from './produtos';

describe('Produtos', () => {
  let service: Produtos;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Produtos);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
