using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoiseLibrary
{
    public class SimplexNoise   // Simplex Noise in 2D, 3D, and 4D
    {
        private static int[,] grad3 = {
                                          {1,1,0}, {-1,1,0}, {1,-1,0}, {-1,-1,0},
                                          {1,0,1}, {-1,0,1}, {1,0,-1}, {-1,0,-1},
                                          {0,1,1}, {0,-1,1}, {0,1,-1}, {0,-1,-1}
                                      };
        private static int[,] grad4 = {
                                          {0,1,1,1}, {0,1,1,-1}, {0,1,-1,1}, {0,1,-1,-1},
                                          {0,-1,1,1}, {0,-1,1,-1}, {0,-1,-1,1}, {0,-1,-1,-1},

                                          {1,0,1,1}, {1,0,1,-1}, {1,0,-1,1}, {1,0,-1,-1},
                                          {-1,0,1,1},{-1,0,1,-1},{-1,0,-1,1},{-1,0,-1,-1},
                                          
                                          {1,1,0,1}, {1,1,0,-1}, {1,-1,0,1}, {1,-1,0,-1},
                                          {-1,1,0,1},{-1,1,0,-1},{-1,-1,0,1},{-1,-1,0,-1},
                                          
                                          {1,1,1,0}, {1,1,-1,0},{1,-1,1,0},{1,-1,-1,0},
                                          {-1,1,1,0},{-1,1,-1,0},{-1,-1,1,0},{-1,-1,-1,0}
                                      };
        private static int[] p = {
                                     151,160,137,91,90,15,131,13,201,95,96,53,194,233,7,225,140,36,103,30,
                                     69,142,8,99,37,240,21,10,23,190,6,148,247,120,234,75,0,26,197,62,94,
                                     252,219,203,117,35,11,32,57,177,33,88,237,149,56,87,174,20,125,136,
                                     171,168,68,175,74,165,71,134,139,48,27,166,77,146,158,231,83,111,229,
                                     122,60,211,133,230,220,105,92,41,55,46,245,40,244,102,143,54,65,25,63,
                                     161,1,216,80,73,209,76,132,187,208,89,18,169,200,196,135,130,116,188,
                                     159,86,164,100,109,198,173,186,3,64,52,217,226,250,124,123,5,202,38,
                                     147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,223,
                                     183,170,213,119,248,152,2,44,154,163,70,221,153,101,155,167,43,172,9,
                                     129,22,39,253,19,98,108,110,79,113,224,232,178,185,112,104,218,246,97,
                                     228,251,34,242,193,238,210,144,12,191,179,162,241,81,51,145,235,249,14,
                                     239,107,49,192,214,31,181,199,106,157,184,84,204,176,115,121,50,45,127,
                                     4,150,254,138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,
                                     61,156,180
                                 };
        
		// Used for 4D Simplex Noise cases
        private static int[,] simplex = {
                                            {0,1,2,3},{0,1,3,2},{0,0,0,0},{0,2,3,1},{0,0,0,0},{0,0,0,0},{0,0,0,0},{1,2,3,0},
                                            {0,2,1,3},{0,0,0,0},{0,3,1,2},{0,3,2,1},{0,0,0,0},{0,0,0,0},{0,0,0,0},{1,3,2,0},
                                            {0,0,0,0},{0,0,0,0},{0,0,0,0},{0,0,0,0},{0,0,0,0},{0,0,0,0},{0,0,0,0},{0,0,0,0},
                                            {1,2,0,3},{0,0,0,0},{1,3,0,2},{0,0,0,0},{0,0,0,0},{0,0,0,0},{2,3,0,1},{2,3,1,0},
                                            {1,0,2,3},{1,0,3,2},{0,0,0,0},{0,0,0,0},{0,0,0,0},{2,0,3,1},{0,0,0,0},{2,1,3,0},
                                            {0,0,0,0},{0,0,0,0},{0,0,0,0},{0,0,0,0},{0,0,0,0},{0,0,0,0},{0,0,0,0},{0,0,0,0},
                                            {2,0,1,3},{0,0,0,0},{0,0,0,0},{0,0,0,0},{3,0,1,2},{3,0,2,1},{0,0,0,0},{3,1,2,0},
                                            {2,1,0,3},{0,0,0,0},{0,0,0,0},{0,0,0,0},{3,1,0,2},{0,0,0,0},{3,2,0,1},{3,2,1,0}
                                        };

        // since C# doesn't like static{blah blah blah} gotta put perm lookup declaration into a utility method
        private static int[] perm = new int[512];
        public static void init()
        {
            for (int i = 0; i < 512; i++)
            {
                perm[i] = p[i & 255];
            }
        }

        // utility methods
        
        // this method is a lot faster than uing (int)Math.floor(x)
        private static int fastfloor(double x)
        {
            return x > 0 ? (int)x : (int)x - 1;
        }
        // 2D dot product
        private static double dot(int[] g, double x, double y){
            return g[0]*x + g[1]*y;
        }
        // 3D dot product
        private static double dot(int[] g, double x, double y, double z)
        {
            return g[0] * x + g[1] * y + g[2] * z;
        }
        // 4D dot product
        private static double dot(int[] g, double x, double y, double z, double w)
        {
            return g[0] * x + g[1] * y + g[2] * z + g[3] * w;
        }

        // generating white noise
        public static float[,] GenerateWhiteNoise(int width, int height)
        {
            Random gen = new Random(); // seed 0 for testing
            float[,] noise = new float[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    noise[i, j] = (float)gen.NextDouble() % 1; // normalizes between [0,1) I think?
                }
            }

            return noise;
        }
        // generate island-y continent
        public static float[,] GenerateIslandNoise(int width, int height, float bias)
        {
            float[,] noise = new float[width, height];

            int px = width / 2; // the center points of the map, used when biasing into a more islandy shape
            int py = height / 2;

            // pick the smallest of px and py to be the bias radius
            int bRad = (px > py) ? py : px;
            noise = GenerateWhiteNoise(width, height);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    // distance function -- Euclidean
                    double distance = Math.Sqrt((px - i) * (px - i) + (py - j) * (py - j));
                    if (distance > bias * bRad)
                    {
                        noise[i, j] *= (float)(bias * bRad / distance);
                    }
                }
            }

            return noise;
        }
        // generate smooth noise and output a floating point 2D graph
        private static float[,] GenerateSmoothNoise(float[,] baseNoise, int octave)
        {
            int width = baseNoise.GetLength(0);
            int height = baseNoise.GetLength(1);

            float[,] smoothNoise = new float[width, height];

            int samplePeriod = 1 << octave;
            float sampleFrequency = 1.0f / samplePeriod;

            for (int i = 0; i < width; i++)
            {
                // calculate the horizontal sampling indices
                int[] sample_i = new int[2];
                sample_i[0] = (i / samplePeriod) * samplePeriod;
                sample_i[1] = (sample_i[0] + samplePeriod) % width; // wrap around

                float horizontalBlend = (i - sample_i[0]) * sampleFrequency;

                for (int j = 0; j < height; j++)
                {
                    int[] sample_j = new int[2];
                    sample_j[0] = (j / samplePeriod) * samplePeriod;
                    sample_j[1] = (sample_j[0] + samplePeriod) % height; // wrap around

                    float verticalBlend = (j - sample_j[0]) * sampleFrequency;

                    // blend top two corners
                    float top = (float)cosInterpolate(baseNoise[sample_i[0], sample_j[0]], baseNoise[sample_i[1], sample_j[0]], horizontalBlend);
                    // blend bottom two corners
                    float bottom = (float)cosInterpolate(baseNoise[sample_i[0], sample_j[1]], baseNoise[sample_i[1], sample_j[1]], horizontalBlend);

                    // final blend
                    smoothNoise[i, j] = (float)cosInterpolate(top, bottom, verticalBlend);
                }
            }


            return smoothNoise;
        }
		
		// 3D smooth noise output of 3D volume
		private static float[,,] GenerateSmoothNoise(float[,,] baseNoise, int octave){
			int width = baseNoise.GetLength(0);
			int height = baseNoise.GetLength(1);
			int depth = baseNoise.GetLength(2);
			
			float[,,] smoothNoise = new float[width, height, depth];
			
			int samplePeriod = 1<<octave;
			float sampleFrequency = 1.0f / samplePeriod;
			
			for (int i = 0; i < width; i++){
				// calculate the horizontal sampling indicis
				int[] sample_i = new int[2];
				sample_i[0] = (i / samplePeriod) * samplePeriod;
				sample_i[1] = (sample_i[0] + samplePeriod) % width;
				
				float horizontalBlend = (i-sample_i[0]) * sampleFrequency;
				
				for (int j = 0; j < height; j++){
					int[] sample_j = new int[2];
					sample_j[0] = (j / samplePeriod) * samplePeriod;
					sample_j[1] = (sample_j[0] + samplePeriod) % height;
					
					float verticalBlend = (j - sample_j[0]) * sampleFrequency;
					
					for (int k = 0; k < depth; k++){
						int[] sample_k = new int[2];
						sample_k[0] = (k / samplePeriod) * samplePeriod;
						sample_k[1] = (sample_k[0] + samplePeriod) % depth;
						/*
							// blend top two corners
		                    float top = (float)cosInterpolate(baseNoise[sample_i[0], sample_j[0]], baseNoise[sample_i[1], sample_j[0]], horizontalBlend);
		                    // blend bottom two corners
		                    float bottom = (float)cosInterpolate(baseNoise[sample_i[0], sample_j[1]], baseNoise[sample_i[1], sample_j[1]], horizontalBlend);
		
		                    // final blend
		                    smoothNoise[i, j] = (float)cosInterpolate(top, bottom, verticalBlend);
						//*/
						
						float depthBlend = (k - sample_k[0]) * sampleFrequency;
						// front side corners
						float TF = (float)cosInterpolate(baseNoise[sample_i[0], sample_j[0], sample_k[0]], baseNoise[sample_i[1], sample_j[0], sample_k[0]], horizontalBlend);
						float BF = (float)cosInterpolate(baseNoise[sample_i[0], sample_j[1], sample_k[0]], baseNoise[sample_i[1], sample_j[1], sample_k[0]], horizontalBlend);
						
						// back side corners
						float TB = (float)cosInterpolate(baseNoise[sample_i[0], sample_j[0], sample_k[1]], baseNoise[sample_i[1], sample_j[0], sample_k[1]], horizontalBlend);
						float BB = (float)cosInterpolate(baseNoise[sample_i[0], sample_j[1], sample_k[1]], baseNoise[sample_i[1], sample_j[1], sample_k[1]], horizontalBlend);
						
						// front side final blend
						float F = (float)cosInterpolate(TF, BF, verticalBlend);
						float B = (float)cosInterpolate(TB, BB, verticalBlend);
						
						// final back to front blend
						smoothNoise[i,j,k] = (float)cosInterpolate(F,B,depthBlend);
					}
				}
			}
			
			return smoothNoise;
		}
		
		// 2D Smooth Simplex
        public static float[,] GenerateSmoothSimplex(float[,] baseNoise, int octaves, float permanance)
        {
            int width = baseNoise.GetLength(0);
            int height = baseNoise.GetLength(1);

            float[][,] smoothNoise = new float[octaves][,];

            float persistance = permanance;
			
			// modify baseNoise to include simplex values
			float[,] simplexBase = new float[width, height];
			for (int i = 0; i < height; i++){
				for (int j = 0; j < width; j++){
					simplexBase[j,i] = baseNoise[j,i] * (float)(noise (j,i)+1)/2f;
				}
			}
            
            // generate smooth noises
            for (int i = 0; i < octaves; i++)
            {
                //smoothNoise[i] = GenerateSmoothNoise(baseNoise, i);
				smoothNoise[i] = GenerateSmoothNoise(simplexBase, i);
            }

            // now to blend everything together
            float[,] simplexNoise = new float[width, height];
            float amplitude = 1.0f;
            float totalAmplitude = 0.0f;

            for (int octave = octaves - 1; octave >= 0; octave--)
            {
                amplitude *= persistance;
                totalAmplitude += amplitude;

                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        simplexNoise[i, j] += smoothNoise[octave][i, j] * amplitude;
                    }
                }
            }
            // now to normalize everything so that it shows up within the proper ranges!
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    simplexNoise[i, j] /= totalAmplitude;
                }
            }
            return simplexNoise;
        }
		
		// 3D Smooth Simplex
		public static float[,,] GenerateSmoothSimplex(float[,,] baseNoise, int octaves, float permanance){
			int width = baseNoise.GetLength(0);
            int height = baseNoise.GetLength(1);
			int depth = baseNoise.GetLength(2);
			
			float[][,,] smoothNoise = new float[octaves][,,];
			
			float persistance = permanance;
			
			// modify baseNoise to include simplex values
			float[,,] simplexBase = new float[width, height, depth];
			for (int i = 0; i < height; i++){
				for (int j = 0; j < width; j++){
					for (int k = 0; k < depth; k++){
						simplexBase[j,i,k] = baseNoise[j,i,k] * (float)(noise (j,i,k)+1)/2f;
					}
				}
			}
			
			// generate smooth noises
            for (int i = 0; i < octaves; i++)
            {
                //smoothNoise[i] = GenerateSmoothNoise(baseNoise, i);
				smoothNoise[i] = GenerateSmoothNoise(simplexBase, i);
            }
			float[,,] simplexNoise = new float[width, height, depth];
			float amplitude = 1.0f;
			float totalAmplitude = 0.0f;
			
			for (int octave = octaves-1; octave >= 0; octave--){
				amplitude *= persistance;
				totalAmplitude += amplitude;
				
				for (int i = 0; i < width; i++){
					for (int j = 0; j < height; j++){
						for (int k = 0; k < depth; k++){
							simplexNoise[i,j,k] += smoothNoise[octave][i,j,k] * amplitude;
						}	
					}
				}
			}
			
			// now to normalize everything so that it shows up within the proper ranges!
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
					for (int k = 0; k < depth; k++){
						
						simplexNoise[i, j, k] /= totalAmplitude;
					}
                }
            }
            return simplexNoise;
			
		}


        private const double skew2D = 0.36602540378443864676372317075294;
        private const double unskew2D = 0.21132486540518711774542560974902;
        // 2D Simplex Noise
        public static double noise(double xIn, double yIn)
        {
            //double n0, n1, n2; // noise contribution from the 3 corners
            // simplified to double[] n
            double[] n = new double[3]; // n0, n1, n2 values

            // Skew the input space to determine which simplex cell we're in
            //double F2 = 0.5 * (Math.Sqrt(3.0) - 1.0);
            // since Math.Sqrt (3.0) = 1.7320408075... we can just make this a constant skew value
            // F2 is now skew2D
            double s = (xIn + yIn) * skew2D; // hairy factor for 2D
            int i = fastfloor(xIn + s);
            int j = fastfloor(yIn + s);

            // double G2 = (3.0 - Math.Sqrt(3.0))/6.0);
            // G2 is the 2D unskew value = unskew2D
            double t = (i + j) * unskew2D;
            double X0 = i - t; // unskew back to x,y space
            double Y0 = j - t;

            double x0 = xIn - X0;
            double y0 = yIn - Y0;

            // For the 2D case, the simplex shape is an equilateral triangle
            // Determine which simplex we are in.
            int i1, j1; // offsets for second(middle) corner of simplex in (i,j) coord
            if (x0 > y0)
            {
                i1 = 1; j1 = 0;
            }
            else
            {
                i1 = 0; j1 = 1;
            }

            // A step of (1,0) in (i,j) means a step of (1-c, -c) in (x,y) and
            // a step of (0,1) in (i,j) means a step of (-c, 1-c) in (x,y), where
            // c = (3 - sqrt(3))/6 = unskew2D

            double x1 = x0 - i1 + unskew2D;
            double y1 = y0 - j1 + unskew2D;
            double x2 = x0 - 1.0 + 2.0 * unskew2D;
            double y2 = y0 - 1.0 + 2.0 * unskew2D;

            // work out the hashed gradient indice of the three simplex corners
            int ii = i & 255;
            int jj = j & 255;

            // see if changing %12 to & 11 speed this up any?
            int gi0 = perm[ii + perm[jj]] % 12;
            int gi1 = perm[ii + i1 + perm[jj + j1]] % 12;
            int gi2 = perm[ii + 1 + perm[jj + 1]] % 12;

            // Calculate the contribution from the three corners
            double t0 = 0.5 - x0 * x0 - y0 * y0;
            if (t0 < 0) n[0] = 0.0;
            else
            {
                t0 *= t0;
                n[0] = t0 * t0 * dot(new int[] { grad3[gi0, 0], grad3[gi0, 1], grad3[gi0, 2] }, x0, y0); // (x,y) of grad3 used for 2D gradient
            }

            double t1 = 0.5 - x1 * x1 - y1 * y1;
            if (t1 < 0) n[1] = 0.0;
            else
            {
                t1 *= t1;
                n[1] = t1 * t1 * dot(new int[] { grad3[gi1, 0], grad3[gi1, 1], grad3[gi1, 2] }, x1, y1);
            }

            double t2 = 0.5 - x2 * x2 - y2 * y2;
            if (t2 < 0) n[2] = 0.0;
            else
            {
                t2 *= t2;
                n[2] = t2 * t2 * dot(new int[] { grad3[gi2, 0], grad3[gi2, 1], grad3[gi2, 2] }, x2, y2);
            }

            // add contribution from each orner to get the final noise value.
            // the result is scaled to return values in the interval [-1,1]

            return 70.0 * (n[0] + n[1] + n[2]);
            //return 0;
        }
		
		private const double skew3D= 1.0/3.0;
		private const double unskew3D= 1.0/6.0;
		// 3D Simplex Noise
		public static double noise(double xIn, double yIn, double zIn){
			double[] n = new double[4]; // n0-n3 Noise contributions from the 4 corners
			
			// Skew the input space to determine which cell we're in
			//double F3 = 1.0/3.0;
			double s = (xIn + yIn + zIn)*skew3D; // Very nice and simple skew factor for 3D
			
			int i = fastfloor(xIn + s);
			int j = fastfloor(yIn + s);
			int k = fastfloor(zIn + s);
			
			//double G3 = 1.0/6.0; // Very nice and simple unskew factor, too
			double t = (i + j + k) * unskew3D;
			
			double X0 = i-t; // unskew the cell origin back to xyz space
			double Y0 = j-t;
			double Z0 = k-t;
			
			double x0 = xIn-X0; // the xyz distances from the cell origin
			double y0 = yIn-Y0;
			double z0 = zIn-Z0;
			
			// For the 3D case, the simplex shape is a slightly irregular tetrahedron.
			// Determine which simplex we are in
			int i1, j1, k1; // offsets for second corner of the simplex in ijk coords
			int i2, j2, k2; // offsets for third corner of the simplex in ijk coords
			
			if (x0 >= y0){
				if (y0>=z0){
					i1 = 1; j1 = 0; k1 = 0; i2 = 1; j2 = 1; k2 = 0; // xyz order
				}else if (x0 >= z0){
					i1 = 1; j1 = 0; k1 = 0; i2 = 1; j2 = 0; k2 = 1; // xzy order
				}else{
					i1 = 0; j1 = 0; k1 = 1; i2 = 1; j2 = 0; k2 = 1; // zxy order
				}
			}else{ // x0 < y0
				if (y0 < z0){
					 i1=0; j1=0; k1=1; i2=0; j2=1; k2=1; // Z Y X order
				}else if(x0 < z0){
					i1=0; j1=1; k1=0; i2=0; j2=1; k2=1;  // Y Z X order
				}else{
					i1=0; j1=1; k1=0; i2=1; j2=1; k2=0;  // Y X Z order
				}
			}
			
			// A step of (1,0,0) in ijk means a step of 1-c, -c, -c in xyz
			// a step of (0,1,0) in ijk means a step of -c, 1-c, -c in xyz
			// a step of (0,0,1) in ijk means a step of -c, -c, 1-c in xyz
			// c = 1/6 = unskew3D
			
			double x1 = x0 - i1 + unskew3D; // Offsets for second corner in (x,y,z) coords
		    double y1 = y0 - j1 + unskew3D;
		    double z1 = z0 - k1 + unskew3D;
		    double x2 = x0 - i2 + 2.0*unskew3D; // Offsets for third corner in (x,y,z) coords
		    double y2 = y0 - j2 + 2.0*unskew3D;
		    double z2 = z0 - k2 + 2.0*unskew3D;
		    double x3 = x0 - 1.0 + 3.0*unskew3D; // Offsets for last corner in (x,y,z) coords
		    double y3 = y0 - 1.0 + 3.0*unskew3D;
		    double z3 = z0 - 1.0 + 3.0*unskew3D;
		    // Work out the hashed gradient indices of the four simplex corners
		    int ii = i & 255;
		    int jj = j & 255;
		    int kk = k & 255;
		    int gi0 = perm[ii+perm[jj+perm[kk]]] % 12;
		    int gi1 = perm[ii+i1+perm[jj+j1+perm[kk+k1]]] % 12;
		    int gi2 = perm[ii+i2+perm[jj+j2+perm[kk+k2]]] % 12;
		    int gi3 = perm[ii+1+perm[jj+1+perm[kk+1]]] % 12;
		    // Calculate the contribution from the four corners
		    double t0 = 0.6 - x0*x0 - y0*y0 - z0*z0;
		    if(t0<0) n[0] = 0.0;
		    else {
		      t0 *= t0;
		      n[0] = t0 * t0 * dot(new int[] { grad3[gi0, 0], grad3[gi0, 1], grad3[gi0, 2] }, x0, y0, z0);
		    }
		    double t1 = 0.6 - x1*x1 - y1*y1 - z1*z1;
		    if(t1<0) n[1] = 0.0;
		    else {
		      t1 *= t1;
		      n[1] = t1 * t1 * dot(new int[] { grad3[gi1, 0], grad3[gi1, 1], grad3[gi1, 2]}, x1, y1, z1);
		    }
		    double t2 = 0.6 - x2*x2 - y2*y2 - z2*z2;
		    if(t2<0) n[2] = 0.0;
		    else {
		      t2 *= t2;
		      n[2] = t2 * t2 * dot(new int[] { grad3[gi2, 0], grad3[gi2, 1], grad3[gi2, 2]}, x2, y2, z2);
		    }
		    double t3 = 0.6 - x3*x3 - y3*y3 - z3*z3;
		    if(t3<0) n[3] = 0.0;
		    else {
		      t3 *= t3;
		      n[3] = t3 * t3 * dot(new int[] { grad3[gi3, 0], grad3[gi3, 1], grad3[gi3, 2]}, x3, y3, z3);
		    }
		    // Add contributions from each corner to get the final noise value.
		    // The result is scaled to stay just inside [-1,1]
		    return 32.0*(n[0] + n[1] + n[2] + n[3]);
		}
		
		
        public static double genNoise(double x, double y, double w, double h, int octaves)
        {
            double noiseVal = 0.0;

            double amplitude = 1.0, totalAmplitude = 0.0;

            for (int i = octaves-1; i >= 0; i--)
            {
                amplitude *= 0.5;
                totalAmplitude += amplitude;

                noiseVal += genSmoothNoise(x, y, w, h, i) * amplitude;
            }

            noiseVal /= totalAmplitude;

            return noiseVal;
        }
        
        // smooth noise functions so that multiple octave solutions can be used
        private static double genSmoothNoise(double x, double y, double w, double h, int octaves)
        {
            int samplePeriod = 1 << octaves;
            double sampleFrequency = 1.0 / (double)samplePeriod;

            double sample_i0 = (x / samplePeriod) * samplePeriod;
            double sample_i1 = (sample_i0 + samplePeriod) % w;

            double sample_j0 = (y / samplePeriod) * samplePeriod;
            double sample_j1 = (sample_j0 + samplePeriod) % h;

            double hBlend = (x - sample_i0) * sampleFrequency;
            double vBlend = (y - sample_j0) * sampleFrequency;

            double top = cosInterpolate(noise(sample_i0, sample_j0), noise(sample_i1, sample_j0), hBlend);
            double bottom = cosInterpolate(noise(sample_i0, sample_j1), noise(sample_i1, sample_j1), hBlend);

            return cosInterpolate(top, bottom, vBlend);
        }

        // Interpolation function(s)
        private static double cosInterpolate(double x0, double x1, double alpha)
        {
            double alpha2 = (1 - Math.Cos(alpha * Math.PI)) / 2.0;

            return x0 * (1 - alpha2) + x1 * alpha;
        }
        private static double linInterpolate(double x0, double x1, double alpha)
        {
            return x0 * (1 - alpha) + x1 * alpha;
        }
        private static double hemInterpolate(double x0, double x1, double alpha)
        {
            // use cosInterpolate to grab a double value to do hermite blending with
            double t = cosInterpolate(x0, x1, alpha);

            return (t * t * t * (10 + t * (-15 + t * 6)));
        }
        private static double hemLinInterpolate(double x0, double x1, double alpha)
        {
            double t = linInterpolate(x0, x1, alpha);

            return (t * t * t * (10 + t * (-15 + t * 6)));
        }
    }
}
