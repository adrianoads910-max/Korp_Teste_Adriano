import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListaNotas } from './lista-notas';

describe('ListaNotas', () => {
  let component: ListaNotas;
  let fixture: ComponentFixture<ListaNotas>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListaNotas]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ListaNotas);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
