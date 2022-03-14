using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace ImageFilters
{
    class Alpha
    {
        public static byte[] counting_sort(byte[] array, int arrlength)
        {
            byte[] count = new byte[256];
            byte[] output = new byte[arrlength];
            for (int i = 0; i < count.Length; i++)
            {
                count[i] = 0;
            }
            for (int i = 0; i < arrlength; i++)
            {
                count[array[i]]++;
            }
            for (int i = 1; i <= 255; i++)
            {
                count[i] += count[i - 1];
            }
            for (int i = arrlength - 1; i >= 0; i--)
            {
                output[count[array[i]] - 1] = array[i];
                --count[array[i]];
            }
            return output;
        }
        public static byte AlphaFilter_counting(byte[,] Image1, int x_pos, int y_pos,int Wmax, int trim_value)
        {
            byte[] window;
            int[] Dimension_x, Dimention_y;
            if (Wmax % 2 != 0)
            {
                window = new byte[Wmax * Wmax];
                Dimension_x = new int[Wmax * Wmax];
                Dimention_y = new int[Wmax * Wmax];
            }
            else
            {
                window = new byte[(Wmax + 1) * (Wmax + 1)];
                Dimension_x = new int[(Wmax + 1) * (Wmax + 1)];
                Dimention_y = new int[(Wmax + 1) * (Wmax + 1)];
            }
            int Index = 0;
            for (int _y = -(Wmax / 2); _y <= (Wmax / 2); _y++)
            {
                for (int _x = -(Wmax / 2); _x <= (Wmax / 2); _x++)
                {
                    Dimension_x[Index] = _x;
                    Dimention_y[Index] = _y;
                    Index++;
                }
            }
            byte Max, Min;
            int ArrayLength, Sum, NewY, NewX, Avg;
            Sum = 0;
            Max = 0;
            Min = 255;
            ArrayLength = 0;
            
            for (int i = 0; i < Wmax * Wmax; i++)
            {
                NewY = y_pos + Dimention_y[i];
                NewX = x_pos + Dimension_x[i];
                if (NewX >= 0 && NewX < ImageOperations.GetWidth(Image1) && NewY >= 0 && NewY < ImageOperations.GetHeight(Image1))
                {
                    window[ArrayLength] = Image1[NewY, NewX];
                    if (window[ArrayLength] > Max)
                        Max = window[ArrayLength];
                    if (window[ArrayLength] < Min)
                        Min = window[ArrayLength];
                    ArrayLength++;
                }
                else if(NewX < 0 || NewX > (ImageOperations.GetWidth(Image1)+ Wmax/2)|| NewY < 0 || NewY > (ImageOperations.GetHeight(Image1)+ Wmax/2))
                {
                    window[ArrayLength] = 1;
                    ArrayLength++;
                }
            }
            byte[] trimmed = new byte[ArrayLength];
            int trimmed_ind = 0;
            window = counting_sort(window, ArrayLength);
            for (int i = trim_value - 1; i <= ArrayLength - trim_value; i++)
            {
                trimmed[trimmed_ind] = window[i];
                trimmed_ind +=1;

            }
            for (int i = 0; i < trimmed.Length; i++)
            {
                Sum += trimmed[i];
            }
            if(trimmed_ind == 0)
            {
                trimmed_ind = Wmax - (trim_value * 2);
            }
            Avg = Sum / trimmed_ind;
            return (byte)Avg;
        }
        public static byte[,] imagefilter(byte[,] Image, int size, int trim)
        {
            byte[,] filtered = Image;
            for (int y = 0; y < ImageOperations.GetHeight(Image); y++)
            {
                for (int x = 0; x < ImageOperations.GetWidth(Image); x++)
                {
                    filtered[y, x] = AlphaFilter_counting(Image,x,y,size,trim);
                }
            }
            return filtered;
        }
        public static byte AlphaFilter_Kth(byte[,] Image1, int x_pos, int y_pos, int Wmax, int trim_value)
        {
            byte[] window;
            int[] Dimension_x, Dimention_y;
            if (Wmax % 2 != 0)
            {
                window = new byte[Wmax * Wmax];
                Dimension_x = new int[Wmax * Wmax];
                Dimention_y = new int[Wmax * Wmax];
            }
            else
            {
                window = new byte[(Wmax + 1) * (Wmax + 1)];
                Dimension_x = new int[(Wmax + 1) * (Wmax + 1)];
                Dimention_y = new int[(Wmax + 1) * (Wmax + 1)];
            }
            int Index = 0;
            for (int _y = -(Wmax / 2); _y <= (Wmax / 2); _y++)
            {
                for (int _x = -(Wmax / 2); _x <= (Wmax / 2); _x++)
                {
                    Dimension_x[Index] = _x;
                    Dimention_y[Index] = _y;
                    Index++;
                }
            }
            int ArrayLength, Sum, NewY, NewX, Avg;
            Sum = 0;
            ArrayLength = 0;
            for (int i = 0; i < Wmax * Wmax; i++)
            {
                NewY = y_pos + Dimention_y[i];
                NewX = x_pos + Dimension_x[i];
                if (NewX >= 0 && NewX < ImageOperations.GetWidth(Image1) && NewY >= 0 && NewY < ImageOperations.GetHeight(Image1))
                {
                    window[ArrayLength] = Image1[NewY, NewX];
                    ArrayLength++;
                }
                else if (NewX < 0 || NewX > (ImageOperations.GetWidth(Image1) +( Wmax / 2)) || NewY < 0 || NewY > (ImageOperations.GetHeight(Image1) + (Wmax / 2)))
                {
                    window[ArrayLength] = 1;
                    ArrayLength++;
                }
            }
            byte[] newwindow = new byte[ArrayLength];
            int trim_size = 0;
            for (int i = 0; i < trim_value; i++)
            {
                byte max = 0;
                byte min = 255;
                int positon = 0;
                for (int j = 0; j < window.Length; j++)
                {
                    if (trim_size < trim_value)
                    {
                        if (window[j] == 0)
                        {       
                            trim_size++;
                            continue;
                        }
                    }
                    if (window[j] < min)
                    {
                        min = window[j];
                        positon = j;
                    }
                }
                window[positon] = 0;
                positon = 0;
                for (int j = 0; j < window.Length; j++)
                {
                    if (window[j] > max)
                    {
                        max = window[j];
                        positon = j;
                    }
                }
                window[positon] = 0;
            }
            int size = 0;
            for(int i = 0; i<ArrayLength;i++)
            {
                if (window[i] == 0)
                {
                    continue;
                }
                else
                {
                    newwindow[size] = window[i];
                    size++;
                }
            }
            for(int i = 0; i < ArrayLength;i++)
            {
                Sum += newwindow[i];
            }
            if(size == 0)
            {
                size = (window.Length - (2 * trim_value));
            }
            Avg = Sum / size;
            return (byte)Avg;
        }
        public static byte[,] imagefilter1(byte[,] Image, int size, int trim)
        {
            byte[,] filtered = Image;
            for (int y = 0; y < ImageOperations.GetHeight(Image); y++)
            {
                for (int x = 0; x < ImageOperations.GetWidth(Image); x++)
                {
                    filtered[y, x] = AlphaFilter_Kth(Image, x, y, size, trim);
                }
            }
            return filtered;
        }
    }
}