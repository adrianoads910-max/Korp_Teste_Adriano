import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CadastroNotaComponent } from './cadastro-nota';

describe('CadastroNotaComponent', () => {
  let component: CadastroNotaComponent;
  let fixture: ComponentFixture<CadastroNotaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CadastroNotaComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CadastroNotaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
