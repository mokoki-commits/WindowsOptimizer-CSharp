using System.Windows;

using System.Windows.Controls;

namespace WindowsOptimizer.Gui.Views
{
    public partial class HelpWindow : Window
    {
        public HelpWindow()
        {
            InitializeComponent();
            BuildHelpContent();
        }

        private void BuildHelpContent()
        {
            var panel = (StackPanel)HelpContent;

            panel.Children.Add(new TextBlock { Text = "Оптимизация Windows - Детальное описание", FontSize = 16, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 0, 0, 10) });
            
            AddSection(panel, "1. Режим Safe (Безопасный)", "Изменяет только визуальные эффекты (Best Performance), план электропитания (High Performance) и активирует базовые твики реестра для ускорения файловой системы. Ничего не отключает из служб. Идеален для повседневных задач.");
            AddSection(panel, "2. Режим Balanced (Сбалансированный)", "Все изменения из Safe, плюс: отключает SysMain (Superfetch), Windows Search (индексация), DiagTrack (телеметрия), BITS (фоновые загрузки), Print Spooler (если нет принтера), очищает %TEMP% и кэши, настраивает автозагрузку.");
            AddSection(panel, "3. Режим Strict (Строгий)", "Все изменения из Balanced, плюс: агрессивные твики реестра (DisablePagingExecutive, IoPageLockLimit), отключение телеметрии (AllowTelemetry=0), отключение Superfetch/Prefetch, отключение UAC (в реестре), отключение Windows Defender Real-time protection (для игр), отключение гибернации и спящего режима, полная очистка EventLog.");
            AddSection(panel, "4. Флажок: Registry Tweaks", "Применяет твики реестра: Win32PrioritySeparation (0x26 для игр), SystemResponsiveness (10), DisableLastAccessTime, LargeSystemCache, DisablePagingExecutive (1), IOPageLockLimit, отключение создания 8.3-имен файлов (NtfsDisable8dot3NameCreation), отключение телеметрии и Bing в реестре.");
            AddSection(panel, "5. Флажок: Services Manager", "Останавливает и отключает службы: DiagTrack (Connected User Experiences Telemetry), WSearch (Windows Search), SysMain (Superfetch), BITS (Background Intelligent Transfer Service), Print Spooler (если отмечено), Xbox services, Windows Update (в Strict). Сохраняет предыдущие Startup Type.");
            AddSection(panel, "6. Флажок: Visual Effects", "Меняет VisualFXSetting в реестре на 2 (Best Performance). Отключает анимацию окон, тени под курсором, сглаживание шрифтов, прозрачность. Экономит CPU/GPU на рендеринге UI.");
            AddSection(panel, "7. Флажок: Power Plan", "Активирует схему Ultimate Performance (или High Performance, если недоступна) через powercfg. Отключает гибернацию (powercfg -h off). Отключает USB selective suspend и дисковый таймер. Не выключает монитор.");
            AddSection(panel, "8. Флажок: Cleanup", "Очищает %TEMP% и C:\\Windows\\Temp, удаляет Prefetch файлы, очищает кэш Windows Update (C:\\Windows\\SoftwareDistribution\\Download), очищает корзину, удаляет старые логи в C:\\Windows\\Logs, очищает DirectX Shader Cache.");
            AddSection(panel, "9. Флажок: Startup Manager", "Анализирует HKLM\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run и HKCU\\...\\Run. Удаляет или отключает (переименовывает в .disabled) записи, помеченные как ненужные. Сканирует папки автозагрузки. В Balanced/Strict отключает задачи планировщика, не связанные с Windows.");
            AddSection(panel, "10. Безопасность и PIN", "Перед запуском проверяется PIN (по умолчанию 111111). Без правильного PIN оптимизация не запускается. Создаётся бэкап реестра (экспорт .reg файлов для HKLM\\SOFTWARE, HKLM\\SYSTEM\\CurrentControlSet\\Services, HKCU\\Software) в папку Backups/ИмяПК_ОС_Дата_Время/ перед любыми изменениями. Также создаётся лог-файл.");

            var closeBtn = new Button { Content = "Понятно", Width = 100, HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0, 10, 0, 0) };
            closeBtn.Click += Close_Click;
            panel.Children.Add(closeBtn);
        }

        private void AddSection(StackPanel panel, string title, string content)
        {
            panel.Children.Add(new TextBlock { Text = title, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 5, 0, 0) });
            panel.Children.Add(new TextBlock { Text = content, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 2, 0, 5) });
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}