using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Calculadora_WP.Resources;

namespace Calculadora_WP
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

        }

        private void clear(object sender, RoutedEventArgs e)
        {
            expression.Text = "";
            result.Text = "0";
        }

        private void delete(object sender, RoutedEventArgs e)
        {
            String temp = expression.Text;
            if(!temp.Equals(""))
                expression.Text = temp.Substring(0, temp.Length - 1);
        }

        private void writeNumber(object sender, RoutedEventArgs e)
        {
            String number = ((Button)sender).Content.ToString();
            if (expression.Text.Equals(""))
                expression.Text =  number;
            else
                expression.Text = expression.Text + number;
        }

        private void negative(object sender, RoutedEventArgs e)
        {
            String temp = expression.Text;
            if (temp.Equals(""))
                expression.Text = "-";
            else
                if (!temp.Substring(0, 1).Equals("-"))
                    expression.Text = "-" + temp;
        }

        private void writeOperation(object sender, RoutedEventArgs e)
        {
            String temp = expression.Text;
            String operation = ((Button)sender).Content.ToString();
            if (!temp.Equals(""))
                if (operation.Equals("+") || operation.Equals("*") || operation.Equals("/") || (operation.Equals("-") &&
                !temp.Substring(temp.Length - 2, 2).Equals("*-") &&
                !temp.Substring(temp.Length - 2, 2).Equals("/-") &&
                !temp.Substring(temp.Length - 2, 2).Equals("+-") &&
                !temp.Substring(temp.Length - 2, 2).Equals("--")))
                    expression.Text = temp + operation;
        }

        public double solveExpression(List<String> array)
        {
            if (array.Count == 1)
            {
                return Convert.ToDouble(array[0]); //Devuelve el valor si no hay operaciones que realizar
            }
            else
            {
                int i = 1;
                bool swm = false;
                while (i < array.Count - 1 && !swm)
                { //Miramos si hay multiplicaciones y divisiones
                    String s = array[i];
                    if (s.Equals("*") || s.Equals("/"))
                    {
                        Double n1 = Convert.ToDouble(array[i - 1]);
                        Double n2 = Convert.ToDouble(array[i + 1]);
                        if (s.Equals("*")) array[i] = "" + (n1 * n2);
                        if (s.Equals("/")) array[i] = "" + (n1 / n2);
                        array.RemoveAt(i-1);
                        array.RemoveAt(i);
                        swm = true;
                    }
                    i++;
                }

                if (!swm)
                {
                    i = 1;
                    bool sw = false;
                    while (i < array.Count - 1 && !sw)
                    { //Miramos si hay sumas y restas
                        String s = array[i];
                        if (s.Equals("+") || s.Equals("-"))
                        {
                            Double n1 = Convert.ToDouble(array[i - 1]);
                            Double n2 = Convert.ToDouble(array[i + 1]);
                            if (s.Equals("+")) array[i] = "" + (n1 + n2);
                            if (s.Equals("-")) array[i] = "" + (n1 - n2);
                            array.RemoveAt(i - 1);
                            array.RemoveAt(i);
                            sw = true;
                        }
                        i++;
                    }
                }
                return solveExpression(array); //Si aun no se han completado las operaciones vuelva hacer el proceso recursivamente
            }
        }

        private void solveExpressionB(object sender, RoutedEventArgs e)
        {
            List<String> components;
            String text;
            text = expression.Text;
            if (!text.Equals("")) {
                components = new List<String>();
                int i = 0;
                String unary = "";
                while (i < text.Length) { //Recorremos la expresion sacando las operaciones y los operandos
                    int j = i;
                    bool sw = false;
                    while (j < text.Length && !sw) {
                        String s = text.Substring(j, 1);
                        if (s.Equals("+") || s.Equals("*") || s.Equals("-") || s.Equals("/")) {
                            if (s.Equals("-") && text.Substring(i, text.Length - j).Equals("")) {
                                unary = s; //s = - si encuentra el simbolo unario
                            } else {
                                String se = unary + text.Substring(i, j - i);
                                if (!se.Equals("")) components.Add(se);
                                components.Add(s);
                                unary = "";
                            }
                            sw = true;
                        }
                        j++; //continuo leyendo
                    }
                    if (j >= text.Length) {
                        String s = unary + text.Substring(i);
                        if (!s.Equals(""))
                            components.Add(s); //Si termine de recorrer la expresion y no encontre una operacion, incluyo lo ultimo no reconocido como un operando
                    }
                    i = j;
                }
                Double r = solveExpression(components); //Calculo el resultado, si hay un error se captura y se muestra por pantalla
                result.Text = r.ToString();
            }
        }

    }
}