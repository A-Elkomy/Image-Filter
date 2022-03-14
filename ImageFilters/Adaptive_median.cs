using System;
using System.Collections.Generic;
using System.Text;

namespace ImageFilters
{
    class Adaptive_median
    {
        public static byte Filter_counting(byte[,] image1,int x_pos,int y_pos,int max_size,int size)
            {
            byte[] window = new byte[size*size];
            int[] x_B = new int[size*size];
            int[] y_B = new int[size * size];
            int index = 0;
            for (int i = -(size/2); i <= (size/2); i++)
            {
                for (int j = -(size/2); j <= (size/2); j++)
                {
                    x_B[index] = j;
                    y_B[index] = i;
                    index++;
                }
            }
            byte max, min, med, z;
            int a1, a2, b1, b2, arraylength, Y, X;
            max = 0;
            min = 255;
            arraylength = 0;
            z = image1[y_pos, x_pos];
            for(int i = 0;i<size*size;i++)
            {
                Y = y_pos + y_B[i];
                X = x_pos + x_B[i];
                if(X >= 0&&X<ImageOperations.GetWidth(image1)&& Y>= 0 && Y< ImageOperations.GetHeight(image1))
                {
                    window[arraylength] = image1[Y, X];
                    if (window[arraylength] > max)
                    {
                        max = window[arraylength];
                    }
                    if(window[arraylength] < min)
                    {
                        min = window[arraylength];
                    }
                    arraylength++;
                }
            }
            window = COUNTING_SORT(window, arraylength,max,min);
            min = window[0];
            med = window[arraylength / 2];
            a1 = med - min;
            a2 = max - med;
            if(a1>0&&a2>0)
            {
                b1 = z - min;
                b2 = max - z;
                if (b1 > 0 && b2 > 0)
                    return z;
                else
                {
                    if (size + 2 < max_size)
                    {
                        return Filter_counting(image1, x_pos, y_pos, max_size, size + 2);
                    }
                    else
                        return med;
                }
                       
            }
            else
                return med;
        }
        public static byte[] COUNTING_SORT(byte[] Array, int ArrayLength, byte Max, byte Min)
        {
            byte[] count = new byte[Max - Min + 1];
            int z = 0;

            for (int i = 0; i < count.Length; i++) { count[i] = 0; }
            for (int i = 0; i < ArrayLength; i++) { count[Array[i] - Min]++; }

            for (int i = Min; i <= Max; i++)
            {
                while (count[i - Min]-- > 0)
                {
                    Array[z] = (byte)i;
                    z++;
                }
            }
            return Array;
        } 
        
        public static byte[,] Adaptive_count(byte[,] imagematrix,int max_size)
        {
            byte[,] image = imagematrix;
            for(int y = 0; y < ImageOperations.GetHeight(imagematrix); y++)
            {
                for(int x = 0; x < ImageOperations.GetWidth(imagematrix); x++)
                {
                    image[y,x] = Filter_counting(imagematrix,x, y, max_size,3);
                }
            }
            return image;
        }
        public static byte Filter_quick(byte[,] image,int x_pos,int y_pos,int max_size,int size)
        {
            byte[] window = new byte[size * size];
            int[] x_B = new int[size * size];
            int[] y_B = new int[size * size];
            int index = 0;
            for (int i = -(size / 2); i <= (size / 2); i++)
            {
                for (int j = -(size / 2); j <= (size / 2); j++)
                {
                    x_B[index] = j;
                    y_B[index] = i;
                    index++;
                }
            }
            byte max, min, med, z;
            int a1, a2, b1, b2, arraylength, Y, X;
            max = 0;
            min = 255;
            arraylength = 0;
            z = image[y_pos, x_pos];
            for (int i = 0; i < size * size; i++)
            {
                Y = y_pos + y_B[i];
                X = x_pos + x_B[i];
                if (X >= 0 && X < ImageOperations.GetWidth(image) && Y >= 0 && Y < ImageOperations.GetHeight(image))
                {
                    window[arraylength] = image[Y, X];
                    if (window[arraylength] > max)
                    {
                        max = window[arraylength];
                    }
                    if (window[arraylength] < min)
                    {
                        min = window[arraylength];
                    }
                    arraylength++;
                }
            }
            window = quick_sort(window, 0, arraylength-1);
            min = window[0];
            med = window[arraylength / 2];
            a1 = med - min;
            a2 = max - med;
            if (a1 > 0 && a2 > 0)
            {
                b1 = z - min;
                b2 = max - z;
                if (b1 > 0 && b2 > 0)
                    return z;
                else
                {
                    if (size + 2 < max_size)
                    {
                        return Filter_quick(image, x_pos, y_pos, max_size, size + 2);
                    }
                    else
                        return med;
                }

            }
            else
            {
                return med;
            } 
        }
        public static int divider(byte[] window, int part, int max)
        {
            byte pivot = window[max];
            byte temp;
            int i = part;
            for(int j = part; j < max; j ++)
            {
              if(window[j]<pivot)
                {
                    
                    temp = window[j];
                    window[j] = window[i];
                    window[i++] = temp;
                }
            }
            temp = window[i];
            window[i] = window[max];
            window[max] = temp;
            return i;
        }
        public static byte[] quick_sort(byte[] window,int part,int max)
        {
            if(max>part)
            {
                int mid = divider(window, part, max);
                quick_sort(window, part, mid - 1);
                quick_sort(window, mid + 1, max);
            }
            return window;
        }
        public static byte[,] Adaptive_quick(byte[,] imagematrix, int max_size)
        {
            byte[,] image = imagematrix;
            for (int y = 0; y < ImageOperations.GetHeight(imagematrix); y++)
            {
                for (int x = 0; x < ImageOperations.GetWidth(imagematrix); x++)
                {
                    image[y, x] = Filter_quick(imagematrix, x, y, max_size, 3);
                }
            }
            return image;
        }
    }
}
