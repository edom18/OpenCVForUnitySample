using UnityEngine;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;
using UnityEngine.UI;

public class BypassFilterTest : MonoBehaviour
{
    [SerializeField] private RawImage _rawImage = null;
    [SerializeField] private Texture2D _texture = null;
    
    private void Start()
    {
        Mat mat = new Mat(_texture.height, _texture.width, CvType.CV_8UC4);
        Mat grayMat = mat.clone();
        Mat bypassMat = new Mat();
        Utils.texture2DToMat(_texture, mat);
        
        Mat kernel = new Mat(3, 3, CvType.CV_64FC1);
        kernel.put(0, 0,
            -1, -1, -1,
            -1, 8, -1,
            -1, -1, -1);

        Imgproc.cvtColor(mat, grayMat, Imgproc.COLOR_RGBA2GRAY);
        Imgproc.filter2D(grayMat, bypassMat, CvType.CV_8UC1, kernel);

        Texture2D output = new Texture2D(bypassMat.cols(), bypassMat.rows(), TextureFormat.RGBA32, false);
        Utils.matToTexture2D(bypassMat, output);

        _rawImage.texture = output;
        
        mat.Dispose();
        grayMat.Dispose();
        bypassMat.Dispose();
        kernel.Dispose();
    }
}
