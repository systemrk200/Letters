using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Linq;

namespace letters;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    // Обработчик клика по кнопке
    private void OnCalculateClick(object sender, RoutedEventArgs e)
    {
        // 1. Получаем текст из TextBox
        string inputText = InputTextBox.Text ?? "";

        if (string.IsNullOrWhiteSpace(inputText))
            return;

        // 2. Считаем количество вхождений каждой буквы
        var letterCounts = inputText
            .ToLower()                  // Приводим к нижнему регистру
            .Where(char.IsLetter)       // Оставляем только буквы (игнорируем цифры, пробелы и знаки)
            .GroupBy(c => c)            // Группируем по буквам
            .OrderBy(g => g.Key)        // Сортируем по алфавиту
            .ToDictionary(g => g.Key, g => g.Count());

        if (letterCounts.Count == 0)
            return;

        // 3. Подготавливаем данные для графика
        double[] values = letterCounts.Values.Select(v => (double)v).ToArray(); // Количество (высота столбцов)
        double[] positions = Enumerable.Range(0, letterCounts.Count).Select(i => (double)i).ToArray(); // Позиции столбцов (0, 1, 2...)
        string[] labels = letterCounts.Keys.Select(k => k.ToString()).ToArray(); // Подписи (буквы)

        // 4. Отрисовываем гистограмму на графике
        MyPlot.Plot.Clear(); // Очищаем прошлый график

        // Добавляем столбчатую диаграмму (Bar Chart)
        MyPlot.Plot.AddBar(values, positions);

        // Устанавливаем подписи к столбцам (буквы)
        MyPlot.Plot.XTicks(positions, labels);

        // Настройки внешнего вида графика
        MyPlot.Plot.Title("Частота встречаемости букв");
        MyPlot.Plot.YLabel("Количество");
        MyPlot.Plot.XLabel("Буквы");
        MyPlot.Plot.SetAxisLimits(yMin: 0); // Начало оси Y с нуля

        // Обновляем отображение
        MyPlot.Refresh();
    }
}