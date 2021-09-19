using System;
using System.Collections;
using System.Collections.Generic;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;
using UnityEngine;
using UnityEngine.UI;

public class EdgeDetectionTest : MonoBehaviour
{
    [SerializeField] private Texture2D _texture = null;
    [SerializeField] private RawImage _rawImage = null;
    
    private void Start()
    {
        Mat mat = new Mat(_texture.height, _texture.width, CvType.CV_8UC4);
        Utils.texture2DToMat(_texture, mat);

        Mat grayMat = new Mat(_texture.height, _texture.width, CvType.CV_8UC1);
        Imgproc.cvtColor(mat, grayMat, Imgproc.COLOR_RGBA2GRAY);

        Mat binMat = new Mat(_texture.height, _texture.width, CvType.CV_8UC1);
        Imgproc.threshold(grayMat, binMat, 127.0, 255.0, Imgproc.THRESH_BINARY);

        Mat hierarchyMat = new Mat();
        List<MatOfPoint> contours = new List<MatOfPoint>();
        Imgproc.findContours(binMat, contours, hierarchyMat, Imgproc.RETR_EXTERNAL, Imgproc.CHAIN_APPROX_NONE);

        for (int i = 0; i < contours.Count; ++i)
        {
            // imageに指定したMatにエッジを書き込む
            Imgproc.drawContours(grayMat, contours, i, new Scalar(255.0, 0.0, 0.0), 2, 8, hierarchyMat, 0, new Point());
        }

        // おもちゃラボさんの記事を参考にメモ。どこかで面積とか求めるときに。
        // double total = 0;
        // foreach (var point in contours)
        // {
        //     // 面積
        //     double area = Imgproc.contourArea(point);
        //     
        //     // 重心
        //     Moments m = Imgproc.moments(point);
        //     int cx = (int)(m.m10 / m.m00);
        //     int cy = (int)(m.m01 / m.m00);
        //
        //     total += area;
        // }

        Texture2D output = new Texture2D(grayMat.cols(), grayMat.rows(), TextureFormat.RGBA32, false);
        Utils.matToTexture2D(grayMat, output);
        _rawImage.texture = output;
        
        mat.Dispose();
        grayMat.Dispose();
        binMat.Dispose();
        hierarchyMat.Dispose();

        foreach (var contour in contours)
        {
            contour.Dispose();
        }
    }
}
