using UnityEngine;
using System.Collections;
using UnityEngine.PostProcessing;

[RequireComponent(typeof(Camera))]
public class VesselsFXManager : MonoBehaviour
{
    public Shader detectiveModeShader;
    public Material postEffectMaterial;
    private RenderTexture _temporaryRenderTexture;
    private bool _glowMapPass;

    public PostProcessingBehaviour postProcessProfile;

    private Camera _camera;

    public Color backgroundColor;

    bool vesselsFXActivated;
    bool ZPressed;

    float _lerpValue;
    public float lerpVesselsSpeed;

    void Awake()
    {
        _camera = GetComponent <Camera>();

        postEffectMaterial.SetFloat("_Lerp", 0);
    }

	void LateUpdate ()
    {
        if (CameraPostProcessManager.lifeShaderActivated)
        {
            vesselsFXActivated = false;
            if (_lerpValue <= 0.1f) //para que las vacijas desaparescan de a poco
            {
                _camera.renderingPath = RenderingPath.DeferredShading;
                postProcessProfile.enabled = true;
            }
            postEffectMaterial.SetFloat("_Lerp", 0);
            //VesselsFX(0, lerpVesselsSpeed);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Z) && !vesselsFXActivated)
            {
                _camera.renderingPath = RenderingPath.Forward;
                postProcessProfile.enabled = false;
                vesselsFXActivated = true;
                ZPressed = true;
            }
            else if (Input.GetKeyDown(KeyCode.Z) && vesselsFXActivated)
            {
                vesselsFXActivated = false;
            }

            if (vesselsFXActivated)
            {
                VesselsFX(1, lerpVesselsSpeed);
            }
            else if (ZPressed && !vesselsFXActivated)
            {
                if (_lerpValue <= 0.1f) //para que las vacijas desaparescan de a poco
                {
                    _camera.renderingPath = RenderingPath.DeferredShading;
                    postProcessProfile.enabled = true;
                }
                VesselsFX(0, lerpVesselsSpeed);
            }
        }

        _camera.backgroundColor = backgroundColor;
        //Obtenemos una RT Temporaria
        _temporaryRenderTexture = RenderTexture.GetTemporary(Screen.width, Screen.height);

        //Sacamos un frame con el shader reemplazado (tag = "" -> reemplaza TODOS los shaders en la escena)
        _camera.clearFlags = CameraClearFlags.Color;
        _camera.SetReplacementShader(detectiveModeShader, "Vessels");
        _camera.targetTexture = _temporaryRenderTexture;
        _glowMapPass = false;

        _camera.Render();

        //Reseteamos el replacement Shader
        _glowMapPass = true;
        _camera.ResetReplacementShader();
        _camera.targetTexture = null;
        _camera.clearFlags = CameraClearFlags.Skybox;
    }

    public void VesselsFX(float max, float lerp)
    {
        _lerpValue = Mathf.MoveTowards(_lerpValue, max, lerp);
        postEffectMaterial.SetFloat("_Lerp", _lerpValue);
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (_glowMapPass)
        {
            _temporaryRenderTexture.wrapMode = TextureWrapMode.Repeat;
            postEffectMaterial.SetTexture("_VesselsMap", _temporaryRenderTexture);
            Graphics.Blit(src, dst, postEffectMaterial);
            RenderTexture.ReleaseTemporary(_temporaryRenderTexture);
        }
        else
        {
            Graphics.Blit(src, dst);
        }
    }
}
