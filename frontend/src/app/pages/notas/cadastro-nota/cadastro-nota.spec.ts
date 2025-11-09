import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CadastroNota } from './cadastro-nota';

describe('CadastroNota', () => {
  let component: CadastroNota;
  let fixture: ComponentFixture<CadastroNota>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CadastroNota]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CadastroNota);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
