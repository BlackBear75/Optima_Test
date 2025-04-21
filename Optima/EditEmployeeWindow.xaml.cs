using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Optima.Entity.Employee;
using Optima.Helper;
using Optima.Service;

namespace Optima
{
    public partial class EditEmployeeWindow : Window
    {
        private Employee _employee;

        private readonly IEmployeeService _employeeService;
        public EditEmployeeWindow(Employee employee)
        {
            _employeeService = App.ServiceProvider.GetRequiredService<IEmployeeService>();


            InitializeComponent();
            _employee = employee;

            TextBoxFirstName.Text = _employee.FirstName;
            TextBoxMiddleName.Text = _employee.MiddleName;
            TextBoxLastName.Text = _employee.LastName;
            TextBoxSalary.Text = _employee.Salary.ToString();
            ComboBoxPosition.ItemsSource = Enum.GetValues(typeof(EmployeePosition));
            ComboBoxPosition.SelectedItem = _employee.Position;
            CheckBoxLiberated.IsChecked = _employee.Liberated;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            string errorMessage;

            if (!EmployeeValidationHelper.ValidateName(TextBoxFirstName.Text, out errorMessage))
            {
                MessageBox.Show(errorMessage, "Validation First Name Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return; 
            }

            if (!EmployeeValidationHelper.ValidateName(TextBoxLastName.Text, out errorMessage))
            {
                MessageBox.Show(errorMessage, "Validation Last Name Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!EmployeeValidationHelper.ValidateName(TextBoxMiddleName.Text, out errorMessage))
            {
                MessageBox.Show(errorMessage, "Validation Middle Name Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!EmployeeValidationHelper.ValidateSalary(TextBoxSalary.Text, out errorMessage))
            {
                MessageBox.Show(errorMessage, "Validation Salary Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

         
            _employee.FirstName = TextBoxFirstName.Text;
            _employee.MiddleName = TextBoxMiddleName.Text;
            _employee.LastName = TextBoxLastName.Text;
            _employee.Position = (EmployeePosition)ComboBoxPosition.SelectedItem;
            _employee.Salary = double.TryParse(TextBoxSalary.Text, out var salary) ? salary : 0;
            _employee.Liberated = CheckBoxLiberated.IsChecked == true;

            _employeeService.UpdateOneAsync(_employee);

            DialogResult = true;
            Close();
        }

    }
}
