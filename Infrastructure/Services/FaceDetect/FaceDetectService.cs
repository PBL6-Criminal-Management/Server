using Application.Dtos.Responses.DetectResult;
using Application.Interfaces.Services;
using Domain.Constants;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using Microsoft.AspNetCore.Http;
using System.Drawing;

namespace Infrastructure.Services.FaceDetect
{
    public class FaceDetectService : IFaceDetectService
    {
        readonly string solutionPath;
        readonly string detectBasePath;
        readonly string modelPath;
        readonly string unknownImagePath;

        EigenFaceRecognizer recognizer;
        CascadeClassifier faceCasacdeClassifier;

        const int ThresholdForDetectedFacesImage = 10330;
        const int ThresholdForNonDetectedFacesImage = 2000;
        readonly Size allowedFaceMinimizeSize = new Size(70, 70);

        List<Image<Gray, byte>> TrainedFaces = new List<Image<Gray, byte>>();
        List<int> PersonsLabes = new List<int>();

        bool isTrained = false;

        public FaceDetectService()
        {
            solutionPath = $"{Directory.GetCurrentDirectory().Split("\\WebApi")[0]}";
            detectBasePath = $"{solutionPath}/Infrastructure/Services/FaceDetect";
            modelPath = $"{detectBasePath}/Model/model.xml";
            unknownImagePath = $"{detectBasePath}/unknown.png";

            if (!Directory.Exists(detectBasePath))
            {
                Directory.CreateDirectory(detectBasePath);
            }
        }

        public void InitRecogizer()
        {
            if (recognizer == null)
            {
                faceCasacdeClassifier = new CascadeClassifier($"{detectBasePath}/haarcascade_frontalface_default.xml");
                recognizer = new EigenFaceRecognizer();
                if (File.Exists(modelPath))
                {
                    //Load model from file
                    recognizer.Read(modelPath);
                    isTrained = true;
                }
            }
                
        }

        public DetectResult FaceDetect(IFormFile file, bool enableSaveImage)
        {
            InitRecogizer();
            DetectResult dr = new DetectResult();
            if (isTrained)
            {
                try
                {
                    // Read the content of the IFormFile into a byte array
                    using (var ms = new MemoryStream())
                    {
                        file.CopyTo(ms);
                        var fileBytes = ms.ToArray();

                        //Step: 1 Create a Mat object from the byte array
                        Mat imageMat = new Mat();
                        CvInvoke.Imdecode(fileBytes, ImreadModes.Color, imageMat);

                        // Convert Mat to Image<Bgr, Byte>
                        Image<Bgr, byte> imageFrame = imageMat.ToImage<Bgr, byte>();

                        //Step 2: Face Detection
                        //Convert from Bgr to Gray Image
                        Mat grayImage = new Mat();
                        CvInvoke.CvtColor(imageFrame, grayImage, ColorConversion.Bgr2Gray);

                        //Enhance the image to get better result
                        CvInvoke.EqualizeHist(grayImage, grayImage);

                        //Find faces
                        Rectangle[] faces = faceCasacdeClassifier.DetectMultiScale(grayImage, 1.1, 3, allowedFaceMinimizeSize, Size.Empty);

                        //If faces detected
                        if (faces.Length > 0)
                        {
                            foreach (var face in faces)
                            {
                                //Draw square arou2nd each face 
                                CvInvoke.Rectangle(imageFrame, face, new Bgr(Color.Red).MCvScalar, 2);

                                //Step 3: Add Person 
                                Image<Bgr, byte> resultImage = imageFrame.Convert<Bgr, byte>();
                                resultImage.ROI = face;

                                //Step 4: Export input image (option)
                                if (enableSaveImage)
                                    SaveImage(imageFrame.Copy(), "/InputImage", "Input");

                                enableSaveImage = false;

                                // Step 5: Recognize the face 
                                Image<Gray, byte> grayFaceResult = resultImage.Convert<Gray, byte>().Resize(200, 200, Inter.Cubic);
                                CvInvoke.EqualizeHist(grayFaceResult, grayFaceResult);
                                var result = recognizer.Predict(grayFaceResult);

                                Console.WriteLine($"Label: {result.Label}, Distance: {result.Distance}");

                                //Here results found known faces
                                if (result.Label != -1 && result.Distance < ThresholdForDetectedFacesImage)
                                {
                                    CvInvoke.PutText(imageFrame, $"Id: {result.Label}", new Point(face.X - 2, face.Y - 2),
                                        FontFace.HersheyComplex, 1.0, new Bgr(Color.Orange).MCvScalar);
                                    CvInvoke.Rectangle(imageFrame, face, new Bgr(Color.Green).MCvScalar, 2);

                                    dr.CriminalId = result.Label;
                                    dr.DetectResultFile = imageFrame.ToJpegData();
                                    dr.DetectConfidence = (1 - result.Distance / ThresholdForDetectedFacesImage) * 100;
                                }
                                //here results did not found any know faces
                                else
                                {
                                    dr.Message = StaticVariable.UNKNOWN;
                                    if (File.Exists(unknownImagePath))
                                        dr.DetectResultFile = File.ReadAllBytes(unknownImagePath);
                                    CvInvoke.PutText(imageFrame, "Khong biet", new Point(face.X - 2, face.Y - 2),
                                        FontFace.HersheyComplex, 1.0, new Bgr(Color.Orange).MCvScalar);
                                    CvInvoke.Rectangle(imageFrame, face, new Bgr(Color.Red).MCvScalar, 2);
                                }
                            }
                        }
                        else
                        {
                            Image<Bgr, byte> resultImage = imageFrame.Convert<Bgr, byte>();

                            //Export input image
                            if (enableSaveImage)
                                SaveImage(imageFrame.Copy(), "/InputImage", "Input");

                            enableSaveImage = false;

                            // Step 5: Recognize the face 
                            Image<Gray, byte> grayFaceResult = resultImage.Convert<Gray, byte>().Resize(200, 200, Inter.Cubic);
                            CvInvoke.EqualizeHist(grayFaceResult, grayFaceResult);
                            var result = recognizer.Predict(grayFaceResult);

                            Console.WriteLine($"Label: {result.Label}, Distance: {result.Distance}");


                            if (result.Label != -1 && result.Distance < ThresholdForNonDetectedFacesImage)
                            {
                                CvInvoke.PutText(imageFrame, $"Id: {result.Label}", new Point(80, 80),
                                    FontFace.HersheyComplex, 1.0, new Bgr(Color.Orange).MCvScalar);

                                dr.CriminalId = result.Label;
                                dr.DetectResultFile = imageFrame.ToJpegData();
                                dr.DetectConfidence = (1 - result.Distance / ThresholdForNonDetectedFacesImage) * 100;
                            }
                            else
                            {
                                dr.Message = StaticVariable.UNKNOWN;
                                if(File.Exists(unknownImagePath))
                                    dr.DetectResultFile = File.ReadAllBytes(unknownImagePath);
                                CvInvoke.PutText(imageFrame, "Khong biet", new Point(30, 30),
                                    FontFace.HersheyComplex, 1.0, new Bgr(Color.Orange).MCvScalar);
                            }
                        }

                        //export to png
                        //SaveImage(imageFrame, "/DetectResult", "Result");
                    }

                }
                catch (Exception ex)
                {
                    dr.Message = ex.Message;
                }
            }
            else
                dr.Message = StaticVariable.AI_MODEL_HAS_NOT_TRAINED_YET;

            return dr;
        }


