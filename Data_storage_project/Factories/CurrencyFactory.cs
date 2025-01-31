using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;

namespace Data_storage_project_library.Factories;

public static class CurrencyRegistrationFactory
{
    public static CurrencyEntity CreateCurrency(CurrencyRegistrationForm form)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Currency registration form cannot be null.");

        return new CurrencyEntity
        {
            Code = form.Code.ToUpper(), // Ensures uppercase codes
            Name = form.Name
        };
    }
}
