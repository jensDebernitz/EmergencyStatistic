using ExcelTabellenAuswerung.DataBase;
using ExcelTabellenAuswerung.Models;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Globalization;


namespace ExcelTabellenAuswerung.ViewModels.Pages;

public partial class DashboardViewModel : ObservableObject
{
    [ObservableProperty] private int _counter = 0;

    [ObservableProperty] private ISeries[]? _series;
    [ObservableProperty] private ISeries[]? _seriesYear;

    [ObservableProperty] private ISeries[]? _seriesStroke;

    [ObservableProperty] private PolarAxis[]? _angleAxes;

    [ObservableProperty] private PolarAxis[]? _radiusAxes;

    [ObservableProperty] private Axis[]? _xAxes;

    [ObservableProperty] private Axis[]? _yAxes;

    [ObservableProperty] private Axis[]? _xAxesYear;

    [ObservableProperty] private Axis[]? _yAxesYear;


    public DashboardViewModel()
    {
        LoadPieChartData();
        LoadLastYearLineChart();
        LoadLastYearsLineChart();
    }

    private void LoadLastYearsLineChart()
    {
        EmergencyCaseDataBase emergencyCaseDataBase = new EmergencyCaseDataBase();

        List<Models.EmergencyCase> tempList = emergencyCaseDataBase.LoadData();

        XAxesYear =
        [
            new Axis
            {
                Name = "Jahr",
                NamePaint = new SolidColorPaint(SKColors.Black),
                LabelsPaint = new SolidColorPaint(SKColors.Blue),
                TextSize = 10,
                SeparatorsPaint = new SolidColorPaint(SKColors.LightSlateGray) { StrokeThickness = 2 },
                Labels = GetLastYears(tempList)
            }
        ];
        YAxesYear =
        [
            new Axis
            {
                Name = "Anzahl",
                NamePaint = new SolidColorPaint(SKColors.Black),

                LabelsPaint = new SolidColorPaint(SKColors.Blue),
                TextSize = 10,

                SeparatorsPaint = new SolidColorPaint(SKColors.LightSlateGray) { StrokeThickness = 2 }
            }
        ];

        SeriesYear = new ISeries[]
        {
            new LineSeries<double>
            {
                Values = GetEmergencyCountOfTheYears(tempList),
                Fill = null
            }
        };
    }

    private void LoadLastYearLineChart()
    {
        EmergencyCaseDataBase emergencyCaseDataBase = new EmergencyCaseDataBase();

        List<Models.EmergencyCase> tempList = emergencyCaseDataBase.LoadData();

        XAxes =
        [
            new Axis
            {
                Name = "Monat",
                NamePaint = new SolidColorPaint(SKColors.Black),
                LabelsPaint = new SolidColorPaint(SKColors.Blue),
                TextSize = 10,
                SeparatorsPaint = new SolidColorPaint(SKColors.LightSlateGray) { StrokeThickness = 2 },
                Labels = GetLast12MonthsNames()
            }
        ];
        YAxes =
        [
            new Axis
            {
                Name = "Anzahl",
                NamePaint = new SolidColorPaint(SKColors.Black),

                LabelsPaint = new SolidColorPaint(SKColors.Blue),
                TextSize = 10,

                SeparatorsPaint = new SolidColorPaint(SKColors.LightSlateGray) { StrokeThickness = 2 }
            }
        ];

        Series = new ISeries[]
        {
            new LineSeries<double>
            {
                Values = new double[]
                {
                    GetEmergencyCountOfTheMonth(tempList, -12),
                    GetEmergencyCountOfTheMonth(tempList, -11),
                    GetEmergencyCountOfTheMonth(tempList, -10),
                    GetEmergencyCountOfTheMonth(tempList, -9),
                    GetEmergencyCountOfTheMonth(tempList, -8),
                    GetEmergencyCountOfTheMonth(tempList, -7),
                    GetEmergencyCountOfTheMonth(tempList, -6),
                    GetEmergencyCountOfTheMonth(tempList, -5),
                    GetEmergencyCountOfTheMonth(tempList, -4),
                    GetEmergencyCountOfTheMonth(tempList, -3),
                    GetEmergencyCountOfTheMonth(tempList, -2),
                    GetEmergencyCountOfTheMonth(tempList, -1),
                    GetEmergencyCountOfTheMonth(tempList, 0)
                },
                Fill = null
            }
        };

    }

