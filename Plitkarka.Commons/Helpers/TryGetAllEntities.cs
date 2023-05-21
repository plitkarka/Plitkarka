using Plitkarka.Commons.Exceptions;

namespace Plitkarka.Commons.Helpers;

public static class TryGetAllEntities
{
    public async static Task<T> Cover<T>(Func<Task<T>> action)
    {
        try
        {
            var res = await action.Invoke();

            return res;
        }
        catch (Exception ex)
        {
            throw new MySqlException(ex.Message);
        }
    }
}
