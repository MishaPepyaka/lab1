using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            // Пути к фалам
            string inputPath = @"C:\Users\Svetlana\Documents\cs\input.txt";
            string outputPath = @"C:\Users\Svetlana\Documents\cs\output.txt";

            // Строка с входными значениями

            string inputString;

            // Считываем файл

            using (StreamReader sr = new StreamReader(inputPath))
            {
                inputString = sr.ReadToEnd();
            }

            // Переработка данных в удобный вид

            string[] splitedInput = inputString.Split('\n');

            int particleTypes = int.Parse(splitedInput[0]);
            int[] particalNumbers = splitedInput[1].Split(' ').Select(x => int.Parse(x)).ToArray();

            // Создаем правила анигиляции частиц

            splitedInput[0] = splitedInput[1] = null;
            splitedInput = splitedInput.Where(x => x != null).ToArray();

            int[,] rulesBook = new int[particleTypes, particleTypes];

            for (int counterX = 0; counterX < particleTypes; counterX++)
            {
                int[] tmp = new int[particleTypes];
                tmp = splitedInput[counterX].Split(' ').Select(x => int.Parse(x)).ToArray();

                for (int counterY = 0; counterY < particleTypes; counterY++)
                {
                    rulesBook[counterX, counterY] = tmp[counterY];
                }
            }

            List<int[]> chanceCollection = new List<int[]>();
            chanceCollection.Add(particalNumbers);
          
            chanceCollection = recursiveCollider(chanceCollection, rulesBook, particleTypes);

            // Записываем в файл

            using (StreamWriter sw = new StreamWriter(outputPath, false, System.Text.Encoding.Default))
            {
                sw.WriteLine(particleTypes);
                foreach (var itemArray in chanceCollection)
                {
                    foreach (var itemPart in itemArray)
                    {
                        sw.Write(itemPart);
                    }
                    sw.WriteLine();
                }
            }

            

        }

        // Комбинируем варианты
        // Принимаем набор вариантов наборов частиц и правила анигиляции.

        static public List<int[]> recursiveCollider(List<int[]> inputList, int[,] rulesBook, int particleTypes)
        {
            List<int[]> tmpCollection = new List<int[]>();
            
            bool processEnded = false;
            processEnded = true;

            // Ходим внутри листа по массивам
            foreach (int[] item in inputList)
            {
                for (var arrayCounter = 0; arrayCounter < particleTypes; arrayCounter++)
                    
                // Для каждой частицы в массиве...
                {
                    if (item[arrayCounter] != 0) // ...Которая существует...
                    {
                        for (var arrayAnCounter = 0; arrayAnCounter < particleTypes; arrayAnCounter++) // ... сталкиваем её с каждой другой частицей
                        {
                            // Не сталкивается ли частица сама с собой? Если  да, то есть ли еще такого же типа частицы? Есть ли куда сталкиваться?
                            if ((arrayAnCounter == arrayCounter) && (item[arrayCounter] == 1) || (item[arrayAnCounter] == 0)) {
                            }
                            else
                            {
                                // Сталкиваем частицы
                                if (rulesBook[arrayCounter, arrayAnCounter] == 0) { // Не ломается! 
                                }
                                else // Ломается!
                                {
                                
                                    int[] tmpArray = new int[particleTypes];

                                    for (var i=0; i < particleTypes; i++)
                                    {
                                        tmpArray[i] = item[i];
                                    }
                                    
                                    tmpArray[arrayAnCounter] --;
                                    processEnded = false;

                                    // Записываем результат, проверяя на повторы

                                    bool fl = true;

                                    foreach (var y in tmpCollection) { if (y.SequenceEqual(tmpArray)) { fl = false; } }
                                                                     
                                    if (fl) { tmpCollection.Add(tmpArray); }
                                }
                            }
                            }
                        }

                       
                }
            }
            //          };
            if (!processEnded) { tmpCollection = recursiveCollider(tmpCollection, rulesBook, particleTypes); } else return inputList;
            return tmpCollection;
        }
    }
}