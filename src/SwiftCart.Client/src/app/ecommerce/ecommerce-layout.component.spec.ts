import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EcommerceLayoutComponent } from './ecommerce-layout.component';

describe('EcommerceLayoutComponent', () => {
  let component: EcommerceLayoutComponent;
  let fixture: ComponentFixture<EcommerceLayoutComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EcommerceLayoutComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EcommerceLayoutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
