/*
 * Academic License - for use in teaching, academic research, and meeting
 * course requirements at degree granting institutions only.  Not for
 * government, commercial, or other organizational use.
 * File: _coder_GetPrimFreq_api.h
 *
 * MATLAB Coder version            : 4.0
 * C/C++ source code generated on  : 27-Feb-2019 12:51:26
 */

#ifndef _CODER_GETPRIMFREQ_API_H
#define _CODER_GETPRIMFREQ_API_H

/* Include Files */
#include "tmwtypes.h"
#include "mex.h"
#include "emlrt.h"
#include <stddef.h>
#include <stdlib.h>
#include "_coder_GetPrimFreq_api.h"

/* Variable Declarations */
extern emlrtCTX emlrtRootTLSGlobal;
extern emlrtContext emlrtContextGlobal;

/* Function Declarations */
extern real_T GetPrimFreq(real_T theta[1732]);
extern void GetPrimFreq_api(const mxArray * const prhs[1], int32_T nlhs, const
  mxArray *plhs[1]);
extern void GetPrimFreq_atexit(void);
extern void GetPrimFreq_initialize(void);
extern void GetPrimFreq_terminate(void);
extern void GetPrimFreq_xil_terminate(void);

#endif

/*
 * File trailer for _coder_GetPrimFreq_api.h
 *
 * [EOF]
 */
