using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDSCalculatorDesktop.Models
{
    public enum MeasurementType
    {
        BackgroundConcentration,
        SubstanceConcentration,
        PDK,
        KNK
    }
    public class Measurement
    {
        public int Id { get; set; }
        public int SubstanceId { get; set; }
        public int? DischargeId { get; set; }
        public int? ControlPointId {  get; set; }
        public MeasurementType MeasurementType { get; set; }
        public double Value { get; set; }
        public DateTime Date { get; set; }
        public required Substance Substance { get; set; }
        public Discharge? Discharge { get; set; }
        public ControlPoint? ControlPoint { get; set; }
    }
}
