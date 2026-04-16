using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaCalc.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
   [ObservableProperty]
    private string? _ResultTexto = "0";
    [ObservableProperty]
    private string? _historial = string.Empty;
    private double _primerNumero;
    private string _operacionActual = string.Empty;
    private bool _limpiarPantalla;

    [RelayCommand]
    public void CmdIngresarNumero(String numero)
    {
        if(ResultTexto == "0" || _limpiarPantalla)
        {
            ResultTexto = numero;
            _limpiarPantalla = false;
        }
        else
        {
            ResultTexto += numero;
        }
    }
    [RelayCommand]
    public void CmdBorrarNumero(String numero)
    {
        if(string.IsNullOrEmpty(ResultTexto) || ResultTexto == "0") return;
        if(ResultTexto.Length == 1)
            ResultTexto = "0";
        else
            //El método Substring devuelve una instancia de la misma cadena a partir de una posicion inicial y una final recibida por parámetros
            ResultTexto = ResultTexto.Substring(0, ResultTexto.Length - 1);
    }
    [RelayCommand]
    private void CmdEstablecerOperacion(string operacion)
    {
        // Guardamos el número que está en pantalla convertido a double
        if (double.TryParse(ResultTexto, out double valor))
        {
            _primerNumero = valor;
            _operacionActual = operacion;
            Historial = $"{valor} {operacion} ";
            _limpiarPantalla = true; // El próximo número que toquemos debe limpiar la pantalla
        }
    }
    [RelayCommand]
    private void CmdLimpiar()
    {
        ResultTexto = "0";
        _primerNumero = 0;
        _operacionActual = string.Empty;
        Historial = string.Empty;
    }
    [RelayCommand]
    public void CmdCalcularResultado()
    {
        if(double.TryParse(ResultTexto, out double segundoNumero))
        {
            double resultadoFinal = _operacionActual switch
            {
                "+" => _primerNumero + segundoNumero,
                "-" => _primerNumero - segundoNumero,
                "X" => _primerNumero * segundoNumero,
                "/" => segundoNumero != 0 ? _primerNumero / segundoNumero : 0,
                _ => segundoNumero
            };
            ResultTexto = resultadoFinal.ToString();
            Historial += $"{segundoNumero} = {ResultTexto}";
            _limpiarPantalla = true;
        }
    }
}
