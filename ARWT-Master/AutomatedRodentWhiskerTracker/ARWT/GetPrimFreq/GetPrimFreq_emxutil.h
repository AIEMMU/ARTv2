/*
 * Academic License - for use in teaching, academic research, and meeting
 * course requirements at degree granting institutions only.  Not for
 * government, commercial, or other organizational use.
 * File: GetPrimFreq_emxutil.h
 *
 * MATLAB Coder version            : 4.0
 * C/C++ source code generated on  : 27-Feb-2019 12:51:26
 */

#ifndef GETPRIMFREQ_EMXUTIL_H
#define GETPRIMFREQ_EMXUTIL_H

/* Include Files */
#include <stddef.h>
#include <stdlib.h>
#include "rtwtypes.h"
#include "GetPrimFreq_types.h"

/* Function Declarations */
extern void emxEnsureCapacity_int16_T(emxArray_int16_T *emxArray, int oldNumel);
extern void emxFree_int16_T(emxArray_int16_T **pEmxArray);
extern void emxInit_int16_T(emxArray_int16_T **pEmxArray, int numDimensions);

#endif

/*
 * File trailer for GetPrimFreq_emxutil.h
 *
 * [EOF]
 */
