/*
 * Academic License - for use in teaching, academic research, and meeting
 * course requirements at degree granting institutions only.  Not for
 * government, commercial, or other organizational use.
 * File: GetPrimFreq_types.h
 *
 * MATLAB Coder version            : 4.0
 * C/C++ source code generated on  : 27-Feb-2019 12:51:26
 */

#ifndef GETPRIMFREQ_TYPES_H
#define GETPRIMFREQ_TYPES_H

/* Include Files */
#include "rtwtypes.h"

/* Type Definitions */
#ifndef struct_emxArray_int16_T
#define struct_emxArray_int16_T

struct emxArray_int16_T
{
  short *data;
  int *size;
  int allocatedSize;
  int numDimensions;
  boolean_T canFreeData;
};

#endif                                 /*struct_emxArray_int16_T*/

#ifndef typedef_emxArray_int16_T
#define typedef_emxArray_int16_T

typedef struct emxArray_int16_T emxArray_int16_T;

#endif                                 /*typedef_emxArray_int16_T*/
#endif

/*
 * File trailer for GetPrimFreq_types.h
 *
 * [EOF]
 */
