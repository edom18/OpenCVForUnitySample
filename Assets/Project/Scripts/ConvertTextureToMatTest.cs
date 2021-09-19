using UnityEngine;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.UnityUtils;
using UnityEngine.UI;

public class ConvertTextureToMatTest : MonoBehaviour
{
    [SerializeField] private RawImage _rawImage = null;
    [SerializeField] private Texture2D _texture = null;

    private void Start()
    {
        Mat mat = new Mat(_texture.height, _texture.width, CvType.CV_8UC4);
        Utils.texture2DToMat(_texture, mat);

        Texture2D output = new Texture2D(mat.cols(), mat.rows(), TextureFormat.RGBA32, false);
        Utils.matToTexture2D(mat, output);

        _rawImage.texture = output;
        
        mat.Dispose();
    }
}
