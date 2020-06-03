namespace DataApi
{
    public class IdService : IDetermineIfIdsAreValid
    {
        public bool IsValidId(string id)
        {
            return id != "not_found_id";
        }
    }
}
