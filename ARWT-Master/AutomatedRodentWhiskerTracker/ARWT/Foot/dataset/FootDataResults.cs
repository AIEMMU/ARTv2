using ARWT.Foot.centroidTracker;
using ARWT.Model.Feet;
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
    public class FootDataResults
    {
        private Dictionary<int, ISingleFrameExtendedResults> _results;

        public FootDataResults(Dictionary<int, ISingleFrameExtendedResults> Results)
        {
            _results = Results;

        }

        public Dictionary<int, ISingleFrameExtendedResults> PostProcess()
        {
            if (_results != null)
            {
                removeDuds();
            }
            return _results;
        }

        private void removeDuds(int maximum = 15)
        {
            List<int> leftFrontNuller = new List<int>();
            List<int> leftHindNuller = new List<int>();
            List<int> RightFrontNuller = new List<int>();
            List<int> RightHindNuller = new List<int>();

            foreach (KeyValuePair<int, ISingleFrameExtendedResults> kvp in _results)
            {
                IFootCollection feet = kvp.Value.FeetCollection;
                int index = kvp.Key;
                //leftFront(leftFrontNuller, index, feet);

                leftFrontNulle(leftFrontNuller, feet, index);
                leftHindNulle(leftHindNuller, feet, index);
                RightFrontNulle(RightFrontNuller, feet, index);
                RightHindNulle(RightHindNuller, feet, index);

            }

        }

        private void RightHindNulle(List<int> rightHindNuller, IFootCollection feet, int index)
        {
            if (feet.rightHind != null)
            {
                rightHindNuller.Add(index);
            }
            else if (feet.rightHind == null && rightHindNuller.Count != 0)
            {

                int max = 0;
                bool found = findRHgap(index, out max);
                int back = rightHindNuller[0] - 1 < 1 ? 0 : rightHindNuller[0] - 1;
                if (!found && rightHindNuller.Count < 15 && _results[back].FeetCollection.rightHind == null)
                
                {
                    clearRightHind(rightHindNuller);
                }else if (!found)
                {
                    rightHindNuller.Clear();
                }

                else if (max != -1)
                {

                    for (int i = index; i < max; i++)
                    {
                        int min = i - 1;
                        IFootPlacement value = getValue(_results[max].FeetCollection.rightHind.value, _results[min].FeetCollection.rightHind.value);
                        _results[i].FeetCollection.rightHind = new feetID { id = _results[max].FeetCollection.rightHind.id, value = value };
                    }
                    rightHindNuller.Add(index);
                    //rightHindNuller.Clear();

                }
            }
        }

        private void RightFrontNulle(List<int> rightFrontNuller, IFootCollection feet, int index)
        {
            if (feet.rightfront != null)
            {
                rightFrontNuller.Add(index);
            }
            else if (feet.rightfront == null && rightFrontNuller.Count != 0)
            {

                int max = 0;
                bool found = findRFgap(index, out max);
                int back = rightFrontNuller[0] - 1 < 1 ? 0 : rightFrontNuller[0] - 1;
                if (!found && rightFrontNuller.Count < 15 && _results[back].FeetCollection.rightfront == null)
                {
                    clearRightFront(rightFrontNuller);
                }else if (!found)
                {
                    rightFrontNuller.Clear();
                }

                else if (max != -1)
                {

                    for (int i = index; i < max; i++)
                    {
                        int min = i - 1;
                        IFootPlacement value = getValue(_results[max].FeetCollection.rightfront.value, _results[min].FeetCollection.rightfront.value);
                        _results[i].FeetCollection.rightfront = new feetID { id = _results[max].FeetCollection.rightfront.id, value = value };
                    }rightFrontNuller.Add(index);
                    //rightFrontNuller.Clear();

                }
            }
        }

        private void leftHindNulle(List<int> leftHindNuller, IFootCollection feet, int index)
        {   if (index == 302)
            {
                int asdds = 0;
            }
            if (index == 303)
            {

                int asdds = 0;
            }
            if (feet.leftHind != null)
            {
                leftHindNuller.Add(index);
            }

            else if (feet.leftHind == null && leftHindNuller.Count != 0 )
            {

                int max = 0;
                bool found = findLHgap(index, out max);
                int back = leftHindNuller[0] - 1 < 1 ? 0 : leftHindNuller[0] - 1;
                if (!found && leftHindNuller.Count < 15 && _results[back].FeetCollection.leftHind == null)
                {
                    clearleftHind(leftHindNuller);
                }else if (!found)
                {
                    leftHindNuller.Clear();
                }

                else if (max != -1)
                {

                    for (int i = index; i < max; i++)
                    {
                        int min = i - 1;
                        IFootPlacement value = getValue(_results[max].FeetCollection.leftHind.value, _results[min].FeetCollection.leftHind.value);
                        _results[i].FeetCollection.leftHind = new feetID { id = _results[max].FeetCollection.leftHind.id, value = value };
                        
                    }
                    leftHindNuller.Add(index);
                   // leftHindNuller.Clear();

                }

            }
        }

        private void leftFrontNulle(List<int> leftFrontNuller, IFootCollection feet, int index)
        {

            if(index == 311)
            {
                int masdaasd = 0;
            }

            if (index == 313)
            {
                int masdaasd = 0;
            }
            if (feet.leftfront != null)
            {
                leftFrontNuller.Add(index);
            }
            else if (feet.leftfront == null && leftFrontNuller.Count != 0)
            {
    
                int max = 0;
                bool found = findLFgap(index, out max);
                int back =  leftFrontNuller[0]-1  < 1? 0 : leftFrontNuller[0] - 1;

                if (!found && leftFrontNuller.Count < 15 && _results[back].FeetCollection.leftfront == null)
                {
                    clearLeftFront(leftFrontNuller);
                }else if (!found)
                {
                    leftFrontNuller.Clear();
                }

                else if (max != -1)
                {

                    for (int i = index; i < max; i++)
                    {
                        int min = i - 1;
                        IFootPlacement value = getValue(_results[max].FeetCollection.leftfront.value, _results[min].FeetCollection.leftfront.value);
                        _results[i].FeetCollection.leftfront = new feetID { id = _results[max].FeetCollection.leftfront.id, value = value };
                    }
                    leftFrontNuller.Add(index);

                    //leftFrontNuller.Clear();

                }

            }
        }



        private IFootPlacement getValue(IFootPlacement value1, IFootPlacement value2)
        {
            IFootPlacement value = new FootPlacement();
            value.centroidX = (value1.centroidX + value2.centroidX) / 2;
            value.centroidY = (value1.centroidY + value2.centroidY) / 2;
            value.height = (value1.height + value2.height) / 2;
            value.width = (value1.width + value2.width) / 2;
            value.minX = (value1.minX + value2.minX) / 2;
            value.minY = (value1.minY + value2.minY) / 2;
            value.maxX = (value1.maxX + value2.maxX) / 2;
            value.maxY = (value1.maxY + value2.maxY) / 2;

            return value;
        }

        private void clearLeftFront(List<int> leftFrontNuller)
        {
            foreach (var item in leftFrontNuller)
            {
                _results[item].FeetCollection.leftfront = null;
            }
            leftFrontNuller.Clear();
        }


        private void clearRightFront(List<int> rightFrontNuller)
        {
            foreach (var item in rightFrontNuller)
            {
                _results[item].FeetCollection.rightfront = null;
            }
            rightFrontNuller.Clear();
        }
        private void clearRightHind(List<int> rightHindNuller)
        {
            foreach (var item in rightHindNuller)
            {
                _results[item].FeetCollection.rightHind= null;
            }
            rightHindNuller.Clear();
        }

        private void clearleftHind(List<int> leftHindNuller)
        {
            foreach (var item in leftHindNuller)
            {
                _results[item].FeetCollection.leftHind = null;
            }
            leftHindNuller.Clear();
        }
        private void leftFront(List<int> leftFrontNuller, int index, IFootCollection feet)
        {
            Console.WriteLine(index);
            
        }
       

        private bool findLFgap( int index,  out int maxIndex)
        {
            int max = -1;
             bool maxFound = false;

            
            for (int i = index + 15; i > index; i--)
            {
                if (i > _results.Count - 1)
                {
                    i = _results.Count - 1;
                }
                if (_results[i].FeetCollection.leftfront != null)
                {
                    max = i;
                    maxFound = true;
                }
            }
            if(maxFound )
            {
               
                maxIndex = max;
                return true;
                
            }else
            {
                
                maxIndex = -1;
                return false;
            }
        }

        private bool findRFgap(int index, out int maxIndex)
        {
            int max = -1;
            bool maxFound = false;


            for (int i = index + 15; i > index; i--)
            {
                if (i > _results.Count - 1)
                {
                    i = _results.Count - 1;
                }
                if (_results[i].FeetCollection.rightfront != null)
                {
                    max = i;
                    maxFound = true;
                }
            }
            if (maxFound)
            {

                maxIndex = max;
                return true;

            }
            else
            {

                maxIndex = -1;
                return false;
            }
        }

        private bool findLHgap(int index, out int maxIndex)
        {
            int max = -1;
            bool maxFound = false;


            for (int i = index + 15; i > index; i--)
            {
                if (i > _results.Count - 1)
                {
                    i = _results.Count - 1;
                }
                if (_results[i].FeetCollection.leftHind != null)
                {
                    max = i;
                    maxFound = true;
                }
            }
            if (maxFound)
            {

                maxIndex = max;
                return true;

            }
            else
            {

                maxIndex = -1;
                return false;
            }
        }
        private bool findRHgap(int index, out int maxIndex)
        {
            int max = -1;
            bool maxFound = false;


            for (int i = index + 15; i > index; i--)
            {
                if (i > _results.Count - 1)
                {
                    i = _results.Count - 1;
                }
                if (_results[i].FeetCollection.rightHind != null)
                {
                    max = i;
                    maxFound = true;
                }
            }
            if (maxFound)
            {

                maxIndex = max;
                return true;

            }
            else
            {

                maxIndex = -1;
                return false;
            }
        }

    }
}
