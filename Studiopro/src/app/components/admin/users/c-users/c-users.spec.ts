import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CUsers } from './c-users';

describe('CUsers', () => {
  let component: CUsers;
  let fixture: ComponentFixture<CUsers>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CUsers]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CUsers);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
