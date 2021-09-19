using UnityEngine;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;
using UnityEngine.UI;

public class SobelFilterTest : MonoBehaviour
{
    [SerializeField] private RawImage _rawImage = null;
    [SerializeField] private Texture2D _texture = null;

    private void Start()
    {
        Mat mat = new Mat(_texture.height, _texture.width, CvType.CV_8UC4);
        Mat grayMat = mat.clone();
        Mat sobelMat = new Mat();
        Utils.texture2DToMat(_texture, mat);

        Imgproc.cvtColor(mat, grayMat, Imgproc.COLOR_RGBA2GRAY);
        Imgproc.Sobel(grayMat, sobelMat, -1, 1, 0);

        Texture2D output = new Texture2D(sobelMat.cols(), sobelMat.rows(), TextureFormat.RGBA32, false);
        Utils.matToTexture2D(sobelMat, output);

        _rawImage.texture = output;
        
        mat.Dispose();
        grayMat.Dispose();
        sobelMat.Dispose();
    }
}
