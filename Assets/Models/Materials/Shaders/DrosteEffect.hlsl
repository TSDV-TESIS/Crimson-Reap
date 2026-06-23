#ifndef DROSTE_EFFECT_INCLUDED
#define DROSTE_EFFECT_INCLUDED

// Logaritmo complejo idéntico a Shadertoy
float2 clog(float2 z)
{
    return float2(log(length(z)), atan2(z.y, z.x));
}

void DrosteUV_float(float2 UV, float TimeInput, float Speed, out float2 OutUV)
{
    // 1. Centrar coordenadas idéntico a: (fragCoord - 0.5 * iResolution.xy) / iResolution.y
    float2 p = UV - 0.5;
    
    // 2. Parámetros de animación originales
    float animate = fmod(TimeInput * Speed, 2.07);
    float rate = sin(TimeInput * 0.5);
    
    // 3. Logaritmo complejo + multiplicación de matriz mat2(1, .11, rate*0.5, 1)
    // En HLSL mul(vector, matriz) multiplica el vector como fila. 
    // Para respetar las columnas de GLSL, definimos la matriz transpuesta.
    float2x2 m = float2x2(
        1.0, rate * 0.5,
        0.11, 1.0
    );
    
    p = clog(p);
    p = mul(p, m);
    
    // 4. Exponencial compleja original: exp(p.x - animate) * vec2(cos(p.y), sin(p.y))
    float r = exp(p.x - animate);
    p = float2(r * cos(p.y), r * sin(p.y));
    
    // 5. El cálculo del mosaico infinito (Grid Droste)
    float2 c = abs(p);
    float2 duv = 0.5 + p * exp2(ceil(-log2(max(c.y, c.x)) - 2.0));
    
    // Retornamos las UVs perfectas al nodo
    OutUV = duv;
}

#endif