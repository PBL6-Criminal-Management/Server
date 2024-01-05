using System.ComponentModel.DataAnnotations;

namespace Application.Exceptions
{
    public static class EnumExtensions
    {
        public static string ToDescriptionString(this Enum val)
        {
            var field = val.GetType().GetField(val.ToString());
            var displayAttribute = field == null? null : (DisplayAttribute?)Attribute.GetCustomAttribute(field, typeof(DisplayAttribute));

            return displayAttribute?.Description == null ? "" : displayAttribute.Description;
        }
    }
}