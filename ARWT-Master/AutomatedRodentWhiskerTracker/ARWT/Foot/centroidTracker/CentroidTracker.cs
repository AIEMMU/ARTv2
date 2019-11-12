using ARWT.Foot.video_processing;
using ARWT.Model.Feet;
using ARWT.ModelInterface.Feet;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;

namespace ARWT.Foot.centroidTracker
{


    public class CentroidTracker
    {
        private int nextObjectID;
        private int firstFoot;


        //private List<IfeetID> feetToo;
        private List<IfeetID> _feetToo;

        public List<IfeetID> feetToo
        {
            get { return _feetToo; }
            set { _feetToo = value;
                //NotifyPropertyChanged();
            }
        }

        private List<disappearedID> disappearedToo { get; set; }

        private int maxDisappeared;
        public CentroidTracker( int _maxDisappeared=1)
        {
            //# initialize the next unique object ID along with two ordered
            //# dictionaries used to keep track of mapping a given object
            //# ID to its centroid and number of consecutive frames it has
            //# been marked as "disappeared", respectively
            nextObjectID = 0;
            firstFoot = 0;

            feetToo = new List<IfeetID>();
            disappearedToo = new List<disappearedID>();
            maxDisappeared = _maxDisappeared;
        }

        private void register(IFootPlacement centroid, string ID)
        {
            
            //var ids = feetToo.Select(x => x.id).ToList();
            //if(ids.Contains(ID))
            //{
            //    return;
            //}
            string id = $"{nextObjectID}_{ID}";
            feetToo.Add(new feetID { id = id, value = centroid });
            disappearedToo.Add(new disappearedID { id = id, value = 0 });
            nextObjectID++;
        }
        private void deregister(int id)
        {
            disappearedToo.RemoveAt(id);
            feetToo.RemoveAt(id);
        }
        public List<IfeetID> update(IFootPlacement[] rects, Point centrePoint, Point headPoint, Point backPoint, Point[] contour)
        {
            //# check to see if the list of input bounding box rectangles
            //# is empty
            //Console.WriteLine(rects.Count());
            if (rects.Count() ==0)
            {
                updateDisappeared();
                return exportFunction();

                //return feetToo;
            }
           
            for (int i =0; i <rects.Length; i++)
            {
                rects[i].centroidX = Convert.ToInt32((rects[i].minX + rects[i].maxX) / 2);
                rects[i].centroidY = Convert.ToInt32((rects[i].minY + rects[i].maxY) / 2);
            }
            

            if (feetToo.Count == 0)
            {
                
                string[] ids = calculateIDS(rects, centrePoint, headPoint, backPoint, contour);
                
                for (int i =0; i<rects.Length; i++)
                {
                    
                    register(rects[i], ids[i]);
                }
            }
            else
            {
                string[] feetIDS = feetToo.Select(x => x.id).ToArray();
                IFootPlacement[] feetValues = feetToo.Select(x => x.value).ToArray();

                List<List<double>> Distances = HelperFunctions.cDist(feetValues, rects);
                foreach(var item in Distances)
                {
                    foreach(var moo in item)
                    {
                     //   Console.Write(moo + " ");
                    }
                  //  Console.Write("\n");
                }
                int[] rows = HelperFunctions.sortRows(Distances);
                int[] cols = HelperFunctions.sortIndexs(Distances, rows);
                List<int> usedCols = new List<int>();
                List<int> usedRows = new List<int>();
                
                for (int i = 0; i < rows.Length; i++)
                {
                    int row = rows[i];
                    int col = cols[i];
                    
                    int rowValue = checkValue(row, feetToo.Count);
                    int colValue = checkValue(col, rects.Length);
                    if (usedRows.Contains(row) || usedCols.Contains(col))
                    {
                        continue;
                    }
                    if(HelperFunctions.getDistance(new Point(feetToo[rowValue].value.centroidX, feetToo[rowValue].value.centroidY), new Point(rects[colValue].centroidX, rects[colValue].centroidY)) < 10)
                    {
                        feetToo[rowValue].value = rects[colValue];
                        disappearedToo[rowValue].value = 0;
                        usedCols.Add(col);
                        usedRows.Add(row);
                    }
                    
                    
                    
                }
               

                List<int> unusedRows = getUnused(usedRows, Distances.Count());
                List<int> unusedCols = getUnused(usedCols, Distances[0].Count());
                
                
                //Console.WriteLine(unusedRows.Count() + " " + unusedCols.Count());
               // Console.WriteLine(Distances.Count() + " " + Distances[0].Count);
                if(Distances.Count ==5 && Distances[0].Count == 3)
                {
                    int moo = 0;
                }
                if (Distances.Count >= Distances[0].Count)
                {
                    List<int> count2 = new List<int>();
                    foreach (var item in unusedRows)
                    {
                        int rowValue2 = checkValue(item, feetToo.Count);

                        //Console.WriteLine("rowValues " +item+" "+ rowValue2 + " " + feetToo.Count+" "+ disappearedToo.Count);
                        disappearedToo[rowValue2].value++;
                        if (disappearedToo[rowValue2].value > maxDisappeared)
                        {
                            if (disappearedToo[rowValue2] != null)
                            {
                                deregister(rowValue2);
                            }
                        }
                    }

                }
                else
                {
                    //Console.WriteLine("Meep hello");
                    string[] ids = calculateIDS(rects, centrePoint, headPoint, backPoint, contour);
                    
                    foreach (var col2 in unusedCols)
                    {
                        int colValue2 = checkValue(col2, ids.Length);
                        int col = checkValue(col2, rects.Length);
                        register(rects[col], ids[colValue2]);
                    }
                }

            }

            return exportFunction();// feetToo;
        }

