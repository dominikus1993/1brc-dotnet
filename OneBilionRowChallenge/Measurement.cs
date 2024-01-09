using System.Runtime.CompilerServices;

namespace OneBilionRowChallenge;

public sealed class Measurement
{
    public string City { get; set; }
    public double Temperature { get; set; }
}

public sealed class MeasurementTemperature
{
    public MeasurementTemperature(double temp)
    {
        Min = temp;
        Max = temp;
        _numberOfMeasurements = 1;
        _sumofMeasurements = temp;
    }

    public double Min { get; private set; }
    public double Max { get; private set; }
    public double Mean => _sumofMeasurements / _numberOfMeasurements;
    
    private uint _numberOfMeasurements;
    private double _sumofMeasurements;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Check(double measurement)
    {
        _numberOfMeasurements++;
        _sumofMeasurements += measurement;
        
        if (measurement < Min)
        {
            Min = measurement;
            return;
        }

        if (measurement > Max)
        {
            Max = measurement;
            return;
        }
    }
}