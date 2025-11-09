import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListaNotasComponent } from './lista-notas';

describe('ListaNotas', () => {
  let component: ListaNotasComponent;
  let fixture: ComponentFixture<ListaNotasComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListaNotasComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ListaNotasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
