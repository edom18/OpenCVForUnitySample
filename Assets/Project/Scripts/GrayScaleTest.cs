using System;
using System.Collections;
using System.Collections.Generic;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;
using UnityEngine;
using UnityEngine.UI;

public class GrayScaleTest : MonoBehaviour
{
    [SerializeField] private Texture2D _texture = null;
    [SerializeField] private RawImage _rawImage = null;
    
    private void Start()
    {
        Mat originalMat = new Mat(_texture.height, _texture.width, CvType.CV_8UC4);
        Utils.texture2DToMat(_texture, originalMat);

        Mat grayMat = new Mat(_texture.height, _texture.width, CvType.CV_8UC1);

        Imgproc.cvtColor(originalMat, grayMat, Imgproc.COLOR_RGBA2GRAY);

        Texture2D output = new Texture2D(grayMat.cols(), grayMat.rows(), TextureFormat.RGBA32, false);
        Utils.matToTexture2D(grayMat, output);

        _rawImage.texture = output;
        
        originalMat.Dispose();
        grayMat.Dispose();
    }
}
