using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class ThresholdLimits
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public float HumidityMin { get; set; } = -999f;
        public float HumidityMax { get; set; } = 999f;
        public float TemperatureMin { get; set; } = -99f;
        public float TemperatureMax { get; set; } = 199f;

        public override bool Equals(object? obj)
        {
            if (obj is not ThresholdLimits) return false;

            var t = (ThresholdLimits)obj;
            return HumidityMax.Equals(t.HumidityMax)
                   && HumidityMin.Equals(t.HumidityMin)
                   && TemperatureMax.Equals(t.TemperatureMax)
                   && TemperatureMin.Equals(t.TemperatureMin);
        }

        protected bool Equals(ThresholdLimits other)
        {
            return HumidityMin.Equals(other.HumidityMin) && HumidityMax.Equals(other.HumidityMax) && TemperatureMin.Equals(other.TemperatureMin) && TemperatureMax.Equals(other.TemperatureMax);
        }
    }
}