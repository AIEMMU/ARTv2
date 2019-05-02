using ArtLibrary.ModelInterface.Video;
using ARWT.Commands;
using ARWT.Foot.centroidTracker;
using ARWT.Model.Results;
using ARWT.ModelInterface.Datasets;
using ARWT.ModelInterface.Feet;
using ARWT.ModelInterface.RBSK2;
using ARWT.ModelInterface.Results;
using ARWT.Resolver;
using ARWT.Services;
using ARWT.ViewModel;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ARWT.Foot.review.viewmodel
{
    public class FootReviewViewModel: WindowBaseModel
    {
        private IVideo _video;
        public Image<Bgr, byte> currentFrame { get; set; }
        public IMouseDataExtendedResult Results { get; }

        public FootReviewViewModel(IVideo video, IMouseDataExtendedResult results)
        {
            _video = video;
            Results = results;
            CurrentFrame = 0;
            FrameCount = _video.FrameCount ;
            
            updateDisplayimage();
        }

        private void updateDisplayimage()
        {
            _video.SetFrame(CurrentFrame);
            using ( currentFrame = _video.GetFrameImage())
            {
                if (currentFrame == null)
                {
                    return;
                }
                drawFeet();
                Image = ImageService.ToBitmapSource(currentFrame);
            }
        }

        private void drawFeet()
        {
            if (Results != null)
            {
                ISingleFrameExtendedResults frame = Results.Results[CurrentFrame];
                if(frame.FeetCollection != null)
                {
                    drawFoot(frame.FeetCollection.leftfront);
                    drawFoot(frame.FeetCollection.leftHind);
                    drawFoot(frame.FeetCollection.rightfront);
                    drawFoot(frame.FeetCollection.rightHind);
                }
            }
        }
        
        private void drawFoot(IfeetID foot)
        {
            if(foot != null && currentFrame !=null)
            {
                IFootPlacement _foot = foot.value;
                Rectangle rect = new Rectangle(_foot.minX, _foot.minY, _foot.maxX - _foot.minX, _foot.maxY - _foot.minY);
                CvInvoke.Rectangle(currentFrame, rect, new MCvScalar(0, 255, 0), 2);
                CvInvoke.PutText(currentFrame, foot.id, new Point(_foot.centroidX, _foot.centroidY), FontFace.HersheySimplex, 0.5, new MCvScalar(0, 255, 255)) ; 
            }
        }
        private void ExportCSV()
        {
            string fileLocation = _video.FilePath;
            string saveLocation = FileBrowser.SaveFile("CSV|*.csv", fileLocation);

            if (string.IsNullOrWhiteSpace(saveLocation))
            {
                return;
            }

            iSaveCSVFile save = ModelResolver.Resolve<iSaveCSVFile>();

            save.saveFeetCSV(saveLocation, Results, true);
        }

        private int _frameCount;
        public int FrameCount
        {
            get { return _frameCount; }
            set
            {
                _frameCount = value;
                NotifyPropertyChanged();
            }
        }
        private int _CurrentFrame;
        public int CurrentFrame
        {
            get { return _CurrentFrame; }
            set { _CurrentFrame = value;
                NotifyPropertyChanged();
                updateDisplayimage();
                FrameNumberDisplay = $"Frame {CurrentFrame}";
            }
        }
        

        private string _FrameNumberDisplay;
        public string FrameNumberDisplay
        {
            get { return _FrameNumberDisplay; }
            set { _FrameNumberDisplay = value;
                NotifyPropertyChanged();
            }
        }
        private BitmapSource _image;
        public BitmapSource Image
        {
            get { return _image; }
            set { _image = value;
                NotifyPropertyChanged();
            }
        }
        private ActionCommand _ExportCSVCommand;
        public ActionCommand ExportCSVCommand
        {
            get
            {
                return _ExportCSVCommand ?? (_ExportCSVCommand = new ActionCommand()
                {
                    ExecuteAction = ExportCSV,
                });
            }
        }



    }
}
