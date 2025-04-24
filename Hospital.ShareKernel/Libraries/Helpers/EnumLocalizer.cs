using Hospital.Resource.Properties;
using Hospital.SharedKernel.Libraries.ExtensionMethods;
using Microsoft.Extensions.Localization;

public class EnumLocalizer
{
    private readonly IStringLocalizer _localizer;

    public EnumLocalizer(IStringLocalizer<Resources> localizer)
    {
        _localizer = localizer;
    }

    public string GetLocalizedDescription(Enum value)
    {
        var desc = EnumerationExtensions.GetDescription(value);
        return _localizer[desc]; // dịch từ resource
    }
}
