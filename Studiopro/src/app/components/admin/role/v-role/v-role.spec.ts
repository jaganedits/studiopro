import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VRole } from './v-role';

describe('VRole', () => {
  let component: VRole;
  let fixture: ComponentFixture<VRole>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VRole]
    })
    .compileComponents();

    fixture = TestBed.createComponent(VRole);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
