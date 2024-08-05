using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Culture
{
    public class ChangeCultureCommand : BaseCommand
    {
        public ChangeCultureCommand(string culture)
        {

            Culture = culture;

        }
        public string Culture { get; }
    }
}
