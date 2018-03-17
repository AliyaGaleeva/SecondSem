using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Polinom
{
    class Data
    {
        public int coef;
        public int deg1;
        public int deg2;
        public int deg3;
    }

    class Element

    {

        public Data Info { get; set; }

        public Element Next { get; set; }

    }

    class List<Data>

    { 
        public Element First { get; set; }

        public void Add(Polinom.Data d)
        {
            if (First == null)
            {
                First = new Element() { Info = d };    
            }
            else
            {
                Element last = First;
                while (last.Next != null)
                {
                    last = last.Next;
                }
                last.Next = new Element() { Info = d };
            }

        }

    }


    class Polinom3
    {
        public List<Data> list = new List<Data>();

        public void DoPolinom3(string filename)
        {
            FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(file);
            
            while (!reader.EndOfStream)
            {
                string[] array = reader.ReadLine().Split(' ');
                var d = new Data();
                d.coef = int.Parse(array[0]);
                d.deg1 = int.Parse(array[1]);
                d.deg2 = int.Parse(array[2]);
                d.deg3 = int.Parse(array[3]);
                list.Add(d);
            }
            reader.Close();
        }

        public override string ToString()
        {
            string str = "";
            var elem = list.First;
            while (elem != null)
            {
                if (elem.Info.coef > 0) str = str + "+";
                str = str + elem.Info.coef + "x^" + elem.Info.deg1 + "y^" + elem.Info.deg2 + "z^" + elem.Info.deg3;
                elem = elem.Next;
            }
            return str;

        }

        public void Insert(int c, int d1, int d2, int d3)
        {
            var elem = list.First;
            var d = new Element();
            var f = new Data();
            var prev = new Element();
            f.coef = c;
            f.deg1 = d1;
            f.deg2 = d2;
            f.deg3 = d3;
            d.Info = f;
            while (Compare(elem, d)>0 && elem != null) 
            {
                prev = elem;
                elem = elem.Next;
            }
            if(Compare(elem, d)!=0)
            {
                d.Next = elem;
                prev.Next = d;
            }

        }

        public int Compare(Element e1,Element e2)
        {
            if (e1.Info.deg1 > e2.Info.deg1) return 1;
            if (e1.Info.deg1 < e2.Info.deg1) return -1;
            if (e1.Info.deg2 > e2.Info.deg2) return 1;
            if (e1.Info.deg2 < e2.Info.deg2) return -1;
            if (e1.Info.deg3 > e2.Info.deg3) return 1;
            if (e1.Info.deg3 < e2.Info.deg3) return -1;
            if (e1.Info.coef > e2.Info.coef) return 1;
            if (e1.Info.coef < e2.Info.coef) return -1;
            return 0;

        }

        public void AddTwoPolinoms(Polinom3 p)
        {
            var elem1 = list.First;
            var elem2 = p.list.First;
            var prev = new Element();
            if (Compare(elem1, elem2) > 0)
            {

                prev = elem1;
                elem1 = elem1.Next;

            }
            else
            {
                prev = elem2;
                elem2 = elem2.Next;
            }
            list.First = prev;
          
            while (elem1 != null || elem2 != null)
            {
                if (elem1 != null && elem2 != null)
                {
                    if (Compare(elem1, elem2) > 0)
                    {
                        prev.Next = elem1;
                        prev = prev.Next;
                        elem1 = elem1.Next;
                    }
                    else
                    {
                        prev.Next = elem2;
                        prev = prev.Next;
                        elem2 = elem2.Next;
                    }
                }
                else
                {
                    if (elem1 != null)
                    {
                        prev.Next = elem1;
                        prev = prev.Next;
                        elem1 = elem1.Next;
                    }
                    else
                    {
                        prev.Next = elem2;
                        prev = prev.Next;
                        elem2 = elem2.Next;
                    }
                }

            }
        }

        public void Derivate(int i)
        {
            var elem = list.First;
           
            while (elem != null)
            {


                if (i == 1)
                {
                    elem.Info.coef *= elem.Info.deg1;
                    elem.Info.deg1--;
                }
                if (i == 2)
                {
                    elem.Info.coef *= elem.Info.deg2;
                    elem.Info.deg2--;
                }
                if (i == 3)
                {
                    elem.Info.coef *= elem.Info.deg3;
                    elem.Info.deg3--;
                }

                elem = elem.Next;

            }
        }

        public void Delete(int d1, int d2, int d3)
        {
            var elem = list.First;
            var prev = list.First;
            while (elem != null)
            {
                if (elem.Info.deg1 == d1 && elem.Info.deg2 == d2 && elem.Info.deg3 == d3)
                {
                    prev.Next = elem.Next;
                    break;
                }
                prev = elem;
                elem = elem.Next;
            }
        }

        public int Value(int x, int y, int z)
        {
            double s = 0;
            var elem = list.First;
            while (elem != null)
            {
                s = s + elem.Info.coef*Math.Pow(x,elem.Info.deg1)*Math.Pow(y,elem.Info.deg2)*Math.Pow(z,elem.Info.deg3);
                elem = elem.Next;
            }
            return (int)s;
        }

        public int[] MinCoef()
        {
            var min = list.First;
            var elem = list.First;
            while(elem != null)
            {
                if (elem.Info.coef < min.Info.coef) min = elem;
                elem = elem.Next;
            }
            return new int[]{ min.Info.deg1, min.Info.deg2, min.Info.deg3 };
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            var pol = new Polinom3();
            pol.DoPolinom3("text.txt");
            Console.Write("pol: ");
            Console.WriteLine(pol.ToString());
            pol.Insert(6, 4, 6, 1);
            Console.Write("Insert: ");
            Console.WriteLine(pol.ToString());
            Console.Write("Delete: ");
            pol.Delete(4, 7, 2);
            Console.WriteLine(pol.ToString());
            Console.Write("Value: ");
            Console.WriteLine(pol.Value(2, 1, 1));
            Console.Write("MinCoef: ");
            var array = pol.MinCoef();
            for (int i = 0; i < 3; i++)
                Console.Write($"{array[i]} ");
            Console.WriteLine();
            Console.Write("Derivate: ");
            pol.Derivate(2);
            Console.WriteLine(pol.ToString());
            var pol2 = new Polinom3();
            pol2.DoPolinom3("text1.txt");
            Console.Write("pol2: ");
            Console.WriteLine(pol2.ToString());
            Console.Write("AddTwoPolinoms: ");
            pol.AddTwoPolinoms(pol2);
            Console.WriteLine(pol.ToString());

        }
    }
}
