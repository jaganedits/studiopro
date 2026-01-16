import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CBranchs } from './c-branchs';

describe('CBranchs', () => {
  let component: CBranchs;
  let fixture: ComponentFixture<CBranchs>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CBranchs]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CBranchs);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
