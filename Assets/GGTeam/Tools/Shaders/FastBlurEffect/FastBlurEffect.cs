using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
// Добавить параметры в меню
[AddComponentMenu("GGTeam/PostFX/FastBlurEffect")]
public class FastBlurEffect : MonoBehaviour
{
    //------------------- Раздел объявления переменных -------------------
    #region Variables

    // Укажите имя шейдера
    private string ShaderName = "GGTeam/PostFX/FastBlurEffect";

    // Примеры шейдеров и материалов
    public Shader CurShader;
    private Material CurMaterial;

    // Несколько промежуточных переменных для настройки параметров
    public static int ChangeValue;
    public static float ChangeValue2;
    public static int ChangeValue3;

    // Время понижающей дискретизации
    [Range(0, 6), Tooltip("[Количество понижающих выборок] Количество понижающих выборок. Чем больше значение, тем больше интервал выборки, тем меньше нужно обрабатывать пикселей и тем выше скорость бега.")]
    public int DownSampleNum = 2;
    // Нечеткая диффузия
    [Range(0.0f, 20.0f), Tooltip("[Распространение размытия] Интервал между соседними пикселями при выполнении размытия по Гауссу. Чем больше это значение, тем больше интервал между соседними пикселями и тем более размыто изображение. Однако слишком большое значение приведет к искажению.")]
    public float BlurSpreadSize = 3.0f;
    // Количество итераций
    [Range(0, 8), Tooltip("[Количество итераций] Чем больше значение, тем больше итераций операции размытия, тем лучше эффект размытия, но тем больше потребление.")]
    public int BlurIterations = 3;

    #endregion

    //------------------------- материал get&set ----------------------------
    #region MaterialGetAndSet
    Material material
    {
        get
        {
            if (CurMaterial == null)
            {
                CurMaterial = new Material(CurShader);
                CurMaterial.hideFlags = HideFlags.HideAndDontSave;
            }
            return CurMaterial;
        }
    }
    #endregion

    #region Functions

    //-----------------------------------------【Start()】---------------------------------------------  
    void Start()
    {
        // Назначение по очереди
        ChangeValue = DownSampleNum;
        ChangeValue2 = BlurSpreadSize;
        ChangeValue3 = BlurIterations;

        // Найти текущий файл шейдера
        CurShader = Shader.Find(ShaderName);

        /*
        // Определите, поддерживает ли текущее устройство экранные эффекты
        if (!SystemInfo.supportsImageEffects)
        {
            enabled = false;
            return;
        }
        */
    }

