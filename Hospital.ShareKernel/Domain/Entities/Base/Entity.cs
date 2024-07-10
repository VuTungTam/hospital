using Hospital.SharedKernel.Domain.Entities.Interfaces;
using Hospital.SharedKernel.Libraries.ExtensionMethods;
using Hospital.SharedKernel.Runtime.Exceptions;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.SharedKernel.Domain.Entities.Base
{
    public class Entity : IEntity
    {
        public object this[string propertyName] 
        {
            get
            {
                var prop = GetType().GetProperty(propertyName);
                if (prop == null)
                {
                    throw new CatchableException(string.Format("Property {0} does not exists in {1}", propertyName, GetType().Name));
                }
                return prop.GetValue(this);
            }

            set
            {
                var prop = GetType().GetProperty(propertyName);
                if (prop == null)
                {
                    throw new CatchableException(string.Format("Property {0} does not exists in {1}", propertyName, GetType().Name));
                }
                prop.SetValue(this, value);
            }
        }

        public string GetTableName()
        {
            if (GetType().IsDefined(typeof(TableAttribute), false))
            {
                return ((TableAttribute)GetType().GetCustomAttributes(typeof(TableAttribute), false).First()).Name;
            }
            return GetType().Name.ToSnakeCaseLowers();
        }
    }
}