        private List<IfeetID> exportFunction()
        {
            List<IfeetID> export = new List<IfeetID>();
            foreach(var item in feetToo)
            {
                export.Add(new feetID { id = item.id, value = item.value });
            }
            //Console.WriteLine("asdasda");
            return export;
        }

        private List<int> getUnused(List<int> used, int Length)
        {   
            List<int> unused = new List<int>();
            List<int> unused2 = new List<int>();
            for (int i = 0; i<Length; i++)
            { 
                if (!used.Contains(i))
                {
                    unused.Add(i);
                }
            }
            
            return unused;
        }

        private void printItem(IEnumerable<int> item)
        {
            foreach(var i in item)
            {
                //Console.Write(i + " ");
            }
            //Console.Write("\n");
        }

        private int checkValue(int col, int length)
        {
            int value = col;
            while (value >= length)
            {
                if (value >= length)
                {
                    value -= length;
                }
            }
            return value;
        }

        private void updateDisappeared()
        {
            List<int> count = new List<int>();
            for (int i = 0; i < disappearedToo.Count(); i++)
            {
                disappearedToo[i].value++;
                if (disappearedToo[i].value > maxDisappeared)
                {
                    count.Add(i);
                }
            }
            foreach (int i in count)
            {
                deregister(i);
            }
        }

        private string[] calculateRightOrder(Point centroid1, Point centroid2, Point headPoint, string ID)
        {
            double dist1 = HelperFunctions.getDistance(centroid1, headPoint);
            double dist2 = HelperFunctions.getDistance(centroid2, headPoint);
            string[] newIds = new string[2];
            if(dist1 < dist2)
            {
                newIds[0] = ID + "F";
                newIds[1] = ID + "H";
                return newIds;
            }
            else
            {
                newIds[0] = ID + "H";
                newIds[1] = ID + "F";
                return newIds;
            }

        }
        private string[] calculateIDS(IFootPlacement[] centroids, Point centrePoint, Point headPoint, Point backPoint, Point[] contour)
        {
            List<string> ids = new List<string>();
            foreach(var centroid in centroids)
            {
                PointF distanceFrom = HelperFunctions.getDistanceFromLine(new Point(centroid.centroidX, centroid.centroidY), centrePoint, headPoint);
                double distToHead = HelperFunctions.getDistance(centrePoint, headPoint);
                double distToBack = HelperFunctions.getDistance(centrePoint, backPoint);
                //PointF backDistance = HelperFunctions.getDistanceFromLine(new Point(centroid.centroidX, centroid.centroidY), centrePoint, backPoint);
                double distFootToHead = HelperFunctions.getDistance(new Point(centroid.centroidX, centroid.centroidY), headPoint);
                double distFootToback = HelperFunctions.getDistance(new Point(centroid.centroidX, centroid.centroidY), backPoint);
                double distContour = HelperFunctions.getDistanceFromContour(new Point(centroid.centroidX, centroid.centroidY), contour);

                if(distanceFrom.X >= 0)
                {
                    if(firstFoot == 0)
                    {
                        ids.Add("RF");
                        firstFoot++;
                    }else
                    {

                        if (distanceFrom.Y > (distToHead / 5) -2 /*&& distContour < distToHead / 2*/)
                        {
                            ids.Add("RF");
                        }
                        else if (distanceFrom.Y < distToHead / 3 && distanceFrom.Y > -Math.Abs(distFootToHead) * 3 && distContour < distToHead / 2)
                        {
                            ids.Add("RH");
                        }
                    }
                }
                else
                {
                    if (firstFoot == 0)
                    {
                        ids.Add("LF");
                        firstFoot++;
                    }
                    else
                    {

                        if (distanceFrom.Y > (distToHead / 5)-2 /*&& distContour < distToHead / 2*/)
                        {
                            ids.Add("LF");
                        }
                        else if (distanceFrom.Y < distToHead / 3 && distanceFrom.Y > -Math.Abs(distFootToHead) * 3 && distContour < distToHead / 2)
                        {
                            ids.Add("LH");
                        }

                    }

                }
            }
            
            return calcIDS(ids.ToArray(), centroids, headPoint);
           
        }
        private string[] calcIDS(string[] ids, IFootPlacement[] centroids, Point headPoint)
        {
            int changeL = 0;
            int changeR = 0;

            var duplicates = ids.GroupBy(x => x).Where(g => g.Count() > 1).Select(i => new { Number = i.Key, Count = i.Count() });

            if (duplicates.Count() == 0)
            {
                return ids;
            }

            
            for (int i = 0; i < ids.Count()-1; i++)
            {
                for (int j = 1; j < ids.Count(); j++)
                {
                    if (ids[i] == "RF" && ids[j] == "RF" || ids[i] == "RH" && ids[j] == "RH" && changeR == 0)
                    {
                       
                            string[] newIds = calculateRightOrder(new Point(centroids[i].centroidX, centroids[i].centroidY), new Point(centroids[j].centroidX, centroids[j].centroidY), headPoint, "R");
                            ids[i] = newIds[0];
                            ids[j] = newIds[1];
                            changeR++;
                     
                    }

                    if (ids[i] == "LF" && ids[j] == "LF" || ids[i] == "LH" && ids[j] == "LH" && changeL ==0)
                    {
                       
                            string[] newIds = calculateRightOrder(new Point(centroids[i].centroidX, centroids[i].centroidY), new Point(centroids[j].centroidX, centroids[j].centroidY), headPoint, "L");
                            ids[i] = newIds[0];
                            ids[j] = newIds[1];
                            changeL++;
                        
                    }
                }
            }
            return ids.ToArray();
        }
    }
   
}
