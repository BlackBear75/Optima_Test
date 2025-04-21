using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using Newtonsoft.Json;
using Optima.Entity.Employee;
using Optima.Service;
using System.Collections.ObjectModel;
using System.Windows;
using System.IO;

using System.Windows.Controls;
namespace Optima;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
   
        private readonly IEmployeeService _employeeService;
        public ObservableCollection<Employee> Employees { get; set; }

        public MainWindow()
        {
            InitializeComponent();

             _employeeService = App.ServiceProvider.GetRequiredService<IEmployeeService>();


              LoadEmployees();
        }

        private async void LoadEmployees()
        {
            var list = await _employeeService.GetAllAsync();

        
        
            Employees = new ObservableCollection<Employee>(list);
            DataGridEmployees.ItemsSource = Employees;
            UpdateStatistics();
    }

    private async void ButtonDeleteEmployee_Click(object sender, RoutedEventArgs e)
    {
        var selectedEmployee = (Employee)DataGridEmployees.SelectedItem;
        if (selectedEmployee != null)
        {
            await _employeeService.DeleteAsync(selectedEmployee.Id);

            LoadEmployees();
        }
    }

    private void ButtonAddEmployee_Click(object sender, RoutedEventArgs e)
    {
        var addWindow = new AddEmployeeWindow(_employeeService);
        bool? result = addWindow.ShowDialog();

        if (result == true)
        {
            LoadEmployees();
        }
    }

    private void ButtonFindEmployee_Click(object sender, RoutedEventArgs e)
    {
        var searchQuery = TextBoxSearch.Text.ToLower();


        var filteredEmployees = Employees.Where(emp =>
            emp.GetFullName().ToLower().Contains(searchQuery)).ToList();



        DataGridEmployees.ItemsSource = new ObservableCollection<Employee>(filteredEmployees);
    }
    private void ButtonEditEmployee_Click(object sender, RoutedEventArgs e)
    {
        if (DataGridEmployees.SelectedItem is Employee selectedEmployee)
        {
            var editWindow = new EditEmployeeWindow(selectedEmployee);
            if (editWindow.ShowDialog() == true)
            {
                DataGridEmployees.Items.Refresh();
            }
        }
        else
        {
            MessageBox.Show("Select an employee to edit", "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private void TextBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
    {
        TextBoxPlaceholder.Visibility = string.IsNullOrWhiteSpace(TextBoxSearch.Text)
            ? Visibility.Visible
            : Visibility.Collapsed;
    }
    private void UpdateStatistics()
    {
        TextBlockEmployeeCount.Text = Employees.Count.ToString();

        if (Employees.Count > 0)
        {
            double averageSalary = Employees.Average(e => e.Salary);
            TextBlockAverageSalary.Text = $"{averageSalary:C}";
        }
        else
        {
            TextBlockAverageSalary.Text = "0";
        }
    }
    private async void ButtonExport_Click(object sender, RoutedEventArgs e)
    {
        var saveFileDialog = new SaveFileDialog
        {
            Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*",
            FileName = "employees.json"
        };

        if (saveFileDialog.ShowDialog() == true)
        {
            var json = JsonConvert.SerializeObject(Employees, Formatting.Indented);
            await File.WriteAllTextAsync(saveFileDialog.FileName, json);
            MessageBox.Show("Data exported successfully!", "Export", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    private async void ButtonImport_Click(object sender, RoutedEventArgs e)
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            try
            {
                string json = await File.ReadAllTextAsync(openFileDialog.FileName);
                var importedEmployees = JsonConvert.DeserializeObject<List<Employee>>(json);

                if (importedEmployees != null)
                {
                    await ImportEmployeesAsync(importedEmployees);
                    MessageBox.Show("Імпорт успішний!", "Імпорт", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка імпорту: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private async Task ImportEmployeesAsync(List<Employee> importedEmployees)
    {
        foreach (var imported in importedEmployees)
        {
            var existing = await _employeeService.FindByIdAsync(imported.Id);

            if (existing != null)
            {
                _employeeService.Detach(existing); // реалізуй метод Detach у своєму сервісі
                await _employeeService.UpdateOneAsync(imported);
            }

            else
            {
                await _employeeService.InsertOneAsync(imported);
            }
        }

        LoadEmployees();
    }

}