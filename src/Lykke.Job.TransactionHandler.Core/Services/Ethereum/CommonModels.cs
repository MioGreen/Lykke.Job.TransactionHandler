namespace Lykke.Job.TransactionHandler.Core.Services.Ethereum
{
    public class EthereumResponse<T> : EthereumBaseResponse
    {
        public T Result { get; set; }
    }

    public class EthereumBaseResponse
    {
        public ErrorResponse Error { get; set; }

        public bool HasError => Error != null;
    }

    public class ErrorResponse
    {
        public string Code { get; set; }
        public string Message { get; set; }

        public ErrorCode ErrorCode
        {
            get
            {
                ErrorCode code;
                int value;
                if (!int.TryParse(Code, out value))
                    code = ErrorCode.Exception;
                else
                    code = (ErrorCode)value;
                return code;
            }
        }
    }

    public enum ErrorCode
    {
        Exception = 0,
    }
}