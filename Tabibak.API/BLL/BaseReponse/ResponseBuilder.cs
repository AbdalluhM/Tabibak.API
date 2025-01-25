using Tabibak.Api.Constants;
using Tabibak.Api.BLL.Constants;
using FluentValidation.Results;
using Tabibak.Api.Helpers.EnumExtention;

namespace Tabibak.Api.BLL.BaseReponse
{
    public class ResponseBuilder<T>
    {
        private bool isSuccess;
        private T data;
        private List<TErrorField> errors;
        private Exception exception;
        private IResponse<T> response;
        internal ResponseBuilder(IResponse<T> response)
        {
            this.response = response;
            errors = new List<TErrorField>();
            exception = null;
        }

        private void InitializeErrorsIfNot()
        {
            if (errors == null)
                errors = new List<TErrorField>();
        }
        internal ResponseBuilder<T> AppendError(TErrorField error)
        {
            return AppendErrors(new List<TErrorField> { error });
        }

        internal ResponseBuilder<T> AppendError(MessageCodes code, string message)
        {
            return AppendErrors(new List<TErrorField>
            {
                new TErrorField
                    {
                        FieldName = "",
                        Code = code.StringValue(),
                        Message = !string.IsNullOrEmpty(message)
                                ? string.Format(code.GetDescription(),message):code.GetDescription()

                    }
               }
            );


        }
        internal ResponseBuilder<T> AppendError(MessageCodes code, string fieldName, string message)
        {
            return AppendErrors(new List<TErrorField> { new TErrorField { FieldName = fieldName, Code = code.StringValue() ,
                  Message =message
            } });
        }
        internal ResponseBuilder<T> AppendError(ValidationFailure error)
        {
            return AppendErrors(new List<ValidationFailure> { error });
        }
        internal ResponseBuilder<T> AppendErrors(List<TErrorField> errors)
        {
            InitializeErrorsIfNot();
            this.errors.AddRange(errors);
            return this;
        }
        internal ResponseBuilder<T> AppendErrors(List<ValidationFailure> errors)
        {
            InitializeErrorsIfNot();
            foreach (var item in errors)
            {
                this.errors.Add(new TErrorField

                {
                    FieldName = item.PropertyName,
                    Code = item.ErrorCode,
                    Message = item.ErrorMessage,
                    FieldLang = item.AttemptedValue?.ToString()
                });

            }
            return this;
        }
        internal ResponseBuilder<T> WithError(TErrorField error)
        {
            return WithErrors(new List<TErrorField> { error });
        }
        internal ResponseBuilder<T> WithError(ValidationFailure error)
        {
            return WithErrors(new List<ValidationFailure> { error });
        }
        internal ResponseBuilder<T> WithErrors(List<TErrorField> errors)
        {
            InitializeErrorsIfNot();
            this.errors.AddRange(errors);
            return this;
        }
        internal ResponseBuilder<T> WithErrors(List<ValidationFailure> errors)
        {
            InitializeErrorsIfNot();
            foreach (var item in errors)
            {
                item.PropertyName = item.PropertyName == "File.File" ? "File" : item.PropertyName;
                this.errors.Add(new TErrorField
                {
                    FieldName = item.PropertyName,
                    Code = item.ErrorCode,
                    Message = string.Format(item.ErrorMessage, $"[" + item.PropertyName + "]"),
                    FieldLang = item.ErrorCode == MessageCodes.Required.StringValue() ? item.AttemptedValue?.ToString() : null
                });
                ;
                ;

            }
            return this;
        }
        internal ResponseBuilder<T> WithData(T data)
        {
            this.data = data;
            return this;
        }

        internal bool IsSuccess { get => (errors == null || errors.Count == 0) && exception == null ? true : false; }
        internal IResponse<T> Build()
        {
            isSuccess = (errors == null || errors.Count == 0) && exception == null ? true : false;
            response.IsSuccess = isSuccess;
            response.Errors = errors;
            response.Data = data;
            return response;

        }

    }

}

