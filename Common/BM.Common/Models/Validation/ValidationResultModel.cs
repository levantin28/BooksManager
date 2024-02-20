namespace BM.Common.Models.Validation
{
    public class ValidationResultModel
    {
        public List<string> ErrorMessages { get; set; }
        public bool IsValid => !this.ErrorMessages.Any();

        public ValidationResultModel()
        {
            this.ErrorMessages = new List<string>();
        }

        public ValidationResultModel(List<string> errorMessages)
        {
            this.ErrorMessages = errorMessages;
        }

        public ValidationResultModel(string message)
        {
            this.ErrorMessages = new List<string> { message };
        }
    }
}
