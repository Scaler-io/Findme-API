using FluentValidation;
using FluentValidation.Validators;

namespace API.Validators.Photo
{
    public class FileExtensionValidator<T, TElement> : PropertyValidator<T, TElement>
    {
        private int _maxSize;

        public FileExtensionValidator(int maxSize)
        {
            _maxSize = maxSize;
        }

        public override string Name => "FileExtensionValidator";

        public override bool IsValid(ValidationContext<T> context, TElement value)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                if(file.Length > _maxSize * 1024 * 1024)
                {
                    context.MessageFormatter.AppendArgument("MaxSize", _maxSize);
                    return false;
                }
            }
            return true;
        }

        protected override string GetDefaultMessageTemplate(string errorCode)
        {
            return "{PropertyName} size should be less than {MaxSize} MB";
        }
    }
}
