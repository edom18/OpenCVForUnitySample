using UnityEngine;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;
using UnityEngine.UI;

public class GaussianBlurTest : MonoBehaviour
{
    [SerializeField] private RawImage _rawImage = null;
    [SerializeField] private Texture2D _texture = null;

    [SerializeField] private double _blurSize = 11;
    [SerializeField] private double _sigma = 0;

    private void Start()
    {
        Create();
    }

    private void Create()
    {
        Mat mat = new Mat(_texture.height, _texture.width, CvType.CV_8UC4);
        Utils.texture2DToMat(_texture, mat);
        
        Mat blur = new Mat();
        Imgproc.GaussianBlur(mat, blur, new Size(_blurSize, _blurSize), _sigma, _sigma);

        Texture2D output = new Texture2D(blur.cols(), blur.rows(), TextureFormat.RGBA32, false);
        Utils.matToTexture2D(blur, output);

        _rawImage.texture = output;
        
        mat.Dispose();
        blur.Dispose();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Create();
        }
    }
}
