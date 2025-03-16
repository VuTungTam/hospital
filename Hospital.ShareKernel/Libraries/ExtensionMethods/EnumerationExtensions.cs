using System.ComponentModel;

namespace Hospital.SharedKernel.Libraries.ExtensionMethods
{
    public static class EnumerationExtensions
    {
        public static string GetDescription(this Enum en)
        {
            var fi = en.GetType().GetField(en.ToString());
            if (fi == null)
            {
                return string.Empty;
            }

            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Any())
            {
                return attributes.First().Description;
            }
            return en.ToString();
        }

        public static List<EnumValue> ToValues<T>(string noneOption) where T : Enum
        {
            var values = Enum.GetValues(typeof(T))
                             .Cast<T>()
                             .Select(value => new EnumValue { Key = value.ToString(), Value = Convert.ToInt32(value), Description = value.GetDescription() });

            switch (noneOption)
            {
                case "replace":
                    var newList = new List<EnumValue>();
                    foreach (var value in values)
                    {
                        if (value.Key == "None")
                        {
                            value.Description = "Tất cả";
                        }
                        newList.Add(value);
                    }
                    return newList;

                case "remove":
                    return values.Where(x => x.Key != "None").ToList();

                default:
                    return values.ToList();
            }
        }
        public class EnumValue
        {
            public string Key { get; set; }

            public int Value { get; set; }

            public string Description { get; set; }
        }
    }
}
