using UnityEngine;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;
using UnityEngine.UI;

public class PerspectiveTest : MonoBehaviour
{
    [SerializeField] private RawImage _rawImage = null;
    [SerializeField] private Texture2D _texture = null;

    [SerializeField] private double _rightTop = 200.0;
    [SerializeField] private double _rightBottom = 200.0;

    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            Convert();
        }
    }

    private void Start()
    {
        Convert();
    }

    private void Convert()
    {
        Mat mat = new Mat(_texture.height, _texture.width, CvType.CV_8UC4);
        Mat outMat = mat.clone();
        Utils.texture2DToMat(_texture, mat);

        Mat srcMat = new Mat(4, 1, CvType.CV_32FC2);
        Mat dstMat = new Mat(4, 1, CvType.CV_32FC2);
        srcMat.put(0, 0,
            0.0, 0.0,
            mat.cols(), 0.0,
            0.0, mat.rows(),
            mat.cols(), mat.rows());
        dstMat.put(0, 0,
            0.0, 0.0,
            mat.cols(), _rightTop,
            0.0, mat.rows(),
            mat.cols(), mat.rows() - _rightBottom);

        Mat T = Imgproc.getPerspectiveTransform(srcMat, dstMat);
        Imgproc.warpPerspective(mat, outMat, T, new Size(mat.cols(), mat.rows()));

        Texture2D output = new Texture2D(outMat.cols(), outMat.rows(), TextureFormat.RGBA32, false);
        Utils.matToTexture2D(outMat, output);

        if (_rawImage.texture != null)
        {
            Destroy(_rawImage.texture);
        }

        _rawImage.texture = output;
        
        mat.Dispose();
        outMat.Dispose();
        srcMat.Dispose();
        dstMat.Dispose();
        T.Dispose();
    }
}