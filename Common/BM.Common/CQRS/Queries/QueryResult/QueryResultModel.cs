namespace BM.Common.CQRS.Queries
{
    public class QueryResultModel<TQueryResult> where TQueryResult : class
    {
        public List<TQueryResult> QueryResults { get; set; }
        public List<string> ErrorMessages { get; set; }
        public bool HasErrors => this.ErrorMessages.Count > 0;

        public QueryResultModel()
        {
            this.ErrorMessages = new List<string>();
        }
        public QueryResultModel(List<string> errorMessages)
        {
            this.ErrorMessages = new List<string>();
            this.ErrorMessages = errorMessages;
        }

        public QueryResultModel(string message)
        {
            this.ErrorMessages = new List<string> { message };
        }

        public QueryResultModel(List<TQueryResult> queryResults)
        {
            this.ErrorMessages = new List<string>();
            this.QueryResults = queryResults;
        }

        public QueryResultModel(TQueryResult queryResult)
        {
            this.ErrorMessages = new List<string>();
            this.QueryResults = new List<TQueryResult> { queryResult };
        }
    }
}
