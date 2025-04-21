using System;
using System.Windows;
using Optima.Entity.Employee;
using Optima.Service;
using System.Linq;
using Optima.Helper;

namespace Optima
{
    public partial class AddEmployeeWindow : Window
    {
        private readonly IEmployeeService _employeeService;

        public AddEmployeeWindow(IEmployeeService employeeService)
        {
            InitializeComponent();
            _employeeService = employeeService;

            var positions = Enum.GetValues(typeof(EmployeePosition)).Cast<EmployeePosition>().ToList();
            PositionComboBox.ItemsSource = positions;
        }

        private async void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            var firstName = FirstNameTextBox.Text;
            var middleName = MiddleNameTextBox.Text;
            var lastName = LastNameTextBox.Text;
            var salary = double.TryParse(SalaryTextBox.Text, out var parsedSalary) ? parsedSalary : 0;
            var selectedPosition = (EmployeePosition)PositionComboBox.SelectedItem;

            string errorMessage;

            if (!EmployeeValidationHelper.ValidateName(firstName, out errorMessage))
            {
                MessageBox.Show(errorMessage, "Validation First Name Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!EmployeeValidationHelper.ValidateName(lastName, out errorMessage))
            {
                MessageBox.Show(errorMessage, "Validation Last Name Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!EmployeeValidationHelper.ValidateName(middleName, out errorMessage))
            {
                MessageBox.Show(errorMessage, "Validation Middle Name Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!EmployeeValidationHelper.ValidateSalary(SalaryTextBox.Text, out errorMessage))
            {
                MessageBox.Show(errorMessage, "Validation Salary Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

           

            var newEmployee = new Employee
            {
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName,
                Salary = salary,
                Position = selectedPosition
            };

            await _employeeService.InsertOneAsync(newEmployee);

            this.DialogResult = true;
            this.Close();
        }
    }
}
