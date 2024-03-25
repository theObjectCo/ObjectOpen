using System.Windows.Input;
using ObjectOpen.MVVMExceptions.Localization;
using ObjectOpen.MVVMExceptions.Models;

namespace ObjectOpen.MVVMExceptions.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private double _weight = 50;
        private double _height = 150;
        private double _bmi;

        private BMICalculator _bmiCalculator = new BMICalculator(Units.Metric);

        public MainWindowViewModel()
        {
        }

        public double Weight { get => _weight; set { _weight = value; OnPropertyChanged(); } }
        public double Height { get => _height; set { _height = value; OnPropertyChanged(); } }
        public double BMI { get => _bmi; set { _bmi = value; OnPropertyChanged(); } }


        private RelayCommand? runCalculator;
        public ICommand RunCalculator => runCalculator ??= new RelayCommand(RunCalculatorMethod);
        private void RunCalculatorMethod(object? param)
        {
            //we know Weight and Height are correct because of the input validation rules applied in the View
            BMI = _bmiCalculator.Calculate(Weight, Height);
        }


    }
}
