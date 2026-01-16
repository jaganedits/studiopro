import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CRole } from './c-role';

describe('CRole', () => {
  let component: CRole;
  let fixture: ComponentFixture<CRole>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CRole]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CRole);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
