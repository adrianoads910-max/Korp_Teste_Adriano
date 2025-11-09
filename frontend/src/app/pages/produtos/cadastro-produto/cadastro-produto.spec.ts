import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CadastroProduto } from './cadastro-produto';

describe('CadastroProduto', () => {
  let component: CadastroProduto;
  let fixture: ComponentFixture<CadastroProduto>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CadastroProduto]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CadastroProduto);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
