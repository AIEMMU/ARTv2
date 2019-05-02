/*
 * Academic License - for use in teaching, academic research, and meeting
 * course requirements at degree granting institutions only.  Not for
 * government, commercial, or other organizational use.
 * File: GetPrimFreq.c
 *
 * MATLAB Coder version            : 4.0
 * C/C++ source code generated on  : 27-Feb-2019 12:51:26
 */

/* Include Files */
#include "rt_nonfinite.h"
#include "GetPrimFreq.h"
#include "GetPrimFreq_emxutil.h"
#include "xcorr.h"

/* Function Definitions */

/*
 * parameters
 * Arguments    : const double theta[1732]
 * Return Type  : double
 */
double GetPrimFreq(const double theta[1732])
{
  double f;
  double tmp1;
  int i;
  emxArray_int16_T *optima;
  double b_theta[1732];
  double c1[3463];
  int m;
  int iyLead;
  double work;
  emxArray_int16_T *b_optima;
  double b_y1[1731];
  boolean_T y;
  boolean_T x[2];
  boolean_T exitg1;
  int loop_ub;
  emxArray_int16_T *c_optima;
  boolean_T b_x[3];
  static const signed char iv0[3] = { -1, 1, -1 };

  double perf[3];

  /*  % pre-filter signal */
  /*  [b, a] = butter(3, (fq_max*2)/fq_nyquist); */
  /*  theta = filter(b, a, theta); */
  /*  theta = theta + m; */
  /*  pre-filter signal */
  /*  smooth(theta, 5); */
  /*  derived */
  /* time plot */
  /*  subplot(3,1,1) */
  /*  plot(t, theta); */
  /*  xlabel('time (secs)'); */
  /*  ylabel('theta (deg)'); */
  /*  acf plot */
  /* subplot(3,1,2) */
  tmp1 = theta[0];
  for (i = 0; i < 1731; i++) {
    tmp1 += theta[i + 1];
  }

  tmp1 /= 1732.0;
  for (i = 0; i < 1732; i++) {
    b_theta[i] = theta[i] - tmp1;
  }

  emxInit_int16_T(&optima, 2);
  autocorr(b_theta, c1);
  scaleXcorr(c1);

  /* smooth(unsmooth_acf, n_smooth); */
  /*  plot(lag * T, acf, '.-') */
  /*  xlabel('lag (seconds)'); */
  /*  ylabel('ACF'); */
  /*  hold on */
  /*  now, we want to find the primary freq, which is the first */
  /*  maximum in the acf away from zero lag */
  m = optima->size[0] * optima->size[1];
  optima->size[0] = 2;
  optima->size[1] = 0;
  emxEnsureCapacity_int16_T(optima, m);
  i = 1732;
  iyLead = 0;
  work = c1[1731];
  for (m = 0; m < 1731; m++) {
    tmp1 = work;
    work = c1[i];
    tmp1 = c1[i] - tmp1;
    i++;
    b_y1[iyLead] = tmp1;
    iyLead++;
  }

  emxInit_int16_T(&b_optima, 2);
  for (i = 0; i < 1730; i++) {
    if (b_y1[i + 1] == 0.0) {
      b_y1[i + 1] = 1.0E-8;
    }

    if (b_y1[i] * b_y1[i + 1] < 0.0) {
      if (b_y1[i + 1] > 0.0) {
        m = b_optima->size[0] * b_optima->size[1];
        b_optima->size[0] = 2;
        b_optima->size[1] = optima->size[1] + 1;
        emxEnsureCapacity_int16_T(b_optima, m);
        loop_ub = optima->size[1];
        for (m = 0; m < loop_ub; m++) {
          for (iyLead = 0; iyLead < 2; iyLead++) {
            b_optima->data[iyLead + b_optima->size[0] * m] = optima->data[iyLead
              + optima->size[0] * m];
          }
        }

        b_optima->data[b_optima->size[0] * optima->size[1]] = -1;
        b_optima->data[1 + b_optima->size[0] * optima->size[1]] = (short)(2 + i);
        m = optima->size[0] * optima->size[1];
        optima->size[0] = 2;
        optima->size[1] = b_optima->size[1];
        emxEnsureCapacity_int16_T(optima, m);
        loop_ub = b_optima->size[1];
        for (m = 0; m < loop_ub; m++) {
          for (iyLead = 0; iyLead < 2; iyLead++) {
            optima->data[iyLead + optima->size[0] * m] = b_optima->data[iyLead +
              b_optima->size[0] * m];
          }
        }
      } else {
        m = b_optima->size[0] * b_optima->size[1];
        b_optima->size[0] = 2;
        b_optima->size[1] = optima->size[1] + 1;
        emxEnsureCapacity_int16_T(b_optima, m);
        loop_ub = optima->size[1];
        for (m = 0; m < loop_ub; m++) {
          for (iyLead = 0; iyLead < 2; iyLead++) {
            b_optima->data[iyLead + b_optima->size[0] * m] = optima->data[iyLead
              + optima->size[0] * m];
          }
        }

        b_optima->data[b_optima->size[0] * optima->size[1]] = 1;
        b_optima->data[1 + b_optima->size[0] * optima->size[1]] = (short)(2 + i);
        m = optima->size[0] * optima->size[1];
        optima->size[0] = 2;
        optima->size[1] = b_optima->size[1];
        emxEnsureCapacity_int16_T(optima, m);
        loop_ub = b_optima->size[1];
        for (m = 0; m < loop_ub; m++) {
          for (iyLead = 0; iyLead < 2; iyLead++) {
            optima->data[iyLead + optima->size[0] * m] = b_optima->data[iyLead +
              b_optima->size[0] * m];
          }
        }
      }
    }
  }

  emxFree_int16_T(&b_optima);
  for (m = 0; m < 2; m++) {
    x[m] = (optima->size[m] == 0);
  }

  y = true;
  i = 1;
  exitg1 = false;
  while ((!exitg1) && (i < 3)) {
    if (!x[i - 1]) {
      y = false;
      exitg1 = true;
    } else {
      i++;
    }
  }

  if (y) {
    f = rtNaN;
  } else {
    /*  plot optima */
    /* plot(lag(lagi) * T, acf(lagi), 'ro') */
    m = optima->size[1];
    if (m < 3) {
      f = rtNaN;
    } else {
      emxInit_int16_T(&c_optima, 2);

      /*  confirm optima types are what we expect */
      loop_ub = optima->size[1];
      m = c_optima->size[0] * c_optima->size[1];
      c_optima->size[0] = 1;
      c_optima->size[1] = loop_ub;
      emxEnsureCapacity_int16_T(c_optima, m);
      for (m = 0; m < loop_ub; m++) {
        c_optima->data[c_optima->size[0] * m] = optima->data[optima->size[0] * m];
      }

      for (m = 0; m < 3; m++) {
        b_x[m] = (c_optima->data[m] != iv0[m]);
      }

      y = false;
      i = 0;
      exitg1 = false;
      while ((!exitg1) && (i < 3)) {
        if (b_x[i]) {
          y = true;
          exitg1 = true;
        } else {
          i++;
        }
      }

      if (y) {
        /*      pause */
        f = rtNaN;
      } else {
        /*  confirm timings are what we expect */
        iyLead = optima->data[1 + optima->size[0]];
        loop_ub = optima->size[1];
        m = c_optima->size[0] * c_optima->size[1];
        c_optima->size[0] = 1;
        c_optima->size[1] = loop_ub;
        emxEnsureCapacity_int16_T(c_optima, m);
        for (m = 0; m < loop_ub; m++) {
          c_optima->data[c_optima->size[0] * m] = optima->data[1 + optima->size
            [0] * m];
        }

        for (m = 0; m < 3; m++) {
          perf[m] = (double)c_optima->data[m] / ((double)iyLead * (0.5 + 0.5 *
            (double)m));
        }

        y = false;
        i = 0;
        exitg1 = false;
        while ((!exitg1) && (i < 3)) {
          if (perf[i] > 2.0) {
            y = true;
            exitg1 = true;
          } else {
            i++;
          }
        }

        if (y) {
          f = rtNaN;
        } else {
          y = false;
          i = 0;
          exitg1 = false;
          while ((!exitg1) && (i < 3)) {
            if (perf[i] < 0.3) {
              y = true;
              exitg1 = true;
            } else {
              i++;
            }
          }

          if (y) {
            f = rtNaN;
          } else {
            /*  we now have a good estimate of whisk freq */
            /*  optimise wrt original (unsmoothed) acf */
            /*  subplot(3,1,3) */
            /*  plot(lag * T, unsmooth_acf, '.-'); */
            /*  hold on */
            /*  optimise */
            while (c1[iyLead + 1731] > c1[iyLead + 1730]) {
              iyLead++;
            }

            while (c1[iyLead + 1729] > c1[iyLead + 1730]) {
              iyLead--;
            }

            /* plot(lag(T_whisk) * T, unsmooth_acf(T_whisk), 'ro') */
            /*  return freq to caller */
            f = 1.0 / (((double)iyLead - 1.0) * 0.002);
          }
        }
      }

      emxFree_int16_T(&c_optima);
    }
  }

  emxFree_int16_T(&optima);
  return f;
}

/*
 * File trailer for GetPrimFreq.c
 *
 * [EOF]
 */
