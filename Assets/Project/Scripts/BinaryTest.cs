using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;
using UnityEngine;
using UnityEngine.UI;

public class BinaryTest : MonoBehaviour
{
    [SerializeField] private Texture2D _texture = null;
    [SerializeField] private RawImage _rawImage = null;
    
    private void Start()
    {
        Mat originalMat = new Mat(_texture.height, _texture.width, CvType.CV_8UC4);
        Utils.texture2DToMat(_texture, originalMat);

        // Gray Scale
        Mat grayMat = new Mat(_texture.height, _texture.width, CvType.CV_8UC1);
        Imgproc.cvtColor(originalMat, grayMat, Imgproc.COLOR_RGBA2GRAY);
        
        // 2値化
        Mat binMat = new Mat(_texture.height, _texture.width, CvType.CV_8UC1);
        Imgproc.threshold(grayMat, binMat, 127.0, 255.0, Imgproc.THRESH_BINARY);

        Texture2D output = new Texture2D(binMat.cols(), binMat.rows(), TextureFormat.RGBA32, false);
        Utils.matToTexture2D(binMat, output);

        _rawImage.texture = output;
        
        originalMat.Dispose();
        grayMat.Dispose();
        binMat.Dispose();
    }
}
