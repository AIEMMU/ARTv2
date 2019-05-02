/*
 * Academic License - for use in teaching, academic research, and meeting
 * course requirements at degree granting institutions only.  Not for
 * government, commercial, or other organizational use.
 * File: power.c
 *
 * MATLAB Coder version            : 4.0
 * C/C++ source code generated on  : 27-Feb-2019 12:51:26
 */

/* Include Files */
#include "rt_nonfinite.h"
#include "GetPrimFreq.h"
#include "power.h"

/* Function Definitions */

/*
 * Arguments    : const double a[4096]
 *                double y[4096]
 * Return Type  : void
 */
void power(const double a[4096], double y[4096])
{
  int k;
  for (k = 0; k < 4096; k++) {
    y[k] = a[k] * a[k];
  }
}

/*
 * File trailer for power.c
 *
 * [EOF]
 */
