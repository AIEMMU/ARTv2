/*
 * Academic License - for use in teaching, academic research, and meeting
 * course requirements at degree granting institutions only.  Not for
 * government, commercial, or other organizational use.
 * File: GetPrimFreq_emxutil.c
 *
 * MATLAB Coder version            : 4.0
 * C/C++ source code generated on  : 27-Feb-2019 12:51:26
 */

/* Include Files */
#include <stdlib.h>
#include <string.h>
#include "rt_nonfinite.h"
#include "GetPrimFreq.h"
#include "GetPrimFreq_emxutil.h"

/* Function Definitions */

/*
 * Arguments    : emxArray_int16_T *emxArray
 *                int oldNumel
 * Return Type  : void
 */
void emxEnsureCapacity_int16_T(emxArray_int16_T *emxArray, int oldNumel)
{
  int newNumel;
  int i;
  void *newData;
  if (oldNumel < 0) {
    oldNumel = 0;
  }

  newNumel = 1;
  for (i = 0; i < emxArray->numDimensions; i++) {
    newNumel *= emxArray->size[i];
  }

  if (newNumel > emxArray->allocatedSize) {
    i = emxArray->allocatedSize;
    if (i < 16) {
      i = 16;
    }

    while (i < newNumel) {
      if (i > 1073741823) {
        i = MAX_int32_T;
      } else {
        i <<= 1;
      }
    }

    newData = calloc((unsigned int)i, sizeof(short));
    if (emxArray->data != NULL) {
      memcpy(newData, (void *)emxArray->data, sizeof(short) * oldNumel);
      if (emxArray->canFreeData) {
        free((void *)emxArray->data);
      }
    }

    emxArray->data = (short *)newData;
    emxArray->allocatedSize = i;
    emxArray->canFreeData = true;
  }
}

/*
 * Arguments    : emxArray_int16_T **pEmxArray
 * Return Type  : void
 */
void emxFree_int16_T(emxArray_int16_T **pEmxArray)
{
  if (*pEmxArray != (emxArray_int16_T *)NULL) {
    if (((*pEmxArray)->data != (short *)NULL) && (*pEmxArray)->canFreeData) {
      free((void *)(*pEmxArray)->data);
    }

    free((void *)(*pEmxArray)->size);
    free((void *)*pEmxArray);
    *pEmxArray = (emxArray_int16_T *)NULL;
  }
}

/*
 * Arguments    : emxArray_int16_T **pEmxArray
 *                int numDimensions
 * Return Type  : void
 */
void emxInit_int16_T(emxArray_int16_T **pEmxArray, int numDimensions)
{
  emxArray_int16_T *emxArray;
  int i;
  *pEmxArray = (emxArray_int16_T *)malloc(sizeof(emxArray_int16_T));
  emxArray = *pEmxArray;
  emxArray->data = (short *)NULL;
  emxArray->numDimensions = numDimensions;
  emxArray->size = (int *)malloc(sizeof(int) * numDimensions);
  emxArray->allocatedSize = 0;
  emxArray->canFreeData = true;
  for (i = 0; i < numDimensions; i++) {
    emxArray->size[i] = 0;
  }
}

/*
 * File trailer for GetPrimFreq_emxutil.c
 *
 * [EOF]
 */
