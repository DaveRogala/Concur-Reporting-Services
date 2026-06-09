using ConcurExpense.Models;
using ConcurReportingDatabaseServices.Models.Bases;
using System.Globalization;

namespace ConcurReporting.Helpers;

internal static class Helpers
{
    public static DateOnly? ToDateOnly(this string dateString) =>
        DateOnly.TryParseExact(dateString, "yyyy-dd-MM", out DateOnly dateout) ? dateout : null;

    public static DateTime? ToDateTime(this string dateString) => 
        DateTime.TryParseExact(dateString,
            "yyyy-MM-ddTHH:mm:ss",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None, 
            out DateTime dateOut) ? dateOut : null;
    
    public static decimal? ToDecimal(this string decimalString) => 
        Decimal.TryParse(decimalString, out decimal outDecimal) ? outDecimal : null;

    public static int? ToInt(this string intString) => 
        int.TryParse(intString, out  int outInt) ? outInt : null;

    public static bool? ToBoolean(this string ynString) =>
        ynString == "Y";

    internal static List<JoinEntity<T,U>> ToJoinedEntities<T,U>(this List<T> entities, List<U> dtos)
        where T : ObjectBase
        where U: ExpenseBaseDto
    {
       return  entities.LeftJoin(dtos,
                                e => e.ConcurID,
                                d => d.ID,
                                (e, d) => new JoinEntity<T, U>(e, d))
        .UnionBy(entities.RightJoin(dtos,
                e => e.ConcurID,
                d => d.ID,
                (e, d) => new JoinEntity<T, U>(e, d))
                , row => row)
        .DistinctBy(e => new { e.Entity?.ConcurID, e.Dto?.ID }).ToList();
    }
    
}
