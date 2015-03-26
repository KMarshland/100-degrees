using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoiseLibrary
{
    public enum _INSTRUCTIONTYPE
    {
        ADD, SUB, MULT
    }
    public class NoiseInstruction
    {
        public _INSTRUCTIONTYPE m_Type;
        public float[,] m_Base;
        int width, height;
        int m_Octaves;
        float m_Perm;

        public NoiseInstruction(float[,] baseNoise, _INSTRUCTIONTYPE type, int octaves, float perm) {
            m_Type = type;
            m_Octaves = octaves;
            m_Base = baseNoise;
            m_Perm = perm;

            width = baseNoise.GetLength(0);
            height = baseNoise.GetLength(1);
        }

        public void GenerateNoise()
        {
            m_Base = SimplexNoise.GenerateSmoothSimplex(m_Base, m_Octaves, m_Perm);
        }

        public static float[,] CombineNoise(NoiseInstruction[] instructions, int width, int height)
        {
            float[,] noise = new float[width, height];

            float pointNoise;
            float low = 0, high = 0;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    pointNoise = 0.0f;

                    foreach (NoiseInstruction n in instructions)
                    {
                        switch (n.m_Type)
                        {
                            case _INSTRUCTIONTYPE.ADD:  pointNoise += n.m_Base[x, y]; break;
                            case _INSTRUCTIONTYPE.SUB:  pointNoise -= n.m_Base[x, y]; break;
                            case _INSTRUCTIONTYPE.MULT: pointNoise *= n.m_Base[x, y]; break;
                        }
                    }

                    noise[x, y] = pointNoise;

                    if (pointNoise > high) high = pointNoise;
                    if (pointNoise < low) low = pointNoise;
                }
            }
            float sigma = 1 / (high - low);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    noise[x, y] = (noise[x, y] - low) * sigma;
                }
            }

            return noise;
        }
    }
}