    //-------------------------------------【OnRenderImage()】------------------------------------  
    // Описание：Эта функция вызывается, когда все визуализированные изображения завершены, используется для визуализации эффектов пост-производства.
    //--------------------------------------------------------------------------------------------------------
    void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
    {
        //Экземпляр шейдера не пустой?
        if (CurShader != null)
        {
            //【0】Подготовка параметров
            // Коэффициент ширины определяется по количеству понижающей дискретизации. Используется для контроля интервала между соседними пикселями после понижающей дискретизации
            float widthMod = 1.0f / (1.0f * (1 << DownSampleNum));
            // Shader Присвоение параметров понижающей дискретизации
            material.SetFloat("_DownSampleValue", BlurSpreadSize * widthMod);
            // Установите режим рендеринга: билинейный
            sourceTexture.filterMode = FilterMode.Bilinear;
            // Подготовьте значения параметров длины и ширины, сдвигая вправо
            int renderWidth = sourceTexture.width >> DownSampleNum;
            int renderHeight = sourceTexture.height >> DownSampleNum;

            // 【1】Обработка канала 0 шейдера для понижающей дискретизации ||Pass 0,for down sample
            // Подготовить буфер renderBuffer для подготовки к сохранению окончательных данных
            RenderTexture renderBuffer = RenderTexture.GetTemporary(renderWidth, renderHeight, 0, sourceTexture.format);
            // Установите режим рендеринга: билинейный
            renderBuffer.filterMode = FilterMode.Bilinear;
            // Скопируйте данные рендеринга в sourceTexture в renderBuffer и рисуйте только данные текстуры указанного pass0
            Graphics.Blit(sourceTexture, renderBuffer, material, 0);

            //【2】Выполните указанное количество итераций на основе BlurIterations
            for (int i = 0; i < BlurIterations; i++)
            {
                //【2.1】Назначение параметров шейдера
                //迭代偏移量参数
                float iterationOffs = (i * 1.0f);
                // Назначение параметров понижающей дискретизации шейдера
                material.SetFloat("_DownSampleValue", BlurSpreadSize * widthMod + iterationOffs);

                // 【2.2】Обработка шейдерного канала 1, размытие в вертикальном направлении || Pass1,for vertical blur
                // Определить временный буфер рендеринга tempBuffer
                RenderTexture tempBuffer = RenderTexture.GetTemporary(renderWidth, renderHeight, 0, sourceTexture.format);
                // Скопируйте данные рендеринга в renderBuffer в tempBuffer и рисуйте только данные текстуры указанного pass1
                Graphics.Blit(renderBuffer, tempBuffer, material, 1);
                // Очистить renderBuffer
                RenderTexture.ReleaseTemporary(renderBuffer);
                // Назначьте tempBuffer для renderBuffer, в это время данные pass0 и pass1 в renderBuffer готовы
                renderBuffer = tempBuffer;

                // 【2.3】Обработка канала 2 шейдера и размытие по вертикали || Pass2,for horizontal blur
                // Получить временную текстуру рендеринга
                tempBuffer = RenderTexture.GetTemporary(renderWidth, renderHeight, 0, sourceTexture.format);
                // Скопируйте данные рендеринга в renderBuffer в tempBuffer и рисуйте только данные текстуры указанного pass2
                Graphics.Blit(renderBuffer, tempBuffer, CurMaterial, 2);

                //【2.4】Получить данные renderBuffer для pass0, pass1 и pass2
                // Опять пустой renderBuffer
                RenderTexture.ReleaseTemporary(renderBuffer);
                // Снова назначьте tempBuffer для renderBuffer, в это время все данные pass0, pass1 и pass2 в renderBuffer готовы.
                renderBuffer = tempBuffer;
            }

            // Скопируйте последний renderBuffer в целевую текстуру и нарисуйте текстуру всех каналов на экране.
            Graphics.Blit(renderBuffer, destTexture);
            // Очистить renderBuffer
            RenderTexture.ReleaseTemporary(renderBuffer);

        }

        // Экземпляр шейдера пуст, напрямую скопируйте эффект на экран. В этом случае экранный спецэффект отсутствует
        else
        {
            // Скопируйте исходную текстуру непосредственно в целевую текстуру рендеринга.
            Graphics.Blit(sourceTexture, destTexture);
        }
    }


    //-----------------------------------------【OnValidate()】--------------------------------------  
    void OnValidate()
    {
        ChangeValue = DownSampleNum;
        ChangeValue2 = BlurSpreadSize;
        ChangeValue3 = BlurIterations;
    }

    //-----------------------------------------【Update()】--------------------------------------  
    void Update()
    {
        if (Application.isPlaying)
        {
            DownSampleNum = ChangeValue;
            BlurSpreadSize = ChangeValue2;
            BlurIterations = ChangeValue3;
        }
        // Если программа не запущена, перейдите к соответствующему файлу Shader
#if UNITY_EDITOR
        if (Application.isPlaying != true)
        {
            CurShader = Shader.Find(ShaderName);
        }
#endif

    }

    //-----------------------------------------【OnDisable()】---------------------------------------  
    void OnDisable()
    {
        if (CurMaterial)
        {
            // Мгновенное уничтожение материальных экземпляров
            DestroyImmediate(CurMaterial);
        }

    }

    #endregion

}