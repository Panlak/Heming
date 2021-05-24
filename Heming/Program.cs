using Pastel;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Hamming
{

   
    class Program 
    {
        static List<char> sequence;
        static int nSpaces = 0;

        static void Main(string[] args)
        {


            
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.InputEncoding = System.Text.Encoding.Unicode;

            Console.WriteLine("Хеммінг");

            string strSent = InputControl("Введіть слово (лише 0 та 1): ");
            Console.WriteLine($"Кодування Хеммінга вхідного слова:\n{Hammingcalculation(strSent)}");
            string strReceived = InputControl("Введіть отримане слово (включаючи код Хеммінга), вставивши максимум одну помилку: ");
            int ris = Comparison(strReceived);

            Console.WriteLine($"Неправильний біт знаходиться в позиції {ris}");
            Console.WriteLine($"Надісланий:\t\t\t\t" + $"{Print(ris, ls: sequence)}");
            Console.WriteLine($"Отримано:\t\t\t\t" + $"{Print(ris, s: strReceived)}");

            // Я виправляю помилку, передаючи рядок за посиланням
            string strRicevutoFixed = SettlementError(ris, strReceived);
            Console.WriteLine($"Реконструкція правильного слова:\t" + $"{Print(ris, s: strRicevutoFixed)}");
        }
        static string InputControl(string msg)
        {
            string strInput;
            bool isValid = true;
            do
            {
                Console.Write(msg);
                strInput = Console.ReadLine();

                // Я перевіряю введення
                for (int i = 0; i < strInput.Length; i++)
                {
                    if (strInput[i] == '0' || strInput[i] == '1')
                        isValid = true;
                    else
                    {
                        isValid = false;
                        Console.WriteLine("Приймаються неправильні введення лише 0 та 1".Pastel("#FF0000"));
                        break;
                    }
                }

            } while (!isValid);

            return strInput;
        }
        static string Hammingcalculation(string s)
        {
            Spacesinsertion(s);
            for (int i = 0; i < nSpaces; i++)
                EnterBitParity((int)Math.Pow(2, i));
            return PrintList(sequence);
        }
        static int Comparison(string ricevuto)
        {
            int retVal = -1;
             
            for (int i = 0; i < nSpaces; i++)
                if (!Paritycalculation((int)Math.Pow(2, i), ricevuto))
                    retVal += (int)Math.Pow(2, i);

            if (retVal != -1) retVal += 1;
            return retVal;
        }

        static string SettlementError(int pos, string s)
        {
            if (pos == -1) return s;

            pos -= 1; 
            char[] chared = s.ToCharArray();
            if (chared[pos] == '1')
                chared[pos] = '0';
            else
                chared[pos] = '1';

            return new string(chared);
        }

        static bool Paritycalculation(int start, string msg) 
        {
            int cont = 0;
            var msgLs = new List<char>(msg.ToCharArray());
            //Істинно, якщо кількість бітів є парною, інакше вона повертає false
            for (int i = start - 1; i < msgLs.Count && i < i + start; i += start * 2)
                for (int j = i; j < i + start && j < msgLs.Count; j++)
                    if (msgLs[j] == '1') cont++;

            return cont % 2 == 0;
        }


        static void Spacesinsertion(string s)
        {
            sequence = new List<char>(s.ToCharArray());
            int esp = 0;
            for (int i = 0; i < sequence.Count; i++)
                if (i == (int)Math.Pow(2, esp) - 1)
                {
                    sequence.Insert(i, '_');
                    nSpaces++;
                    esp++;
                }
        }


        static void EnterBitParity(int start)
        {
            int cont = 0;

            for (int i = start - 1; i < sequence.Count; i += start * 2)
                for (int j = i; j < i + start && j < sequence.Count; j++)
                    if (sequence[j] == '1')
                        cont++;

            Enterequality(cont);
        }
      
        static void Enterequality(int cont)
        {
            if (cont % 2 == 0)
                sequence[sequence.IndexOf('_')] = '0';
            else
                sequence[sequence.IndexOf('_')] = '1';
        }

        static string PrintList(List<char> ls) //Друк списку
        {
            string s = "";
            int cont = 0;
            for (int i = 0; i < ls.Count; i++)
            {
                if (i == (int)Math.Pow(2, cont) - 1)
                {
                    s += $"{ls[i]} ".Pastel("#00FF00");
                    cont++;
                }
                else
                    s += $"{ls[i]} ";
            }

            return s;
        }

        static string Print(int pos, string s = "", List<char> ls = null)
        {
            string toPrint = "";
            pos -= 1; 
            int cont = 0;
            if (ls != null)
            {
                for (int i = 0; i < ls.Count; i++)
                {
                    // біти парності
                    if (i == (int)Math.Pow(2, cont) - 1)
                    {
                        
                        if (i == pos)
                            toPrint += $"{ls[i]} ".Pastel("#FF0000");//червоний
                        else
                            toPrint += $"{ls[i]} ".Pastel("#24fc03");//контрольний розряд
                        cont++;
                    }
                    else if (i == pos)
                        toPrint += $"{ls[i]} ".Pastel("#FF0000");//червоний
                    else
                        toPrint += $"{ls[i]} ";//повідомлення
                }
            }
            else if (s != "")
            {
                for (int i = 0; i < s.Length; i++)
                {
                    //біти парності
                    if (i == (int)Math.Pow(2, cont) - 1)
                    {
                        // Якщо біт парності збігається з неправильним, тоді він забарвлюється в червоний колір
                        if (i == pos)
                            toPrint += $"{s[i]} ".Pastel("#FF0000");
                        else
                            toPrint += $"{s[i]} ".Pastel("#24fc03");
                        cont++;
                    }
                    else if (i == pos)
                        toPrint += $"{s[i]} ".Pastel("#FF0000");
                    else
                        toPrint += $"{s[i]} ";
                }
            }

            return toPrint;
        }
    }
}