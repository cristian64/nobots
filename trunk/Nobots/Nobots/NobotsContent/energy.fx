/* ********************************************************
 * A Simple toon shader based on the work of Petri T. Wilhelmsen
 * found on his blog post XNA Shader Programming – Tutorial 7, Toon shading
 * http://digitalerr0r.wordpress.com/2009/03/22/xna-shader-programming-tutorial-7-toon-shading/.
 * Which in turn is based on the shader "post edgeDetect" from nVidias Shader library
 * http://developer.download.nvidia.com/shaderlibrary/webpages/shader_library.html
 *
 * This process will use a Sobell convolution filter to determine contrast across each pixel.
 * pixels that have a contrast greater than a given threshold value will be treated
 * as an edge pixel and turned black.
 *
 * Author: John Marquiss
 * Email: txg1152@gmail.com
 *  
 * This work by John Marquiss is licensed under a 
 * Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
 * http://creativecommons.org/licenses/by-nc-sa/3.0/
 */

sampler ColorMapSampler : register(s0);

/* Screen size (really texture size) is used to
 * scale the outline line thickness nicely around the
 * image
 */
float2 ScreenSize = float2(800.0f, 600.0f);

/* Outline line thickness scale
 */
float Thickness = 0.3f;

/* Edge detection threshold
 * Contrast values over the threshold are considered
 * edges.  That means smaller values for the threshold make the
 * image more "edgy" higher values less so.
 */
float Threshold = 1;

/* getGray
 * a simple helper function to return a grey scale
 * value for a given pixel
 */
float getGray(float4 c)
{
	/* The closer a color is to a pure gray
	 * value the closer its dot product and gray
	 * will be to 0.
	 */
	return(dot(c.rgb,((0.33333).xxx)));
}

struct VertexShaderOutput
{
    float2 Tex : TEXCOORD0;
};

/* Shade each pixel turning edge pixels black
 */
float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	// Get the source pixel color
	float4 Color = tex2D(ColorMapSampler, input.Tex);
	Color[0] = Color[3];
	Color[1] = Color[3];
	Color[2] = Color[3] * 0.5f;

	return Color;
}

technique PostOutline
{
    pass Pass0
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}

