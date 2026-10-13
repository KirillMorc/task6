using System;

class Program
{
    static void Main()
    {
        const int rows = 7;
        const int cols = 7;

        char[,] islandMap = new char[rows, cols];
        int[,] goldMap = new int[rows, cols];
        Random random = new Random();

        // Заполняем карту морем и задаём случайную стоимость находок.
        for (int i = 0; i < islandMap.GetLength(0); i++)
        {
            for (int j = 0; j < islandMap.GetLength(1); j++)
            {
                islandMap[i, j] = '~';
                goldMap[i, j] = random.Next(0, 101);
            }
        }

        // Центральный квадрат 3x3 — суша.
        for (int i = 2; i <= 4; i++)
        {
            for (int j = 2; j <= 4; j++)
            {
                islandMap[i, j] = 'L';
            }
        }

        // Главная диагональ — морские рифы.
        for (int i = 0; i < islandMap.GetLength(0); i++)
        {
            islandMap[i, i] = 'R';
        }

        // Размещаем клад на суше, вне главной диагонали.
        int treasureRow = 2;
        int treasureCol = 3;
        islandMap[treasureRow, treasureCol] = 'X';
        goldMap[treasureRow, treasureCol] = 1000;

        // Размещаем пиратский фрегат в море.
        islandMap[0, 3] = 'F';

        // Выводим карту без цветового оформления.
        Console.WriteLine("Карта архипелага:");
        for (int i = 0; i < islandMap.GetLength(0); i++)
        {
            for (int j = 0; j < islandMap.GetLength(1); j++)
            {
                Console.Write($"{islandMap[i, j],3}");
            }

            Console.WriteLine();
        }

        // Подсчитываем клетки суши, включая клетку с кладом.
        int landCount = 0;
        for (int i = 0; i < islandMap.GetLength(0); i++)
        {
            for (int j = 0; j < islandMap.GetLength(1); j++)
            {
                if (islandMap[i, j] == 'L' || islandMap[i, j] == 'X')
                {
                    landCount++;
                }
            }
        }

        Console.WriteLine($"\nКоличество клеток суши: {landCount}");

        // Находим максимальную ценность среди морских клеток.
        int maxSeaGold = -1;
        int maxSeaRow = -1;
        int maxSeaCol = -1;

        for (int i = 0; i < goldMap.GetLength(0); i++)
        {
            for (int j = 0; j < goldMap.GetLength(1); j++)
            {
                if (islandMap[i, j] == '~' && goldMap[i, j] > maxSeaGold)
                {
                    maxSeaGold = goldMap[i, j];
                    maxSeaRow = i;
                    maxSeaCol = j;
                }
            }
        }

        Console.WriteLine(
            $"Максимальная ценность среди морских клеток: {maxSeaGold} пиастров " +
            $"(строка {maxSeaRow + 1}, столбец {maxSeaCol + 1})");

        // Считаем золото по периметру центрального острова.
        int islandBorderGold = 0;
        for (int i = 2; i <= 4; i++)
        {
            for (int j = 2; j <= 4; j++)
            {
                if (i == 2 || i == 4 || j == 2 || j == 4)
                {
                    islandBorderGold += goldMap[i, j];
                }
            }
        }

        Console.WriteLine(
            $"Сумма золота по периметру острова: {islandBorderGold} пиастров");

        // Игрок вводит координаты раскопок, пока не найдёт клад.
        while (true)
        {
            Console.Write("\nВведите строку для раскопок (1–7): ");
            bool rowParsed = int.TryParse(Console.ReadLine(), out int inputRow);

            Console.Write("Введите столбец для раскопок (1–7): ");
            bool colParsed = int.TryParse(Console.ReadLine(), out int inputCol);

            // Проверяем ввод и границы массива.
            if (!rowParsed || !colParsed ||
                inputRow < 1 || inputRow > islandMap.GetLength(0) ||
                inputCol < 1 || inputCol > islandMap.GetLength(1))
            {
                Console.WriteLine("Некорректные координаты. Введите числа от 1 до 7.");
                continue;
            }

            // Преобразуем координаты в индексы массива от 0 до 6.
            int row = inputRow - 1;
            int col = inputCol - 1;

            // После каждого хода показываем стоимость находки в выбранной клетке.
            Console.WriteLine(
                $"Стоимость находки в этой клетке: {goldMap[row, col]} пиастров.");

            if (islandMap[row, col] == 'X')
            {
                Console.WriteLine("Победа! Вы нашли клад!");
                break;
            }
        }
    }
}