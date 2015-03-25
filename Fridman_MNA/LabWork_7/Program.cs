// Интерполирование функций естественными кубическими сплайнами

#region

using System;
using System.Linq;
using System.Text;

#endregion

namespace LabWork_7
{
    public static class Program
    {
        public static void Main()
        {
            Func<double, double> func = (x) => Math.Exp(-x);

            var xValues = Enumerable.Range(0, 500).Select(v => v/(double) 100).ToArray();
            var yValues = xValues.Select(v => func(v)).ToArray();
            var spline = new CubicSpline();
            spline.BuildSpline(xValues, yValues, 500);
            Console.WriteLine(spline.Interpolate(2.1, true));
            Console.WriteLine(func(2.1));
        }
    }

    internal class CubicSpline
    {
        private SplineTuple[] _splines; // Сплайн

        // Структура, описывающая сплайн на каждом сегменте сетки
        private struct SplineTuple
        {
            public double A, B, C, D, X;
        }

        // Построение сплайна
        // x - узлы сетки, должны быть упорядочены по возрастанию, кратные узлы запрещены
        // y - значения функции в узлах сетки
        // n - количество узлов сетки
        public void BuildSpline(double[] x, double[] y, int n)
        {
            // Инициализация массива сплайнов
            _splines = new SplineTuple[n];
            for (var i = 0; i < n; ++i)
            {
                _splines[i].X = x[i];
                _splines[i].A = y[i];
            }
            _splines[0].C = _splines[n - 1].C = 0.0;

            // Решение СЛАУ относительно коэффициентов сплайнов c[i] методом прогонки для трехдиагональных матриц
            // Вычисление прогоночных коэффициентов - прямой ход метода прогонки
            var alpha = new double[n - 1];
            var beta = new double[n - 1];
            alpha[0] = beta[0] = 0.0;
            for (var i = 1; i < n - 1; ++i)
            {
                var hi = x[i] - x[i - 1];
                var hi1 = x[i + 1] - x[i];
                var a = hi;
                var c = 2.0*(hi + hi1);
                var b = hi1;
                var f = 6.0*((y[i + 1] - y[i])/hi1 - (y[i] - y[i - 1])/hi);
                var z = (a*alpha[i - 1] + c);
                alpha[i] = -b/z;
                beta[i] = (f - a*beta[i - 1])/z;
            }

            // Нахождение решения - обратный ход метода прогонки
            for (var i = n - 2; i > 0; --i)
            {
                _splines[i].C = alpha[i]*_splines[i + 1].C + beta[i];
            }

            // По известным коэффициентам c[i] находим значения b[i] и d[i]
            for (var i = n - 1; i > 0; --i)
            {
                var hi = x[i] - x[i - 1];
                _splines[i].D = (_splines[i].C - _splines[i - 1].C)/hi;
                _splines[i].B = hi*(2.0*_splines[i].C + _splines[i - 1].C)/6.0 + (y[i] - y[i - 1])/hi;
            }
        }

        // Вычисление значения интерполированной функции в произвольной точке
        public double Interpolate(double x, bool trace = false)
        {
            if (_splines == null)
            {
                return double.NaN; // Если сплайны ещё не построены - возвращаем NaN
            }

            var n = _splines.Length;
            SplineTuple s;

            if (x <= _splines[0].X) // Если x меньше точки сетки x[0] - пользуемся первым эл-тов массива
            {
                s = _splines[0];
            }
            else if (x >= _splines[n - 1].X) // Если x больше точки сетки x[n - 1] - пользуемся последним эл-том массива
            {
                s = _splines[n - 1];
            }
            else // Иначе x лежит между граничными точками сетки - производим бинарный поиск нужного эл-та массива
            {
                var i = 0;
                var j = n - 1;
                while (i + 1 < j)
                {
                    var k = i + (j - i)/2;
                    if (x <= _splines[k].X)
                    {
                        j = k;
                    }
                    else
                    {
                        i = k;
                    }
                }
                s = _splines[j];
            }

            var dx = x - s.X;
            // Вычисляем значение сплайна в заданной точке по схеме Горнера (в принципе, "умный" компилятор применил бы схему Горнера сам, но ведь не все так умны, как кажутся)
            if (trace)
            {
                var type = typeof (SplineTuple);
                var fields = type.GetFields();
                var result = new StringBuilder();
                object spline = s;
                for (var pow = 3; pow >= 0; pow--)
                {
                    switch (pow)
                    {
                        case 3:
                            result.Append((double) fields[0].GetValue(spline) >= 0 ? "" : "- ");
                            result.AppendFormat("{0:F3}*x^3 ", (double) fields[0].GetValue(spline));
                            break;
                        case 0:
                            result.Append((double) fields[3].GetValue(spline) >= 0 ? "" : "- ");
                            result.AppendFormat("{0:F3}", (double) fields[0].GetValue(spline));
                            break;
                        default:
                            result.Append((double) fields[3 - pow].GetValue(spline) >= 0 ? "" : "- ");
                            result.AppendFormat("{0:F3}*x^{1} ", Math.Abs((double) fields[3 - pow].GetValue(spline)),
                                pow);
                            break;
                    }
                }

                Console.WriteLine(result);
                //Console.WriteLine(
                //    (s.A > 0 ? "" : "- ") + "{0:F4}*x^3 " + (s.B > 0 ? "+" : "-") + " {1:F4}*x^2 " +
                //    (s.C > 0 ? "+" : "-") + " {2:F4}*x " +
                //    (s.D > 0 ? "+" : "-") + " {3:F4}", Math.Abs(s.A), Math.Abs(s.B), Math.Abs(s.C), Math.Abs(s.D));
            }
            return s.A + (s.B + (s.C/2.0 + s.D*dx/6.0)*dx)*dx;
        }
    }
}