        //Re-train
        //Step 4: train Images .. we will use the saved images
        public string TrainImagesFromDir()
        {
            TrainedFaces.Clear();
            PersonsLabes.Clear();
            try
            {
                string path = $"{solutionPath}/WebApi/Files/TrainedImages";
                if (!Directory.Exists(path))
                    return StaticVariable.NOT_FOUND_IMAGE_TO_TRAIN;

                string[] folders = Directory.GetDirectories(path);

                foreach (var folder in folders)
                {
                    string[] images = Directory.GetFiles(folder);
                    foreach (string image in images)
                    {
                        byte[] fileBytes = File.ReadAllBytes(image);
                        Mat imageMat = new Mat();
                        CvInvoke.Imdecode(fileBytes, ImreadModes.Color, imageMat);

                        // Convert Mat to Image<Bgr, Byte>
                        Image<Bgr, byte> resultImage = imageMat.ToImage<Bgr, byte>();

                        Mat grayImage = new Mat();
                        CvInvoke.CvtColor(resultImage, grayImage, ColorConversion.Bgr2Gray);
                        CvInvoke.EqualizeHist(grayImage, grayImage);

                        Rectangle[] faces = faceCasacdeClassifier.DetectMultiScale(grayImage, 1.1, 3, allowedFaceMinimizeSize, Size.Empty);

                        //If faces detected
                        if (faces.Length > 0)
                        {
                            var face = faces[0];

                            //Draw square arou2nd each face 
                            CvInvoke.Rectangle(resultImage, face, new Bgr(Color.Red).MCvScalar, 2);

                            resultImage = resultImage.Convert<Bgr, byte>();
                            resultImage.ROI = face;

                            //Export input image
                            //SaveImage(resultImage, "/ImageToTrain", $"ImageToTrain");
                        }

                        Image<Gray, byte> trainedImage = resultImage.Convert<Gray, byte>().Resize(200, 200, Inter.Cubic);

                        CvInvoke.EqualizeHist(trainedImage, trainedImage);

                        //SaveImage(trainedImage, "/TrainGrayImage", "Train");
                        TrainedFaces.Add(trainedImage);
                        PersonsLabes.Add(Convert.ToInt32(folder.Split('\\').Last()));
                    }
                }

                if (TrainedFaces.Count() > 0)
                {
                    recognizer = new EigenFaceRecognizer(100, double.PositiveInfinity);
                    recognizer.Train(TrainedFaces.Select(img => img.Mat).ToArray(), PersonsLabes.ToArray());

                    // Save model to file
                    string? directoryPath = Path.GetDirectoryName(modelPath);
                    if (directoryPath != null && !Directory.Exists(directoryPath))
                        Directory.CreateDirectory(directoryPath);

                    recognizer.Write(modelPath);

                    isTrained = true;
                    Console.WriteLine(TrainedFaces.Count);

                    return StaticVariable.AI_MODEL_HAS_TRAINED_SUCCESSFULLY + $" Số lượng ảnh train: {TrainedFaces.Count}";
                }
                else
                {
                    isTrained = false;
                    return StaticVariable.NOT_FOUND_IMAGE_TO_TRAIN;
                }
            }
            catch (Exception ex)
            {
                isTrained = false;
                return ex.Message;
            }

        }

        void SaveImage<TColor>(Image<TColor, byte> resultImage, string path, string imageType) where TColor : struct, IColor
        {
            string savePath = detectBasePath + path;
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);

            //to avoid hang GUI we will create a new task
            Task.Factory.StartNew(() =>
            {
                if (resultImage != null)
                    resultImage.Save(savePath + "/" + imageType + "_" + Guid.NewGuid().ToString() + ".png");
            });
        }
    }
}