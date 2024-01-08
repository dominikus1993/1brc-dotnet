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

    public void Check(Measurement measurement)
    {
        _numberOfMeasurements++;
        _sumofMeasurements += measurement.Temperature;
        if (measurement.Temperature < Min)
        {
            Min = measurement.Temperature;
            return;
        }

        if (measurement.Temperature > Max)
        {
            Max = measurement.Temperature;
            return;
        }
    }
}