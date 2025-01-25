using FluentValidation;
namespace Tabibak.API.BLL.Validations
{
    public class DtoValidationAbstractBase<T> : AbstractValidator<T> where T : class //Base Dto
    {

        public string GetValidationMessage(string validationMessage, string fieldName)
        {
            return string.Format(validationMessage, fieldName);
        }

        public string GetValidationMessageCode(int messageCode)
        {
            return messageCode.ToString();
        }

        public bool IsInteger(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;
            int.TryParse(input, out int result);
            return result % 1 == 0 ? true : false;
        }
        public bool IsInteger(int input)
        {
            return input % 1 == 0 ? true : false;
        }
        public int GetInteger(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return -1;
            int result;
            if (int.TryParse(input, out result))
                return result;
            else
                return -1;

        }
        public bool IsDecimal(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;
            decimal result;
            return decimal.TryParse(input, out result);

        }
        public decimal GetDecimal(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return -1;
            decimal result;
            if (decimal.TryParse(input, out result))
                return result;
            else
                return -1;

        }
    }
}
