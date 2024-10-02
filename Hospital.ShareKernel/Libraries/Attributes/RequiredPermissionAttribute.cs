using Hospital.SharedKernel.Application.Services.Auth.Enums;

namespace Hospital.SharedKernel.Libraries.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RequiredPermissionAttribute : Attribute
    {
        public ActionExponent[] Exponents { get; } = new ActionExponent[] { };

        public bool HasAnyPermission { get; }

        public RequiredPermissionAttribute(params ActionExponent[] exponents)
        {
            Exponents = Exponents.Concat(exponents)
                                 .Distinct()
                                 .ToArray();
        }

        public RequiredPermissionAttribute(bool hasAnyPermission = false, params ActionExponent[] exponents)
        {
            Exponents = Exponents.Concat(exponents)
                                 .Distinct()
                                 .ToArray();

            HasAnyPermission = hasAnyPermission;
        }
    }
}
