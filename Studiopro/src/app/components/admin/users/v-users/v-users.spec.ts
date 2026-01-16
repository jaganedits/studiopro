import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VUsers } from './v-users';

describe('VUsers', () => {
  let component: VUsers;
  let fixture: ComponentFixture<VUsers>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VUsers]
    })
    .compileComponents();

    fixture = TestBed.createComponent(VUsers);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
