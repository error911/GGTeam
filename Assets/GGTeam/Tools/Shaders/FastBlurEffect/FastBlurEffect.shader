// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "GGTeam/PostFX/FastBlurEffect"
{
	//-----------------------------------【Properties】------------------------------------------  
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
	}

	//----------------------------------【SubShader】---------------------------------------  
	SubShader
	{
		ZWrite Off
		Blend Off

		//---------------------------------------【Pass 0】------------------------------------
		// Pass 0: Down Sample Pass
		Pass
		{
			ZTest Off
			Cull Off

			CGPROGRAM

			//Укажите вершинный шейдер для этого канала как: vert_DownSmpl
			#pragma vertex vert_DownSmpl
			//Укажите пиксельный шейдер для этого канала как: frag_DownSmpl
			#pragma fragment frag_DownSmpl

			ENDCG

		}

		//---------------------------------------【Pass 1】------------------------------------
		// Pass 1: Vertical Pass
		Pass
		{
			ZTest Always
			Cull Off

			CGPROGRAM

			//Укажите вершинный шейдер для этого канала как: vert_BlurVertical
			#pragma vertex vert_BlurVertical
			//Укажите пиксельный шейдер для этого канала как: frag_Blur
			#pragma fragment frag_Blur

			ENDCG
		}

		//---------------------------------------【Pass 2】------------------------------------
		// Pass 2: Horizontal Pass
		Pass
		{
			ZTest Always
			Cull Off

			CGPROGRAM

			//Укажите вершинный шейдер для этого канала как: vert_BlurHorizontal
			#pragma vertex vert_BlurHorizontal
			//Укажите пиксельный шейдер для этого канала как: frag_Blur
			#pragma fragment frag_Blur

			ENDCG
		}
	}


	//------------------------- Begin CG Include Part ----------------------  
	CGINCLUDE

	//【include】
	#include "UnityCG.cginc"

	//【Variable Declaration】
	sampler2D _MainTex;
	//UnityCG.cginc Переменные встроены, один размер пикселя в текстуре || it is the size of a texel of the texture
	uniform half4 _MainTex_TexelSize;
	//C# Скрипт контролируемых переменных || Parameter
	uniform half _DownSampleValue;

	//【3】Структура ввода вершин || Vertex Input Struct
	struct VertexInput
	{
		// Координаты вершины
		float4 vertex : POSITION;
		// Текстурные координаты уровня 1
		half2 texcoord : TEXCOORD0;
	};

	//【4】Структура выходного сигнала понижающей дискретизации || Vertex Input Struct
	struct VertexOutput_DownSmpl
	{
		// Пиксельные координаты положения
		float4 pos : SV_POSITION;
		// Текстурные координаты уровня 1（В правом верхнем углу）
		half2 uv20 : TEXCOORD0;
		// Текстурные координаты уровня 2（Нижний левый）
		half2 uv21 : TEXCOORD1;
		// Текстурные координаты уровня 3（Нижний правый）
		half2 uv22 : TEXCOORD2;
		// Текстурные координаты уровня 4（Верхний левый）
		half2 uv23 : TEXCOORD3;
	};


	//【5】Подготовить матрицу с параметрами матрицы нечеткого веса Гаусса 7x4 ||  Gauss Weight
	static const half4 GaussWeight[7] =
	{
		half4(0.0205,0.0205,0.0205,0),
		half4(0.0855,0.0855,0.0855,0),
		half4(0.232,0.232,0.232,0),
		half4(0.324,0.324,0.324,1),
		half4(0.232,0.232,0.232,0),
		half4(0.0855,0.0855,0.0855,0),
		half4(0.0205,0.0205,0.0205,0)
	};


	//【6】Функция затенения вершин || Vertex Shader Function
	VertexOutput_DownSmpl vert_DownSmpl(VertexInput v)
	{
		//【6.1】Создание выходной структуры с понижением выборки
		VertexOutput_DownSmpl o;

		//【6.2】Заполните структуру вывода
		// Координаты проекта в 3D пространстве в 2D окне  
		o.pos = UnityObjectToClipPos(v.vertex);
		// Понижение изображения: возьмите точки вокруг пикселя вверх, вниз, влево и вправо и сохраните их в четырех уровнях координат текстуры
		o.uv20 = v.texcoord + _MainTex_TexelSize.xy* half2(0.5h, 0.5h);;
		o.uv21 = v.texcoord + _MainTex_TexelSize.xy * half2(-0.5h, -0.5h);
		o.uv22 = v.texcoord + _MainTex_TexelSize.xy * half2(0.5h, -0.5h);
		o.uv23 = v.texcoord + _MainTex_TexelSize.xy * half2(-0.5h, 0.5h);

		//【6.3】Вернуть окончательный результат
		return o;
	}

	//【7】Функция затенения фрагментов || Fragment Shader Function
	fixed4 frag_DownSmpl(VertexOutput_DownSmpl i) : SV_Target
	{
		//【7.1】Определить временное значение цвета
		fixed4 color = (0,0,0,0);

	//【7.2】Добавить значения текстуры в четырех смежных пикселях
	color += tex2D(_MainTex, i.uv20);
	color += tex2D(_MainTex, i.uv21);
	color += tex2D(_MainTex, i.uv22);
	color += tex2D(_MainTex, i.uv23);

	//【7.3】Возвращает итоговое среднее
	return color / 4;
	}

		//【8】Структура ввода вершин || Vertex Input Struct
	struct VertexOutput_Blur
	{
		// Пиксельные координаты
		float4 pos : SV_POSITION;
		// Основная текстура（Координаты текстуры）
		half4 uv : TEXCOORD0;
		// Вторичная текстура（офсет）
		half2 offset : TEXCOORD1;
	};

	//【9】Функция затенения вершин || Vertex Shader Function
	VertexOutput_Blur vert_BlurHorizontal(VertexInput v)
	{
		//【9.1】Создание выходной структуры
		VertexOutput_Blur o;

		//【9.2】Заполните структуру вывода
		// Координаты проекта в 3D пространстве в 2D окне
		o.pos = UnityObjectToClipPos(v.vertex);
		// Координаты текстуры
		o.uv = half4(v.texcoord.xy, 1, 1);
		// Рассчитать смещение в направлении X
		o.offset = _MainTex_TexelSize.xy * half2(1.0, 0.0) * _DownSampleValue;

		//【9.3】Вернуть окончательный результат
		return o;
	}

	//【10】Функция затенения вершин || Vertex Shader Function
	VertexOutput_Blur vert_BlurVertical(VertexInput v)
	{
		//【10.1】Создание выходной структуры
		VertexOutput_Blur o;

		//【10.2】Заполните структуру вывода
		// Координаты проекта в 3D пространстве в 2D окне  
		o.pos = UnityObjectToClipPos(v.vertex);
		// Координаты текстуры
		o.uv = half4(v.texcoord.xy, 1, 1);
		// Рассчитать смещение в направлении Y
		o.offset = _MainTex_TexelSize.xy * half2(0.0, 1.0) * _DownSampleValue;

		//【10.3】Вернуть окончательный результат
		return o;
	}

	//【11】Функция затенения фрагментов || Fragment Shader Function
	half4 frag_Blur(VertexOutput_Blur i) : SV_Target
	{
		//【11.1】Получить оригинальные координаты UV
		half2 uv = i.uv.xy;

		//【11.2】Получить смещение
		half2 OffsetWidth = i.offset;
		// Смещение 3 интервалов от центра, взвешенное накопление, начиная с самого левого или самого верхнего
		half2 uv_withOffset = uv - OffsetWidth * 3.0;

		//【11.3】Цикл взвешенных значений цвета
		half4 color = 0;
		for (int j = 0; j< 7; j++)
		{
			// Значение текстуры пикселя после смещения
			half4 texCol = tex2D(_MainTex, uv_withOffset);
			// Выводится значение цвета += смещенное значение текстуры пикселя x вес по Гауссу
			color += texCol * GaussWeight[j];
			// Перейти к следующему пикселю и подготовиться к следующему циклическому взвешиванию
			uv_withOffset += OffsetWidth;
		}

		//【11.4】Возвращает окончательное значение цвета
		return color;
	}

	//------------------- Завершить раздел описания языка раскраски CG  || End CG Programming Part------------------  			
	ENDCG

	FallBack Off
}