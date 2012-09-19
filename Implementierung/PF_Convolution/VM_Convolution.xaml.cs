using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;

namespace PF_Convolution
{
    /// <summary>
    /// Interaktionslogik für VM_Convolution.xaml
    /// </summary>
    [Serializable()]
    public partial class VM_Convolution : UserControl
    {
        public VM_Convolution()
        {
            InitializeComponent();
        }
    }




    [ValueConversion(typeof(int[,]), typeof(string))]
    public class IntMatrixConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int[,] matrix = (int[,])value;
            string text = "";

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    text += matrix[i, j].ToString() + " ";
                }
                text += "\n";
            }
            
            return text;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int[,] matrix = null;
            string text = (string)value;

            string[] rows = text.Trim().Split('\n');
            for (int i = 0; i < rows.Length; i++)
            {
                string[] fields = rows[i].Trim().Split(' ');

                if (matrix == null) matrix = new int[rows.Length, fields.Length];

                for (int j = 0; j < fields.Length; j++)
                {
                    if (j < matrix.GetLength(1))
                    {
                        try
                        {
                            matrix[i, j] = int.Parse(fields[j]);
                        }
                        catch (FormatException)
                        {

                        }
                    }
                }
            }

            return matrix;
        }
    }
}
