namespace WebAPIDemo.Filters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class NotTransactionalAttribute:Attribute
    {
    }
}
