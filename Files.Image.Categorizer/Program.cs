using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace FaceDetection
{
    class Program
    {
        static void Main(string[] args)
        {
            // Diretório com as imagens a serem processadas
            string imagesDirectory = @"G:\Juan\Imagens";

            // Carregar o classificador de detecção de face
            CascadeClassifier faceClassifier = new CascadeClassifier("haarcascade_frontalface_default.xml");

            // Processar cada imagem do diretório
            foreach (string imagePath in Directory.GetFiles(imagesDirectory, "*.jpg"))
            {
                // Carregar a imagem
                using (Mat image = CvInvoke.Imread(imagePath))
                {
                    // Converter a imagem para tons de cinza
                    Mat grayImage = new();
                    CvInvoke.CvtColor(image, grayImage, ColorConversion.Bgr2Gray);

                    // Detectar as faces na imagem
                    Rectangle[] faces = faceClassifier.DetectMultiScale(grayImage);

                    // Salvar cada face detectada em um arquivo separado
                    for (int i = 0; i < faces.Length; i++)
                    {
                        // Obter a região da imagem correspondente à face detectada
                        Rectangle face = faces[i];
                        Mat faceImage = new Mat(grayImage, face);

                        // Salvar a imagem da face em um arquivo separado
                        string facePath = Path.Combine(Path.GetDirectoryName(imagePath), Path.GetFileNameWithoutExtension(imagePath) + "_" + i + ".jpg");
                        CvInvoke.Imwrite(facePath, faceImage);
                    }
                }
            }
        }
    }
}