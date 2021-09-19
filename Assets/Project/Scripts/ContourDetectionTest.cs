using System.Collections.Generic;
using UnityEngine;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;
using UnityEngine.UI;

public class ContourDetectionTest : MonoBehaviour
{
    [SerializeField] private RawImage _rawImage = null;
    [SerializeField] private Texture2D _texture = null;

    private void Start()
    {
        Mat mat = new Mat(_texture.height, _texture.width, CvType.CV_8UC4);
        Utils.texture2DToMat(_texture, mat);

        // グレースケール化して、
        Mat gray = new Mat(_texture.height, _texture.width, CvType.CV_8UC4);
        Imgproc.cvtColor(mat, gray, Imgproc.COLOR_RGB2GRAY);

        // 画像を2値化したものを利用する
        Mat bin = new Mat(_texture.height, _texture.width, CvType.CV_8UC1);
        
        Imgproc.threshold(gray, bin, 0, 255, Imgproc.THRESH_BINARY_INV | Imgproc.THRESH_OTSU);

        // 輪郭の抽出
        List<MatOfPoint> contours = new List<MatOfPoint>();
        Mat hierarchy = new Mat();
        Imgproc.findContours(bin, contours, hierarchy, Imgproc.RETR_EXTERNAL, Imgproc.CHAIN_APPROX_SIMPLE);

        // 輪郭の表示
        for (int i = 0; i < contours.Count; ++i)
        {
            Imgproc.drawContours(mat, contours, i, new Scalar(255, 0, 0, 255), 2, 8, hierarchy, 0, new Point());
        }
        
        Texture2D output = new Texture2D(mat.cols(), mat.rows(), TextureFormat.RGBA32, false);
        Utils.matToTexture2D(mat, output);
        
        _rawImage.texture = output;
        
        mat.Dispose();
        gray.Dispose();
        bin.Dispose();
        hierarchy.Dispose();
        
        foreach (var contour in contours)
        {
            contour.Dispose();
        }
    }
}