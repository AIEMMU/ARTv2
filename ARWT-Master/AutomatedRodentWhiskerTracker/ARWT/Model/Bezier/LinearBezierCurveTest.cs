using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MVVM_Template1.Model.Bezier
{
    public class LinearBezierCurveTest
    {
        public void Test()
        {
            LinearBezierCurve curve = new LinearBezierCurve();
            Point p1 = new Point(0, 0);
            Point p2 = new Point(2, 2);
            var result = curve.GenerateLinearBezierCurve(p1, p2);

            var midPoint = result[result.Length / 2];

            Assert.AreEqual(new Point(1,1), midPoint);
        }
        
    }
}
