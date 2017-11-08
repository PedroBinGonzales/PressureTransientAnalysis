# Pressure Transient Analysis

Library with analytical models for pressure transient analysis (well testing)

Models (To-Do list)
------

**Wells**
- [x] Vertical well
- [ ] Vertical well with partial completion
- [ ] Vertical well with finite-conductivity vertical fracture
- [ ] Vertical well with infinite conductivite vertical fracture
- [ ] Horizontal well

**Reservoirs**
- [x] Infinite homogeneous reservoir
- [ ] Reservor with dual porosity
- [ ] Reservor with dual permeability

**Boundaries**
- [x] Linear sealing fault
- [x] Linear constant pressure
- [x] Perpendicular sealing faults
- [x] Perpendicular constant pressures
- [x] Perpendicular mixed boundaries

Example
-------

This is a basic example which shows you how to solve a common problem:

``` csharp
var fluid = new Fluid() { B = 1, Mu = 1 };
var well = new Well() { C = 1, Rw = 0.15, SkinFactor = 0 };
var reservoir = new Reservoir() { Ct = 0.00001, Porosity = 0.2, H = 10, K = 10};
var pta = new InfHomPtaModel(fluid, well, reservoir);

var q = 500;
var time = 100;

var pressureDrop = pta.PressureDrop(time, q);
```


References
----------

1. [COMPUTER APPLICATION ON WELL TEST MATHEMATICAL MODEL COMPUTATION OF HOMOGENOUS AND MULTIPLE-BOUNDED RESERVOIRS](http://www.arpapress.com/volumes/vol11issue1/ijrras_11_1_05.pdf)