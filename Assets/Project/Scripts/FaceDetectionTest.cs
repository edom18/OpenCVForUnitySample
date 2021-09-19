using UnityEngine;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.ObjdetectModule;
using OpenCVForUnity.UnityUtils;
using UnityEngine.UI;
using Rect = OpenCVForUnity.CoreModule.Rect;

public class FaceDetectionTest : MonoBehaviour
{
    [SerializeField] private RawImage _rawImage = null;
    [SerializeField] private Texture2D _texture = null;

    private void Start()
    {
        Mat mat = new Mat(_texture.height, _texture.width, CvType.CV_8UC4);
        Mat grayMat = mat.clone();
        Utils.texture2DToMat(_texture, mat);

        Imgproc.cvtColor(mat, grayMat, Imgproc.COLOR_RGB2GRAY);

        CascadeClassifier cascade = new CascadeClassifier();
        cascade.load(Utils.getFilePath("haarcascade_frontalface_alt.xml"));

        MatOfRect faces = new MatOfRect();
        cascade.detectMultiScale(grayMat, faces, 1.1, 2, 2, new Size(20, 20), new Size());

        Rect[] rects = faces.toArray();
        foreach (var rect in rects)
        {
            Imgproc.rectangle(mat, new Point(rect.x, rect.y), new Point(rect.x + rect.width, rect.y + rect.height), new Scalar(255, 0, 0, 255), 2);
        }

        Texture2D output = new Texture2D(mat.cols(), mat.rows(), TextureFormat.RGBA32, false);
        Utils.matToTexture2D(mat, output);

        _rawImage.texture = output;
        
        mat.Dispose();
        grayMat.Dispose();
        cascade.Dispose();
        faces.Dispose();
    }
}
