using ARWT.Foot.video_processing;
using ARWT.ModelInterface.Feet;
using ARWT.ModelInterface.Results;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARWT.Foot.dataset
{
    public class FootStrideData
    {
        public GenericFootStats leftFront;
        public GenericFootStats leftHind;
        public GenericFootStats rightFront;
        public GenericFootStats righHind;
        private Dictionary<int, ISingleFrameExtendedResults> _results;
        private double _unitsToMM;

        public FootStrideData(Dictionary<int, ISingleFrameExtendedResults> results, double unitstomm)
        {
            _results = results;
            _unitsToMM = unitstomm;
            leftFront = new GenericFootStats();
            leftHind = new GenericFootStats();
            rightFront = new GenericFootStats();
            righHind = new GenericFootStats();

            generateResults();
        }

        public FootStrideData()
        {
        }

        private void generateResults()
        {
            genStrideData();
        }

        private void genStrideData()
        {
            genLeftStride();
            genRightFrontStride();
            genLeftHindStride();
            genRightHindStride();
        }

        private void genLeftStride()
        {
            int count = 0;
            List<List<IFootPlacement>> feetplacement = new List<List<IFootPlacement>>();
            List<List<int>> placement = new List<List<int>>();
            List<List<int>> aitTime = new List<List<int>>();

            while(count < _results.Count)
            {
                List<IFootPlacement> foot = new List<IFootPlacement>();
                List<int> down = new List<int>();
                List<int> up = new List<int>();
                //Console.WriteLine(count + "Hey" );
                if(count < _results.Count -1)
                {
                    while (_results[count].FeetCollection.leftfront != null && count < _results.Count - 1)
                    {
                        foot.Add(_results[count].FeetCollection.leftfront.value);
                        down.Add(count);
                        //Console.WriteLine(count + "Hey you ");
                        count++;
                    }
                }
                if (count < _results.Count-1)
                {
                    while (_results[count].FeetCollection.leftfront == null && count <_results.Count-1)
                    {
                        up.Add(count);
                        //Console.WriteLine(count + "Hey Business");
                        count++;
                    }
                }
                if (foot.Count !=0)
                {
                    feetplacement.Add(foot);
                }
                if (up.Count != 0)
                {
                    aitTime.Add(up);
                }
                if (down.Count != 0)
                {
                    placement.Add(down);
                }
                if (count >= _results.Count - 1)
                {
                    break;
                }
            }
           
            int air = Convert.ToInt32(getAverageInt(aitTime));
            int place = Convert.ToInt32(getAverageInt(placement));
            double stride = getAverageStrides(feetplacement);
            leftFront.airTime = air;
            leftFront.footPlacementTime = place;
            leftFront.strideLength = stride * _unitsToMM;
        }

        private void genLeftHindStride()
        {
            int count = 0;
            List<List<IFootPlacement>> feetplacement = new List<List<IFootPlacement>>();
            List<List<int>> placement = new List<List<int>>();
            List<List<int>> aitTime = new List<List<int>>();

            while (count < _results.Count)
            {
                List<IFootPlacement> foot = new List<IFootPlacement>();
                List<int> down = new List<int>();
                List<int> up = new List<int>();
                if (count < _results.Count - 1)
                {
                    while (_results[count].FeetCollection.leftHind != null && count < _results.Count - 1)
                    {
                        foot.Add(_results[count].FeetCollection.leftHind.value);
                        down.Add(count);
                        count++;
                    }
                }
                if (count < _results.Count - 1)
                {
                    while (_results[count].FeetCollection.leftHind == null && count < _results.Count - 1)
                    {
                        up.Add(count);
                        count++;
                    }
                }
                if (foot.Count != 0)
                {
                    feetplacement.Add(foot);
                }
                if (up.Count != 0)
                {
                    aitTime.Add(up);
                }
                if (down.Count != 0)
                {
                    placement.Add(down);
                }
                if (count >= _results.Count - 1)
                {
                    break;
                }
            }
            int air = Convert.ToInt32(getAverageInt(aitTime));
            int place = Convert.ToInt32(getAverageInt(placement));
            double stride = getAverageStrides(feetplacement);
            leftHind.airTime = air;
            leftHind.footPlacementTime = place;
            leftHind.strideLength = stride * _unitsToMM;
        }

        private void genRightHindStride()
        {
            int count = 0;
            List<List<IFootPlacement>> feetplacement = new List<List<IFootPlacement>>();
            List<List<int>> placement = new List<List<int>>();
            List<List<int>> aitTime = new List<List<int>>();

            while (count < _results.Count)
            {
                List<IFootPlacement> foot = new List<IFootPlacement>();
                List<int> down = new List<int>();
                List<int> up = new List<int>();
                if (count < _results.Count - 1)
                {
                    while (_results[count].FeetCollection.rightHind != null && count < _results.Count - 1)
                    {
                        foot.Add(_results[count].FeetCollection.rightHind.value);
                        down.Add(count);
                        count++;
                    }
                }
                if (count < _results.Count - 1)
                {
                    while (_results[count].FeetCollection.rightHind == null && count < _results.Count - 1)
                    {
                        up.Add(count);
                        count++;
                    }
                }
                if (foot.Count != 0)
                {
                    feetplacement.Add(foot);
                }
                if (up.Count != 0)
                {
                    aitTime.Add(up);
                }
                if (down.Count != 0)
                {
                    placement.Add(down);
                }
                if (count >= _results.Count - 1)
                {
                    break;
                }
            }
            int air = Convert.ToInt32(getAverageInt(aitTime));
            int place = Convert.ToInt32(getAverageInt(placement));
            double stride = getAverageStrides(feetplacement);
            righHind.airTime = air;
            righHind.footPlacementTime = place;
            righHind.strideLength = stride * _unitsToMM;
        }

        private void genRightFrontStride()
        {
            int count = 0;
            List<List<IFootPlacement>> feetplacement = new List<List<IFootPlacement>>();
            List<List<int>> placement = new List<List<int>>();
            List<List<int>> aitTime = new List<List<int>>();

            while (count < _results.Count)
            {
                List<IFootPlacement> foot = new List<IFootPlacement>();
                List<int> down = new List<int>();
                List<int> up = new List<int>();
                if (count < _results.Count - 1)
                {
                    while (_results[count].FeetCollection.rightfront != null && count < _results.Count - 1)
                    {
                        foot.Add(_results[count].FeetCollection.rightfront.value);
                        down.Add(count);
                        count++;
                    }
                }
                if (count < _results.Count - 1)
                {
                    while (_results[count].FeetCollection.rightfront == null && count < _results.Count - 1)
                    {
                        up.Add(count);
                        count++;
                    }
                }
                if (foot.Count != 0)
                {
                    feetplacement.Add(foot);
                }
                if (up.Count != 0)
                {
                    aitTime.Add(up);
                }
                if (down.Count != 0)
                {
                    placement.Add(down);
                }
                if (count >= _results.Count - 1)
                {
                    break;
                }
            }
            int air = Convert.ToInt32(getAverageInt(aitTime));
            int place = Convert.ToInt32(getAverageInt(placement));
            double stride = getAverageStrides(feetplacement);
            rightFront.airTime = air;
            rightFront.footPlacementTime = place;
            rightFront.strideLength = stride * _unitsToMM;
        }
        private double getAverageStrides(List<List<IFootPlacement>> feet)
        {
            if(feet.Count <= 1)
            {
                return 0.0;
            }
            List<Point> points = new List<Point>();
            foreach(var item in feet)
            {
                double x = item.Select(cx => cx.centroidX).ToArray().Average();
                double y = item.Select(cy => cy.centroidY).ToArray().Average();
                points.Add(new Point(Convert.ToInt32(x), Convert.ToInt32(y)));
            }
            List<double> strides = new List<double>() ;
            for(int i =1; i <points.Count;i++)
            {
                strides.Add( HelperFunctions.getDistance(points[i - 1], points[i]));
            }
           
            return strides.Average();
        }

        private double getAverageInt(List<List<int>> items)
        {
            if (items.Count == 0)
            {
                return 0;
            }
            List<int> item = new List<int>();
            foreach (var it in items)
            {
                item.Add(it.Count);
            }
            return item.Average();
        }
    }
    
}
