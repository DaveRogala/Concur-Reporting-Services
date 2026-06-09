using ConcurExpense.Models;
using ConcurReportingDatabaseServices.Models.Bases;

namespace ConcurReporting;

internal class JoinEntity<T, U>
    where T : ObjectBase
    where U : ExpenseBaseDto
{
    public JoinEntity(T? entity, U? dto)
    {
        Entity = entity;
        Dto = dto;
    }

    public T? Entity { get; set; }
    public U? Dto { get; set; }
}