    private void LoadPieChartData()
    {
        EmergencyCaseDataBase emergencyCaseDataBase = new EmergencyCaseDataBase();

        List<Models.EmergencyCase> tempList = emergencyCaseDataBase.LoadData();

        var upScalingBewusstseinCount = tempList.Count(x => x.ScalingBewusstsein == Scaling.upScaling);
        var downScalingBewusstseinCount = tempList.Count(x => x.ScalingBewusstsein == Scaling.downScaling);
        var noMeasureBewusstseinCount = tempList.Count(x => x.ScalingBewusstsein == Scaling.noMeasure);
        var neScalingBewusstseinCount = tempList.Count(x => x.ScalingBewusstsein == Scaling.noScaling);

        var upScalingAtmungCount = tempList.Count(x => x.ScalingAtmung == Scaling.upScaling);
        var downScalingAtmungCount = tempList.Count(x => x.ScalingAtmung == Scaling.downScaling);
        var noMeasureAtmungCount = tempList.Count(x => x.ScalingAtmung == Scaling.noMeasure);
        var neScalingAtmungCount = tempList.Count(x => x.ScalingAtmung == Scaling.noScaling);


        var upScalingKreislaufCount = tempList.Count(x => x.ScalingKreislauf == Scaling.upScaling);
        var downScalingKreislaufCount = tempList.Count(x => x.ScalingKreislauf == Scaling.downScaling);
        var noMeasureKreislaufCount = tempList.Count(x => x.ScalingKreislauf == Scaling.noMeasure);
        var neScalingKreislaufCount = tempList.Count(x => x.ScalingKreislauf == Scaling.noScaling);


        var upScalingVerletzungCount = tempList.Count(x => x.ScalingVerletzung == Scaling.upScaling);
        var downScalingVerletzungCount = tempList.Count(x => x.ScalingVerletzung == Scaling.downScaling);
        var noMeasureVerletzungCount = tempList.Count(x => x.ScalingVerletzung == Scaling.noMeasure);
        var neScalingVerletzungCount = tempList.Count(x => x.ScalingVerletzung == Scaling.noScaling);


        var upScalingNeurologieCount = tempList.Count(x => x.ScalingNeurologie == Scaling.upScaling);
        var downScalingNeurologieCount = tempList.Count(x => x.ScalingNeurologie == Scaling.downScaling);
        var noMeasureNeurologieCount = tempList.Count(x => x.ScalingNeurologie == Scaling.noMeasure);
        var neScalingNeurologieCount = tempList.Count(x => x.ScalingNeurologie == Scaling.noScaling);


        var upScalingSchmerzCount = tempList.Count(x => x.ScalingSchmerz == Scaling.upScaling);
        var downScalingSchmerzCount = tempList.Count(x => x.ScalingSchmerz == Scaling.downScaling);
        var noMeasureSchmerzCount = tempList.Count(x => x.ScalingSchmerz == Scaling.noMeasure);
        var neScalingSchmerzCount = tempList.Count(x => x.ScalingSchmerz == Scaling.noScaling);

        AngleAxes =
        [
            new PolarAxis
            {
                Labels = new[] { "RMC runter", "RMC hoch", "noch keine Rückmeldung", "keine Veränderung" },
                MinStep = 1000,
                ForceStepToMin = false
            }
        ];

        RadiusAxes =
        [
            new PolarAxis
            {
                LabelsRotation = 45,
                LabelsAngle = 45
            }
        ];

        SeriesStroke = new ISeries[]
        {
            new PolarLineSeries<int>
            {
                Values = new[]
                {
                    upScalingBewusstseinCount, downScalingBewusstseinCount, noMeasureBewusstseinCount,
                    neScalingBewusstseinCount
                },
                Stroke = new SolidColorPaint(SKColors.Yellow) { StrokeThickness = 4 },
                Fill = null,
                Name = "Bewusstsein",
                LineSmoothness = 1,
                GeometryFill = new SolidColorPaint(SKColors.AliceBlue),
                GeometryStroke = new SolidColorPaint(SKColors.Gray) { StrokeThickness = 4 }

            },
            new PolarLineSeries<int>
            {
                Values = new[]
                    { upScalingAtmungCount, downScalingAtmungCount, noMeasureAtmungCount, neScalingAtmungCount },
                Stroke = new SolidColorPaint(SKColors.Aqua) { StrokeThickness = 8 },
                Fill = null,
                Name = "Atmung",
                GeometryFill = new SolidColorPaint(SKColors.AliceBlue),
                GeometryStroke = new SolidColorPaint(SKColors.Gray) { StrokeThickness = 4 },
                LineSmoothness = 1,
            },
            new PolarLineSeries<int>
            {
                Values = new[]
                {
                    upScalingKreislaufCount, downScalingKreislaufCount, noMeasureKreislaufCount, neScalingKreislaufCount
                },
                Stroke = new SolidColorPaint(SKColors.Crimson) { StrokeThickness = 1 },
                Fill = null,
                Name = "Kreislauf",
                GeometryFill = new SolidColorPaint(SKColors.AliceBlue),
                GeometryStroke = new SolidColorPaint(SKColors.Gray) { StrokeThickness = 4 },
                LineSmoothness = 1,
            },
            new PolarLineSeries<int>
            {
                Values = new[]
                {
                    upScalingVerletzungCount, downScalingVerletzungCount, noMeasureVerletzungCount,
                    neScalingVerletzungCount
                },
                Stroke = new SolidColorPaint(SKColors.Green) { StrokeThickness = 1 },
                Fill = null,
                Name = "Verletzung",
                GeometryFill = new SolidColorPaint(SKColors.AliceBlue),
                GeometryStroke = new SolidColorPaint(SKColors.Gray) { StrokeThickness = 4 },
                LineSmoothness = 1,
            },
            new PolarLineSeries<int>
            {
                Values = new[]
                {
                    upScalingNeurologieCount, downScalingNeurologieCount, noMeasureNeurologieCount,
                    neScalingNeurologieCount
                },
                Stroke = new SolidColorPaint(SKColors.Gray) { StrokeThickness = 1 },
                Fill = null,
                Name = "Neurologie",
                GeometryFill = new SolidColorPaint(SKColors.AliceBlue),
                GeometryStroke = new SolidColorPaint(SKColors.Gray) { StrokeThickness = 4 },
                LineSmoothness = 1,
            },
            new PolarLineSeries<int>
            {
                Values = new[]
                    { upScalingSchmerzCount, downScalingSchmerzCount, noMeasureSchmerzCount, neScalingSchmerzCount },
                Stroke = new SolidColorPaint(SKColors.BlueViolet) { StrokeThickness = 1 },
                Fill = null,
                Name = "Schmerz",
                GeometryFill = new SolidColorPaint(SKColors.AliceBlue),
                GeometryStroke = new SolidColorPaint(SKColors.Gray) { StrokeThickness = 4 },
                LineSmoothness = 1,
            }
        };
    }
    private static double GetEmergencyCountOfTheMonth(List<EmergencyCase> tempList, int monthBack)
    {
        var countOfThMonth = tempList.Count(x =>
        {
            if (DateTime.TryParse(x.EmergencyDate, out DateTime parseDateTime))
            {
                return parseDateTime.Month == DateTime.Now.AddMonths(monthBack).Month;
            }

            return false;
        });

        return countOfThMonth;
    }

