using FluentValidation.Results;
using Tabibak.Api.BLL.Constants;
using Tabibak.API.BLL.Validations;

namespace Tabibak.Api.BLL.BaseReponse
{
    internal static class ResponseBuilderExtensions
    {
        internal static IResponse<T> CreateResponse<T>(this ResponseBuilder<T> response)
        {
            return response.Build();
        }

        internal static IResponse<T> CreateResponse<T>(this ResponseBuilder<T> response, T data)
        {
            return response.WithData(data).Build();
        }

        internal static IResponse<T> CreateResponse<T>(this ResponseBuilder<T> response, List<ValidationFailure> inputValidations = null)
        {
            return response.WithErrors(inputValidations).Build();
        }

        internal static IResponse<T> CreateResponse<T, IInputDto, IValidator>(this ResponseBuilder<T> response, IInputDto dto, IValidator validator) where IValidator : DtoValidationAbstractBase<IInputDto> where IInputDto : class

        {
            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
                return response.WithErrors(validationResult.Errors).Build();
            else
                return response.Build();
        }

        //for one business error
        internal static IResponse<T> CreateResponse<T>(this ResponseBuilder<T> response, MessageCodes messageCode, string message = "")
        {
            return response.AppendError(messageCode, message).Build();
        }


    }
}