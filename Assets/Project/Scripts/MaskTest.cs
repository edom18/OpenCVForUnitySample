using UnityEngine;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;
using UnityEngine.UI;

public class MaskTest : MonoBehaviour
{
    [SerializeField] private RawImage _rawImage = null;
    [SerializeField] private Texture2D _texture = null;

    private void Start()
    {
        Mat mat = new Mat(_texture.height, _texture.width, CvType.CV_8UC4);
        Utils.texture2DToMat(_texture, mat);
        Mat maskMat = new Mat(_texture.height, _texture.width, CvType.CV_8UC4, new Scalar(0, 0, 0, 255));
        
        Imgproc.circle(maskMat, new Point(mat.width() / 2, mat.height() / 2), 300, new Scalar(255, 255, 255, 255), -1);

        Mat dst = new Mat();
        Core.bitwise_and(mat, maskMat, dst);
        
        Texture2D output = new Texture2D(dst.cols(), dst.rows(), TextureFormat.RGBA32, false);
        Utils.matToTexture2D(dst, output);

        _rawImage.texture = output;
        
        mat.Dispose();
        maskMat.Dispose();
        dst.Dispose();
    }
}
