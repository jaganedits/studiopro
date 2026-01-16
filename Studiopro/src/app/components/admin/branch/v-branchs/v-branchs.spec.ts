import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VBranchs } from './v-branchs';

describe('VBranchs', () => {
  let component: VBranchs;
  let fixture: ComponentFixture<VBranchs>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VBranchs]
    })
    .compileComponents();

    fixture = TestBed.createComponent(VBranchs);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
