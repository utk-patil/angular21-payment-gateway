import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DemoPayComponent } from './demo-pay';

describe('DemoPay', () => {
  let component: DemoPayComponent;
  let fixture: ComponentFixture<DemoPayComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DemoPayComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DemoPayComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
