using System;
using System.Collections;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;
using UnityEngine;
using UnityEngine.UI;

public class WebCamTest : MonoBehaviour
{
    [SerializeField] private RawImage _rawImage = null;
    [SerializeField] private string _requestDeviceName = "";
    [SerializeField] private int _requestedWidth = 640;
    [SerializeField] private int _requestedHeight = 480;
    [SerializeField] private int _requestedFPS = 30;
    [SerializeField] private bool _requestedIsFrontFacing = false;

    private WebCamDevice _webCamDevice = default;
    private WebCamTexture _webCamTexture = null;
    private bool _isInitWaiting = false;
    private bool _hasInitDone = false;

    private Color32[] _colors = null;
    private Texture2D _texture = null;
    private Mat _rgbaMat = null;
    private Mat _grayMat = null;

    private void Start()
    {
        StartCoroutine(Initialize());
    }

    private void OnDestroy()
    {
       Dispose(); 
    }

    private void Update()
    {
        if (_hasInitDone && _webCamTexture.isPlaying && _webCamTexture.didUpdateThisFrame)
        {
            Utils.webCamTextureToMat(_webCamTexture, _rgbaMat, _colors);

            Imgproc.cvtColor(_rgbaMat, _grayMat, Imgproc.COLOR_RGB2GRAY);
            
            Utils.matToTexture2D(_grayMat, _texture, _colors);
        }
    }

    private IEnumerator Initialize()
    {
        _isInitWaiting = true;
        
#if UNITY_ANDROID
        string permission = UnityEngine.Android.Permission.Camera;
        if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(permission))
        {
            UnityEngine.Android.Permission.RequestUserPermission(permission);

            yield return new WaitForSeconds(1f);
        }

        if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(permission))
        {
            Debug.LogWarning("User didn't allow to use a camera.");
            yield break;
        }
#endif
        
        WebCamDevice[] devices = WebCamTexture.devices;
        if (!String.IsNullOrEmpty(_requestDeviceName))
        {
            if (Int32.TryParse(_requestDeviceName, out int requestedDeviceIndex))
            {
                if (requestedDeviceIndex >= 0 && requestedDeviceIndex < devices.Length)
                {
                    _webCamDevice = devices[requestedDeviceIndex];
                    _webCamTexture = new WebCamTexture(_webCamDevice.name, _requestedWidth, _requestedHeight, _requestedFPS);
                }
            }
            else
            {
                for (int cameraIndex = 0; cameraIndex < devices.Length; ++cameraIndex)
                {
                    if (devices[cameraIndex].name == _requestDeviceName)
                    {
                        _webCamDevice = devices[cameraIndex];
                        _webCamTexture = new WebCamTexture(_webCamDevice.name, _requestedWidth, _requestedHeight, _requestedFPS);
                        break;
                    }
                }
            }

            if (_webCamTexture == null)
            {
                Debug.Log($"Cannot find camera device {_requestDeviceName}.");
            }
        }

        if (_webCamTexture == null)
        {
            for (int cameraIndex = 0; cameraIndex < devices.Length; ++cameraIndex)
            {
                if (devices[cameraIndex].kind != WebCamKind.ColorAndDepth && devices[cameraIndex].isFrontFacing == _requestedIsFrontFacing)
                {
                    _webCamDevice = devices[cameraIndex];
                    _webCamTexture = new WebCamTexture(_webCamDevice.name, _requestedWidth, _requestedHeight, _requestedFPS);
                    break;
                }
            }
        }

        if (_webCamTexture == null)
        {
            if (devices.Length > 0)
            {
                _webCamDevice = devices[0];
                _webCamTexture = new WebCamTexture(_webCamDevice.name, _requestedWidth, _requestedHeight, _requestedFPS);
            }
            else
            {
                Debug.LogError("Camera device does not exist.");
                _isInitWaiting = false;
                yield break;
            }
        }

        _webCamTexture.Play();

        while (true)
        {
            if (_webCamTexture.didUpdateThisFrame)
            {
                _isInitWaiting = false;
                _hasInitDone = true;
                
                OnInitialized();
                
                break;
            }
            else
            {
                yield return null;
            }
        }
    }

    private void OnInitialized()
    {
        _texture = new Texture2D(_requestedWidth, _requestedHeight, TextureFormat.RGBA32, false);
        _rgbaMat = new Mat(_texture.height, _texture.width, CvType.CV_8UC4, new Scalar(0, 0, 0, 255));
        _grayMat = _rgbaMat.clone();
        _colors = new Color32[_texture.width * _texture.height];

        Utils.matToTexture2D(_rgbaMat, _texture, _colors);

        _rawImage.texture = _texture;
    }

    private void Dispose()
    {
        if (_webCamTexture != null)
        {
            _webCamTexture.Stop();
            Destroy(_webCamTexture);
            _webCamTexture = null;
        }

        if (_texture != null)
        {
            Destroy(_texture);
        }

        if (_rgbaMat != null)
        {
            _rgbaMat.Dispose();
            _rgbaMat = null;
        }

        if (_grayMat != null)
        {
            _grayMat.Dispose();
            _grayMat = null;
        }
    }
}