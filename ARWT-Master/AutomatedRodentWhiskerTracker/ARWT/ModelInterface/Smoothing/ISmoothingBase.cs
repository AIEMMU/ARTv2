﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARWT.ModelInterface.Smoothing
{
    public interface ISmoothingBase : IModelObjectBase
    {
        string Name
        {
            get;
        }

        double[] Smooth(double[] orignalvalues);
    }
}
