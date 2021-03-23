using OpenCvSharp;
using OpenCvSharp.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenCv
{
    static class Program
    {

        //Application.EnableVisualStyles();
        //Application.SetCompatibleTextRenderingDefault(false);
        //Application.Run(new Form1());
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        //[STAThread]
        static ResourcesTracker t = new ResourcesTracker();
        static void Main()
        {
            ImportImage();
        }

        static void ImportImage()
        {
            var img = t.T(new Mat(@".\Resource\test.jpg", ImreadModes.AnyColor));
            var rgb = img.Split();
            var img_new = t.NewMat();
            Cv2.Merge(rgb.Reverse<Mat>().ToArray(), img_new);
            img_new = img_new.Resize(new Size(img_new.Width * 2, img_new.Height * 2));
            var t_mat = t.NewMat(new Size(3, 2), MatType.CV_32F, Scalar.Black);
            t_mat.At<float>(0, 0) = 1;
            t_mat.At<float>(0, 2) = 20;
            t_mat.At<float>(1, 1) = 1;
            t_mat.At<float>(1, 2) = 10;
            img_new = img_new.WarpAffine(t_mat, img_new.Size());
            Cv2.ImShow("Image", img);
            Cv2.ImShow("Image new", img_new);
            Cv2.WaitKey(0);
        }

        static void ImportVideo()
        {
            VideoCapture cap = new VideoCapture(@"Resource\video.mp4");
            Mat img = t.NewMat();
            while (true)
            {
                cap.Read(img);
                Cv2.ImShow("Image", img);
                Cv2.WaitKey(1);
            }
        }

        static void BasicsFunc()
        {
            var img = t.T(new Mat(@".\Resource\a.jpg", ImreadModes.AnyColor));
            var imgGary = img.CvtColor(ColorConversionCodes.BGR2GRAY);
            var imgBlur = img.GaussianBlur(new Size(3, 3), 3, 0);
            var imgCanny = img.Canny(100, 250);
            var kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(5, 5));
            var imgDilate = imgCanny.Dilate(kernel);
            var imgErode = imgDilate.Erode(kernel);
            Cv2.ImShow("Image", img);
            Cv2.ImShow("Image Gary", imgGary);
            Cv2.ImShow("Image Blur", imgBlur);
            Cv2.ImShow("Image Canny", imgCanny);
            Cv2.ImShow("Image Dilate", imgDilate);
            Cv2.ImShow("Image Erode", imgErode);
            Cv2.WaitKey(0);
        }

        static void ImgReSizeAndClone()
        {
            var img = t.T(new Mat(@".\Resource\a.jpg", ImreadModes.AnyColor));
            var imgsize = img.Size();
            var imgResize = img.Resize(new Size(), 0.5, 0.5);
            var roi = new Rect(290, 280, 285, 100);
            var imgCrop = img.Clone(roi);
            Cv2.ImShow("Image", img);
            Cv2.ImShow("Image Resize", imgResize);
            Cv2.ImShow("Image Crop", imgCrop);
            Cv2.WaitKey(0);
        }
        static void DrawShapesAndText()
        {
            var img = t.NewMat(new Size(515, 512), MatType.CV_8UC3, Scalar.White);
            img.Circle(new Point(256, 256), 155, Scalar.FromRgb(231, 101, 26), -1);
            img.Rectangle(new Rect(new Point(130, 236), new Size(250, 50)), Scalar.White, -1);
            img.Line(new Point(130, 296), new Point(375, 296), Scalar.White, 2);
            img.PutText("冀D·ABC12", new Point(137, 263), HersheyFonts.Italic, 0.75, Scalar.Black, 2);
            Cv2.ImShow("Img Black", img);
            Cv2.WaitKey(0);
        }
        static void WarpImg()
        {
            int w = 400, h = 100;
            var img = t.T(new Mat(@".\Resource\test.jpg", ImreadModes.AnyColor));
            Point2f[] src = new Point2f[] { new Point2f(223, 196), new Point2f(219, 243), new Point2f(337, 284), new Point2f(346, 219) };
            Point2f[] dst = new Point2f[] { new Point2f(0.0f, 0.0f), new Point2f(0.0f, h), new Point2f(w, h), new Point2f(w, 0.0f) };
            var matrix = Cv2.GetPerspectiveTransform(src, dst);
            var imgWarp = img.WarpPerspective(matrix, new Size(w, h));
            for (int i = 0; i < 4; i++)
            {
                img.Circle(src[i].ToPoint(), 5, Scalar.Red, -1);
            }
            Cv2.ImShow("Img", img);
            Cv2.ImShow("Img Warp", imgWarp);
            Cv2.WaitKey(0);
        }
        static void ImgColorDetection()
        {
            var img = t.T(new Mat(@"Resource\a.jpg", ImreadModes.AnyColor));
            var imgHsv = img.CvtColor(ColorConversionCodes.BGR2HSV);
            int hmin = 0, smin = 13, vmin = 164;
            int hmax = 179, smax = 111, vmax = 255;
            //Cv2.NamedWindow("Tark Tools", WindowFlags.FullScreen);
            //Cv2.CreateTrackbar("hmin", "Tark Tools", ref hmin, 179);
            //Cv2.CreateTrackbar("hmax", "Tark Tools", ref hmax, 179);
            //Cv2.CreateTrackbar("smin", "Tark Tools", ref smin, 255);
            //Cv2.CreateTrackbar("smax", "Tark Tools", ref smax, 255);
            //Cv2.CreateTrackbar("vmin", "Tark Tools", ref vmin, 255);
            //Cv2.CreateTrackbar("vmax", "Tark Tools", ref vmax, 255);
            while (true)
            {
                Scalar lowerb = new Scalar(hmin, smin, vmin);
                Scalar upperb = new Scalar(hmax, smax, vmax);
                var ImgMark = imgHsv.InRange(lowerb, upperb);
                Cv2.ImShow("Img", img);
                Cv2.ImShow("Img HSV", imgHsv);
                Cv2.ImShow("Img Mark", ImgMark);
                Cv2.WaitKey(1);
            }
        }
        static void ShapesOrContourDetection()
        {
            var img = t.T(new Mat(@".\Resource\Rect.png", ImreadModes.AnyColor));
            var imgGary = img.CvtColor(ColorConversionCodes.BGR2GRAY);
            var imgBlur = imgGary.GaussianBlur(new Size(3, 3), 3, 0);
            var imgCanny = img.Canny(100, 250);
            var kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(5, 5));
            var imgDilate = imgCanny.Dilate(kernel);

            Point[][] contours = null;
            HierarchyIndex[] hierarchy = null;
            imgDilate.FindContours(out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);
            //img.DrawContours(contours, -1, Scalar.FromRgb(255, 0, 255), 10);

            for (int i = 0; i < contours.Length; i++)
            {
                double area = Cv2.ContourArea(contours[i]);
                if (area > 1000)
                {
                    string shapeType = "";
                    double peri = Cv2.ArcLength(contours[i], true);
                    var Apporx = Cv2.ApproxPolyDP(contours[i], 0.03 * peri, true);
                    var rect = Cv2.BoundingRect(Apporx);
                    switch (Apporx.Length)
                    {
                        case 3:
                            shapeType = "SanJiao";
                            break;
                        case 4:
                            if ((double)((double)rect.Width / (double)rect.Height) < 1.1 && (double)((double)rect.Height / (double)rect.Width) < 1.1)
                            {
                                shapeType = "ZFXing";
                            }
                            else
                            {
                                shapeType = "JuXing";
                            }
                            break;
                        default:
                            shapeType = "Yuan";
                            break;
                    }
                    img.Rectangle(rect, Scalar.FromRgb(255, 0, 255), 2);
                    img.PutText(shapeType, new Point(rect.Left, rect.Top - 2), HersheyFonts.HersheyPlain, 1, Scalar.FromRgb(0, 255, 0), 2);
                    //img.DrawContours(contours, i, Scalar.FromRgb(255, 0, 255), 2);
                }
            }

            Cv2.ImShow("Image", img);
            //Cv2.ImShow("Image Gary", imgGary);
            //Cv2.ImShow("Image Blur", imgBlur);
            //Cv2.ImShow("Image Canny", imgCanny);
            //Cv2.ImShow("Image Dilate", imgDilate);
            Cv2.WaitKey(0);
        }
        static void FaceDetection()
        {
            var img = t.T(new Mat(@"Resource\face.jpg", ImreadModes.AnyColor));
            CascadeClassifier facesCascade = new CascadeClassifier(@"Resource\haarcascade_frontalface_default.xml");
            var faces = facesCascade.DetectMultiScale(img, 1.1, 10);
            for (int i = 0; i < faces.Length; i++)
            {
                img.Rectangle(new Point(faces[i].Left, faces[i].Top), new Point(faces[i].Left + faces[i].Width, faces[i].Top + faces[i].Height), Scalar.FromRgb(0, 255, 0), 1);
            }
            Cv2.ImShow("Img", img);
            Cv2.WaitKey(0);
        }
        static void TextDetection()
        {
            var img = t.T(new Mat(@".\Resource\a.jpg", ImreadModes.AnyColor));
            var tess = OCRTesseract.Create();
            string Text;
            Rect[] rect;
            string[] componentTexts;
            float[] componentConfidences;
            tess.Run(img, out Text, out rect, out componentTexts, out componentConfidences);
            Console.WriteLine(Text);
            Cv2.WaitKey(0);
        }
    }
}