    private static string[] GetLast12MonthsNames()
    {
        CultureInfo culture = new CultureInfo("de-DE"); // Deutsch (Deutschland)
        string[] last12Months = new string[13];
        DateTime currentDate = DateTime.Now;

        for (int i = 0; i < 13; i++)
        {
            // Gehe einen Monat zurück
            DateTime month = currentDate.AddMonths(-i);
            // Formatieren des Datums um den Monatsnamen zu erhalten
            last12Months[i] = month.ToString("MMMM", culture);
        }

        // Umkehren der Reihenfolge, damit der aktuellste Monat zuerst kommt
        Array.Reverse(last12Months);

        return last12Months;
    }

    private static string[] GetLastYears(List<Models.EmergencyCase> list)
    {
        int lastYear = 0;

        IEnumerable<EmergencyCase> returnList = list.Where(x =>
        {
            if (DateTime.TryParse(x.EmergencyDate, out DateTime parseDateTime))
            {
                if (parseDateTime.Year != lastYear)
                {
                    lastYear = parseDateTime.Year;
                    return true;
                }
            }
            
            return false;
        });

        List<string> returnList2 = new List<string>();
        foreach (EmergencyCase emergencyCase in returnList)
        {
            if (DateTime.TryParse(emergencyCase.EmergencyDate, out DateTime parseDateTime))
            {
                returnList2.Add(parseDateTime.Year.ToString());
            }
        }

        return returnList2.ToArray();
    }

    private static double[] GetEmergencyCountOfTheYears(List<EmergencyCase> list)
    {
        string[] years = GetLastYears(list);
        List<double> returnList = new List<double>();

        foreach (string year in years)
        {

            var count = list.Count(x =>
            {
                if (DateTime.TryParse(x.EmergencyDate, out DateTime parseDateTime))
                {
                    if (parseDateTime.Year == Convert.ToInt32(year))
                    {
                        return true;
                    }
                }

                return false;
            });

            returnList.Add(count);
        }

;

        return returnList.ToArray();
    }
}